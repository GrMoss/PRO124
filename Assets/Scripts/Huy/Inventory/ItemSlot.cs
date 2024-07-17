using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class ItemSlot : MonoBehaviourPun
{
    public int itemID;
    public delegate void ItemSelectedHandler(int itemID);
    public event ItemSelectedHandler OnItemSelected;

    public void OnClick()
    {
        if (photonView.IsMine)
        {
            OnItemSelected?.Invoke(itemID);
        }
    }
}
