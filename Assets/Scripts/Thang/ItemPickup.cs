using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    void PickUp()
    {
        // Hủy đối tượng hiện tại
        Destroy(this.gameObject);

        // Thêm vật phẩm vào Inventory
        InventoryManager.Instance.Add(item);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player")) // Kiểm tra nếu Collider là của nhân vật
        //{
            PickUp(); // Gọi hàm nhặt vật phẩm
        //}

    }
}