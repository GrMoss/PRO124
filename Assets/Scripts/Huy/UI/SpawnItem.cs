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
    [SerializeField] float timeSpawnItem = 60f;
    [SerializeField] int maxItemsActive = 3; // Số lượng tối đa các item có thể được bật cùng lúc

    private bool canSpawn = true;
    private LobbyManager lobbyManager;
    private int currentActiveItems = 0;

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
        DisableAllItems(); // Tắt tất cả các item ban đầu
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (canSpawn && lobbyManager.offLobby && currentActiveItems < maxItemsActive)
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
        if (itemObjects == null || itemObjects.Count == 0)
        {
            Debug.LogError("itemObjects list is null or empty. Cannot enable items.");
            return;
        }

        int randomIndex = Random.Range(0, itemObjects.Count);
        GameObject itemToEnable = itemObjects[randomIndex];

        while (itemToEnable.activeSelf)
        {
            randomIndex = Random.Range(0, itemObjects.Count);
            itemToEnable = itemObjects[randomIndex];
        }

        itemToEnable.SetActive(true);
        currentActiveItems++;

        // Đặt vị trí ngẫu nhiên trong bán kính đã cho
        Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
        itemToEnable.transform.position = new Vector3(randomPosition.x, randomPosition.y, 0) + transform.position;
    }

    private IEnumerator TimeSpawnItem()
    {
        EnableRandomItem();
        canSpawn = false;
        yield return new WaitForSeconds(timeSpawnItem);
        canSpawn = true;
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
