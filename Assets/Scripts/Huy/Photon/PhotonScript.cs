//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using Photon.Pun;
//using Photon.Realtime;
//using TMPro;

//public class PhotonScript : MonoBehaviourPunCallbacks
//{
//    private TMP_Text BXHText;
//    private TMP_Text BXHText2;
//    private TMP_Text scoreText;
//    public static int score;
//    public static bool showBXH = false;
//    private bool canStart = true;


//    private static Dictionary<int, int> playerScores = new Dictionary<int, int>(); // Lưu trữ điểm số
//    PhotonView view;



//    private void FixedUpdate()
//    {
//        //if (view.IsMine)
//        //{
//            //scoreText.text = score.ToString();
//        //}

//        if (PhotonNetwork.IsMasterClient)
//        {
//            if (Timer.TimeOver)
//            {
//                if (BXHText != null)
//                {
//                    BXHText2.text = BXHText.text;
//                    Time.timeScale = 0;
//                }
//            }
//        }
//    }

//    void Update()
//    {

//        if (scoreText != null)
//        {
//            scoreText.text = score.ToString();
//        }

//        if (canStart && LobbyManager.offLobby)
//        {
//            CanStart();
//            canStart = false;
//        }
//    }

//    void CanStart()
//    {
//        Time.timeScale = 1;

//        // Tìm và gán ScoreText và BXHText
//        scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
//        BXHText = GameObject.Find("BXHText")?.GetComponent<TMP_Text>();
//        BXHText2 = GameObject.Find("BXHText2")?.GetComponent<TMP_Text>();


//        score = 0;
//        scoreText.text = "0";
//        view = GetComponent<PhotonView>();

//        ResetPlayerScores();

//        // Thêm điểm của player vào từ điển
//        if (!playerScores.ContainsKey(view.Owner.ActorNumber))
//        {
//            playerScores.Add(view.Owner.ActorNumber, 0);
//        }

//        // Nếu không tìm thấy, không báo lỗi, chỉ ghi nhật ký thông tin
//        if (scoreText == null)
//        {
//            Debug.Log("Không tìm thấy ScoreText trong cảnh. Vui lòng kiểm tra tên đối tượng.");
//        }

//        if (BXHText == null)
//        {
//            Debug.Log("Không tìm thấy BXHText trong cảnh. Vui lòng kiểm tra tên đối tượng.");
//        }
//        else
//        {
//            BXHText.text = "Bảng xếp hạng:";
//        }

//        if (BXHText2 == null)
//        {
//            Debug.Log("Không tìm thấy BXHText2 trong cảnh. Vui lòng kiểm tra tên đối tượng.");
//        }

//    }

//    public void CongDiem(int diem)
//    {
//        Debug.Log("Ham cong diem hoat dong");
//        if (view.IsMine)
//        {
//            Debug.Log("Cong diem");
//            score += diem;

//            // Cập nhật điểm số trong từ điển
//            playerScores[view.Owner.ActorNumber] = score;

//            // Đồng bộ hóa điểm số với các player khác
//            photonView.RPC("UpdateScore", RpcTarget.All, view.Owner.ActorNumber, score);
//        }
//    }

//    public void TruDiem(int diem)
//    {
//        Debug.Log("Ham tru diem hoat dong");
//        if (view.IsMine)
//        {
//            Debug.Log("Tru diem");
//            score += diem;

//            // Cập nhật điểm số trong từ điển
//            playerScores[view.Owner.ActorNumber] = score;

//            // Đồng bộ hóa điểm số với các player khác
//            photonView.RPC("UpdateScore", RpcTarget.All, view.Owner.ActorNumber, score);
//        }
//    }

//    [PunRPC]
//    void UpdateScore(int actorNumber, int newScore)
//    {
//        if (playerScores.ContainsKey(actorNumber))
//        {
//            playerScores[actorNumber] = newScore;
//        }
//        else
//        {
//            playerScores.Add(actorNumber, newScore);
//        }

//        UpdateLeaderboard();
//    }

//    void UpdateLeaderboard()
//    {
//        // Lấy danh sách các player được sắp xếp theo điểm số giảm dần
//        var sortedScores = new List<KeyValuePair<int, int>>(playerScores);
//        sortedScores.Sort((x, y) => y.Value.CompareTo(x.Value));

//        // Cập nhật bảng xếp hạng
//        string leaderboardText = "BẢNG XẾP HẠNG:\n";
//        int i = 1;
//        foreach (var scoreEntry in sortedScores)
//        {
//            Player player = null;
//            if (PhotonNetwork.CurrentRoom != null)
//            {
//                PhotonNetwork.CurrentRoom.Players.TryGetValue(scoreEntry.Key, out player);
//            }
//            if (player != null)
//            {
//                leaderboardText += $"{i} - {player.NickName}: {scoreEntry.Value} điểm\n";
//                i++;
//            }

//        }

//        // Chỉ cập nhật BXHText nếu không phải là null
//        if (BXHText != null)
//        {
//            BXHText.text = leaderboardText;
//        }
//    }

//    public override void OnJoinedRoom()
//    {
//        base.OnJoinedRoom();
//        ResetPlayerScores();
//    }

//    private void ResetPlayerScores()
//    {
//        if (PhotonNetwork.IsMasterClient)
//        {
//            // Xóa hết các giá trị đã lưu trong từ điển
//            playerScores.Clear();

//            // Đồng bộ hóa điểm số với các player khác
//            photonView.RPC("SyncScores", RpcTarget.All, new Dictionary<int, int>());
//        }
//    }

//    [PunRPC]
//    void SyncScores(Dictionary<int, int> scores)
//    {
//        playerScores = scores;
//        UpdateLeaderboard();
//    }

//    // Gọi hàm này khi bắt đầu trò chơi
//    public void StartGame()
//    {
//        if (PhotonNetwork.IsMasterClient)
//        {
//            ResetPlayerScores();
//            PhotonNetwork.LoadLevel("GameScene"); // Thay đổi với tên scene của bạn
//        }
//    }
//    //BẢNG XẾP HẠNG
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonScript : MonoBehaviourPunCallbacks
{
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
