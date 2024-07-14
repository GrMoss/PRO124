using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ItemSlot : MonoBehaviourPun
{
    public int itemID;
    private Inventory_Manager inventory_Manager;
    private Inventory_UI inventory_UI;
    private Shooting shooting;

    private void Start()
    {
        inventory_UI = FindObjectOfType<Inventory_UI>();
        inventory_Manager = FindObjectOfType<Inventory_Manager>();
        shooting = FindObjectOfType<Shooting>();

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
            shooting.indexChooseFood = itemID;
        }
    }
}
