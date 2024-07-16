using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM_Caller : MonoBehaviour
{
    AudioManager audioManager;
    string currentScene;

    public float waitTime = 2f;
    void Start()
    {
        while (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();

        currentScene = SceneManager.GetActiveScene().name;

        if(currentScene == "Home" || currentScene == "Game1")
        {
            if (!(audioManager.GetCurrentSong() == "Lobby"))
            {
                audioManager.fadeDuration = waitTime;
                audioManager.PlayBackgroundMusic(audioManager.lobby);
            }
        }
        else if (currentScene == "Login")
        {
            audioManager.PlaySFX(audioManager.logo);
            audioManager.PlayBackgroundMusic(audioManager.login);
        }
    }

    public void GameplayMusic()
    {
        audioManager.fadeDuration = waitTime;
        int i = Random.Range(0, 1);
        if (i == 0)
            audioManager.PlayBackgroundMusic(audioManager.gameplay1);
        else 
            audioManager.PlayBackgroundMusic(audioManager.gameplay2);
    }
}
