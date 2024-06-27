using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField createInput;
    [SerializeField] TMP_InputField joinInput;
    [SerializeField] TMP_Text notificationText;
    [SerializeField] float timeWayNotificationText = 2f;

    public void CreateRoom()
    {
        if (createInput.text == "")
        {
            notificationText.text = "Xin vui lòng nhập tên phòng!";
            StartCoroutine(NotificationText());
        }
        else
        {
            notificationText.text = "Đang tạo phòng, chờ mình chút nhé!";
            StartCoroutine(NotificationText());
            PhotonNetwork.CreateRoom(createInput.text);
        }
    }

    public void JointRoom()
    {
        if (joinInput.text == "")
        {
            notificationText.text = "Xin vui lòng nhập tên phòng!";
            StartCoroutine(NotificationText());
        }
        else
        {
            notificationText.text = "Đang tìm phòng, chờ mình chút nhé!";
            StartCoroutine(NotificationText());
            PhotonNetwork.JoinRoom(joinInput.text);
        }
    }

    public void JointRandomRoom()
    {
        notificationText.text = "Đang tìm phòng!";
        StartCoroutine(NotificationText());
        PhotonNetwork.JoinRandomRoom();
    }

    // Callback khi vào phòng thành công
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game1");
        Debug.Log("Da vao phong: " + PhotonNetwork.CurrentRoom.Name);
    }

    // Callback khi có người chơi khác vào phòng
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Nguoi choi moi da vao phong: " + newPlayer.NickName);
        
    }

    private IEnumerator NotificationText()
    {
        yield return new WaitForSeconds(timeWayNotificationText);
        notificationText.text = null;
    }
}
