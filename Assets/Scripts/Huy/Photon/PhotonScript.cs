using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonScript : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject setATInLobby;
    [SerializeField] GameObject setATIHPInLobby;
    private TMP_Text BXHText;
    private TMP_Text BXHText2;
    private TMP_Text scoreText;
    public static int score;
    public static bool showBXH = false;
    private bool canStart = true;

    private static Dictionary<int, int> playerScores = new Dictionary<int, int>(); // Lưu trữ điểm số
    PhotonView view;

    private void FixedUpdate()
    {
  

        if (PhotonNetwork.IsMasterClient)
        {

            photonView.RPC("SetActiveObjectsForAll", RpcTarget.All, LobbyManager.offLobby);
        }
        
        
        if (PhotonNetwork.IsMasterClient)
        {
            if (Timer.TimeOver)
            {
                if (BXHText != null)
                {
                    photonView.RPC("UpdateBXHText2ForAll", RpcTarget.All, BXHText.text);
                    Time.timeScale = 0;
                }
            }
        }
    }

    public void SetATShootingOn()
    {
        setATInLobby.SetActive(false);
        setATIHPInLobby.SetActive(false);
    }

    public void SetATShootingOff()
    {
        setATInLobby.SetActive(true);
        setATIHPInLobby.SetActive(true);
    }

    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        if (canStart && LobbyManager.offLobby)
        {
            CanStart();
            canStart = false;
        }
    }

    void CanStart()
    {
        Time.timeScale = 1;

        // Tìm và gán ScoreText và BXHText
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
        BXHText = GameObject.Find("BXHText")?.GetComponent<TMP_Text>();
        BXHText2 = GameObject.Find("BXHText2")?.GetComponent<TMP_Text>();

        score = 0;
        scoreText.text = "0";
        view = GetComponent<PhotonView>();

        ResetPlayerScores();

        // Thêm điểm của player vào từ điển
        if (!playerScores.ContainsKey(view.Owner.ActorNumber))
        {
            playerScores.Add(view.Owner.ActorNumber, 0);
        }

        if (scoreText == null)
        {
            Debug.Log("Không tìm thấy ScoreText trong cảnh. Vui lòng kiểm tra tên đối tượng.");
        }

        if (BXHText == null)
        {
            Debug.Log("Không tìm thấy BXHText trong cảnh. Vui lòng kiểm tra tên đối tượng.");
        }
        else
        {
            BXHText.text = "Bảng xếp hạng:";
        }

        if (BXHText2 == null)
        {
            Debug.Log("Không tìm thấy BXHText2 trong cảnh. Vui lòng kiểm tra tên đối tượng.");
        }
    }

    public void CongDiem(int diem)
    {
        if (view.IsMine)
        {
            score += diem;
            playerScores[view.Owner.ActorNumber] = score;
            photonView.RPC("UpdateScore", RpcTarget.All, view.Owner.ActorNumber, score);
        }
    }

    public void TruDiem(int diem)
    {
        if (view.IsMine)
        {
            score -= diem;
            playerScores[view.Owner.ActorNumber] = score;
            photonView.RPC("UpdateScore", RpcTarget.All, view.Owner.ActorNumber, score);
        }
    }

    [PunRPC]
    void UpdateScore(int actorNumber, int newScore)
    {
        if (playerScores.ContainsKey(actorNumber))
        {
            playerScores[actorNumber] = newScore;
        }
        else
        {
            playerScores.Add(actorNumber, newScore);
        }

        UpdateLeaderboard();
    }

    void UpdateLeaderboard()
    {
        var sortedScores = new List<KeyValuePair<int, int>>(playerScores);
        sortedScores.Sort((x, y) => y.Value.CompareTo(x.Value));

        string leaderboardText = "BẢNG XẾP HẠNG:\n";
        int i = 1;
        foreach (var scoreEntry in sortedScores)
        {
            Player player = null;
            if (PhotonNetwork.CurrentRoom != null)
            {
                PhotonNetwork.CurrentRoom.Players.TryGetValue(scoreEntry.Key, out player);
            }
            if (player != null)
            {
                leaderboardText += $"{i} - {player.NickName}: {scoreEntry.Value} điểm\n";
                i++;
            }
        }

        if (BXHText != null)
        {
            BXHText.text = leaderboardText;
        }

        if (Timer.TimeOver)
        {
            // Gọi RPC để cập nhật BXHText2 cho tất cả người chơi
            photonView.RPC("UpdateBXHText2ForAll", RpcTarget.All, leaderboardText);
        }
    }

    [PunRPC]
    void UpdateBXHText2ForAll(string text)
    {
        if (BXHText2 != null)
        {
            BXHText2.text = text;
        }
    }

    [PunRPC]
    void SetActiveObjectsForAll(bool isActive)
    {
        setATInLobby.SetActive(isActive);
        setATIHPInLobby.SetActive(isActive);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        ResetPlayerScores();
    }

    private void ResetPlayerScores()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerScores.Clear();
            photonView.RPC("SyncScores", RpcTarget.All, new Dictionary<int, int>());
        }
    }

    [PunRPC]
    void SyncScores(Dictionary<int, int> scores)
    {
        playerScores = scores;
        UpdateLeaderboard();
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ResetPlayerScores();
            PhotonNetwork.LoadLevel("GameScene"); // Thay đổi với tên scene của bạn
        }
    }
}
