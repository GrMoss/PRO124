using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<Item> items = new List<Item>();
    public Transform itemHolder;
    public GameObject itemPrefab;

    private Dictionary<string, int> itemCounts = new Dictionary<string, int>(); // Dictionary để lưu trữ số lượng vật phẩm theo tên
    public Toggle enableRemoveItem;

    private float itemHeightOffset = 0.1f; // Độ cao mỗi vật phẩm so với vật phẩm trước đó
    private float currentYPosition = 0f; // Vị trí Y hiện tại để đặt vật phẩm mới

    public TextMeshProUGUI txtPoint; // Tham chiếu đến TextMeshPro để hiển thị số lượng vật phẩm
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // Destroy current instance if not the first one
        }

        Instance = this;
    }

    public void Add(Item item)
    {
        items.Add(item);

        if (itemCounts.ContainsKey(item.itemName))
        {
            itemCounts[item.itemName]++;
        }
        else
        {
            itemCounts.Add(item.itemName, 1);
        }

        DisplayInventory();
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (itemCounts.ContainsKey(item.itemName))
        {
            itemCounts[item.itemName]--;
            if (itemCounts[item.itemName] <= 0)
            {
                itemCounts.Remove(item.itemName);
            }
        }

        DisplayInventory();
    }

    public void DisplayInventory()
    {
        foreach (Transform item in itemHolder)
        {
            Destroy(item.gameObject);
        }

        currentYPosition = 0f; // Reset vị trí Y cho vật phẩm đầu tiên

        foreach (Item item in items)
        {
            GameObject obj = Instantiate(itemPrefab, itemHolder);
            var itemImage = obj.transform.Find("Image").GetComponent<Image>();
            itemImage.sprite = item.image;
            obj.GetComponent<ItemController>().SetItem(item);

            // Đặt vị trí của vật phẩm theo chiều Y
            obj.transform.position = new Vector3(obj.transform.position.x, currentYPosition, obj.transform.position.z);

            currentYPosition += itemHeightOffset; // Tăng vị trí Y cho vật phẩm tiếp theo
        }

        // Gán số lượng vật phẩm đã đếm được vào TextMeshPro
        txtPoint.text = "Số lượng vật phẩm đã đếm: " + itemCounts.Count; // itemCounts.Count lấy số lượng các vật phẩm khác nhau
    }

    /*void EnableRemoveButton()
    {
        if (enableRemoveItem.isOn)
        {
            foreach (Transform item in itemHolder)
            {
                item.transform.Find("RemoveButton").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform item in itemHolder)
            {
                item.transform.Find("RemoveButton").gameObject.SetActive(false);
            }
        }
        
    }*/
}