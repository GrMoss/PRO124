using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventorySlotPrefab; // Prefab của slot trong kho
    public Transform inventoryPanel; // Panel chứa các slot

    private Inventory_Manager inventory_Manager; // Tham chiếu đến Inventory_Manager

    private void Start()
    {
        inventory_Manager = GetComponent<Inventory_Manager>();

        if (inventory_Manager != null)
        {
            Debug.Log("Inventory_Manager đã được gán cho Inventory_UI.");
            UpdateInventoryUI();
        }
        else
        {
            Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng này.");
        }
    }

    private void UpdateInventoryUI()
    {
        // Xóa các slot cũ trước khi cập nhật lại
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        // Duyệt qua danh sách các item trong kho và hiển thị trên UI
        foreach (var item in inventory_Manager.GetInventoryItems())
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
            slot.transform.Find("ItemName").GetComponent<Text>().text = item.ItemName;
            slot.transform.Find("Quantity").GetComponent<Text>().text = $"Quantity: {item.QuantityItem}";
            slot.transform.Find("ItemImage").GetComponent<Image>().sprite = item.ImageItem;
        }
    }
}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Inventory_UI : MonoBehaviour
//{
//    public GameObject inventorySlotPrefab; // Prefab của slot trong kho
//    public Transform inventoryPanel; // Panel chứa các slot

//    private Inventory_Manager inventoryManager; // Tham chiếu đến Inventory_Manager

//    private void Start()
//    {
//        inventoryManager = GetComponent<Inventory_Manager>();

//        if (inventoryManager != null)
//        {
//            Debug.Log("Inventory_Manager đã được gán cho Inventory_UI.");
//            UpdateInventoryUI();
//        }
//        else
//        {
//            Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng này.");
//        }
//    }

//    public void UpdateInventoryUI()
//    {
//        // Xóa các slot cũ trước khi cập nhật lại
//        foreach (Transform child in inventoryPanel)
//        {
//            Destroy(child.gameObject);
//        }

//        // Duyệt qua danh sách các item trong kho và hiển thị trên UI
//        foreach (var item in inventoryManager.GetInventoryItems())
//        {
//            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
//            slot.transform.Find("ItemName").GetComponent<Text>().text = item.ItemName;
//            slot.transform.Find("Quantity").GetComponent<Text>().text = $"Quantity: {item.QuantityItem}";
//            slot.transform.Find("ItemImage").GetComponent<Image>().sprite = item.ImageItem;

//            // Thêm sự kiện OnClick cho nút xóa vật phẩm
//            Button removeButton = slot.transform.Find("RemoveButton").GetComponent<Button>();
//            if (removeButton != null)
//            {
//                int itemId = item.ItemID;
//                removeButton.onClick.AddListener(() => RemoveItem(itemId));
//            }
//        }
//    }

//    public void RemoveItem(int itemId)
//    {
//        inventoryManager.RemoveItemFromList(itemId);
//        UpdateInventoryUI();
//    }
//}
