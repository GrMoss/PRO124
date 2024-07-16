using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omelet : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;

    public override void SpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartSlow", RpcTarget.All, 3f);
        }
    }

    public override void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
        base.Start();
    }
}
