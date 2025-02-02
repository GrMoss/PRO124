using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI playersListText; // Text để hiển thị danh sách người chơi
    [SerializeField] private Button startGameButton; // Nút bắt đầu game
    [SerializeField] private GameObject roomGame;
    [SerializeField] private GameObject roomlobby;
    [SerializeField] private GameObject canvalobby;
    [SerializeField] private CinemachineVirtualCamera setATCameraVS;
    private PlayerController[] playerController;
    public bool offLobby = false;
    private bool isStartGame = false;

    private void Start()
    {
        roomGame.SetActive(false);
        roomlobby.SetActive(true);
        canvalobby.SetActive(true);
        setATCameraVS.gameObject.SetActive(false);

        // Cập nhật danh sách người chơi và kiểm tra nếu người chơi là chủ phòng
        UpdatePlayersList();
        CheckIfMasterClient();
    }

    private void Update()
    {
        if(!isStartGame)
        {
            playerController = GameObject.FindObjectsOfType<PlayerController>();
        }
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
        playersListText.text = "Người chơi trong phòng:\n";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playersListText.text += "- " + player.NickName + "\n";
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
            isStartGame = true;
            Debug.Log("Chủ phòng đã nhấn nút bắt đầu game.");

            // Khóa phòng và ngăn người chơi mới vào
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            // Chuyển tất cả người chơi vào scene game chính
            photonView.RPC("ChangeRoomState", RpcTarget.All, true);
            foreach(var player in playerController)
            {
                player.view.RPC("TurnOnHealth", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void ChangeRoomState(bool isGameActive)
    {
        roomGame.SetActive(isGameActive);
        //setATCameraVS.SetActive(isGameActive);
        roomlobby.SetActive(!isGameActive);
        offLobby = true;
        canvalobby.SetActive(false);
        setATCameraVS.gameObject.SetActive(true);
        Timer.timerIsRunning = true;
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
