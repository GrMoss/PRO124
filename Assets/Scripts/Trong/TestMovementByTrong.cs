using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class TestMovementByTrong : MonoBehaviour
{
    public float runSpeed = 6f;
    private Vector2 moveInput;
    private Rigidbody2D myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Run();
        Flip();
    }

    void Flip()
    {
        if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        Vector2 playerHorizontal = new Vector2(myRigidbody.velocity.x, moveInput.y * runSpeed);
        myRigidbody.velocity = playerHorizontal;
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
