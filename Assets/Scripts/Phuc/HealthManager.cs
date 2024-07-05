using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private PhotonView view;
    private SpriteRenderer sprite;

    private void Start()
    {
        view = GetComponentInParent<PhotonView>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(view.IsMine)
        sprite.size = new Vector2(GetComponentInParent<PlayerController>().health, 1);
    }
}
