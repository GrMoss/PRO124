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
        Moving();
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
    void Moving()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 moving = new Vector2(x, y);
        rb.velocity = moving * speed * Time.deltaTime;
        if (Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f)
        {
            isMoving = true;
            Flip(x);
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
