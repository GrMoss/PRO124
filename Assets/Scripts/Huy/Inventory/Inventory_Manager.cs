
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Inventory_Manager : MonoBehaviour
{
    [SerializeField] Sprite[] spritesItem;
    private PhotonView view;
    private List<InventoryData> lisInventoryDatas = new List<InventoryData>();

    // Các biến khác đã được định nghĩa ở trước
    private TMP_Text debugLogText; // Biến để lưu trữ TMP_Text

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        // Tìm đối tượng chứa TMP_Text có tên "DebugLog" trong Scene
        GameObject debugLogObject = GameObject.Find("DebugLog");

        if (debugLogObject != null)
        {
            debugLogText = debugLogObject.GetComponent<TMP_Text>();

            if (debugLogText == null)
            {
                Debug.LogError("Không tìm thấy TMP_Text component trên đối tượng có tên 'DebugLog'.");
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy đối tượng có tên 'DebugLog' trong Scene.");
        }

        KhoiTao();
    }

    public void KhoiTao()
    {
        if (view != null && view.IsMine)
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
    }

    // Thêm phương thức để trả về danh sách các mục trong kho
    public List<InventoryData> GetInventoryItems()
    {
        return lisInventoryDatas;
    }

    public void AddItemInList(int id, int quantity)
    {
        if (view != null && view.Owner != null && view.Owner.IsLocal)
        {
            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
            if (item != null)
            {
                item.QuantityItem += quantity;
                view.RPC("SyncAddItemInList", RpcTarget.Others, id, quantity);
            }
        }
    }

    public void QuitItemInList(int id, int quantity)
    {
        if (view != null && view.Owner != null && view.Owner.IsLocal)
        {
            var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
            if (item != null)
            {
                item.QuantityItem -= quantity;
                view.RPC("SyncQuitItemInList", RpcTarget.Others, id, quantity);
            }
        }
    }


    [PunRPC]
    private void SyncAddItemInList(int id, int quantity)
    {
        var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
        if (item != null)
        {
            item.QuantityItem += quantity;
        }
    }

    [PunRPC]
    private void SyncQuitItemInList(int id, int quantity)
    {
        var item = lisInventoryDatas.SingleOrDefault(x => x.ItemID == id);
        if (item != null)
        {
            item.QuantityItem -= quantity;
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
        else
        {
            Debug.LogWarning("Không phải kho của người chơi này hoặc view chưa được khởi tạo.");
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
