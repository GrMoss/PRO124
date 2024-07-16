using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChilliSauce : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;
    [SerializeField] private int damageBleeding;

    public override void SpecialEffects()
    {
        if (playerController != null)
        {
            targetPhotonView.RPC("StartBleeding", RpcTarget.All, damageBleeding, 5f);
        }
    }

    public override void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
        base.Start();
    }
}
