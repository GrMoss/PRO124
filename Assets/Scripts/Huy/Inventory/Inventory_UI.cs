
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using Photon.Pun;
//using TMPro;

//public class Inventory_UI : MonoBehaviourPun
//{
//    public GameObject inventorySlotPrefab; // Prefab của slot trong kho
//    public Transform inventoryPanel; // Panel chứa các slot
//    public GameObject inventoryUI;

//    private Inventory_Manager inventory_Manager; // Tham chiếu đến Inventory_Manager
//    public static bool inventoryUIOn;

//    private void Start()
//    {
//        inventoryUI.SetActive(false);
//        inventoryUIOn = false;
//        if (photonView.IsMine)
//        {
//            // Tìm Inventory_Manager trên đối tượng cha trước
//            inventory_Manager = GetComponentInParent<Inventory_Manager>();

//            // Nếu không tìm thấy, dùng FindObjectOfType để tìm trong toàn bộ scene
//            if (inventory_Manager == null)
//            {
//                inventory_Manager = FindObjectOfType<Inventory_Manager>();
//            }

//            if (inventory_Manager != null)
//            {
//                Debug.Log("Đã gán Inventory_Manager cho Inventory_UI.");
//            }
//            else
//            {
//                Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng này hoặc trong scene.");
//            }
//        }
//    }

//    private void Update()
//    {
//        if (photonView.IsMine)
//        {
//            if (Input.GetKeyDown(KeyCode.Tab))
//            {
//                inventoryUI.SetActive(!inventoryUI.activeSelf);
//                UpdateInventoryUI();

//                //if (inventoryUI.activeSelf)
//                //{
//                //    UpdateInventoryUI();
//                //}
//            }
//        }
//    }

//    public void UpdateInventoryUI()
//    {
//        if (photonView.IsMine && inventory_Manager != null)
//        {
//            // Xóa các slot cũ trước khi cập nhật lại
//            foreach (Transform child in inventoryPanel)
//            {
//                Destroy(child.gameObject);
//            }

//            // Duyệt qua danh sách các item trong kho và hiển thị trên UI
//            foreach (var item in inventory_Manager.GetInventoryItems())
//            {
//                if (item.QuantityItem > 0)
//                {
//                    Debug.Log($"Đang tạo slot cho item: {item.ItemName}, Quantity: {item.QuantityItem}");
//                    GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
//                    //TMP_Text itemNameText = slot.transform.Find("ItemName").GetComponent<TMP_Text>();
//                    TMP_Text quantityText = slot.transform.Find("Quantity").GetComponent<TMP_Text>();
//                    Image itemImage = slot.transform.Find("ItemImage").GetComponent<Image>();

//                    //if (itemNameText != null)
//                    //{
//                    //    itemNameText.text = item.ItemName;
//                    //}
//                    //else
//                    //{
//                    //    Debug.LogError("Không tìm thấy thành phần TMP_Text cho ItemName.");
//                    //}

//                    if (quantityText != null)
//                    {
//                        quantityText.text = item.QuantityItem.ToString();
//                    }
//                    else
//                    {
//                        Debug.LogError("Không tìm thấy thành phần TMP_Text cho Quantity.");
//                    }

//                    if (itemImage != null)
//                    {
//                        itemImage.sprite = item.ImageItem;
//                    }
//                    else
//                    {

//                    }
//                    {
//                        Debug.LogError("Không tìm thấy thành phần Image cho ItemImage.");
//                    }
//                }
//            }
//        }
//    }
//}
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
    public GameObject inventoryUI;
    public static bool inventoryUIOn = false;
    private Inventory_Manager inventory_Manager; // Tham chiếu đến Inventory_Manager
    private ItemPrefab itemPrefab;

    private void Start()
    {
        inventoryUI.SetActive(false);
        inventoryUIOn = false;
        // Tìm Inventory_Manager trên đối tượng cha trước
        inventory_Manager = GetComponentInParent<Inventory_Manager>();
        itemPrefab = FindObjectOfType<ItemPrefab>();

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
                if (item.QuantityItem > 0)
                {
                    Debug.Log($"Đang tạo slot cho item: {item.ItemName}, Quantity: {item.QuantityItem}");
                    GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);

                    // Lấy component ItemPrefab từ slot mới tạo
                    ItemPrefab itemPrefabComponent = slot.GetComponent<ItemPrefab>();

                    if (itemPrefabComponent != null)
                    {
                        itemPrefabComponent.itemID = item.ItemID; 
                    }
                    else
                    {
                        Debug.LogError("Không tìm thấy thành phần ItemPrefab trên slot.");
                    }

                    TMP_Text quantityText = slot.transform.Find("Quantity").GetComponent<TMP_Text>();
                    Image itemImage = slot.transform.Find("ItemImage").GetComponent<Image>();

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
                }
            }
        }
    }
}
