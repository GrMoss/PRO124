using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEm : MonoBehaviour
{
    public GameObject objectToThrow;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Kiểm tra khi click chuột trái
        {
            ThrowObject();
        }
    }

    void ThrowObject()
    {
        // Tạo một phiên bản của GameObject và ném ra
        GameObject thrownObject = Instantiate(objectToThrow, transform.position, Quaternion.identity);

        // Đặt hướng và tốc độ cho việc ném ra
        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * 500f); // Thay đổi hướng và tốc độ ném tại đây
        }
    }
}