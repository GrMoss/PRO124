using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Choose : MonoBehaviourPunCallbacks
{

    // Callback khi vào phòng thành công
    public override void OnJoinedRoom()
    {
        Debug.Log("Đã vào phòng: " + PhotonNetwork.CurrentRoom.Name);
    }

    // Callback khi có người chơi khác vào phòng
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Người chơi mới đã vào phòng: " + newPlayer.NickName);
    }

    // Thoát khỏi phòng
    public void LeaveRoom()
    {
        Debug.Log("Đang thoát khỏi phòng.");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Home");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Đã rời khỏi phòng.");
        // Quay lại màn hình chính hoặc thực hiện hành động khác
        // Ví dụ: PhotonNetwork.LoadLevel("MainMenu");
    }

    // Callback khi có người chơi khác rời phòng
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Người chơi đã rời phòng: " + otherPlayer.NickName);
        HandleHostLeaving(otherPlayer);
    }

    private void HandleHostLeaving(Player player)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Nếu chính người chơi này là chủ phòng, chọn người chơi khác làm host
            Debug.Log("Chủ phòng đã rời. Đang chọn chủ phòng mới...");
            ChooseNewHost();
        }
    }

    private void ChooseNewHost()
    {
        Player[] players = PhotonNetwork.PlayerList;
        if (players.Length > 0)
        {
            // Chọn người chơi đầu tiên trong danh sách làm host mới
            Player newHost = players[0];
            PhotonNetwork.SetMasterClient(newHost);
            Debug.Log("Chủ phòng mới là: " + newHost.NickName);
        }
    }
}
