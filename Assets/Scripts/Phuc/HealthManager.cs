using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviourPun
{
    private SpriteRenderer sprite;
    public PlayerController player;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(photonView.IsMine)
        sprite.size = new Vector2(player.health, 1);
    }
}
