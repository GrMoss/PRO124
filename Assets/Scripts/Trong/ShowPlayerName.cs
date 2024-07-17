using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class ShowPlayerName : MonoBehaviourPun
{
    public TMP_Text myName;
    void Start()
    {
        Renderer renderer = myName.GetComponent<Renderer>();
        renderer.sortingLayerName = "Player";
        renderer.sortingOrder = 0;

        string name = PlayerPrefs.GetString("NamePlayer");
        PhotonNetwork.NickName = name;

        if (photonView.IsMine)
        {
            photonView.RPC("DisplayName", RpcTarget.AllBuffered, name);
        }
        else
        {
            myName.color = Color.red;
        }
    }

    [PunRPC]
    public void DisplayName(string name)
    {
        myName.text = name;
    }
}
