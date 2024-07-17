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
            if (inventory_Manager.GetQuantityItem(4) > 0)
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
            if (inventory_Manager.GetQuantityItem(1) > 0)
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
            if (inventory_Manager.GetQuantityItem(2) > 0 && inventory_Manager.GetQuantityItem(3) > 0 && inventory_Manager.GetQuantityItem(4) > 0)
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
