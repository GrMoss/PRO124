using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Timer : MonoBehaviourPunCallbacks
{
    public float timeRemaining = 300f; // Thời gian ban đầu là 5 phút (300 giây)
    private float initialTime; // Để lưu trữ giá trị thời gian ban đầu
    public static bool timerIsRunning;
    public TMP_Text timeText;
    public GameObject onPanel;
    public static bool TimeOver = false;

    void Start()
    {
        timerIsRunning = false;
        Time.timeScale = 1;
        // Lưu trữ thời gian ban đầu
        initialTime = timeRemaining;
        DisplayTime(timeRemaining); // Hiển thị thời gian ban đầu khi khởi động
    }

    void Update()
    {
        // Chỉ chủ phòng mới thực hiện việc đếm ngược thời gian
        if (PhotonNetwork.IsMasterClient && timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                // Giảm thời gian còn lại
                timeRemaining -= Time.deltaTime;

                // Gọi RPC để cập nhật thời gian cho tất cả người chơi khác
                photonView.RPC("UpdateTimeForAll", RpcTarget.All, timeRemaining);
            }
            else
            {
                // Khi thời gian kết thúc
                timeRemaining = 0;
                if (timeRemaining == 0)
                {
                    TimeOver = true;
                    photonView.RPC("ShowPanelForAll", RpcTarget.All);
                }
                Debug.Log("Hết thời gian!");
            }
            Debug.Log("LobbyManager.offLobby: " + LobbyManager.offLobby);
        }
        if (!timerIsRunning)
        {
            ResetTime();
        }
    }

    [PunRPC]
    void UpdateTimeForAll(float time)
    {
        // Cập nhật thời gian cho tất cả người chơi
        timeRemaining = time;
        DisplayTime(timeRemaining);
    }

    [PunRPC]
    void ShowPanelForAll()
    {
        // Kích hoạt panel cho tất cả người chơi
        onPanel.SetActive(true);
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1; // Thêm 1 để làm tròn lên

        // Tính số phút và giây
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); // Tính số phút
        float seconds = Mathf.FloorToInt(timeToDisplay % 60); // Tính số giây còn lại

        // Cập nhật văn bản UI với thời gian hiện tại, định dạng "mm:ss"
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Đặt lại thời gian về trạng thái ban đầu
    public void ResetTime()
    {
        TimeOver = false;
        timeRemaining = initialTime; // Đặt lại thời gian về thời gian ban đầu
        timerIsRunning = false; // Dừng bộ đếm thời gian
        DisplayTime(timeRemaining); // Cập nhật UI với thời gian đã được đặt lại
    }
}
