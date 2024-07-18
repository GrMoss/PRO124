using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnItem : MonoBehaviourPun
{
    [SerializeField] List<GameObject> itemObjects;
    [SerializeField] float spawnRadius;
    [SerializeField] Color gizmoColor = Color.green;
    [SerializeField] float waitForSecond = 10f;
    [SerializeField] float timeSpawnItem = 10f;
    [SerializeField] int maxItemsActive = 100;

    private bool canSpawn = true;
    private LobbyManager lobbyManager;
    private int currentActiveItems = 0;

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();

        if (itemObjects == null || itemObjects.Count == 0)
        {
            Debug.LogError("Danh sách itemObjects đang null hoặc rỗng. Hãy gán các item trong Inspector.");
            return;
        }

        DisableAllItems();
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

            photonView.RPC("RPC_EnableItem", RpcTarget.OthersBuffered, randomIndex, randomPosition);
        }
    }

    [PunRPC]
    private void RPC_EnableItem(int itemIndex, Vector2 position)
    {
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
        yield return new WaitForSeconds(1f);
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

    public void OnItemTouched(GameObject item)
    {
        item.SetActive(false);
        currentActiveItems--;
        StartCoroutine(RespawnItem());
    }

    private IEnumerator RespawnItem()
    {
        // Thay đổi thời gian respawn item thành 60 giây (hoặc giá trị mong muốn)
        yield return new WaitForSeconds(60f);
        if (currentActiveItems < maxItemsActive)
        {
            EnableRandomItem();
        }
    }
}
