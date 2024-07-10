
using Photon.Pun;
using UnityEngine;

public abstract class Food : MonoBehaviour
{
    public float speedFly { get; set; }
    public int damage { get; set; }
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rig;
    private Vector3 startPosition;
    private float maxDistance = 9;
    private PhotonView view;
    public int ownerId;
    public PhotonView targetPhotonView = null;
    public PlayerController playerController;

    public abstract void SpecialEffects();

    public virtual void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rig = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        rig.velocity = new Vector2(direction.x, direction.y).normalized * speedFly;
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);

        startPosition = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        view = GetComponent<PhotonView>();
    }

    public virtual void Update()
    {
        if (view != null && view.IsMine)
        {
            if (Vector3.Distance(startPosition, transform.position) > maxDistance)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHitBox"))
        {
            targetPhotonView = collision.gameObject.GetComponentInParent<PhotonView>();
            playerController = collision.gameObject.GetComponentInParent<PlayerController>();

            if (targetPhotonView != null && playerController != null)
            {
                Debug.Log($"Collision detected: targetPhotonView.ViewID = {targetPhotonView.ViewID}, ownerId = {ownerId}");

                if (playerController.view != null && playerController.view.ViewID != ownerId && ownerId != 0)
                {
                    Debug.Log("Damage is being applied");
                    targetPhotonView.RPC("TakeDamage", RpcTarget.All, damage);
                    SpecialEffects();
                    if (view.IsMine)
                    {
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
        }
    }
}
