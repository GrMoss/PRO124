using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omelet : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;

    public override void BadSpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartEggEffect", RpcTarget.All, 5f, 1f, 2f);
        }
    }

    public override void GoodSpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartEggEffect", RpcTarget.All, 5f, 9f, 0.3f);
        }
    }

    public override void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
        base.Start();
    }
}
