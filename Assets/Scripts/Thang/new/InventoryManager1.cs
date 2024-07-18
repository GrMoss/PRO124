using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnItem : MonoBehaviourPunCallbacks
{
    [SerializeField] List<GameObject> itemObjects; // Danh sách các object item để bật/tắt
    [SerializeField] float spawnRadius; // Bán kính spawn item
    [SerializeField] Color gizmoColor = Color.green; // Màu của Gizmo
    [SerializeField] float waitForSecond = 10f;
    private float timeSpawnItem = 10f;
    [SerializeField] int maxItemsActive = 100; // Số lượng tối đa các item có thể được bật cùng lúc

    private bool canSpawn = true;
    private int currentActiveItems = 0;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            enabled = false; // Tắt script nếu không phải là Master Client
            return;
        }

        DisableAllItems(); // Tắt tất cả các item ban đầu
        StartCoroutine(InitialSpawnItems());
    }

    private void FixedUpdate()
    {
        // Chỉ có Master Client mới có quyền spawn item
        if (PhotonNetwork.IsMasterClient)
        {
            if (canSpawn && currentActiveItems < maxItemsActive)
            {
                StartCoroutine(TimeSpawnItem());
            }
        }
    }

    private void DisableAllItems()
    {
        foreach (var item in itemObjects)
        {
            item.SetActive(false);
        }
        currentActiveItems = 0; // Đặt lại số lượng item đang hoạt động
    }

    private void EnableRandomItem()
    {
        int attempts = 0;
        const int maxAttempts = 100; // Giới hạn số lần thử để tránh vòng lặp vô hạn
        int randomIndex = Random.Range(0, itemObjects.Count);
        GameObject itemToEnable = itemObjects[randomIndex];

        while (itemToEnable.activeSelf && attempts < maxAttempts)
        {
            randomIndex = Random.Range(0, itemObjects.Count);
            itemToEnable = itemObjects[randomIndex];
            attempts++;
        }

        if (!itemToEnable.activeSelf)
        {
            itemToEnable.SetActive(true);
            currentActiveItems++;

            // Đặt vị trí ngẫu nhiên trong bán kính đã cho
            Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
            itemToEnable.transform.position = new Vector3(randomPosition.x, randomPosition.y, 0) + transform.position;
        }
    }

    private IEnumerator TimeSpawnItem()
    {
        EnableRandomItem();
        canSpawn = false;
        yield return new WaitForSeconds(timeSpawnItem);
        canSpawn = true;
    }

    private IEnumerator InitialSpawnItems()
    {
        for (int i = 0; i < maxItemsActive; i++)
        {
            EnableRandomItem();
            yield return new WaitForSeconds(waitForSecond); // Thời gian chờ nhỏ để đảm bảo tất cả item được bật lên
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor; // Đặt màu cho Gizmo
        Gizmos.DrawWireSphere(transform.position, spawnRadius); // Vẽ hình tròn
    }

    // Hàm này được gọi khi người chơi chạm vào item
    public void OnItemTouched(GameObject item)
    {
        item.SetActive(false); // Tắt item
        currentActiveItems--;
        StartCoroutine(RespawnItem());
    }

    private IEnumerator RespawnItem()
    {
        yield return new WaitForSeconds(timeSpawnItem);
        if (currentActiveItems < maxItemsActive)
        {
            EnableRandomItem();
        }
    }
}
