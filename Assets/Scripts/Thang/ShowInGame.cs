using UnityEngine;

public class OpenCloseInventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject xButton;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);
            xButton.SetActive(isOpen);
        }
    }
}