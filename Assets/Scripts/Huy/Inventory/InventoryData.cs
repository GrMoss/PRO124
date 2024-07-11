using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
  
    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public int QuantityItem { get; set; }
    public GameObject ItemObject { get; set; }

    public InventoryData(int itemID, string itemName, int quantity, GameObject itemObject)
    {
        ItemID = itemID;
        ItemName = itemName;
        QuantityItem = quantity;
        ItemObject = itemObject;
    }

    public InventoryData(int itemID, string itemName, int quantity)
    {
        ItemID = itemID;
        ItemName = itemName;
        QuantityItem = quantity;
    }

    public InventoryData(int itemID, int quantity)
    {
        ItemID = itemID;
        QuantityItem = quantity;
    }

    public InventoryData() {}
}
