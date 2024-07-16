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
            //Debug.Log("Spawn Item");

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
            PhotonNetwork.Instantiate(itemToSpawn.name, spawnPosition, Quaternion.identity);
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
        SpawnItems();
        canSpawn = false;
        yield return new WaitForSeconds(timeSpawnItem);
        canSpawn = true;
    }
}
