using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        Run();
        Flip();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")
            || collision.gameObject.CompareTag("Trap")
            || collision.gameObject.CompareTag("InWater"))
        {
            jumpCount = 0;
        }

        if (collision.gameObject.CompareTag("InWater"))
        {

        }
    }

    void Flip()
    {
        if (moveInput.x == -1)
        {
            mySpriteRenderer.flipX = true;
            //PlayerDie.isLeft = true;
        }
        else if (moveInput.x == 1)
        {
            mySpriteRenderer.flipX = false;
            //PlayerDie.isLeft = false;
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
