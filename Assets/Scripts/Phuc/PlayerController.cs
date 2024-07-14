using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int health = 0;
    private int healthMax = 100;
    public Slider healthSlider;
    public GameObject slider;

    private InputSystem input;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb;
    public float moveSpeed;
    public PhotonView view;
    public GameObject hitBox;
    public bool isDie = false;
    public GameObject rotatePoint;
    private float bleedingTimeRemaining = 0f;

    private PlayerAnimatorController aniController;
    private PlayerAudio audi;

    private void Awake()
    {
        input = new InputSystem();
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        aniController = GetComponent<PlayerAnimatorController>();
        audi = FindObjectOfType<PlayerAudio>();
    }

    private void Start()
    {
        if (view.IsMine)
        {
            isDie = false;
        }

        healthSlider.maxValue = healthMax;
    }

    private void FixedUpdate()
    {
        if (view.IsMine && !isDie)
        {
            rb.velocity = moveVector * moveSpeed;
            aniController.RunAnimation(moveVector);
            audi.PlayerRunning((moveVector.x > 0.1f || moveVector.x < -0.1f));
        }
    }

    private void Update()
    {
        if (view.IsMine)
        {
            healthSlider.value = health;
            if (health >= healthMax)
            {
                isDie = true;
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
        if (view.IsMine && isDie == false)
        {
            health += damage;
            view.RPC("UpdateHealthSlider", RpcTarget.AllBuffered, health);
            view.RPC("UpdateHealthSlider", RpcTarget.AllBuffered);
            if (health >= healthMax)
            {
                health = 0;
                view.RPC("Die", RpcTarget.AllBuffered);
                view.RPC("UpdateHealthSlider", RpcTarget.AllBuffered, health);
                view.RPC("UpdateHealthSlider", RpcTarget.AllBuffered);
                aniController.FaintedAnimation();

                audi.PlayerFainted();
            }
            else
            {
                view.RPC("PlayHurtAnimation", RpcTarget.AllBuffered);

                audi.PlayerHurt();
            }
        }
    }

    [PunRPC]
    private void UpdateHealthSlider(int updatedHealth)
    {
        healthSlider.value = updatedHealth;
    }

    [PunRPC]
    public void PlayHurtAnimation()
    {
        aniController.HurtAnimation();
    }

    [PunRPC]
    public void StartSlow(float time)
    {
        StartCoroutine(Slow(time));
    }

    private IEnumerator Slow(float time)
    {
        if(view.IsMine)
        {
            moveSpeed = 2.5f;
        }
        yield return new WaitForSeconds(time);
        if (view.IsMine)
        {
            moveSpeed = 4f;
        }
    }

    [PunRPC]
    public void StartBleeding(int damage, float time)
    {
        if (bleedingTimeRemaining > 0)
        {
            bleedingTimeRemaining += time;
        }
        else
        {
            StartCoroutine(Bleeding(damage, time));
        }
    }

    private IEnumerator Bleeding(int damage, float time)
    {
        bleedingTimeRemaining = time;
        GetComponentInChildren<Shooting>().timeBetweenFiring = 0.65f;
        while (bleedingTimeRemaining > 0)
        {
            yield return new WaitForSeconds(1);
            if (view.IsMine)
            {
                TakeDamage(damage);
            }
            bleedingTimeRemaining -= 1f;
        }
        GetComponentInChildren<Shooting>().timeBetweenFiring = 1f;
    }

    [PunRPC]
    private void Die()
    {
        isDie = true;
        aniController.FaintedIdleAnimation(isDie);
        rb.velocity = Vector2.zero;
        rotatePoint.SetActive(false);
        hitBox.SetActive(false);
        StartCoroutine(RevivalTime());
    }

    private IEnumerator RevivalTime()
    {
        yield return new WaitForSeconds(10);
        isDie = false;
        aniController.FaintedIdleAnimation(isDie);
        rotatePoint.SetActive(true);
        hitBox.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    [PunRPC]
    public void TurnOnHealth()
    {
        slider.SetActive(true);
    }
}
