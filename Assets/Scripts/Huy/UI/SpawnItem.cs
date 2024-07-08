using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class SpawnItem : MonoBehaviour
{
    [SerializeField] List<GameObject> itemSpawn; // Danh sách các prefab item để spawn
    [SerializeField] int soLuongItemSpawn; // Số lượng item muốn spawn
    [SerializeField] float spawnRadius; // Bán kính spawn item
    [SerializeField] Color gizmoColor = Color.green; // Màu của Gizmo
    [SerializeField] float timeSpawnItem = 60f;
    private bool canSpawn = false;

    private void Start()
    {
        SpawnItems();
    }

    private void FixedUpdate()
    {
        if (canSpawn && LobbyManager.offLobby)
        {
            StartCoroutine(TimeSpawnItem());
            canSpawn = false;
        }

    }

    // Hàm spawn các item
    public void SpawnItems()
    {
        for (int i = 0; i < soLuongItemSpawn; i++)
        {
            Debug.Log("Spawn Item");

            // Chọn một item ngẫu nhiên từ danh sách itemSpawn
            int randomIndex = Random.Range(0, itemSpawn.Count);
            GameObject itemToSpawn = itemSpawn[randomIndex];

            // Tạo vị trí ngẫu nhiên trong bán kính đã cho
            Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(randomPosition.x, randomPosition.y, 0) + transform.position;

            // Spawn item sử dụng Unity's Instantiate
            Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);
        }
        canSpawn = true;
    }

    // Vẽ Gizmo để hiển thị bán kính spawn trên Scene
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor; // Đặt màu cho Gizmo
        Gizmos.DrawWireSphere(transform.position, spawnRadius); // Vẽ hình tròn
    }

    private IEnumerator TimeSpawnItem()
    {
        yield return new WaitForSeconds(timeSpawnItem);
        SpawnItems();
    }
}