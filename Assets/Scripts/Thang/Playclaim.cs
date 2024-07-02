using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playclaim : MonoBehaviour
{
    private Transform holdPosition;
    private GameObject itemHeld; // Vật phẩm hiện đang nhặt
    private Dictionary<string, int> itemCounts = new Dictionary<string, int>(); // Dictionary để lưu trữ số lượng vật phẩm

    void Start()
    {
        itemCounts.Add("Chilli", 0); // Khởi tạo số lượng "Chilli" là 0
        itemCounts.Add("Egg", 0); // Khởi tạo số lượng "Egg" là 0
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Chilli") || other.CompareTag("Egg"))
        {
            string itemType = other.CompareTag("Chilli") ? "Chilli" : "Egg";
            PickupItem(other.gameObject, itemType);
        }
    }

    void PickupItem(GameObject item, string itemType)
    {
        if (holdPosition == null)
        {
            holdPosition = new GameObject("HoldPosition").transform; // Tạo một Transform mới nếu holdPosition bị hủy
        }

        item.transform.position = holdPosition.position;
        item.transform.parent = holdPosition;
        itemHeld = item;
        Debug.Log("Đã nhặt vật phẩm: " + itemType);

        if (itemCounts.ContainsKey(itemType))
        {
            itemCounts[itemType]++; // Tăng số lượng vật phẩm loại itemType
            Debug.Log("Số lượng " + itemType + " đã nhặt: " + itemCounts[itemType]);
        }

        Destroy(item);
    }

    void DropItem()
    {
        if (itemHeld != null)
        {
            itemHeld.transform.parent = null;
            // Đặt vị trí của vật phẩm đã nhặt vào vị trí ngẫu nhiên để vứt
            itemHeld.transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f);
            itemHeld = null;
        }
    }
}