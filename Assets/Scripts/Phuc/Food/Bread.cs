using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;

    public override void BadSpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartBadBread", RpcTarget.All, 5f);
        }
    }

    public override void GoodSpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartGoodBread", RpcTarget.All, 10f);
        }
    }

    public override void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
        base.Start();
    }
}
