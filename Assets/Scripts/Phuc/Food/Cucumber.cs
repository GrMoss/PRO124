using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;

    public override void BadSpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartBadCucumber", RpcTarget.All, 5f);
        }
    }
    public override void GoodSpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartGoodCucumber", RpcTarget.All, 5f, 4f, 0f);
        }
    }

    public override void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
        base.Start();
    }
}
