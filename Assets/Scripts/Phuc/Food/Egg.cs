using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Food
{
    [SerializeField] private float speedFly;
    [SerializeField] private int damage;

    public override void SpecialEffects()
    {
        
    }

    private void Start()
    {
        base.speedFly = speedFly;
        base.damage = damage;
    }
}
