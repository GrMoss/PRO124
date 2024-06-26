//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using Photon.Pun;
//using TMPro;
//using UnityEngine.UI;

//public class PlayerTest : MonoBehaviour
//{
//    public static bool climbLadder = true;
//    public float runSpeed = 6f;
//    private int jumpCount = 0;
//    private Vector2 moveInput;
//    private Rigidbody2D myRigidbody;
//    private SpriteRenderer mySpriteRenderer;
//    private Animator myAnimator;
//    private CapsuleCollider2D myCapsuleCollider;
//    private float gravityScaleAtStart;
//    public TMP_Text scoreText;
//    public static int score;

//    PhotonView view;

//    void Start()
//    {
//        score = 0;
//        myRigidbody = GetComponent<Rigidbody2D>();
//        mySpriteRenderer = GetComponent<SpriteRenderer>();
//        myAnimator = GetComponent<Animator>();
//        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
//        gravityScaleAtStart = myRigidbody.gravityScale;
//        view = GetComponent<PhotonView>();
//    }

//    void Update()
//    {
//        if (scoreText == null)
//        {
//            scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
//        }
//        if (view.IsMine)
//        {
//            Run();
//            Flip();

//            scoreText.text = score.ToString();
//        }
//    }

//    void OnMove(InputValue value)
//    {
//        moveInput = value.Get<Vector2>();
//    }



//    void Flip()
//    {
//        if (moveInput.x == -1)
//        {
//            mySpriteRenderer.flipX = true;
//            //PlayerDie.isLeft = true;
//        }
//        else if (moveInput.x == 1)
//        {
//            mySpriteRenderer.flipX = false;
//            //PlayerDie.isLeft = false;
//        }
//    }

//    void Run()
//    {
//        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
//        myRigidbody.velocity = playerVelocity;

//        Vector2 playerHorizontal = new Vector2(myRigidbody.velocity.x, moveInput.y * runSpeed);
//        myRigidbody.velocity = playerHorizontal;

//        //bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
//        //myAnimator.SetBool("IsRunning", playerHasHorizontalSpeed);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.gameObject.CompareTag("Coint"))
//        {
//            if (view.IsMine)
//            {
//                score += 1;
//            }
//        }
//    }
//}
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
    private int jumpCount = 0;
    private Vector2 moveInput;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySpriteRenderer;
    private Animator myAnimator;
    private CapsuleCollider2D myCapsuleCollider;
    private float gravityScaleAtStart;
    private TMP_Text scoreText;
    public static int score;

    PhotonView view;

    void Start()
    {
        score = 0;
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        view = GetComponent<PhotonView>();

        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (view.IsMine)
        {
            Run();
            Flip();

            scoreText.text = score.ToString();

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

                // Đồng bộ hóa điểm số với các người chơi khác
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
