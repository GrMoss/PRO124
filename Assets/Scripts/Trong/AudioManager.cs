using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource SFX;

    [SerializeField] private float fadeDuration = 2f;

    [Header("Background Musics")]
    public AudioClip lobby;
    public AudioClip login;
    public AudioClip gameplay1;
    public AudioClip gameplay2;

    [Header("Sound Effects")]
    public AudioClip buttonPressed1;
    public AudioClip buttonPressed2;
    public AudioClip logo;

    [Header("Player Effects")]
    public AudioClip fainted1;
    public AudioClip fainted2;
    public AudioClip fainted3;
    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip hurt3;

    private static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Login")
        {
            PlaySFX(logo);
            PlayBackgroundMusic(lobby);
        }
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (backgroundMusic.isPlaying)
        {
            StartCoroutine(MusicFadeOut(clip));
        }
        else
        {
            StartCoroutine(PlayNewMusic(clip));
        }
    }

    IEnumerator MusicFadeOut(AudioClip newSong)
    {
        float starVolume = backgroundMusic.volume;
        float timer = 0;

        while (backgroundMusic.volume > 0)
        {
            timer += Time.deltaTime;
            backgroundMusic.volume = Mathf.Lerp(starVolume, 0, timer / fadeDuration);
            yield return null;
        }

        backgroundMusic.Stop();
        backgroundMusic.volume = starVolume;

        StartCoroutine(PlayNewMusic(newSong));
    }

    IEnumerator PlayNewMusic(AudioClip newSong)
    {
        if (SceneManager.GetActiveScene().name == "Login")
        {
            yield return new WaitForSeconds(4f);
        }
        backgroundMusic.clip = newSong;
        backgroundMusic.Play();

        float targetVolume = backgroundMusic.volume;
        backgroundMusic.volume = 0;
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            backgroundMusic.volume = Mathf.Lerp(0, targetVolume, timer /  fadeDuration);
            yield return null;
        }
        backgroundMusic.volume = targetVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }
}