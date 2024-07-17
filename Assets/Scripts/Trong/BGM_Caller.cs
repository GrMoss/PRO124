using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class BGM_Caller : MonoBehaviourPun
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
        photonView.RPC("_GameplayMusic", RpcTarget.All);
    }
    [PunRPC]
    public void _GameplayMusic()
    {
        audioManager.fadeDuration = waitTime;
        int i = Random.Range(0, 2);
        if (i == 0)
            audioManager.PlayBackgroundMusic(audioManager.gameplay1);
        else 
            audioManager.PlayBackgroundMusic(audioManager.gameplay2);
    }
}
