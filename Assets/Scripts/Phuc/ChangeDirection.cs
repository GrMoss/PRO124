using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirection : MonoBehaviour
{
    public GameObject player;
    private SpriteRenderer sprite;
    private PhotonView view;
    private Shooting shooting;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        view = GetComponentInParent<PhotonView>();
        shooting = GetComponentInParent<Shooting>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            sprite.enabled = true;
            if (transform.position.x <= player.transform.position.x)
            {
                transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z);
                shooting.directionY = -1;
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
                shooting.directionY = 1;
            }
        }
        else
        {
            sprite.enabled = false;
        }
    }
}
