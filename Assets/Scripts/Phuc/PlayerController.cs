using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int health = 0;
    private int healthMax = 100;
    private InputSystem input;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb;
    public float moveSpeed;
    public Animator animator;
    public PhotonView view;
    public GameObject hitBox;
    private bool isDie = false;
    public GameObject rotatePoint;

    private void Awake()
    {
        input = new InputSystem();
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (view.IsMine && !isDie)
        {
            rb.velocity = moveVector * moveSpeed;

            if (Mathf.Abs(moveVector.x) > 0.1f)
            {
                animator.SetBool("isMoving", true);

                animator.SetFloat("moveX", moveVector.x);
            }
            else if (Mathf.Abs(moveVector.y) > 0.1f)
            {
                animator.SetBool("isMoving", true);

                animator.SetFloat("moveY", moveVector.y);
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

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (view.IsMine)
        {
            health += damage;
            if (health >= healthMax)
            {
                health = 0;
                view.RPC("Die", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void Die()
    {
        isDie = true;
        rb.velocity = Vector2.zero;
        rotatePoint.SetActive(false);
        hitBox.SetActive(false);
        StartCoroutine(RevivalTime());
    }

    private IEnumerator RevivalTime()
    {
        yield return new WaitForSeconds(10);
        isDie = false;
        rotatePoint.SetActive(true);
        hitBox.SetActive(true);
    }
}
