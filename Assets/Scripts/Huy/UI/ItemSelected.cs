using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class ItemSelected : MonoBehaviourPun, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Shooting shooting;
    private bool selectedButton;

    private void Start()
    {
        shooting = FindObjectOfType<Shooting>();
        selectedButton = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemSelectedBar(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        ItemSelectedBar(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemSelectedBar(true);
    }

    public void ItemSelectedBar(bool var)
    {
        shooting.SetSelectingItem(var);
        Debug.Log("selectedButton: " + selectedButton);
    }
}
