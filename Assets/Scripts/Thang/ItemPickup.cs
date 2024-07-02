using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    void PickUp()
    {
        //Destroy
        Destroy(this.gameObject);
        // Add inventory
        InventoryManager.Instance.Add(item);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp();
    }
}
