using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void RunAnimation(Vector2 moveVector)
    {
        if (Mathf.Abs(moveVector.x) > 0.1f)
        {
            ani.SetBool("isMoving", true);

            ani.SetFloat("moveX", moveVector.x);

        }
        else if (Mathf.Abs(moveVector.y) > 0.1f)
        {
            ani.SetBool("isMoving", true);

            ani.SetFloat("moveY", moveVector.y);

        }
        else
        {
            ani.SetBool("isMoving", false);

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
    public void FaintedIdleAnimation(bool isTrue)
    {
        ani.SetBool("IsFainted", isTrue);
    }
}
