using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHome : MonoBehaviour
{
    [SerializeField] TMP_Text tenNguoiChoiText;

    void Start()
    {
        tenNguoiChoiText.text = PlayerPrefs.GetString("NamePlayer");
    }

}
