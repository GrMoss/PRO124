using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int health = 0;
    private int healthMax = 100;
    public Slider healthSlider;

    private InputSystem input;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb;
    public float moveSpeed;
    public PhotonView view;
    public GameObject hitBox;
    public bool isDie = false;
    public GameObject rotatePoint;

    private PlayerAnimatorController aniController;
    //private HealthManager healthManager;

    private void Awake()
    {
        input = new InputSystem();
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        aniController = GetComponent<PlayerAnimatorController>();
        //healthManager = GetComponentInChildren<HealthManager>();
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
            //Call Animation
            aniController.RunAnimation(moveVector);
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
            //healthManager.UpdateHealthSlider();
            view.RPC("UpdateHealthSlider", RpcTarget.AllBuffered);
            if (health >= healthMax)
            {
                health = 0;
                view.RPC("Die", RpcTarget.AllBuffered);
                view.RPC("UpdateHealthSlider", RpcTarget.AllBuffered, health);
                //healthManager.UpdateHealthSlider();
                view.RPC("UpdateHealthSlider", RpcTarget.AllBuffered);
                aniController.FaintedAnimation();
            }
            else
            {
                //aniController.HurtAnimation();
                view.RPC("PlayHurtAnimation", RpcTarget.AllBuffered);
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
    public void StartBleeding(int damage, float time)
    {
        StartCoroutine(Bleeding(damage, time));
    }

    private IEnumerator Bleeding(int damage, float time)
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(time);
            if (view.IsMine)
            {
                TakeDamage(damage);
            }
            Debug.Log(i + " Bleeding");
        }
    }

    [PunRPC]
    private void Die()
    {
        isDie = true;
        //Call Animation
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
        //Call Animation
        aniController.FaintedIdleAnimation(isDie);
        rotatePoint.SetActive(true);
        hitBox.SetActive(true);
    }
}
