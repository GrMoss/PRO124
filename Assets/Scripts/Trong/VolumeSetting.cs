using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    private AudioManager audioManager;
    private PlayerAudio PlayerAudio;

    private Slider BGM_Slider;
    private Slider buttonSlider;
    private Slider playerSoundEffect;
    private Slider playerFootstepNoise;

    GameObject panel;
    Button thisButton;

    private bool ignoreChange;
    private void Start()
    {
        thisButton = GetComponent<Button>();
        audioManager = FindObjectOfType<AudioManager>();

        BGM_Slider = GameObject.Find("SliderBGM").GetComponent<Slider>();
        buttonSlider = GameObject.Find("SliderSFX").GetComponent <Slider>();
        playerSoundEffect = GameObject.Find("SliderPlayerSound").GetComponent<Slider>();
        playerFootstepNoise = GameObject.Find("SliderFootstep").GetComponent<Slider>();

        BGM_Slider.onValueChanged.AddListener(UpdateBGMVolume);
        buttonSlider.onValueChanged.AddListener(UpdateSFXVolume);
        playerFootstepNoise.onValueChanged.AddListener(UpdatePlayerFootstep);
        playerSoundEffect.onValueChanged.AddListener(UpdatePlayerSound);

        SetSliderValue(audioManager.playerSound, playerSoundEffect);
        SetSliderValue(audioManager.playerFootstep, playerFootstepNoise);
        SetSliderValue(audioManager.backgroundMusic.volume, BGM_Slider);
        SetSliderValue(audioManager.SFX.volume, buttonSlider);

        panel = GameObject.Find("VolumePanel");
        panel.SetActive(false);
    }
    public void SetSliderValue(float newValue, Slider mySlider)
    {
        ignoreChange = true;
        mySlider.value = newValue;
        ignoreChange = false;
    }

    public void UpdatePlayerSound(float value)
    {
        if (ignoreChange)
            return;
        audioManager.playerSound = playerSoundEffect.value;
    }
    public void UpdatePlayerFootstep(float value)
    {
        if (ignoreChange)
            return;
        audioManager.playerFootstep = value;
    }
    public void UpdateBGMVolume(float value)
    {
        if (ignoreChange)
            return;
        audioManager.backgroundMusic.volume = value;
    }
    public void UpdateSFXVolume(float value)
    {
        if (ignoreChange)
            return;
        audioManager.SFX.volume = value;
        //Debug.Log(audioManager.playerSound + "|||" + audioManager.playerFootstep + "|||" + audioManager.backgroundMusic.volume + "|||" + audioManager.SFX.volume);
    }
}
