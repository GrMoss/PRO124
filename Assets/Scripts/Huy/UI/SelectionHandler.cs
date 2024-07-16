using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    private void Start()
    {
        // Đăng ký sự kiện cho tất cả các ItemSlot
        foreach (ItemSlot itemSlot in FindObjectsOfType<ItemSlot>())
        {
            itemSlot.OnSelectionChanged += HandleSelectionChanged;
        }
    }

    private void HandleSelectionChanged(bool isSelected)
    {
        // Xử lý khi trạng thái được thay đổi
        Debug.Log("SelectedButton changed: " + isSelected);
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện khi đối tượng bị hủy
        foreach (ItemSlot itemSlot in FindObjectsOfType<ItemSlot>())
        {
            itemSlot.OnSelectionChanged -= HandleSelectionChanged;
        }
    }
}
