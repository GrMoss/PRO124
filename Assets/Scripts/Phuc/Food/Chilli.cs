using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chilli : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;
    [SerializeField] private int damageBleeding;

    public override void SpecialEffects()
    {
        StartCoroutine(Bleeding());
    }

    public override void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
        base.Start();
    }

    private IEnumerator Bleeding()
    {
        if (targetPhotonView != null)
        {
            Debug.Log("oke");
            yield return new WaitForSeconds(1);
            targetPhotonView.RPC("TakeDamage", RpcTarget.All, damageBleeding);
            Debug.Log("oke");
            yield return new WaitForSeconds(1);
            targetPhotonView.RPC("TakeDamage", RpcTarget.All, damageBleeding);
            Debug.Log("oke");
            yield return new WaitForSeconds(1);
            targetPhotonView.RPC("TakeDamage", RpcTarget.All, damageBleeding);
            Debug.Log("oke");
            yield return new WaitForSeconds(1);
            targetPhotonView.RPC("TakeDamage", RpcTarget.All, damageBleeding);
            Debug.Log("oke");
            yield return new WaitForSeconds(1);
            targetPhotonView.RPC("TakeDamage", RpcTarget.All, damageBleeding);

        }
    }
}
