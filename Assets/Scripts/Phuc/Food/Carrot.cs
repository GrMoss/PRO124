using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;

    public override void BadSpecialEffects()
    {
        throw new System.NotImplementedException();
    }

    public override void GoodSpecialEffects()
    {

    }

    public override void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
        base.Start();
    }
}
