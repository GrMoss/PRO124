using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text notificationText;

    [SerializeField] float timeWayNotificationText = 2f;

    private void Start()
    {
        notificationText.text = "Đang tải...";
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        StartCoroutine(NotificationText());
    }

    private IEnumerator NotificationText()
    {
        notificationText.text = "Đang tải..";
        yield return new WaitForSeconds(1f);
        notificationText.text = "Đang tải...";
        yield return new WaitForSeconds(0.7f);
        notificationText.text = "Đang tải.";
        yield return new WaitForSeconds(1f);
        notificationText.text = "Đang kết nối máy chủ.";
        yield return new WaitForSeconds(1f);
        notificationText.text = "Đang kết nối máy chủ..";
        yield return new WaitForSeconds(1f);
        notificationText.text = "Đang kết nối máy chủ...";
        yield return new WaitForSeconds(1f);
        notificationText.text = "Đang kết nối máy chủ.";
        yield return new WaitForSeconds(0.7f);
        notificationText.text = "Đang kết nối máy chủ..";
        yield return new WaitForSeconds(1f);
        notificationText.text = "Đang kết nối máy chủ...";
        yield return new WaitForSeconds(1.5f);
        notificationText.text = "Đã kết nối với máy chủ!";
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("Home");
    }
}