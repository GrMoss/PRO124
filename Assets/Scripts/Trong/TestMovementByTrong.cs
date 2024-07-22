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
    public bool isMoving;
    bool isFainted;
    float recoverTime = 5f;
    Animator animator;
    SpriteRenderer sr;
    public float moveX, moveY;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
        sr = GameObject.Find("Player_Sprite").GetComponent<SpriteRenderer>();
        if (sr == null) Debug.Log("no");
    }
    private void Update()
    {
        //Animation

        animator.SetBool("IsMoving", isMoving);
        if (Input.GetMouseButtonDown(0))
        {
            //Atack
            animator.SetTrigger("Attack");
        }
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Eat");

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
    private void FixedUpdate()
    {

        Moving();
    }
    void Moving()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        Vector2 moving = new Vector2(moveX, moveY);
        rb.velocity = moving * speed * Time.deltaTime;
        if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveY) > 0.1f)
        {
            isMoving = true;
            if (Mathf.Abs(moveX) > 0.1f)
                Flip(moveX);
        }
        else isMoving = false;
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
