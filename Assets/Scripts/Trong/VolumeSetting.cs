using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeSetting : MonoBehaviour
{
    private AudioManager audioManager;
    private PlayerAudio playerAudio;

    public Slider BGM_Slider;
    public Slider buttonSlider;
    public Slider playerSoundEffect;
    public Slider playerFootstepNoise;

    private bool ignoreChange;
    public GameObject panel;
    bool toggle = false;

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }
    public void OpenVolumeSetting()
    {
        SetSliderValue(audioManager.playerSound, playerSoundEffect);
        SetSliderValue(audioManager.playerFootstep, playerFootstepNoise);
        SetSliderValue(audioManager.backgroundMusic.volume, BGM_Slider);
        SetSliderValue(audioManager.SFX.volume, buttonSlider);
    }
    public void AddListener()
    {
        if (!toggle)
        {
            toggle = true;
            panel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            BGM_Slider.onValueChanged.AddListener(UpdateBGMVolume);
            buttonSlider.onValueChanged.AddListener(UpdateSFXVolume);
            playerFootstepNoise.onValueChanged.AddListener(UpdatePlayerFootstep);
            playerSoundEffect.onValueChanged.AddListener(UpdatePlayerSound);
        }
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
    }
}
