using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Inventory_Bar : MonoBehaviourPun
{
    public GameObject inventoryBar;
    public GameObject[] slot;
    private Inventory_Manager inventory_Manager;
    private int indexShooting;
    private LobbyManager lobbyManager;

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
        inventoryBar.SetActive(false);
        inventory_Manager = GetComponentInParent<Inventory_Manager>();

        if (inventory_Manager == null)
        {
            inventory_Manager = FindObjectOfType<Inventory_Manager>();
        }
    }

    private void FixedUpdate()
    {
        inventoryBar.SetActive(lobbyManager.offLobby);
    }


    public void UpdateInventoryBar()
    {
        var inventoryItems = inventory_Manager.GetInventoryItems();

        for (int i = 0; i < slot.Length; i++)
        {
            if (i < inventoryItems.Count && inventoryItems[i].QuantityItem > 0)
            {
                var item = inventoryItems[i];
                ItemSlot itemSlotComponent = slot[i].GetComponent<ItemSlot>();

                if (itemSlotComponent != null)
                {
                    itemSlotComponent.itemID = item.ItemID;
                    itemSlotComponent.OnItemSelected += HandleItemSelected;
                }else
                { 
                    Debug.LogError("Không tìm thấy thành phần ItemSlot trên slot.");
                }

                TMP_Text quantityText = slot[i].transform.Find("Quantity").GetComponent<TMP_Text>();
                Image itemImage = slot[i].transform.Find("ItemImage").GetComponent<Image>();
                if (quantityText != null) 
                { 
                    quantityText.text = item.QuantityItem.ToString();
                }else
                {
                    Debug.LogError("Không tìm thấy thành phần TMP_Text cho Quantity.");
                }
                
                if (itemImage != null)
                { 
                    itemImage.sprite = item.ImageItem;
                }
                else
                {
                    Debug.LogError("Không tìm thấy thành phần Image cho ItemImage.");
                }

                slot[i].SetActive(true);
            }
            else
            {
                slot[i].SetActive(false);
            }
        }
    }

    private void HandleItemSelected(int itemID)
    {
        indexShooting = itemID;
        Debug.Log("Item Selected with ID: " + itemID);
    }

    public int GetIndexShooting()
    {
        return indexShooting;
    }

}
