
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance; // Singleton instance

    [SerializeField] TMP_Dropdown regionDropdown;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField findRoomInputField;
    [SerializeField] TMP_InputField maxPlayersInputField;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] Button startGameButton;

    public static bool isRoomPrivate;
    private bool isReadyToCreateRoom = false; // Cờ kiểm tra trạng thái kết nối

    void Start()
    {
        ConnectToPhoton();
    }

    // Kết nối đến Photon
    public void ConnectToPhoton()
    {
        Debug.Log("Đang kết nối tới Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    // Tạo phòng
    public void CreateRoom()
    {
        if (!isReadyToCreateRoom)
        {
            Debug.LogError("Client chưa sẵn sàng để tạo phòng. Hãy chờ kết nối thành công.");
            return;
        }

        string roomName = roomNameInputField.text;
        int maxPlayers;
        int.TryParse(maxPlayersInputField.text, out maxPlayers);

        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = (byte)maxPlayers,
            IsVisible = !isRoomPrivate,
            IsOpen = true
        };

        Debug.Log("Đang tạo phòng: " + roomName);
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    // Vào phòng
    public void JoinRoom()
    {
        string roomName = findRoomInputField.text;
        Debug.Log("Đang vào phòng: " + roomName);
        PhotonNetwork.JoinRoom(roomName);
    }

    // Thoát khỏi phòng
    public void LeaveRoom()
    {
        Debug.Log("Đang thoát khỏi phòng.");
        PhotonNetwork.LeaveRoom();
    }

    // Bắt đầu trò chơi
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Bắt đầu trò chơi...");
            PhotonNetwork.LoadLevel("HuyTest 2");
        }
    }

    // Kick thành viên ra khỏi phòng
    public void KickPlayer(Player player)
    {
        if (PhotonNetwork.IsMasterClient && player != PhotonNetwork.LocalPlayer)
        {
            Debug.Log("Đã kick người chơi: " + player.NickName);
            PhotonNetwork.CloseConnection(player);
        }
    }

    // Hiển thị danh sách người chơi trong phòng
    public List<Player> GetPlayersInRoom()
    {
        List<Player> players = new List<Player>(PhotonNetwork.PlayerList);
        return players;
    }

    // Callback khi kết nối thành công tới Photon
    public override void OnConnectedToMaster()
    {
        Debug.Log("Kết nối thành công tới Photon!");
        statusText.text = "Kết nối thành công tới Photon!";
        isReadyToCreateRoom = true; // Đánh dấu là sẵn sàng để tạo phòng
    }

    // Callback khi vào lobby thành công
    public override void OnJoinedLobby()
    {
        Debug.Log("Đã vào lobby thành công.");
        isReadyToCreateRoom = true; // Đánh dấu là sẵn sàng để tạo phòng
    }

    // Callback khi tạo phòng thành công
    public override void OnCreatedRoom()
    {
        Debug.Log("Phòng đã được tạo: " + PhotonNetwork.CurrentRoom.Name);
        statusText.text = "Phòng đã được tạo: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    // Callback khi vào phòng thành công
    public override void OnJoinedRoom()
    {
        Debug.Log("Đã vào phòng: " + PhotonNetwork.CurrentRoom.Name);
        statusText.text = "Đã vào phòng: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    // Callback khi thoát khỏi phòng
    public override void OnLeftRoom()
    {
        Debug.Log("Đã thoát khỏi phòng.");
        statusText.text = "Đã thoát khỏi phòng.";
    }

    // Callback khi có người chơi khác vào phòng
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Người chơi mới đã vào phòng: " + newPlayer.NickName);
        statusText.text = newPlayer.NickName + " đã vào phòng!";
        UpdatePlayerList();
    }

    // Callback khi có người chơi khác rời phòng
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Người chơi đã rời phòng: " + otherPlayer.NickName);
        statusText.text = otherPlayer.NickName + " đã rời phòng.";
        UpdatePlayerList();
    }

    // Cập nhật danh sách người chơi trong phòng
    private void UpdatePlayerList()
    {
        List<Player> players = GetPlayersInRoom();
        statusText.text = "Danh sách thành viên trong phòng:\n";
        foreach (Player player in players)
        {
            statusText.text += player.NickName + "\n";
        }
    }

    // Callback khi bị ngắt kết nối
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Đã ngắt kết nối từ Photon. Lý do: " + cause.ToString());
        statusText.text = "Đã ngắt kết nối từ Photon. Lý do: " + cause.ToString();
        isReadyToCreateRoom = false; // Đánh dấu là không sẵn sàng để tạo phòng
    }

    // Kiểm tra thông tin kết nối
    public void CheckConnectionInfo()
    {
        string region = PhotonNetwork.CloudRegion;
        int ping = PhotonNetwork.GetPing();
        Debug.Log("Vùng máy chủ hiện tại: " + region);
        Debug.Log("Ping hiện tại: " + ping + " ms");
    }
}
