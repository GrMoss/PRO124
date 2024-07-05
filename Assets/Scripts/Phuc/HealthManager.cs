using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private PhotonView view;
    public GameObject player;
    private SpriteRenderer sprite;

    private void Start()
    {
        view = player.GetComponent<PhotonView>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(view.IsMine)
        sprite.size = new Vector2(player.GetComponent<PlayerController>().health, 1);
    }
}
