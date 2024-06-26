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

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JointRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void JointRandomRoom()
    {
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
}
