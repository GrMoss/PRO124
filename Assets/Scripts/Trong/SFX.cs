using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        while (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlaySFX(AudioClip clip)
    {
        audioManager.PlaySFX(clip);
    }
}
