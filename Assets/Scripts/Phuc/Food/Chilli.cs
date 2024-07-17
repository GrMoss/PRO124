using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chilli : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;
    [SerializeField] private int damageBleeding;

    public override void BadSpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartBadChilli", RpcTarget.All, 5f, damageBleeding);
        }
    }

    public override void GoodSpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartGoodChilli", RpcTarget.All, 5f, 5f, 0.5f);
        }
    }

    public override void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
        base.Start();
    }
}
