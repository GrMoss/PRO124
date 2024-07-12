using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseItem : MonoBehaviour
{
    [SerializeField] Sprite[] spriteItemChoose;
    [SerializeField] Image imageItem;
    [SerializeField] TMP_Text textItemQuatity;

    private int id;

 
    private void FixedUpdate()
    {
        ChooseItemHand();

    }

    public void ItemID(int id, int quatity)
    {
        this.id = id;
        textItemQuatity.text = quatity.ToString();
        Shooting.indexChooseFood = id;
    }

    public void ChooseItemHand()
    {
        imageItem.sprite = spriteItemChoose[id];
    }
}
