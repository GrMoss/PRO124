//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CookingController : MonoBehaviour
//{
//    public GameObject viewCooker;
//    private PhotonView view;
//    private Inventory_Manager inventory;

//    private void Start()
//    {
//        view = GetComponent<PhotonView>();
//        if (view.IsMine)
//        {
//            inventory = GameObject.Find("Inventory_Manager").GetComponent<Inventory_Manager>();
//            if (inventory != null)
//            {
//                Debug.Log("oke");
//            }
//        }
//    }

//    private void FixedUpdate()
//    {
//        if (Input.GetButton("Q"))
//        {
//            viewCooker.SetActive(true);
//            inventory.ShowAllInventoryData();
//        }

//    }


//    public void OmeletButton()
//    {
//        if (view.IsMine)
//        {
//            if (inventory.GetQuatityItem(4) > 0)
//            {
//                inventory.QuitItemInList(4, 1);
//                inventory.AddItemInList(5, 1);
//            }
//        }

//    }

//    public void ChiliSauceButton()
//    {
//        if (view.IsMine)
//        {
//            if (inventory.GetQuatityItem(1) > 0)
//            {
//                inventory.QuitItemInList(1, 1);
//                inventory.AddItemInList(7, 1);
//            }
//        }
//    }

//    public void BreadButton()
//    {
//        if (view.IsMine)
//        {
//            if (inventory.GetQuatityItem(2) > 0 && inventory.GetQuatityItem(3) > 0 && inventory.GetQuatityItem(4) > 0)
//            {
//                inventory.QuitItemInList(2, 1);
//                inventory.QuitItemInList(3, 1);
//                inventory.QuitItemInList(4, 1);
//                inventory.AddItemInList(6, 1);
//            }
//        }
//    }



//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.gameObject.CompareTag("Cooker"))
//        {
//            if (view.IsMine)
//            {
//                viewCooker.SetActive(true);
//                inventory.ShowAllInventoryData();
//            }
//        }

//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.gameObject.CompareTag("Cooker"))
//        {
//            if (view.IsMine)
//            {
//                viewCooker.SetActive(false);
//            }
//        }
//    }
//}
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingController : MonoBehaviour
{
    public GameObject viewCooker;
    private PhotonView view;
    private Inventory_Manager inventory_Manager;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        inventory_Manager = GetComponent<Inventory_Manager>();

        if (inventory_Manager != null)
        {
            Debug.Log("Inventory_Manager ???c g?n cho ButtonCooker.");
        }
        else
        {
            Debug.LogError("Kh?ng t?m th?y Inventory_Manager tr?n ??i t??ng n?y.");
        }
    }

    private void FixedUpdate()
    {
        if (view.IsMine && Input.GetButton("Q"))
        {
            viewCooker.SetActive(true);
            inventory_Manager?.ShowAllInventoryData(); // S? d?ng to?n t? null-conditional
        }
    }

    public void OmeletButton()
    {
        if (view.IsMine && inventory_Manager != null)
        {
            if (inventory_Manager.GetQuatityItem(4) > 0)
            {
                inventory_Manager.QuitItemInList(4, 1);
                inventory_Manager.AddItemInList(5, 1);
            }
        }
    }

    public void ChiliSauceButton()
    {
        if (view.IsMine && inventory_Manager != null)
        {
            if (inventory_Manager.GetQuatityItem(1) > 0)
            {
                inventory_Manager.QuitItemInList(1, 1);
                inventory_Manager.AddItemInList(7, 1);
            }
        }
    }

    public void BreadButton()
    {
        if (view.IsMine && inventory_Manager != null)
        {
            if (inventory_Manager.GetQuatityItem(2) > 0 && inventory_Manager.GetQuatityItem(3) > 0 && inventory_Manager.GetQuatityItem(4) > 0)
            {
                inventory_Manager.QuitItemInList(2, 1);
                inventory_Manager.QuitItemInList(3, 1);
                inventory_Manager.QuitItemInList(4, 1);
                inventory_Manager.AddItemInList(6, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cooker"))
        {
            viewCooker.SetActive(true);
            inventory_Manager?.ShowAllInventoryData();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cooker"))
        {
            viewCooker.SetActive(false);
        }
    }
}
