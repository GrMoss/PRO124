using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CheckPhotonPun : MonoBehaviour
{
    void Start()
    {
        ConnectToPhoton();
    }

    // Kết nối tới Photon
    public void ConnectToPhoton()
    {
        Debug.Log("Bắt đầu kết nối tới Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    // Callback khi kết nối thành công tới máy chủ chính
    public void OnConnectedToMaster()
    {
        Debug.Log("Kết nối thành công tới Photon! Server: " + PhotonNetwork.CloudRegion + " Ping: " + PhotonNetwork.GetPing());
        PhotonNetwork.JoinLobby(); // Tham gia vào lobby mặc định
    }

    //// Callback khi tham gia thành công vào lobby
    //public void OnJoinedLobby()
    //{
    //    Debug.Log("Đã tham gia vào lobby: " + PhotonNetwork.CurrentLobby.Name);
    //}

    // Callback khi bị ngắt kết nối
    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Đã ngắt kết nối khỏi Photon. Lý do: " + cause.ToString());
    }

    // Callback khi kết nối thất bại
    public void OnConnectionFail(DisconnectCause cause)
    {
        Debug.LogError("Kết nối tới Photon thất bại. Lý do: " + cause.ToString());
    }
}
