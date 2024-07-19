using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnOff : MonoBehaviourPun
{
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject inventoryBar;
    private void Update()
    {
        if (PhotonScript.showBXH)
        {
            healthBar.SetActive(false);
            inventoryBar.SetActive(false);
        }
    }
}
