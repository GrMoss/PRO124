using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItem : MonoBehaviour
{
    private SpawnItem spawnItem;

    private void Start()
    {
        spawnItem = FindObjectOfType<SpawnItem>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra xem đối tượng chạm vào có tag là "Player" và có component BoxCollider2D không
        if (collision.gameObject.CompareTag("Player"))
        {
            spawnItem.OnItemTouched(gameObject);
        }
    }
}
