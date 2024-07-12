using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingController : MonoBehaviour
{
    public GameObject viewCooker;
    private PhotonView view;
    private Inventory_Manager inventory;

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
