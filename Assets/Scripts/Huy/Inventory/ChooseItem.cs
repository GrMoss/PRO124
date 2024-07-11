using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseItem : MonoBehaviour
{
    [SerializeField] Image[] imageItemChoose;
    private int id;

    public void ItemID(int id)
    {
        this.id = id;
    }

    public void ChooseItemHand()
    {
        //item1.GetInstanceID
    }
}
