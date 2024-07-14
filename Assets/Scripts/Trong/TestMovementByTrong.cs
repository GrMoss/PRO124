using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class TestMovementByTrong : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 6f;
    bool isMoving;
    bool isFainted;
    float recoverTime = 5f;
    Animator animator;
    SpriteRenderer sr;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
        sr = GameObject.Find("Player_Sprite").GetComponent<SpriteRenderer>();
        if (sr == null) Debug.Log("no");
    }
    private void Update()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f)
        {
            float x = Input.GetAxisRaw("Horizontal");
            rb.velocity = Vector2.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            isMoving = true;
            Flip(x);
        }
        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
        {
            rb.velocity = Vector2.up * Input.GetAxis("Vertical") * speed * Time.deltaTime;
            isMoving = true;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            rb.velocity = Vector2.zero;
            isMoving = false;
        }

        //Animation

        animator.SetBool("IsMoving", isMoving);
        if (Input.GetMouseButtonDown(0))
        {
            //Atack
            animator.SetTrigger("Attack");
        }
        if (Input.GetMouseButtonDown(1))
        {
            //eat

        }
        if (Input.GetMouseButtonDown(2))
        {
            //hurt
            animator.SetTrigger("Hurt");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Fainted
            animator.SetTrigger("Fainted");
            StartCoroutine(Recover());
        }
        animator.SetBool("IsFainted", isFainted);
    }

    void Flip(float x)
    {
        if (x > 0.1f)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX= true;
        }
    }

    IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.1f);
        isFainted = true;
        yield return new WaitForSeconds(recoverTime);
        isFainted = false;
    }
}
