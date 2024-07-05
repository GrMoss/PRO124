using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : MonoBehaviour
{
    private int idPlayer;
    public float speedFly { get; set; }
    public int damage { get; set; }
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rig;
    private Vector3 startPosition;
    private float maxDistance = 7;
    private PhotonView view;
    public GameObject player;

    public abstract void SpecialEffects();

    public virtual void Start()
    {
        idPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().id;
        view = GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonView>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rig = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rig.velocity = new Vector2(direction.x, direction.y).normalized * speedFly;
        float rot = Mathf.Atan2(rotation.x, rotation.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);

        startPosition = transform.position;
    }

    public virtual void Update()
    {
        if (view.IsMine)
        {
            if (Vector3.Distance(startPosition, transform.position) > maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerHitBox") && collision.gameObject.GetComponent<PlayerController>().id != idPlayer)
        {
            if(collision.gameObject.GetComponentInParent<PlayerController>() != null)
            collision.gameObject.GetComponentInParent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
