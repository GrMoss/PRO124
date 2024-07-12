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
        buttonCooker[5].interactable = inventory_Manager.GetQuatityItem(4) > 0;
        buttonCooker[7].interactable = inventory_Manager.GetQuatityItem(1) > 0;
        buttonCooker[6].interactable = inventory_Manager.GetQuatityItem(2) > 0 && inventory_Manager.GetQuatityItem(3) > 0 && inventory_Manager.GetQuatityItem(4) > 0;
    }
}
