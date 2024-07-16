using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ItemSlot : MonoBehaviourPun
{
    public int itemID;
    public bool indexShooting;
    private Inventory_Manager inventory_Manager;
    private Inventory_UI inventory_UI;

    public delegate void ItemSelectedHandler(int itemID);
    public event ItemSelectedHandler OnItemSelected;

    private void Start()
    {
        inventory_UI = FindObjectOfType<Inventory_UI>();
        inventory_Manager = FindObjectOfType<Inventory_Manager>();

        if (photonView.IsMine)
        {
            if (inventory_Manager != null)
            {
                Debug.Log("Đã gán Inventory_Manager cho ItemPrefab.");
            }
            else
            {
                Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng này hoặc trong scene.");
            }

            if (inventory_UI != null)
            {
                Debug.Log("Đã gán inventory_UI cho ItemPrefab.");
            }
            else
            {
                Debug.LogError("Không tìm thấy inventory_UI trên đối tượng này hoặc trong scene.");
            }
        }
    }

    public void PressButton()
    {
        if (photonView.IsMine)
        {
            indexShooting = true;
            OnItemSelected?.Invoke(itemID);
        }
    }
}
