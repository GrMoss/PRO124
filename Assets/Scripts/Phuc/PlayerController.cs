using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int id;
    public int health = 100;
    private InputSystem input;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb;
    public float moveSpeed;
    public Animator animator;
    private PhotonView view;

    private void Awake()
    {
        id = Random.Range(0, 1000);
        input = new InputSystem();
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        if(view.IsMine)
        {
            rb.velocity = moveVector * moveSpeed;

            if (Mathf.Abs(moveVector.x) > 0.1f)
            {
                animator.SetBool("isMoving", true);

                animator.SetFloat("moveX", moveVector.x);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    public void TakeDamage(int damage)
    {
        if(view.IsMine)
        {
            health -= damage;
            if(health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {

    }
}
