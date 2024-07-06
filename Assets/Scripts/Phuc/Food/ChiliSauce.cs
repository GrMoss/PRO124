using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiliSauce : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;

    public override void SpecialEffects()
    {

    }

    public override void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
        base.Start();
    }
}
