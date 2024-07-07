using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private PhotonView view;
    private Slider health;
    private PlayerController player;

    private void Start()
    {
        view = GetComponentInParent<PhotonView>();
        health = GetComponent<Slider>();
        player = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            UpdateHealth();
            view.RPC("UpdateHealth", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void UpdateHealth()
    {
        health.value = player.health;
    }
}


