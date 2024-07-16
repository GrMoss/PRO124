using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Inventory_Bar : MonoBehaviourPun
{
    public GameObject[] slot; // Danh sách các slot có sẵn
    private Inventory_Manager inventory_Manager; // Tham chiếu đến Inventory_Manager
    public int indexShooting;

    private void Start()
    {
        indexShooting = 0;
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

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            UpdateInventoryBar();
        }
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            //UpdateInventoryUI();
        }
    }

    public void UpdateInventoryBar()
    {
        // Làm mới dữ liệu của các slot
        var inventoryItems = inventory_Manager.GetInventoryItems();

        for (int i = 0; i < slot.Length; i++)
        {
            if (i < inventoryItems.Count && inventoryItems[i].QuantityItem > 0)
            {
                var item = inventoryItems[i];
                ItemSlot itemSlotComponent = slot[i].GetComponent<ItemSlot>();

                if (itemSlotComponent != null)
                {
                    itemSlotComponent.itemID = item.ItemID;
                }
                else
                {
                    Debug.LogError("Không tìm thấy thành phần ItemSlot trên slot.");
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


    public int GetIndexShooting()
    {
        if (photonView.IsMine)
        {
            Debug.Log("Item Selected with ID: " + indexShooting);
            return indexShooting;
        }
        return 0;
    }

}
