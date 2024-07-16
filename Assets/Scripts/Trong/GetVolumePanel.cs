using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetVolumePanel : MonoBehaviour
{
    GameObject panel;
    Button myButton;
    private void Start()
    {
        panel = GameObject.Find("VolumePanel");
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(OpenPanel);
    }
    void OpenPanel()
    {
        panel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
