using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager1 : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot1[] itemSlot1;
    private void Update()
    {
        if (Input.GetButtonDown("Inventory") && menuActivated)
        {
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }
        else if (Input.GetButtonDown("Inventory") && !menuActivated)
        {
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }


    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription )
    {
        for (int i = 0; i < itemSlot1.Length; i++)
        {
            if (itemSlot1[i].isFull == false && itemSlot1[i].name == name || itemSlot1[i].quantity == 0 )
            {
                int leftOverItems = itemSlot1[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                if (leftOverItems > 0)
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                return leftOverItems;
            }
        }
        return quantity;
    }

    public void DeselectAllSlots()
    {
        for(int i = 0;i < itemSlot1.Length;i++)
        {
            itemSlot1[i].selectedShader.SetActive(false);
            itemSlot1[i].thisItemSelected = false;
        }
    }
}
