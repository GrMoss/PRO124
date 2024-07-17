using Cinemachine;
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
    private bool isConect = false;

    private PlayerAnimatorController aniController;
    private PlayerAudio audi;

    private float bleedingTimeRemaining = 0f;
    private bool isEgg = false;
    private bool isCarrot = false;
    private CinemachineVirtualCamera cam;
    private Image effectCarrot;

    private void Awake()
    {
        input = new InputSystem();
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        aniController = GetComponent<PlayerAnimatorController>();
        audi = FindObjectOfType<PlayerAudio>();
        if (view.IsMine)
        {
            cam = FindAnyObjectByType<CinemachineVirtualCamera>();
        }
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

            audi.PlayerRunning(Mathf.Abs(moveVector.x) > 0.1f || Mathf.Abs(moveVector.y) > 0.1f);
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

            if (GameObject.Find("EffectCarrot") != null && !isConect)
            {
                isConect = true;
                effectCarrot = GameObject.Find("EffectCarrot").GetComponent<Image>();
                Debug.Log("Conect");
            }
        }

        //TestDamage
        if (Input.GetKeyDown(KeyCode.J))
        {
            //hurt
            TakeDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //Fainted
            TakeDamage(100);
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
            if (health >= healthMax)
            {
                health = 0;
                view.RPC("Die", RpcTarget.AllBuffered);
                view.RPC("UpdateHealthSlider", RpcTarget.AllBuffered, health);
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
    private void Die()
    {
        isDie = true;
        aniController.RecoverAnimation(isDie);
        rb.velocity = Vector2.zero;
        rotatePoint.SetActive(false);
        hitBox.SetActive(false);
        StartCoroutine(RevivalTime());
    }

    private IEnumerator RevivalTime()
    {
        yield return new WaitForSeconds(10);
        isDie = false;
        aniController.RecoverAnimation(isDie);
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

    public void SetTransparency(float alpha255)
    {
        if (effectCarrot != null)
        {
            float alpha = alpha255 / 255f;

            Color color = effectCarrot.color;

            color.a = alpha;

            effectCarrot.color = color;
        }
        else
        {
            Debug.LogError("Image component is not assigned.");
        }
    }

    //----------------------- Effect -----------------------

    //----------------------- Carrot -----------------------

    [PunRPC]
    public void StartBadCarrot(float time, float blur)
    {
        StartCoroutine(BadCarrot(time, blur));
    }

    private IEnumerator BadCarrot(float time, float blur)
    {
        SetTransparency(blur);
        yield return new WaitForSeconds(time);
        SetTransparency(0);
    }

    [PunRPC]
    public void StartGoodCarrot(float time, float duration, float size)
    {
        if(!isCarrot)
        StartCoroutine(GoodCarrot(time, duration, size));
    }

    private IEnumerator GoodCarrot(float time, float duration, float size)
    {
        float startSize = 5f;
        float endSize = size;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, endSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cam.m_Lens.OrthographicSize = endSize;

        yield return new WaitForSeconds(time);

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(endSize, startSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cam.m_Lens.OrthographicSize = startSize;
    }

    //----------------------- Egg -----------------------

    [PunRPC]
    public void StartGoodEgg(float time, float moveSpeed, float scale)
    {
        if (!isEgg)
        StartCoroutine(GoodEgg(time, moveSpeed, scale));
    }

    private IEnumerator GoodEgg(float time, float moveSpeed, float scale)
    {
        isEgg = true;
        this.moveSpeed = moveSpeed;
        transform.localScale = new Vector3 (scale, scale, 1);
        yield return new WaitForSeconds(time);
        isEgg = false;
        this.moveSpeed = 4;
        transform.localScale = new Vector3(1, 1, 1);
    }

    [PunRPC]
    public void StartBadEgg(float time, float moveSpeed)
    {
        StartCoroutine(BadEgg(time, moveSpeed));
    }

    private IEnumerator BadEgg(float time, float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
        yield return new WaitForSeconds(time);
        this.moveSpeed = 4f;
    }

    //----------------------- Chilli -----------------------
    [PunRPC]
    public void StartGoodChilli(float time, float moveSpeed, float attackSpeed)
    {
        StartCoroutine(GoodChilli(time, moveSpeed, attackSpeed));
    }

    private IEnumerator GoodChilli(float time, float moveSpeed, float attackSpeed)
    {
        this.moveSpeed = moveSpeed;
        GetComponentInChildren<Shooting>().timeBetweenFiring = attackSpeed;
        yield return new WaitForSeconds(time);
        this.moveSpeed = 4;
        GetComponentInChildren<Shooting>().timeBetweenFiring = 1f;
    }

    [PunRPC]
    public void StartBadChilli(float time, int damage)
    {
        if (bleedingTimeRemaining > 0)
        {
            bleedingTimeRemaining = time;
        }
        else
        {
            StartCoroutine(BadChilli(time, damage));
        }
    }

    private IEnumerator BadChilli(float time, int damage)
    {
        bleedingTimeRemaining = time;
        while (bleedingTimeRemaining > 0)
        {
            yield return new WaitForSeconds(1);
            TakeDamage(damage);
            bleedingTimeRemaining -= 1f;
        }
    }
}
