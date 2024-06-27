using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField tenInputField;
    [SerializeField] TMP_InputField SDTInputField;
    [SerializeField] TMP_InputField mailInputField;
    [SerializeField] TMP_Dropdown vungDropdown;

    void Start()
    {
       
    }

    void Update()
    {
        PlayerPrefs.SetString("NamePlayer", tenInputField.text);
    }

    // Kết nối đến Photon với vùng đã chọn
    public void ConnectToPhoton()
    {
        string[] regionCodes = { "asia", "hk", "us" };  // Mã vùng tương ứng với các tùy chọn trong Dropdown
        int selectedIndex = vungDropdown.value;  // Lấy chỉ mục đã chọn từ Dropdown
        string selectedRegion = regionCodes[selectedIndex];  // Lấy mã vùng dựa trên chỉ mục

        Debug.Log("Đang kết nối tới vùng: " + selectedRegion);
    

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();  // Ngắt kết nối hiện tại trước khi kết nối lại
        }

        // Gán tên người chơi cho Photon
        PhotonNetwork.NickName = PlayerPrefs.GetString("NamePlayer");
        Debug.Log("Ten nguoi choi: " + PhotonNetwork.NickName);

        // Thiết lập vùng và kết nối lại
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = selectedRegion;
        PhotonNetwork.ConnectUsingSettings();

        SceneManager.LoadScene("Loading");
    }

    
}
