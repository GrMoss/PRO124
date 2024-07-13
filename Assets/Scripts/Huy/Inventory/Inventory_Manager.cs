using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Inventory_Manager : MonoBehaviourPun
{
    [SerializeField] Sprite[] spritesItem;
    private List<InventoryData> lisInventoryDatas = new List<InventoryData>();
    public TMP_Text debugLogText; // Biến để lưu trữ TMP_Text

    private void Start()
    {
        KhoiTao(); // Khởi tạo kho hàng
    }

    public void KhoiTao()
    {
        lisInventoryDatas.Add(new InventoryData(0, "Null", 0, spritesItem[0]));
        lisInventoryDatas.Add(new InventoryData(1, "Chilli", 0, spritesItem[1]));
        lisInventoryDatas.Add(new InventoryData(2, "Carrot", 0, spritesItem[2]));
        lisInventoryDatas.Add(new InventoryData(3, "Cucumber", 0, spritesItem[3]));
        lisInventoryDatas.Add(new InventoryData(4, "Egg", 0, spritesItem[4]));
        lisInventoryDatas.Add(new InventoryData(5, "Omelet", 0, spritesItem[5]));
        lisInventoryDatas.Add(new InventoryData(6, "Bread", 0, spritesItem[6]));
        lisInventoryDatas.Add(new InventoryData(7, "ChinSo", 0, spritesItem[7]));
    }

    // Thêm phương thức để trả về danh sách các mục trong kho
    public List<InventoryData> GetInventoryItems()
    {
        return lisInventoryDatas;
    }

    public void AddItemInList(int id, int quantity)
    {
        // Chỉ thực hiện thêm vật phẩm nếu đúng là kho hàng của người chơi hiện tại
        if (photonView.IsMine)
        {
            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
            if (item != null)
            {
                item.QuantityItem += quantity;
                // Gọi RPC để đồng bộ hóa thay đổi với các người chơi khác
                photonView.RPC("SyncAddItemInList", RpcTarget.Others, id, quantity);
                // Cập nhật giao diện hiển thị
                ShowItemInInventory();
            }
        }
    }

    public int GetQuantityItem(int id)
    {
        // Lấy số lượng của vật phẩm theo ID
        if (photonView.IsMine)
        {
            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
            return item != null ? item.QuantityItem : 0;
        }
        return 0;
    }

    [PunRPC]
    private void SyncAddItemInList(int id, int quantity)
    {
        var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
        if (item != null)
        {
            item.QuantityItem += quantity;
            // Cập nhật giao diện hiển thị
            ShowItemInInventory();
        }
    }

    public void QuitItemInList(int id, int quantity)
    {
        // Chỉ thực hiện xoá vật phẩm nếu đúng là kho hàng của người chơi hiện tại
        if (photonView.IsMine)
        {
            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
            if (item != null)
            {
                item.QuantityItem -= quantity;
                // Gọi RPC để đồng bộ hóa thay đổi với các người chơi khác
                photonView.RPC("SyncQuitItemInList", RpcTarget.Others, id, quantity);
                // Cập nhật giao diện hiển thị
                ShowItemInInventory();
            }
        }
    }

    [PunRPC]
    private void SyncQuitItemInList(int id, int quantity)
    {
        var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
        if (item != null)
        {
            item.QuantityItem -= quantity;
            // Cập nhật giao diện hiển thị
            ShowItemInInventory();
        }
    }

    public void ShowItemInInventory()
    {
        // Xây dựng chuỗi để lưu trữ thông tin kho hàng
        string inventoryInfo = "";
        foreach (var item in lisInventoryDatas)
        {
            inventoryInfo += $"ID: {item.ItemID} | Name: {item.ItemName} | Quantity: {item.QuantityItem}\n";
            Debug.Log($"ID: {item.ItemID} | Name: {item.ItemName} | Quality: {item.QuantityItem}");
        }

        // Gán chuỗi này vào TMP_Text để hiển thị
        if (debugLogText != null)
        {
            debugLogText.text = inventoryInfo;
        }
    }
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
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using Photon.Pun;
//using TMPro;

//public class Inventory_Manager : MonoBehaviourPun
//{
//    [SerializeField] Sprite[] spritesItem;
//    [SerializeField] string[] itemInformation;
//    private List<InventoryData> lisInventoryDatas = new List<InventoryData>();
//    public TMP_Text debugLogText; // Biến để lưu trữ TMP_Text

//    private void Start()
//    {
//        KhoiTao(); // Khởi tạo kho hàng
//    }

//    public void KhoiTao()
//    {
//        lisInventoryDatas.Add(new InventoryData(0, "Null", itemInformation[0], 0, spritesItem[0]));
//        lisInventoryDatas.Add(new InventoryData(1, "Chilli", itemInformation[1], 0, spritesItem[1]));
//        lisInventoryDatas.Add(new InventoryData(2, "Carrot", itemInformation[2], 0, spritesItem[2]));
//        lisInventoryDatas.Add(new InventoryData(3, "Cucumber", itemInformation[3], 0, spritesItem[3]));
//        lisInventoryDatas.Add(new InventoryData(4, "Egg", itemInformation[4], 0, spritesItem[4]));
//        lisInventoryDatas.Add(new InventoryData(5, "Omelet", itemInformation[5], 0, spritesItem[5]));
//        lisInventoryDatas.Add(new InventoryData(6, "Bread", itemInformation[6], 0, spritesItem[6]));
//        lisInventoryDatas.Add(new InventoryData(7, "ChinSo", itemInformation[7], 0, spritesItem[7]));
//    }

//    // Thêm phương thức để trả về danh sách các mục trong kho
//    public List<InventoryData> GetInventoryItems()
//    {
//        return lisInventoryDatas;
//    }

//    public void AddItemInList(int id, int quantity)
//    {
//        // Chỉ thực hiện thêm vật phẩm nếu đúng là kho hàng của người chơi hiện tại
//        if (photonView.IsMine)
//        {
//            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
//            if (item != null)
//            {
//                item.QuantityItem += quantity;
//                // Gọi RPC để đồng bộ hóa thay đổi với các người chơi khác
//                photonView.RPC("SyncAddItemInList", RpcTarget.Others, id, quantity);
//                // Cập nhật giao diện hiển thị
//                ShowItemInInventory();
//            }
//        }
//    }

//    public int GetQuantityItem(int id)
//    {
//        // Lấy số lượng của vật phẩm theo ID
//        if (photonView.IsMine)
//        {
//            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
//            return item != null ? item.QuantityItem : 0;
//        }
//        return 0;
//    }

//    [PunRPC]
//    private void SyncAddItemInList(int id, int quantity)
//    {
//        var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
//        if (item != null)
//        {
//            item.QuantityItem += quantity;
//            // Cập nhật giao diện hiển thị
//            ShowItemInInventory();
//        }
//    }

//    public void QuitItemInList(int id, int quantity)
//    {
//        // Chỉ thực hiện xoá vật phẩm nếu đúng là kho hàng của người chơi hiện tại
//        if (photonView.IsMine)
//        {
//            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
//            if (item != null)
//            {
//                item.QuantityItem -= quantity;
//                // Gọi RPC để đồng bộ hóa thay đổi với các người chơi khác
//                photonView.RPC("SyncQuitItemInList", RpcTarget.Others, id, quantity);
//                // Cập nhật giao diện hiển thị
//                ShowItemInInventory();
//            }
//        }
//    }

//    [PunRPC]
//    private void SyncQuitItemInList(int id, int quantity)
//    {
//        var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
//        if (item != null)
//        {
//            item.QuantityItem -= quantity;
//            // Cập nhật giao diện hiển thị
//            ShowItemInInventory();
//        }
//    }

//    public void RemoveItemFromList(int id)
//    {
//        // Chỉ thực hiện xóa vật phẩm nếu đúng là kho hàng của người chơi hiện tại
//        if (photonView.IsMine)
//        {
//            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
//            if (item != null)
//            {
//                lisInventoryDatas.Remove(item);
//                // Gọi RPC để đồng bộ hóa thay đổi với các người chơi khác
//                photonView.RPC("SyncRemoveItemFromList", RpcTarget.Others, id);
//                // Cập nhật giao diện hiển thị
//                ShowItemInInventory();
//            }
//        }
//    }

//    [PunRPC]
//    private void SyncRemoveItemFromList(int id)
//    {
//        var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
//        if (item != null)
//        {
//            lisInventoryDatas.Remove(item);
//            // Cập nhật giao diện hiển thị
//            ShowItemInInventory();
//        }
//    }

//    public void ShowItemInInventory()
//    {
//        // Xây dựng chuỗi để lưu trữ thông tin kho hàng
//        string inventoryInfo = "";
//        foreach (var item in lisInventoryDatas)
//        {
//            inventoryInfo += $"ID: {item.ItemID} | Name: {item.ItemName} | Quantity: {item.QuantityItem}\n";
//            Debug.Log($"ID: {item.ItemID} | Name: {item.ItemName} | Quality: {item.QuantityItem}");
//        }

//        // Gán chuỗi này vào TMP_Text để hiển thị
//        if (debugLogText != null)
//        {
//            debugLogText.text = inventoryInfo;
//        }
//    }
//}

//[System.Serializable]
//public class InventoryData
//{
//    public int ItemID { get; set; }
//    public string ItemName { get; set; }
//    public string ItemInformation { get; set; }
//    public int QuantityItem { get; set; }
//    public Sprite ImageItem { get; set; }

//    public InventoryData(int itemID, string itemName, string itemInformation, int quantityItem, Sprite imageItem)
//    {
//        ItemID = itemID;
//        ItemName = itemName;
//        ItemInformation = itemInformation;
//        QuantityItem = quantityItem;
//        ImageItem = imageItem;
//    }

//    public InventoryData(int itemID, string itemName, int quantity)
//    {
//        ItemID = itemID;
//        ItemName = itemName;
//        QuantityItem = quantity;
//    }

//    public InventoryData(int itemID, Sprite imageItem)
//    {
//        ItemID = itemID;
//        ImageItem = imageItem;
//    }

//    public InventoryData(int itemID, int quantity)
//    {
//        ItemID = itemID;
//        QuantityItem = quantity;
//    }

//    public InventoryData() { }
//}
