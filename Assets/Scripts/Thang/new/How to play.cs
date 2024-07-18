using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    public GameObject objectS;
    public GameObject objectD;
    public GameObject panelNhat; // Panel nhặt, kéo và thả Panel vào trường này trong Inspector
    public GameObject Text;
    public GameObject Text1;

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("W"))
        {
            Destroy(collision.gameObject);
            if (objectD != null)
            {
                objectD.SetActive(true);
                if (objectS != null)
                {
                    objectS.SetActive(false);
                }
            }
        }
        else if (collision.gameObject.CompareTag("D"))
        {
            Destroy(collision.gameObject);
            if (objectS != null)
            {
                objectS.SetActive(true);
                if (objectD != null)
                {
                    objectD.SetActive(false);
                }
            }
        }
        else if (collision.gameObject.CompareTag("S"))
        {
            if (panelNhat != null)
            {
                panelNhat.SetActive(true);
            }
            Destroy(collision.gameObject);

        }
        if (collision.gameObject.CompareTag("Cooker"))
        {
            Text.SetActive(false);
            Text1.SetActive(true);
        }
        
    }
}