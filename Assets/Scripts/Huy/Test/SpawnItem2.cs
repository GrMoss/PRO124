using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnItem2 : MonoBehaviourPun
{
    [SerializeField] List<GameObject> itemPrefabs; // Danh sách các prefab item để spawn
    [SerializeField] float spawnRadius; // Bán kính spawn item
    [SerializeField] Color gizmoColor = Color.green; // Màu của Gizmo
    [SerializeField] float waitForSecond = 10f;
    private float timeSpawnItem = 10f;
    [SerializeField] int maxItemsActive = 100; // Số lượng tối đa các item có thể được spawn cùng lúc

    private bool canSpawn = true;
    private LobbyManager lobbyManager;
    private int currentActiveItems = 0;
    private List<GameObject> spawnedItems = new List<GameObject>();

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();

        // Kiểm tra xem itemPrefabs có null hoặc rỗng không
        if (itemPrefabs == null || itemPrefabs.Count == 0)
        {
            Debug.LogError("itemPrefabs list is null or empty. Please assign the items in the Inspector.");
            return;
        }

        StartCoroutine(InitialSpawnItems());
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

    private void SpawnRandomItem()
    {
        int randomIndex = Random.Range(0, itemPrefabs.Count);
        GameObject itemToSpawn = itemPrefabs[randomIndex];

        // Đặt vị trí ngẫu nhiên trong bán kính đã cho
        Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPosition.x, randomPosition.y, 0) + transform.position;

        // Spawn the item using PhotonNetwork.Instantiate
        GameObject spawnedItem = PhotonNetwork.Instantiate(itemToSpawn.name, spawnPosition, Quaternion.identity);
        spawnedItems.Add(spawnedItem);
        currentActiveItems++;
    }

    private IEnumerator TimeSpawnItem()
    {
        SpawnRandomItem();
        canSpawn = false;
        yield return new WaitForSeconds(timeSpawnItem);
        canSpawn = true;
    }

    private IEnumerator InitialSpawnItems()
    {
        for (int i = 0; i < maxItemsActive; i++)
        {
            SpawnRandomItem();
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
        if (spawnedItems.Contains(item))
        {
            PhotonNetwork.Destroy(item); // Xóa item khỏi mạng
            spawnedItems.Remove(item);
            currentActiveItems--;
            StartCoroutine(RespawnItem());
        }
    }

    private IEnumerator RespawnItem()
    {
        yield return new WaitForSeconds(timeSpawnItem);
        if (currentActiveItems < maxItemsActive)
        {
            SpawnRandomItem();
        }
    }
}
