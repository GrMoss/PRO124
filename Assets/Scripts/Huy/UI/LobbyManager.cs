using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI playersListText; // Text để hiển thị danh sách người chơi
    [SerializeField] private Button startGameButton; // Nút bắt đầu game
    [SerializeField] private GameObject roomGame;
    [SerializeField] private GameObject roomlobby;
    public static bool offLobby = false; 

    private void Start()
    {
        roomGame.SetActive(false);
        roomlobby.SetActive(true);

        // Cập nhật danh sách người chơi và kiểm tra nếu người chơi là chủ phòng
        UpdatePlayersList();
        CheckIfMasterClient();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Đã tham gia phòng chờ.");

        // Cập nhật danh sách người chơi khi tham gia phòng
        UpdatePlayersList();

        // Kiểm tra nếu người chơi là chủ phòng
        CheckIfMasterClient();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Người chơi mới đã vào phòng.");

        // Cập nhật danh sách người chơi khi có người mới vào phòng
        UpdatePlayersList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Người chơi đã rời khỏi phòng.");

        // Cập nhật danh sách người chơi khi có người rời phòng
        UpdatePlayersList();

        // Kiểm tra lại nếu người chơi là chủ phòng
        CheckIfMasterClient();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("Chủ phòng đã thay đổi.");

        // Cập nhật quyền bắt đầu game cho chủ phòng mới
        CheckIfMasterClient();
    }

    private void UpdatePlayersList()
    {
        playersListText.text = "Players in room:\n";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playersListText.text += player.NickName + "\n";
        }
    }

    private void CheckIfMasterClient()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.interactable = true;
            startGameButton.onClick.RemoveListener(OnStartGameButtonClicked); // Remove previous listener
            startGameButton.onClick.AddListener(OnStartGameButtonClicked); // Add new listener
        }
        else
        {
            startGameButton.interactable = false;
            startGameButton.onClick.RemoveListener(OnStartGameButtonClicked); // Ensure no listeners for non-master clients
        }
    }

    private void OnStartGameButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Chủ phòng đã nhấn nút bắt đầu game.");

            // Khóa phòng và ngăn người chơi mới vào
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            // Chuyển tất cả người chơi vào scene game chính
            photonView.RPC("ChangeRoomState", RpcTarget.All, true);
        }
    }

    [PunRPC]
    private void ChangeRoomState(bool isGameActive)
    {
        roomGame.SetActive(isGameActive);
        roomlobby.SetActive(!isGameActive);
        offLobby = true;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Không thể tham gia phòng: " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Không thể tạo phòng: " + message);
    }

    // Thoát khỏi phòng
    public void LeaveRoom()
    {
        Debug.Log("Đang thoát khỏi phòng.");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Home");
    }
}
