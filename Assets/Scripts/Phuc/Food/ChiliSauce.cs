using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiliCauce : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;
    [SerializeField] private int damageBleeding;

    public override void BadSpecialEffects()
    {
        targetPhotonView.RPC("StartBadChilli", RpcTarget.All, 5f, damageBleeding);
    }

    public override void GoodSpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartGoodChilli", RpcTarget.All, 5f, 6f, 0.3f);
        }
    }

    public override void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
        base.Start();
    }
}
