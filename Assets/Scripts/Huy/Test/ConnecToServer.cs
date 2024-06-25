using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class ConnecToServer : MonoBehaviour
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
}
