using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField createInput;
    [SerializeField] TMP_InputField joinInput;
    [SerializeField] TMP_Text notificationText;
    [SerializeField] TMP_Text pingText;
    [SerializeField] float timeWayNotificationText = 2f;
    [SerializeField] Button phongRiengTuButtonOn; // Sử dụng Button
    [SerializeField] Button phongRiengTuButtonOff; // Sử dụng Button


    private bool phongRiengTu = false;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        CheckPing();
        phongRiengTuButtonOn.onClick.AddListener(TogglePhongRiengTuOn); // Liên kết sự kiện OnClick
        phongRiengTuButtonOff.onClick.AddListener(TogglePhongRiengTuOff); // Liên kết sự kiện OnClick
    }

    // Phương thức để chuyển đổi trạng thái của phongRiengTu
    private void TogglePhongRiengTuOn()
    {
        phongRiengTu = true;
        Debug.Log("Phòng riêng tư: " + phongRiengTu);
    }
    private void TogglePhongRiengTuOff()
    {
        phongRiengTu = false;
        Debug.Log("Phòng riêng tư: " + phongRiengTu);
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createInput.text))
        {
            notificationText.text = "Xin vui lòng nhập tên phòng!";
            StartCoroutine(NotificationText());
        }
        else
        {
            notificationText.text = "Đang tạo phòng, chờ chút nhé!";
            StartCoroutine(NotificationText());

            RoomOptions roomOptions = new RoomOptions
            {
                IsVisible = !phongRiengTu,
                IsOpen = true,
                MaxPlayers = 5
            };

            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        }
    }

    public void JointRoom()
    {
        if (joinInput.text == "")
        {
            notificationText.text = "Xin vui lòng nhập tên phòng!";
            StartCoroutine(NotificationText());
        }
        else
        {
            notificationText.text = "Đang tìm phòng, chờ chút nhé!";
            StartCoroutine(NotificationText());
            PhotonNetwork.JoinRoom(joinInput.text);
        }
    }

    public void JointRandomRoom()
    {
        notificationText.text = "Đang tìm phòng!";
        StartCoroutine(NotificationText());
        PhotonNetwork.JoinRandomRoom();
    }
    // Load scence HowToPlay
    public void OnHowToPlay()
    {
        PhotonNetwork.LoadLevel("How To Play");
    }
    public void OnHome()
    {
        PhotonNetwork.LoadLevel("Home");
    }

    // Callback khi vào phòng thành công
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game1");
    }

    // Callback khi có người chơi khác vào phòng
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Người chơi mới vào phòng: " + newPlayer.NickName);
        
    }

    private IEnumerator NotificationText()
    {
        yield return new WaitForSeconds(timeWayNotificationText);
        notificationText.text = null;
    }

    // Phương thức kiểm tra ping
    public void CheckPing()
    {
        int ping = PhotonNetwork.GetPing();  // Lấy giá trị ping từ Photon
        pingText.text = "Ping: " + ping + " ms";  // Cập nhật giá trị ping trên giao diện người dùng
    }

    public void ExitToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
