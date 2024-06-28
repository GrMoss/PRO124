//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using Photon.Pun;
//using Photon.Realtime;
//using TMPro;

//public class PlayerTest : MonoBehaviourPunCallbacks
//{
//    public float runSpeed = 6f;
//    private Vector2 moveInput;
//    private Rigidbody2D myRigidbody;
//    private SpriteRenderer mySpriteRenderer;
//    private TMP_Text BXHText;
//    private TMP_Text scoreText;
//    public static int score;

//    private static Dictionary<int, int> playerScores = new Dictionary<int, int>(); // Lưu trữ điểm số
//    PhotonView view;

//    void Start()
//    {
//        BXHText.text = "Bảng xếp hạng:";
//        score = 0;
//        myRigidbody = GetComponent<Rigidbody2D>();
//        mySpriteRenderer = GetComponent<SpriteRenderer>();
//        view = GetComponent<PhotonView>();

//        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
//        BXHText = GameObject.Find("BXHText").GetComponent<TMP_Text>();

//        // Thêm điểm của player vào từ điển
//        if (!playerScores.ContainsKey(view.Owner.ActorNumber))
//        {
//            playerScores.Add(view.Owner.ActorNumber, 0);
//        }
//    }

//    private void FixedUpdate()
//    {
//        if (view.IsMine)
//        {
//            Flip();
//            scoreText.text = score.ToString();
//        }
//    }
//    void Update()
//    {
//        if (view.IsMine)
//        {
//            Run();
//            //Flip();
//        }
//    }

//    void OnMove(InputValue value)
//    {
//        moveInput = value.Get<Vector2>();
//    }

//    void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.gameObject.CompareTag("Coin"))
//        {
//            if (view.IsMine)
//            {
//                score += 1;

//                // Cập nhật điểm số trong từ điển
//                playerScores[view.Owner.ActorNumber] = score;

//                // Đồng bộ hóa điểm số với các player khác
//                photonView.RPC("UpdateScore", RpcTarget.All, view.Owner.ActorNumber, score);
//            }
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
//        string leaderboardText = "Bảng xếp hạng:\n";
//        foreach (var scoreEntry in sortedScores)
//        {
//            Player player = null;
//            if (PhotonNetwork.CurrentRoom != null)
//            {
//                PhotonNetwork.CurrentRoom.Players.TryGetValue(scoreEntry.Key, out player);
//            }
//            if (player != null)
//            {
//                leaderboardText += $"{player.NickName}: {scoreEntry.Value} điểm\n";
//            }
//            else
//            {
//                //leaderboardText += $"Player {scoreEntry.Key}: {scoreEntry.Value} điểm\n"; // Trường hợp player đã rời phòng
//            }
//        }

//        // Cập nhật lên UI
//        BXHText.text = leaderboardText;
//    }

//    void Flip()
//    {
//        if (moveInput.x < 0)
//        {
//            mySpriteRenderer.flipX = true;
//        }
//        else if (moveInput.x > 0)
//        {
//            mySpriteRenderer.flipX = false;
//        }
//    }

//    void Run()
//    {
//        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
//        myRigidbody.velocity = playerVelocity;

//        Vector2 playerHorizontal = new Vector2(myRigidbody.velocity.x, moveInput.y * runSpeed);
//        myRigidbody.velocity = playerHorizontal;
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerTest : MonoBehaviourPunCallbacks
{
    public float runSpeed = 6f;
    private Vector2 moveInput;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySpriteRenderer;
    private TMP_Text BXHText;
    private TMP_Text scoreText;
    public static int score;

    private static Dictionary<int, int> playerScores = new Dictionary<int, int>(); // Lưu trữ điểm số
    PhotonView view;

    void Start()
    {
        // Tìm và gán ScoreText và BXHText
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
        BXHText = GameObject.Find("BXHText")?.GetComponent<TMP_Text>();

        // Nếu không tìm thấy, không báo lỗi, chỉ ghi nhật ký thông tin
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

        score = 0;
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        view = GetComponent<PhotonView>();

        // Thêm điểm của player vào từ điển
        if (!playerScores.ContainsKey(view.Owner.ActorNumber))
        {
            playerScores.Add(view.Owner.ActorNumber, 0);
        }
    }

    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            Flip();

            // Chỉ cập nhật scoreText nếu không phải là null
            if (scoreText != null)
            {
                scoreText.text = score.ToString();
            }
        }
    }

    void Update()
    {
        if (view.IsMine)
        {
            Run();
            //Flip();
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            if (view.IsMine)
            {
                score += 1;

                // Cập nhật điểm số trong từ điển
                playerScores[view.Owner.ActorNumber] = score;

                // Đồng bộ hóa điểm số với các player khác
                photonView.RPC("UpdateScore", RpcTarget.All, view.Owner.ActorNumber, score);
            }
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
        // Lấy danh sách các player được sắp xếp theo điểm số giảm dần
        var sortedScores = new List<KeyValuePair<int, int>>(playerScores);
        sortedScores.Sort((x, y) => y.Value.CompareTo(x.Value));

        // Cập nhật bảng xếp hạng
        string leaderboardText = "Bảng xếp hạng:\n";
        foreach (var scoreEntry in sortedScores)
        {
            Player player = null;
            if (PhotonNetwork.CurrentRoom != null)
            {
                PhotonNetwork.CurrentRoom.Players.TryGetValue(scoreEntry.Key, out player);
            }
            if (player != null)
            {
                leaderboardText += $"{player.NickName}: {scoreEntry.Value} điểm\n";
            }
            else
            {
                //leaderboardText += $"Player {scoreEntry.Key}: {scoreEntry.Value} điểm\n"; // Trường hợp player đã rời phòng
            }
        }

        // Chỉ cập nhật BXHText nếu không phải là null
        if (BXHText != null)
        {
            BXHText.text = leaderboardText;
        }
    }

    void Flip()
    {
        if (moveInput.x < 0)
        {
            mySpriteRenderer.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            mySpriteRenderer.flipX = false;
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        Vector2 playerHorizontal = new Vector2(myRigidbody.velocity.x, moveInput.y * runSpeed);
        myRigidbody.velocity = playerHorizontal;
    }
}
