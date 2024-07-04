using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;
    public CinemachineVirtualCamera virtualCamera;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void UpdateVirtualCameraTarget(Transform playerTransform)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerTransform;
            virtualCamera.LookAt = playerTransform;
        }
    }

    private void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
       
        // Gọi hàm để cập nhật mục tiêu của Virtual Camera
        UpdateVirtualCameraTarget(player.transform);
    }
}
