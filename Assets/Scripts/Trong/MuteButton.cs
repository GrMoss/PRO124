using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    Image muteButton;
    public Sprite buttonSR1, buttonSR2;
    bool toggle;

    private void Start()
    {
        muteButton = GetComponent<Image>();
    }
    public void ToggleSprite()
    {
        toggle = !toggle;
        if (toggle)
        {
            muteButton.sprite = buttonSR1;
        }
        else
        {
            muteButton.sprite = buttonSR2;
        }
    }
}
