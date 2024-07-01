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
    private TMP_Text BXHText2;
    private TMP_Text scoreText;
    public static int score;
    public static bool showBXH = false;

    private static Dictionary<int, int> playerScores = new Dictionary<int, int>(); // Lưu trữ điểm số
    PhotonView view;

    void Start()
    {
        
        // Tìm và gán ScoreText và BXHText
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
        BXHText = GameObject.Find("BXHText")?.GetComponent<TMP_Text>();
        BXHText2 = GameObject.Find("BXHText2")?.GetComponent<TMP_Text>();


        score = 0;
        scoreText.text = "0";
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        view = GetComponent<PhotonView>();

        ResetPlayerScores();

        // Thêm điểm của player vào từ điển
        if (!playerScores.ContainsKey(view.Owner.ActorNumber))
        {
            playerScores.Add(view.Owner.ActorNumber, 0);
        }

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

        if (BXHText2 == null)
        {
            Debug.Log("Không tìm thấy BXHText2 trong cảnh. Vui lòng kiểm tra tên đối tượng.");
        }
        else
        {
            BXHText2.text = "Bảng xếp hạng:";

        }

    }

    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            Flip();
            //scoreText.text = score.ToString();
        }


        if (Timer.TimeOver)
        {

            // Cập nhật điểm số trong từ điển
            playerScores[view.Owner.ActorNumber] = score;

            // Đồng bộ hóa điểm số với các player khác
            photonView.RPC("UpdateScore", RpcTarget.All, view.Owner.ActorNumber, score);

            showBXH = true;

        }
        if (PhotonNetwork.IsMasterClient)
        {
           
        }
    }

    void Update()
    {

        if (view.IsMine)
        {
            Run();
        }

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    // Hàm đệ quy để tìm GameObject đã bị tắt theo tên
    GameObject FindInactiveObjectByName(GameObject parent, string name)
    {
        // Kiểm tra các con của GameObject hiện tại
        foreach (Transform child in parent.transform)
        {
            // Nếu tên khớp, trả về GameObject đó
            if (child.gameObject.name == name)
            {
                return child.gameObject;
            }

            // Gọi đệ quy để kiểm tra các con của GameObject hiện tại
            GameObject foundObj = FindInactiveObjectByName(child.gameObject, name);
            if (foundObj != null)
            {
                return foundObj;
            }
        }

        // Nếu không tìm thấy, trả về null
        return null;
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

        // Chỉ cập nhật BXHText nếu không phải là null
        if (BXHText != null)
        {
            BXHText.text = leaderboardText;
        }

        if (BXHText2 != null)
        {
            BXHText2.text = leaderboardText;
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

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        ResetPlayerScores();
    }

    private void ResetPlayerScores()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Xóa hết các giá trị đã lưu trong từ điển
            playerScores.Clear();

            // Đồng bộ hóa điểm số với các player khác
            photonView.RPC("SyncScores", RpcTarget.All, new Dictionary<int, int>());
        }
    }

    [PunRPC]
    void SyncScores(Dictionary<int, int> scores)
    {
        playerScores = scores;
        UpdateLeaderboard();
    }

    // Gọi hàm này khi bắt đầu trò chơi
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ResetPlayerScores();
            PhotonNetwork.LoadLevel("GameScene"); // Thay đổi với tên scene của bạn
        }
    }
    //BẢNG XẾP HẠNG
}
