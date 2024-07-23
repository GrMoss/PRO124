using Cinemachine;
using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int health = 0;
    private int healthMax = 100;
    public Slider healthSlider;
    public GameObject slider;
    public GameObject sliderMain;
    public Slider healthSliderMain;
    public Image healthBackground;
    public Image healthFill;
    public TMP_Text name;

    private InputSystem input;
    [HideInInspector] public Vector2 moveVector = Vector2.zero;
    [HideInInspector] public Rigidbody2D rb;
    public float moveSpeed;
    public PhotonView view;
    public GameObject hitBox;
    public bool isDie = false;
    public GameObject rotatePoint;
    private bool isConect = false;
    public SpriteRenderer playerSpriteRenderer;
    private bool isTurnHealth = false;

    private PlayerAnimatorController aniController;
    private PlayerAudio audi;

    private float bleedingTimeRemaining = 0f;
    private bool isEgg = false;
    private bool isCarrot = false;
    private CinemachineVirtualCamera cam;
    private Image effectCarrot;
    private float eatingSpeed = 1f;
    private float effectCucumber = 1;
    private bool isBread = false;

    //Particle System
    private SpecialEffects spef;
    private void Awake()
    {
        input = new InputSystem();
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        aniController = GetComponent<PlayerAnimatorController>();
        audi = FindObjectOfType<PlayerAudio>();
        spef = GetComponentInChildren<SpecialEffects>();

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
            rb.velocity = moveVector * moveSpeed * eatingSpeed;

            aniController.RunAnimation(moveVector);

            audi.PlayerRunning(Mathf.Abs(moveVector.x) > 0.1f || Mathf.Abs(moveVector.y) > 0.1f);
        }
    }

    private void Update()
    {
        if (view.IsMine)
        {
            healthSliderMain.value = health;
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

            if (isTurnHealth)
            {
                isTurnHealth = false;
                sliderMain.SetActive(true);
            }

            healthBackground.enabled = false;
            healthFill.enabled = false;
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
        moveVector = value.ReadValue<Vector2>() * effectCucumber;
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
            if(!isBread)
            health += damage;
            view.RPC("UpdateHealthSlider", RpcTarget.AllBuffered, health);

            if (health >= healthMax)
            {
                if (audi.isEating)
                    audi.PlayerEat();

                health = 0;
                view.RPC("Die", RpcTarget.AllBuffered);
                view.RPC("UpdateHealthSlider", RpcTarget.AllBuffered, health);
                aniController.FaintedAnimation();
                audi.PlayerFainted();
            }
            else
            {
                if (!audi.isEating)
                {
                    view.RPC("PlayHurtAnimation", RpcTarget.AllBuffered);
                    audi.PlayerHurt();
                }
                else
                {
                    audi.PlayerEat();
                    aniController.EatAnimation();
                    StartCoroutine(WaitForEating());
                }
            }
        }
    }
    IEnumerator WaitForEating()
    {
        eatingSpeed = 0f;
        yield return new WaitForSeconds(.8f);
        eatingSpeed = 1f;
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
        yield return new WaitForSeconds(5);
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
        isTurnHealth = true;
    }

    [PunRPC]
    public void TurnOffHealth()
    {
        sliderMain.SetActive(false);
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

    //----------------------- Bread -----------------------
    [PunRPC]
    public void StartBadBread(float time)
    {
        StartCoroutine(BadBread(time));
        spef.HitBread();
    }

    private IEnumerator BadBread(float time)
    {
        GetComponentInChildren<Shooting>().SetCanFire(false);
        yield return new WaitForSeconds(time);
        GetComponentInChildren<Shooting>().SetCanFire(true);
    }

    [PunRPC]
    public void StartGoodBread(float time)
    {
        StartCoroutine(GoodBread(time));
        spef.EatBread();
    }

    private IEnumerator GoodBread(float time)
    {
        isBread = true;
        yield return new WaitForSeconds(time);
        isBread = false;
    }

    //----------------------- Cucumber -----------------------
    [PunRPC]
    public void StartBadCucumber(float time)
    {
        StartCoroutine(BadCucumber(time));
        spef.HitCumcumber();
    }

    private IEnumerator BadCucumber(float time)
    {
        effectCucumber = -1;
        yield return new WaitForSeconds(time);
        effectCucumber = 1;
    }

    [PunRPC]
    public void StartGoodCucumber(float time, float moveSpeed, float blur)
    {
        StartCoroutine(GoodCucumber(time, moveSpeed, blur));
        spef.EatCucumber();
    }

    private IEnumerator GoodCucumber(float time,float moveSpeed, float blur)
    {
        Color color;
        this.moveSpeed = moveSpeed;
        if(view.IsMine)
        {
            color = playerSpriteRenderer.color;
            color.a = 150 / 255f;
            playerSpriteRenderer.color = color;
        }
        else
        {
            name.enabled = false;
            healthBackground.enabled = false;
            healthFill.enabled = false;
            color = playerSpriteRenderer.color;
            color.a = blur / 255f;
            playerSpriteRenderer.color = color;
        }
        yield return new WaitForSeconds(time);
        this.moveSpeed = 4;
        name.enabled = true;
        healthBackground.enabled = true;
        healthFill.enabled = true;
        color = playerSpriteRenderer.color;
        color.a = 1;
        playerSpriteRenderer.color = color;
    }

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
        spef.EatCarrot();
    }

    private IEnumerator GoodCarrot(float time, float duration, float size)
    {
        float startSize = 5f;
        float endSize = size;
        float elapsedTime = 0f;
        isCarrot = true;

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
        isCarrot = false;
    }

    //----------------------- Egg -----------------------
    [PunRPC]
    public void StartEggEffect(float time, float moveSpeed, float scale)
    {
        if (!isEgg)
        StartCoroutine(EggEffect(time, moveSpeed, scale));
        if (scale <= 0.5f)
        {
            spef.EatEgg();
        }
        else
        {
            spef.HitEgg();
        }

    }

    private IEnumerator EggEffect(float time, float moveSpeed, float scale)
    {
        isEgg = true;
        this.moveSpeed = moveSpeed;
        transform.localScale = new Vector3 (scale, scale, 1);
        yield return new WaitForSeconds(time);
        isEgg = false;
        this.moveSpeed = 4;
        transform.localScale = new Vector3(1, 1, 1);
    }

    //----------------------- Chilli -----------------------
    [PunRPC]
    public void StartGoodChilli(float time, float moveSpeed, float attackSpeed)
    {
        StartCoroutine(GoodChilli(time, moveSpeed, attackSpeed));

        if (attackSpeed < 1f)
            spef.EatPepper();
        else
            spef.HitPepper();
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
