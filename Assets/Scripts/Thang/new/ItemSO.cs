using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangesStat;

    public AttributeToChange attributeToChange = new AttributeToChange();
    public int amountToChangeAttrinute;


    public enum StatToChange
    {
        none,
        health,
        mana,
        stamina
    };
    public enum AttributeToChange
    {
        none,
        strength, 
        defense,
        intellegence,
        agility
    };
}
