using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playertest : MonoBehaviour
{
    public GameObject dan;
    private int dem = 0;
    public GameObject Panel;
    public GameObject Last;
    public GameObject bar;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chilli"))
        {
            dem++;
            if (dem == 2)
            {
                Panel.SetActive(true);
                Last.SetActive(false);
                bar.SetActive(false);

            }
        }
    }
}
