using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ButtonCooker : MonoBehaviour
{
    [SerializeField] Button[] buttonCooker;

    private PhotonView view;
    private Inventory_Manager inventory_Manager;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        inventory_Manager = GetComponent<Inventory_Manager>();

        if (inventory_Manager != null)
        {
            Debug.Log("Inventory_Manager được gán cho ButtonCooker.");
        }
        else
        {
            Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng này.");
        }
    }

    void Update()
    {
        if (view != null && view.IsMine)
        {
            UpdateButtonState();
        }
    }

    private void UpdateButtonState()
    {
        if (inventory_Manager != null)
        {
            // Chỉ cho phép nhấn nút nếu số lượng tồn kho đủ
            buttonCooker[5].interactable = inventory_Manager.GetQuantityItem(4) > 1;
            buttonCooker[7].interactable = inventory_Manager.GetQuantityItem(1) > 2;
            buttonCooker[6].interactable = inventory_Manager.GetQuantityItem(2) > 1 && inventory_Manager.GetQuantityItem(3) > 1 && inventory_Manager.GetQuantityItem(4) > 1;
        }
        else
        {
            // Nếu inventory_Manager chưa được gán, tất cả nút sẽ không thể nhấn
            buttonCooker[5].interactable = false;
            buttonCooker[7].interactable = false;
            buttonCooker[6].interactable = false;
        }
    }
}
