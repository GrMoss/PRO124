
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using TMPro;

public class PlayerTest : MonoBehaviour
{
    public static bool climbLadder = true;
    public float runSpeed = 6f;
    private Vector2 moveInput;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySpriteRenderer;
    private TMP_Text scoreText;
    public static int score;

    PhotonView view;

    void Start()
    {

        //// Đồng bộ hóa scene cho tất cả người chơi
        //PhotonNetwork.AutomaticallySyncScene = true;

        score = 0;
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        view = GetComponent<PhotonView>();

        // Kiểm tra nếu scoreText chưa được gán trong Inspector
        if (scoreText == null)
        {
            // Tìm kiếm đối tượng "ScoreText"
            GameObject scoreTextObject = GameObject.Find("ScoreText");
            if (scoreTextObject != null)
            {
                scoreText = scoreTextObject.GetComponent<TMP_Text>();
            }
            // Không cần báo lỗi nếu không tìm thấy đối tượng
        }
    }

    void Update()
    {
        if (view.IsMine)
        {
            Run();
            Flip();

            // Cập nhật điểm số nếu scoreText đã được gán
            if (scoreText != null)
            {
                scoreText.text = score.ToString();
            }
            // Không cần báo lỗi nếu scoreText vẫn chưa được gán
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

                //// Đồng bộ hóa điểm số với các người chơi khác
                PhotonNetwork.RaiseEvent(0, score, Photon.Realtime.RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);
            }
        }
    }

    void Flip()
    {
        if (moveInput.x < 0)
        {
            mySpriteRenderer.flipX = true;
            // PlayerDie.isLeft = true;
        }
        else if (moveInput.x > 0)
        {
            mySpriteRenderer.flipX = false;
            // PlayerDie.isLeft = false;
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        Vector2 playerHorizontal = new Vector2(myRigidbody.velocity.x, moveInput.y * runSpeed);
        myRigidbody.velocity = playerHorizontal;

        //bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //myAnimator.SetBool("IsRunning", playerHasHorizontalSpeed);
    }

}
