using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//IPointerClickHandler
public class ButtonBag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject OnOffBag;
    public bool selectedButton = false;
    private LobbyManager lobbyManager;
    private bool canOn = true;

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
    }

        public void OnPointerEnter(PointerEventData eventData)
    {
        selectedButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedButton = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canOn)
        {
            OnOffBag.SetActive(true);
            canOn = false;

        }
        else
        {
            OnOffBag.SetActive(false);
            canOn = true;
        }
        
    }

    private void Update()
    {
        Debug.Log("SelectedButton: " + selectedButton);
    }
}
