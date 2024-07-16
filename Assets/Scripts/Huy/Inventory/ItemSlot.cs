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

    public bool selectedButton = false;

    public delegate void SelectionChangedHandler(bool isSelected);
    public event SelectionChangedHandler OnSelectionChanged;

    public delegate void ItemSelectedHandler(int itemID);
    public event ItemSelectedHandler OnItemSelected;

    private void Start()
    {
        inventory_Manager = FindObjectOfType<Inventory_Manager>();
        inventory_Bar = FindObjectOfType<Inventory_Bar>();

        if (photonView.IsMine)
        {
            if (inventory_Manager != null)
            {
                Debug.Log("Đã gán Inventory_Manager cho ItemPrefab.");
            }
            else
            {
                Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng này hoặc trong scene.");
            }

            if (inventory_Bar != null)
            {
                Debug.Log("Đã gán Inventory_UI cho ItemPrefab.");
            }
            else
            {
                Debug.LogError("Không tìm thấy Inventory_UI trên đối tượng này hoặc trong scene.");
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (photonView.IsMine)
        {
            selectedButton = true;
            OnSelectionChanged?.Invoke(selectedButton);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (photonView.IsMine)
        {
            selectedButton = false;
            OnSelectionChanged?.Invoke(selectedButton);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (photonView.IsMine)
        {
            selectedButton = true;
            indexShooting = true;
            inventory_Bar.indexShooting = itemID;
            OnSelectionChanged?.Invoke(selectedButton);
        }
    }
}
