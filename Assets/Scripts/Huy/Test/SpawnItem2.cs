using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnItem2 : MonoBehaviourPun
{
    [SerializeField] List<GameObject> itemPrefabs; // Danh sách các prefab item để spawn
    [SerializeField] float spawnRadius; // Bán kính spawn item
    [SerializeField] Color gizmoColor = Color.green; // Màu của Gizmo
    [SerializeField] float waitForSecond = 10f;
    [SerializeField] float timeSpawnItem = 60f; // Thời gian spawn item sau khi bị nhặt (60 giây)
    [SerializeField] int maxItemsActive = 100; // Số lượng tối đa các item có thể được spawn cùng lúc

    private bool canSpawn = true;
    private int currentActiveItems = 0;
    private List<GameObject> spawnedItems = new List<GameObject>();

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(InitialSpawnItems());
        }
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (canSpawn && currentActiveItems < maxItemsActive)
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
