using System.Collections.Generic;
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

    private Text txtPoint;
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
        DisplayInventory();
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public void DisplayInventory()
    {
        foreach (Transform item in itemHolder)
        {
            Destroy(item.gameObject);
        }

        foreach (Item item in items)
        {
            GameObject obj = Instantiate(itemPrefab, itemHolder);
            var itemName = item.itemName;

            var itemImage = obj.transform.Find("Image").GetComponent<Image>();

            itemImage.sprite = item.image;

            obj.GetComponent<ItemController>().SetItem(item);

            // Đếm vật phẩm theo tên
            if (itemCounts.ContainsKey(itemName))
            {
                itemCounts[itemName]++;
            }
            else
            {
                itemCounts.Add(itemName, 1);
            }
        }

        EnableRemoveButton();

        // Hiển thị số lượng vật phẩm đã nhặt theo tên
        foreach (var kvp in itemCounts)
        {
            Debug.Log("Số lượng vật phẩm " + kvp.Key + " đã nhặt: " + kvp.Value);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Xử lý khi có va chạm với vật phẩm (nếu cần)
    }

    void EnableRemoveButton()
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
        
    }
}