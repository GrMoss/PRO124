using UnityEngine;
using UnityEngine.UI;

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
    public GameObject button;
    bool toggle = false;

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }
    public void OpenVolumeSetting()
    {
        SetSliderValue(audioManager.ASPlayerSound.volume, playerSoundEffect);
        SetSliderValue(audioManager.ASPlayerFootstep.volume, playerFootstepNoise);
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
    public void CloseButton()
    {
        button.transform.localScale = new Vector3(0f, 1, 1);
    }
    public void ClosePanel()
    {
        panel.transform.localScale = new Vector3(0.00f, 0.5f, 0.5f);
    }
    public void OpenPanel()
    {
        panel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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
        audioManager.ASPlayerSound.volume = playerSoundEffect.value;
    }
    public void UpdatePlayerFootstep(float value)
    {
        if (ignoreChange)
            return;
        audioManager.ASPlayerFootstep.volume = value;
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
