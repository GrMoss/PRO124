using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;


public class Inventory_Manager : MonoBehaviour
{
    public static List<InventoryData> lisInventoryDatas = new List<InventoryData>
    {
        new InventoryData(0, "Null" , 0),
        new InventoryData(1, "Chilli", 0),
        new InventoryData(2, "Carrot", 0),
        new InventoryData(3, "Cucumber", 0),
        new InventoryData(4, "Egg", 0),
        new InventoryData(5, "Omelet", 0),
        new InventoryData(6, "Bread", 0),

    };

    public void AddItemInList(int id, int quantity)
    {
        var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
        if (item != null) item.QuantityItem += quantity;
    }

    public void QuitItemInList(int id, int quantity)
    {
        var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
        if (item != null) item.QuantityItem -= quantity;
    }

    public int GetQuatityItem(int id)
    {
        var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
        
        return item.QuantityItem;
    }

    public void ShowAllInventoryData()
    {
        var showALL = lisInventoryDatas.ToList();
        foreach (var item in showALL)
        {
            Debug.Log($"ID: {item.ItemID} | Name: {item.ItemName} | Quality: {item.QuantityItem}" );
        }
    }
}
