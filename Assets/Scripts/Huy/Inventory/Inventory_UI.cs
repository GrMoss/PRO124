using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Inventory_UI : MonoBehaviourPun
{
    public GameObject inventorySlotPrefab; // Prefab của slot trong kho
    public Transform inventoryPanel; // Panel chứa các slot
    public GameObject[] slot; // Danh sách các slot có sẵn
    public GameObject inventoryUI;
    public bool inventoryUIOn = false;
    private Inventory_Manager inventory_Manager; // Tham chiếu đến Inventory_Manager

    private void Start()
    {
        inventoryUI.SetActive(false);
        inventoryUIOn = false;

        // Tìm Inventory_Manager trên đối tượng cha trước
        inventory_Manager = GetComponentInParent<Inventory_Manager>();

        // Nếu không tìm thấy, dùng FindObjectOfType để tìm trong toàn bộ scene
        if (inventory_Manager == null)
        {
            inventory_Manager = FindObjectOfType<Inventory_Manager>();
        }

        if (photonView.IsMine)
        {
            if (inventory_Manager != null)
            {
                Debug.Log("Đã gán Inventory_Manager cho Inventory_UI.");
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
                inventoryUI.SetActive(!inventoryUI.activeSelf);
                inventoryUIOn = inventoryUI.activeSelf;
                UpdateInventoryUI();
            }
        }
    }

    public void UpdateInventoryUI()
    {
        // Làm mới dữ liệu của các slot
        var inventoryItems = inventory_Manager.GetInventoryItems();

        for (int i = 0; i < slot.Length; i++)
        {
            if (i < inventoryItems.Count && inventoryItems[i].QuantityItem > 0)
            {
                var item = inventoryItems[i];
                ItemPrefab itemPrefabComponent = slot[i].GetComponent<ItemPrefab>();

                if (itemPrefabComponent != null)
                {
                    itemPrefabComponent.itemID = item.ItemID;
                }
                else
                {
                    Debug.LogError("Không tìm thấy thành phần ItemPrefab trên slot.");
                }

                TMP_Text quantityText = slot[i].transform.Find("Quantity").GetComponent<TMP_Text>();
                Image itemImage = slot[i].transform.Find("ItemImage").GetComponent<Image>();

                if (quantityText != null)
                {
                    quantityText.text = item.QuantityItem.ToString();
                }
                else
                {
                    Debug.LogError("Không tìm thấy thành phần TMP_Text cho Quantity.");
                }

                if (itemImage != null)
                {
                    itemImage.sprite = item.ImageItem;
                }
                else
                {
                    Debug.LogError("Không tìm thấy thành phần Image cho ItemImage.");
                }

                slot[i].SetActive(true); // Hiển thị slot
            }
            else
            {
                slot[i].SetActive(false); // Ẩn slot nếu không có vật phẩm
            }
        }
    }
}
