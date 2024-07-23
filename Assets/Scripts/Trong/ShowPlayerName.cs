using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class ShowPlayerName : MonoBehaviourPun
{
    public TMP_Text myName;
    public PhotonView myID;
    [HideInInspector] public Color color;
    [HideInInspector] public bool isUniqueName;
    void Start()
    {
        Renderer renderer = myName.GetComponent<Renderer>();
        color = Color.white;
        renderer.sortingLayerName = "Player";
        renderer.sortingOrder = 0;

        string name = PlayerPrefs.GetString("NamePlayer");
        if (name == "") name = "PlayerID: " + myID.ViewID.ToString();
        PhotonNetwork.NickName = name;

        if (photonView.IsMine)
        {
            photonView.RPC("DisplayName", RpcTarget.AllBuffered, name);
        }
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (PhotonNetwork.NickName.ToUpper() == "RAINBOW" || PhotonNetwork.NickName.ToUpper().ToUpper() == "UNICORN" || PhotonNetwork.NickName.ToUpper().ToUpper() == "RAINBOW UNICORN")
            {
                isUniqueName = true;
                float r = Mathf.Sin(Time.time) * 0.5f + 0.5f;
                float g = Mathf.Sin(Time.time + 2.0f) * 0.5f + 0.5f;
                float b = Mathf.Sin(Time.time + 4.0f) * 0.5f + 0.5f;

                color = new Color(r, g, b);
                myName.color = color;

                photonView.RPC("DisplayColor", RpcTarget.AllBuffered, r, g, b);
            }

        }
    }
    [PunRPC]
    public void DisplayName(string name)
    {
        myName.text = name;
    }
    [PunRPC]
    public void DisplayColor(float r, float g, float b)
    {
        Color newColor = new Color(r, g, b);
        myName.color = newColor;
    }
}
