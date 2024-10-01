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
    [SerializeField] TMP_Text notificationText;
    [SerializeField] TMP_Text pingText;

    [SerializeField] float timeWayNotificationText = 2f;
    private LobbyManager lobbyManager;

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
    }
    private void FixedUpdate()
    {
        CheckPing();
    }

    // Callback khi vào phòng thành công
    public override void OnJoinedRoom()
    {
        Debug.Log("Đã vào phòng: " + PhotonNetwork.CurrentRoom.Name);
    }

    // Callback khi có người chơi khác vào phòng
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Người chơi mới đã vào phòng: " + newPlayer.NickName);
        notificationText.text = "Người chơi mới đã vào phòng: " + newPlayer.NickName;
        StartCoroutine(NotificationText());
    }

    // Thoát khỏi phòng
    public void LeaveRoom()
    {
        StartCoroutine (Leaving());
    }

    IEnumerator Leaving()
    {
        yield return new WaitForSeconds(0f);
        Debug.Log("Đang thoát khỏi phòng.");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Home");
        lobbyManager.offLobby = false;
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
        notificationText.text = "Người chơi đã rời phòng: " + otherPlayer.NickName;
        StartCoroutine(NotificationText());
        HandleHostLeaving(otherPlayer);
    }

    private void HandleHostLeaving(Player player)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Nếu chính người chơi này là chủ phòng, chọn người chơi khác làm host
            Debug.Log("Chủ phòng đã rời. Đang chọn chủ phòng mới...");
            notificationText.text = "Chủ phòng đã rời. Đang chọn chủ phòng mới...";
            StartCoroutine(NotificationText());
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
            notificationText.text = "Chủ phòng mới là: " + newHost.NickName;
            StartCoroutine(NotificationText());
        }
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
}
