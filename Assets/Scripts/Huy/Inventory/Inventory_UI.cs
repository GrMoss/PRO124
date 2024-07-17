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
//    public GameObject[] slot; // Danh sách các slot có sẵn
//    public GameObject inventoryUI;
//    public GameObject OnOffBag;
//    public bool inventoryUIOn;
//    private Inventory_Manager inventory_Manager; // Tham chiếu đến Inventory_Manager
//    private int indexShooting;
//    public GameObject rotatePoint;
//    private bool isCooker = false;

//    private LobbyManager lobbyManager;

//    private void Start()
//    {
//        lobbyManager = FindObjectOfType<LobbyManager>();
//        inventoryUI.SetActive(false);
//        inventoryUIOn = false;
//        // Tìm Inventory_Manager trên đối tượng cha trước
//        inventory_Manager = GetComponentInParent<Inventory_Manager>();

//        // Nếu không tìm thấy, dùng FindObjectOfType để tìm trong toàn bộ scene
//        if (inventory_Manager == null)
//        {
//            inventory_Manager = FindObjectOfType<Inventory_Manager>();
//        }

//        if (photonView.IsMine)
//        {
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
//            if (Input.GetKeyDown(KeyCode.Tab) && !isCooker && lobbyManager.offLobby)
//            {
//                inventoryUI.SetActive(!inventoryUI.activeSelf);
//                rotatePoint.SetActive(!rotatePoint.activeSelf);
//                inventoryUIOn = !inventoryUI.activeSelf;
//                UpdateInventoryUI();
//            }
//        }
//    }

//    public void OffBag()
//    {
//        inventoryUI.SetActive(!inventoryUI.activeSelf);
//        rotatePoint.SetActive(!rotatePoint.activeSelf);
//        inventoryUIOn = !inventoryUI.activeSelf;
//        UpdateInventoryUI();
//    }

//    public void UpdateInventoryUI()
//    {
//        // Làm mới dữ liệu của các slot
//        var inventoryItems = inventory_Manager.GetInventoryItems();

//        for (int i = 0; i < slot.Length; i++)
//        {
//            if (i < inventoryItems.Count && inventoryItems[i].QuantityItem > 0)
//            {
//                var item = inventoryItems[i];
//                ItemSlot itemSlotComponent = slot[i].GetComponent<ItemSlot>();

//                if (itemSlotComponent != null)
//                {
//                    itemSlotComponent.itemID = item.ItemID;
//                    itemSlotComponent.OnItemSelected += HandleItemSelected; // Đăng ký sự kiện
//                }
//                else
//                {
//                    Debug.LogError("Không tìm thấy thành phần ItemSlot trên slot.");
//                }

//                TMP_Text quantityText = slot[i].transform.Find("Quantity").GetComponent<TMP_Text>();
//                Image itemImage = slot[i].transform.Find("ItemImage").GetComponent<Image>();

//                if (quantityText != null)
//                {
//                    quantityText.text = item.QuantityItem.ToString();
//                }
//                else
//                {
//                    Debug.LogError("Không tìm thấy thành phần TMP_Text cho Quantity.");
//                }

//                if (itemImage != null)
//                {
//                    itemImage.sprite = item.ImageItem;
//                }
//                else
//                {
//                    Debug.LogError("Không tìm thấy thành phần Image cho ItemImage.");
//                }

//                slot[i].SetActive(true); // Hiển thị slot
//            }
//            else
//            {
//                slot[i].SetActive(false); // Ẩn slot nếu không có vật phẩm
//            }
//        }
//    }

//    private void HandleItemSelected(int itemID)
//    {
//        indexShooting = itemID;
//        Debug.Log("Item Selected with ID: " + itemID);
//    }

//    public int GetIndexShooting()
//    {
//        return indexShooting;
//    }

//    public void setIsCooker(bool x)
//    {
//        isCooker = x;
//    }
//}
