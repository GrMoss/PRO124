using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingController : MonoBehaviour
{
    public GameObject viewCooker;
    private PhotonView view;
    private Inventory_Manager inventory;

    public void OmeletButton()
    {
        if (view.IsMine)
        {
            if (inventory.GetQuatityItem(4) > 0)
            {
                inventory.QuitItemInList(4, 1);
                inventory.AddItemInList(5, 1);
            }
        }
        
    }

    public void ChiliSauceButton()
    {
        if (inventory.GetQuatityItem(1) > 0)
        {
            inventory.QuitItemInList(1, 1);
            inventory.AddItemInList(7, 1);
        }
    }

    public void BreadButton()
    {
        if (inventory.GetQuatityItem(2) > 0 && inventory.GetQuatityItem(3) > 0 && inventory.GetQuatityItem(4) > 0)
        {
            inventory.QuitItemInList(2, 1);
            inventory.QuitItemInList(3, 1);
            inventory.QuitItemInList(4, 1);
            inventory.AddItemInList(6, 1);
        }
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
        if(view.IsMine)
        {
            inventory = GameObject.Find("Inventory_Manager").GetComponent<Inventory_Manager>();
            if(inventory != null)
            {
                Debug.Log("oke");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            viewCooker.SetActive(true);
            inventory.ShowAllInventoryData();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            viewCooker.SetActive(false);
        }
    }
}
