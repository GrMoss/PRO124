using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DestroyItem : MonoBehaviour
{
    private SpawnItem spawnItem;

    private void Start()
    {
        spawnItem = FindObjectOfType<SpawnItem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spawnItem.OnItemTouched(gameObject);
        }
    }
}
