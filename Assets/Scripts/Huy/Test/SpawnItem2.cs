using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnItem2 : MonoBehaviourPun
{
    [SerializeField] List<GameObject> itemPrefabs; // Danh sách các prefab item để spawn
    [SerializeField] float spawnRadius; // Bán kính spawn item
    [SerializeField] Color gizmoColor = Color.green; // Màu của Gizmo
    [SerializeField] float waitForSecond = 10f; // Thời gian chờ giữa các lần spawn
    [SerializeField] float timeSpawnItem = 60f; // Thời gian để spawn lại item sau khi bị nhặt
    [SerializeField] int maxItemsActive = 100; // Số lượng tối đa các item có thể được spawn cùng lúc

    private bool canSpawn = true;
    private int currentActiveItems = 0;
    private List<GameObject> spawnedItems = new List<GameObject>();
    private Dictionary<GameObject, float> itemRespawnTimes = new Dictionary<GameObject, float>();

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
            // Kiểm tra và spawn lại item khi thời gian respawn đã hết
            CheckRespawnTimes();

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
        yield return new WaitForSeconds(waitForSecond);
        canSpawn = true;
    }

    private IEnumerator InitialSpawnItems()
    {
        for (int i = 0; i < maxItemsActive; i++)
        {
            SpawnRandomItem();
            yield return new WaitForSeconds(waitForSecond);
        }
    }

    private void CheckRespawnTimes()
    {
        List<GameObject> itemsToRespawn = new List<GameObject>();

        foreach (var item in itemRespawnTimes.Keys)
        {
            if (Time.time >= itemRespawnTimes[item])
            {
                itemsToRespawn.Add(item);
            }
        }

        foreach (var item in itemsToRespawn)
        {
            itemRespawnTimes.Remove(item);
            if (currentActiveItems < maxItemsActive)
            {
                SpawnRandomItem();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    public void OnItemTouched(GameObject item)
    {
        if (spawnedItems.Contains(item))
        {
            PhotonNetwork.Destroy(item); // Xóa item khỏi mạng
            spawnedItems.Remove(item);
            currentActiveItems--;

            // Đặt thời gian respawn cho item
            itemRespawnTimes[item] = Time.time + timeSpawnItem;
        }
    }
}
