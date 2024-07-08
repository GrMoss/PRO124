using UnityEngine;

public class OpenCloseInventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);
        }
    }
}