using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator ani;
    private SpriteRenderer sr;
    void Start()
    {
        ani = GetComponent<Animator>();
        sr = GameObject.Find("Player_Sprite").GetComponent<SpriteRenderer>();
    }

    public void RunAnimation(Vector2 moveVector)
    {
        if (Mathf.Abs(moveVector.x) > 0.1f)
        {
            ani.SetBool("IsMoving", true);
            ani.SetFloat("MoveX", moveVector.x);
        }
        else if (Mathf.Abs(moveVector.y) > 0.1f)
        {
            ani.SetBool("IsMoving", true);
            ani.SetFloat("MoveY", moveVector.y);
        }
        else
        {
            ani.SetBool("IsMoving", false);
        }
    }
    public void HurtAnimation()
    {
        ani.SetTrigger("Hurt");
    }
    public void FaintedAnimation()
    {
        ani.SetTrigger("Fainted");
    }
    public void RecoverAnimation(bool isTrue)
    {
        ani.SetBool("IsFainted", isTrue);
    }
    public void AttackAnimation()
    {
        ani.SetTrigger("Attack");
    }
    public void EatAnimation()
    {
        ani.SetTrigger("Eat");
        Debug.Log("Eat Animation");
    }
}
