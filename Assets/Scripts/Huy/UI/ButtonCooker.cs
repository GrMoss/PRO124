using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCooker : MonoBehaviour
{
    [SerializeField] Button[] buttonCooker;
    public Inventory_Manager inventory_Manager;


    void Start()
    {
        
    }

    
    void Update()
    {
        if (inventory_Manager.GetQuatityItem(4) > 0)
        {
            buttonCooker[5].interactable = true;
        }
        else
        {
            buttonCooker[5].interactable = false;
        }

        if (inventory_Manager.GetQuatityItem(1) > 0)
        {
            buttonCooker[7].interactable = true;
        }
        else
        {
            buttonCooker[7].interactable = false;
        }

        if (inventory_Manager.GetQuatityItem(2) > 0 && inventory_Manager.GetQuatityItem(3) > 0 && inventory_Manager.GetQuatityItem(4) > 0)
        {
            buttonCooker[6].interactable = true;
        }
        else
        {
            buttonCooker[6].interactable = false;
        }
    }
}
