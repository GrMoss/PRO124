using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Inventory_UI : MonoBehaviourPun
{
    public GameObject inventorySlotPrefab; // Prefab của slot trong kho
    public Transform inventoryPanel; // Panel chứa các slot
    public GameObject inventoryUI;

    private Inventory_Manager inventory_Manager; // Tham chiếu đến Inventory_Manager
    public static bool inventoryUIOn;

    private void Start()
    {
        inventoryUI.SetActive(false);
        inventoryUIOn = false;
        if (photonView.IsMine)
        {
            // Tìm Inventory_Manager trên đối tượng cha trước
            inventory_Manager = GetComponentInParent<Inventory_Manager>();

            // Nếu không tìm thấy, dùng FindObjectOfType để tìm trong toàn bộ scene
            if (inventory_Manager == null)
            {
                inventory_Manager = FindObjectOfType<Inventory_Manager>();
            }

            if (inventory_Manager != null)
            {
                Debug.Log("Đã gán Inventory_Manager cho Inventory_UI.");
                UpdateInventoryUI();
            }
            else
            {
                Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng này hoặc trong scene.");
            }
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleInventoryUI();
            }
        }
    }

    private void UpdateInventoryUI()
    {
        if (photonView.IsMine && inventory_Manager != null)
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

    private void ToggleInventoryUI()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (inventoryUI.activeSelf)
            {
                UpdateInventoryUI();
            }
        }
        else
        {
            Debug.LogError("inventoryUI chưa được gán trong Inspector.");
        }
    }
}
