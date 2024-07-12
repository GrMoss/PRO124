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
    PhotonView view;
    List<InventoryData> lisInventoryDatas = new List<InventoryData>();

    private void Start()
    {
        KhoiTao();
    }

    public void KhoiTao()
    {
        if (view.IsMine)
        {
            lisInventoryDatas.Add(new InventoryData(0, "Null", 0));
            lisInventoryDatas.Add(new InventoryData(1, "Chilli", 0));
            lisInventoryDatas.Add(new InventoryData(3, "Cucumber", 0));
            lisInventoryDatas.Add(new InventoryData(4, "Egg", 0));
            lisInventoryDatas.Add(new InventoryData(5, "Omelet", 0));
            lisInventoryDatas.Add(new InventoryData(6, "Bread", 0));
            lisInventoryDatas.Add(new InventoryData(7, "ChinSo", 0));
        }

    }

    public void AddItemInList(int id, int quantity)
    {
        if (view.IsMine)
        {
            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
            if (item != null) item.QuantityItem += quantity;
            // Tùy chọn, đồng bộ hóa thay đổi này với các client khác
            //view.RPC("DongBoThemVatPham", RpcTarget.Others, id, soLuong);
        }
    }

    public void QuitItemInList(int id, int quantity)
    {
        if (view.IsMine)
        {
            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
            if (item != null) item.QuantityItem -= quantity;
            // Tùy chọn, đồng bộ hóa thay đổi này với các client khác
            // view.RPC("DongBoXoaVatPham", RpcTarget.Others, id, soLuong);
        }
    
    }

    public int GetQuatityItem(int id)
    {
        if (view.IsMine)
        {
            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);

            return item.QuantityItem;
        }
        return 0; 
    }

    public void ShowAllInventoryData()
    {
        if (view.IsMine)
        {
            var showALL = lisInventoryDatas.ToList();
            foreach (var item in showALL)
            {
                Debug.Log($"ID: {item.ItemID} | Name: {item.ItemName} | Quality: {item.QuantityItem}");
            }
        }
    
    }

    // Ví dụ về các phương thức RPC để đồng bộ hóa (tùy chọn)
    /*
    [PunRPC]
    private void DongBoThemVatPham(int id, int soLuong)
    {
        var vatPham = danhSachDuLieuKho.SingleOrDefault(x => x.ItemID == id);
        if (vatPham != null) vatPham.QuantityItem += soLuong;
    }

    [PunRPC]
    private void DongBoXoaVatPham(int id, int soLuong)
    {
        var vatPham = danhSachDuLieuKho.SingleOrDefault(x => x.ItemID == id);
        if (vatPham != null) vatPham.QuantityItem -= soLuong;
    }
    */
}
