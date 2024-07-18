using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SpawnItem : MonoBehaviourPunCallbacks
{
    [SerializeField] List<GameObject> itemSpawn; // Danh sách các prefab item để spawn
    [SerializeField] int soLuongItemSpawn; // Số lượng item muốn spawn
    [SerializeField] float spawnRadius; // Bán kính spawn item
    [SerializeField] Color gizmoColor = Color.green; // Màu của Gizmo
    [SerializeField] float timeSpawnItem = 60f;

    private bool canSpawn = true;
    private LobbyManager lobbyManager;
    private List<GameObject> spawnedItems = new List<GameObject>();

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (canSpawn && lobbyManager.offLobby)
            {
                StartCoroutine(TimeSpawnItem());
            }
        }
    }

    // Hàm spawn các item
    public void SpawnItems()
    {
        // Check if itemSpawn list is null or empty
        if (itemSpawn == null || itemSpawn.Count == 0)
        {
            Debug.LogError("itemSpawn list is null or empty. Cannot spawn items.");
            return;
        }

        for (int i = 0; i < soLuongItemSpawn; i++)
        {
            // Chọn một item ngẫu nhiên từ danh sách itemSpawn
            int randomIndex = Random.Range(0, itemSpawn.Count);
            GameObject itemToSpawn = itemSpawn[randomIndex];

            if (itemToSpawn == null)
            {
                Debug.LogError("itemToSpawn is null. Cannot spawn this item.");
                continue;
            }

            // Tạo vị trí ngẫu nhiên trong bán kính đã cho
            Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(randomPosition.x, randomPosition.y, 0) + transform.position;

            // Spawn item sử dụng PhotonNetwork.Instantiate
            GameObject spawnedItem = PhotonNetwork.Instantiate(itemToSpawn.name, spawnPosition, Quaternion.identity);
            spawnedItems.Add(spawnedItem);
            spawnedItem.GetComponent<PhotonView>().RPC("SetSpawner", RpcTarget.All, photonView.ViewID);
        }
    }

    // RPC method to set the spawner reference on the spawned items
    [PunRPC]
    public void SetSpawner(int spawnerViewID)
    {
        PhotonView spawnerView = PhotonView.Find(spawnerViewID);
        if (spawnerView != null)
        {
            SpawnItem spawner = spawnerView.GetComponent<SpawnItem>();
            if (spawner != null)
            {
                spawner.AddSpawnedItem(this.gameObject);
            }
        }
    }

    public void AddSpawnedItem(GameObject item)
    {
        spawnedItems.Add(item);
        item.GetComponent<DestroyableItem>().OnItemDestroyed += HandleItemDestroyed;
    }

    private void HandleItemDestroyed(GameObject item)
    {
        spawnedItems.Remove(item);
        if (spawnedItems.Count == 0)
        {
            canSpawn = true;
        }
    }

    // Vẽ Gizmo để hiển thị bán kính spawn trên Scene
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor; // Đặt màu cho Gizmo
        Gizmos.DrawWireSphere(transform.position, spawnRadius); // Vẽ hình tròn
    }

    private IEnumerator TimeSpawnItem()
    {
        canSpawn = false;
        SpawnItems();
        yield return new WaitForSeconds(timeSpawnItem);
    }
}

// Script trên từng item để gọi sự kiện khi bị phá hủy
public class DestroyableItem : MonoBehaviour
{
    public delegate void ItemDestroyedHandler(GameObject item);
    public event ItemDestroyedHandler OnItemDestroyed;

    private void OnDestroy()
    {
        OnItemDestroyed?.Invoke(this.gameObject);
    }
}
