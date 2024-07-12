using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Inventory_Manager : MonoBehaviour
{
    private PhotonView view;
    private List<InventoryData> lisInventoryDatas = new List<InventoryData>();

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        KhoiTao();
    }

    public void KhoiTao()
    {
        if (view != null && view.IsMine)
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
        if (view != null && view.IsMine)
        {
            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
            if (item != null) item.QuantityItem += quantity;
            // Tùy chọn, đồng bộ hóa thay đổi này với các client khác
            //view.RPC("DongBoThemVatPham", RpcTarget.Others, id, soLuong);
        }
    }

    public void QuitItemInList(int id, int quantity)
    {
        if (view != null && view.IsMine)
        {
            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
            if (item != null) item.QuantityItem -= quantity;
            // Tùy chọn, đồng bộ hóa thay đổi này với các client khác
            // view.RPC("DongBoXoaVatPham", RpcTarget.Others, id, soLuong);
        }
    }

    public int GetQuatityItem(int id)
    {
        if (view != null && view.IsMine)
        {
            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
            return item != null ? item.QuantityItem : 0;
        }
        return 0;
    }

    public void ShowAllInventoryData()
    {
        if (view != null && view.IsMine)
        {
            foreach (var item in lisInventoryDatas)
            {
                Debug.Log($"ID: {item.ItemID} | Name: {item.ItemName} | Quality: {item.QuantityItem}");
            }
        }
        else
        {
            Debug.LogWarning("Không phải kho của người chơi này hoặc view chưa được khởi tạo.");
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

[System.Serializable]
public class InventoryData
{
    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public int QuantityItem { get; set; }
    public Sprite ImageItem { get; set; }

    public InventoryData(int itemID, string itemName, int quantityItem, Sprite imageItem)
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

    public InventoryData(int itemID, Sprite imageItem)
    {
        ItemID = itemID;
        ImageItem = imageItem;
    }

    public InventoryData(int itemID, int quantity)
    {
        ItemID = itemID;
        QuantityItem = quantity;
    }

    public InventoryData() { }

}
