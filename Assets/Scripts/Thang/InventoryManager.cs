using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<Item> items = new List<Item>();

    public Transform itemHolder;// 
    public GameObject itemPrefab;
    private void Awake()
    {
        if (Instance != null || Instance !=this)
        {
            Destroy(Instance);
        }

        Instance = this;
    }
    // 
    public void Add(Item item)
    {
        items.Add(item);
    }

    public void DisplayIventory()
    {
        foreach (Item item in items)
        {
            GameObject obj = Instantiate(itemPrefab, itemHolder);

            var itemImage = obj.transform.Find("ItemImage").GetComponent<Image>();

            itemImage.sprite = item.image;  
        }
    }
}
