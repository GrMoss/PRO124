using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : MonoBehaviour
{
    public float speedFly {  get; set; }
    public float damage { get; set; }

    public abstract void SpecialEffects();
}
