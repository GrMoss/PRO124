using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "inventory/item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int count = 0;
    public Sprite image;
}
