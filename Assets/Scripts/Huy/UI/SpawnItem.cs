using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnItem : MonoBehaviourPun
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
        // Chỉ chủ phòng mới có thể spawn item
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(InitialSpawnItems());
        }
    }

    private void FixedUpdate()
    {
        // Chủ phòng mới có thể kiểm soát spawn item
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
        currentActiveItems = 0;
    }

    private void EnableRandomItem()
    {
        int attempts = 0;
        const int maxAttempts = 100;
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

            Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
            itemToEnable.transform.position = new Vector3(randomPosition.x, randomPosition.y, 0) + transform.position;

            // Gửi RPC để thông báo cho các người chơi khác biết về việc spawn item
            photonView.RPC("RPC_EnableItem", RpcTarget.OthersBuffered, randomIndex, randomPosition);
        }
    }

    [PunRPC]
    private void RPC_EnableItem(int itemIndex, Vector2 position)
    {
        // Kích hoạt item dựa trên thông tin nhận được từ RPC
        GameObject itemToEnable = itemObjects[itemIndex];
        itemToEnable.SetActive(true);
        itemToEnable.transform.position = new Vector3(position.x, position.y, 0) + transform.position;
        currentActiveItems++;
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
        DisableAllItems();
        yield return new WaitForSeconds(1f); // Đợi 1 giây trước khi spawn item ban đầu
        for (int i = 0; i < maxItemsActive; i++)
        {
            EnableRandomItem();
            yield return new WaitForSeconds(waitForSecond);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
