using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Item1 : MonoBehaviour
{
    [SerializeField]
    private int id;

    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite sprite;

    [TextArea]
    [SerializeField]
    private string itemDescription;

    private InventoryManager1 inventoryManager1;
    private Inventory_Manager inventory_Manager;

    PhotonView view;

    private Inventory_Manager inventoryManager;

    private void Start()
    {
        inventoryManager1 = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager1>();

        // Giả sử Inventory_Manager nằm trên đối tượng có tên "Player"
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            inventoryManager = player.GetComponent<Inventory_Manager>();

            if (inventoryManager != null)
            {
                Debug.LogError("Inventory_Manager duoc gan cho Item1.");
            }
            else
            {
                Debug.LogError("khong tim Inventory_Manager tren doi tuong Player.");
            }
        }
        else
        {
            Debug.LogError("khong tim thay doi tuong Player.");
        }
    }

    //public void AddItemToInventory(int id, int quantity)
    //{
    //    if (inventoryManager != null)
    //    {
    //        inventoryManager.AddItemInList(id, quantity);
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int leftOverItems = inventoryManager1.AddItem(id, itemName, quantity, sprite, itemDescription);
            if (leftOverItems <= 0)
                Destroy(gameObject);
            else
                quantity = leftOverItems;
            inventory_Manager.AddItemInList(id, quantity);
        }
    }
}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;

//public class Item1 : MonoBehaviour
//{
//    [SerializeField]
//    private int id;

//    [SerializeField]
//    private string itemName;

//    [SerializeField]
//    private int quantity;

//    [SerializeField]
//    private Sprite sprite;

//    [TextArea]
//    [SerializeField]
//    private string itemDescription;

//    private InventoryManager1 inventoryManager1;
//    private Inventory_Manager inventory_Manager;
//    private PhotonView view;

//    private void Start()
//    {
//        inventoryManager1 = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager1>();

//        // Lấy PhotonView từ GameObject này
//        view = GetComponent<PhotonView>();
//        if (!view.IsMine && PhotonNetwork.IsConnected)
//        {
//            view.TransferOwnership(PhotonNetwork.LocalPlayer);
//        }

//        // Tìm đối tượng Player và lấy Inventory_Manager của nóa
//        GameObject player = GameObject.Find("Player(Clone)");
//        if (player != null)
//        {
//            inventory_Manager = player.GetComponent<Inventory_Manager>();

//            if (inventory_Manager != null)
//            {
//                Debug.Log("Đã tìm thấy Inventory_Manager trên đối tượng Player.");
//            }
//            else
//            {
//                Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng Player.");
//            }
//        }
//        else
//        {
//            Debug.LogError("Không tìm thấy đối tượng Player.");
//        }
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.tag == "Player")
//        {
//            // Thêm item vào kho đồ của người chơi hiện tại
//            int leftOverItems = inventoryManager1.AddItem(id, itemName, quantity, sprite, itemDescription);

//            // Nếu số lượng item còn lại sau khi thêm vào kho là 0, hủy đối tượng item
//            if (leftOverItems <= 0)
//            {
//                if (view.IsMine)
//                {
//                    PhotonNetwork.Destroy(gameObject);
//                }
//            }
//            else
//            {
//                quantity = leftOverItems;
//            }

//            // Cập nhật số lượng item trong kho của người chơi hiện tại
//            if (inventory_Manager != null && view.IsMine)
//            {
//                inventory_Manager.AddItemInList(id, quantity);
//            }
//        }
//    }
//}
