using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class ItemSlot : MonoBehaviourPun, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int itemID;
    public bool indexShooting;
    private Inventory_Manager inventory_Manager;
    private Inventory_Bar inventory_Bar;

    private bool selectedButton = false;

    public delegate void ItemSelectedHandler(int itemID);
    public event ItemSelectedHandler OnItemSelected;

    private Shooting shooting; // Tham chi?u t?i script Shooting

    private void Start()
    {
        shooting = FindObjectOfType<Shooting>();
        selectedButton = false;
        inventory_Bar = FindObjectOfType<Inventory_Bar>();
        inventory_Manager = FindObjectOfType<Inventory_Manager>();

        if (photonView.IsMine)
        {
            if (inventory_Manager != null)
            {
                Debug.Log("?? g?n Inventory_Manager cho ItemPrefab.");
            }
            else
            {
                Debug.LogError("Kh?ng t?m th?y Inventory_Manager tr?n ??i t??ng n?y ho?c trong scene.");
            }

            if (inventory_Bar != null)
            {
                Debug.Log("?? g?n inventory_Bar cho ItemPrefab.");
            }
            else
            {
                Debug.LogError("Kh?ng t?m th?y inventory_Bar tr?n ??i t??ng n?y ho?c trong scene.");
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (photonView.IsMine)
        {
            selectedButton = true;
            shooting.SetSelectingItem(true); 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (photonView.IsMine)
        {
            selectedButton = false;
            shooting.SetSelectingItem(false); 
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (photonView.IsMine)
        {
            selectedButton = true;
            indexShooting = true;
            OnItemSelected?.Invoke(itemID);
        }
    }

    private void Update()
    {
        // Debug.Log("SelectedButton: " + selectedButton);
    }
}
