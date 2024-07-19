using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEm : MonoBehaviour
{
    public GameObject objectToThrow;
    private int hits = 0;
    private bool canShoot = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot) // Kiểm tra khi click chuột trái và có thể bắn
        {
            StartCoroutine(ShootWithDelay());
        }
    }

    IEnumerator ShootWithDelay()
    {
        canShoot = false; // Đánh dấu là không thể bắn cho đến khi kết thúc delay
        ThrowObject(); // Bắn đạn ngay lập tức

        yield return new WaitForSeconds(1f); // Chờ 1 giây trước khi có thể bắn tiếp

        canShoot = true; // Kết thúc delay, có thể bắn lại
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