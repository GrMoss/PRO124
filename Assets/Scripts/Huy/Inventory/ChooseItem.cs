using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ChooseItem : MonoBehaviour
{
    [SerializeField] Sprite[] spriteItemChoose;
    [SerializeField] Image imageItem;
    [SerializeField] TMP_Text textItemQuatity;
    [SerializeField] Inventory_Manager inventory_Manager;
    private int id;
    private int quatity;
    private PhotonView view;
 
    private void FixedUpdate()
    {
       
        if (view.IsMine)
        {
            ChooseItemHand();
            textItemQuatity.text = inventory_Manager.GetQuantityItem(id).ToString();
        }
        

    }

    public void ItemID(int id, int quatity)
    {
        this.id = id;
        this.quatity = quatity;
    }

    public void ChooseItemHand()
    {
        imageItem.sprite = spriteItemChoose[id];
        //textItemQuatity.text = quatity.ToString();
    }
}
