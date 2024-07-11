using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingController : MonoBehaviour
{
    public GameObject viewCooker;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            viewCooker.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            viewCooker.SetActive(false);
        }
    }
}
