using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryData
{
  
    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public int QuantityItem { get; set; }
    public Image ImageItem { get; set; }

    public InventoryData(int itemID, string itemName, int quantityItem, Image imageItem)
    {
        ItemID = itemID;
        ItemName = itemName;
        QuantityItem = quantityItem;
        ImageItem = imageItem;
    }

    public InventoryData(int itemID, string itemName, int quantity)
    {
        ItemID = itemID;
        ItemName = itemName;
        QuantityItem = quantity;
    }

    public InventoryData(int itemID, Image imageItem)
    {
        ItemID = itemID;
        ImageItem = imageItem;
    }

    public InventoryData(int itemID, int quantity)
    {
        ItemID = itemID;
        QuantityItem = quantity;
    }

    public InventoryData() {}

    
}
