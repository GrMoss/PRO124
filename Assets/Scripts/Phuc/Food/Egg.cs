using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;

    public override void BadSpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartEggEffect", RpcTarget.All, 5f, 2f, 1.5f);
        }
    }

    public override void GoodSpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartEggEffect", RpcTarget.All, 5f, 7f, 0.5f);
        }
    }

    public override void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
        base.Start();
    }
}
