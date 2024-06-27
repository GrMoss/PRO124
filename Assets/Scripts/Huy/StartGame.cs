
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class StartGame : MonoBehaviourPunCallbacks
{

    [SerializeField] Button startGameButton;  // Nút bắt đầu trò chơi (chỉ hiển thị cho chủ phòng)

    public static bool isRoomPrivate;  // Cờ để kiểm tra phòng riêng tư


    void Start()
    {
        // Đồng bộ hóa scene cho tất cả người chơi
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Bắt đầu trò chơi
    public void StartGames()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Bắt đầu trò chơi...");
            PhotonNetwork.LoadLevel("Game1");  // Thay "GameScene" bằng tên scene của bạn
        }
    }

    //public void LoadNewScene(string sceneName)
    //{
    //    if (PhotonNetwork.IsMasterClient) // Chỉ người chủ phòng mới có quyền tải scene
    //    {
    //        PhotonNetwork.LoadLevel(sceneName);
    //    }
    //}

    // Callback khi vào phòng thành công
    public override void OnJoinedRoom()
    {
        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);  // Chỉ hiển thị nút bắt đầu cho chủ phòng
    }

  
}
