using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingController : MonoBehaviour
{
    public GameObject viewCooker;
    private PhotonView view;
    private Inventory_Manager inventory_Manager;
    public bool cookingOn;
    public GameObject rotatePoint;
    public Inventory_Bar inventory_Bar;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        inventory_Manager = GetComponent<Inventory_Manager>();
    }

    public void OmeletButton()
    {
        if (view.IsMine && inventory_Manager != null)
        {
            if (inventory_Manager.GetQuantityItem(4) >= 2)
            {
                inventory_Manager.QuitItemInList(4, 2);
                inventory_Manager.AddItemInList(5, 1);
            }
        }
    }

    public void ChiliSauceButton()
    {
        if (view.IsMine && inventory_Manager != null)
        {
            if (inventory_Manager.GetQuantityItem(1) >= 3)
            {
                inventory_Manager.QuitItemInList(1, 3);
                inventory_Manager.AddItemInList(7, 1);
            }
        }
    }

    public void BreadButton()
    {
        if (view.IsMine && inventory_Manager != null)
        {
            if (inventory_Manager.GetQuantityItem(7) >= 1 && inventory_Manager.GetQuantityItem(3) >= 2 && inventory_Manager.GetQuantityItem(5) >= 2)
            {
                inventory_Manager.QuitItemInList(7, 1);
                inventory_Manager.QuitItemInList(3, 2);
                inventory_Manager.QuitItemInList(5, 2);
                inventory_Manager.AddItemInList(6, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (view.IsMine && collision.gameObject.CompareTag("Cooker"))
        {
            viewCooker.SetActive(true);
            rotatePoint.SetActive(false);
            cookingOn = true;
            inventory_Manager?.ShowItemInInventory();
            //inventory_UI.setIsCooker(cookingOn);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (view.IsMine && collision.gameObject.CompareTag("Cooker"))
        {
            viewCooker.SetActive(false);
            rotatePoint.SetActive(true);
            cookingOn = false;
            //inventory_UI.setIsCooker(cookingOn);
        }
    }
}
