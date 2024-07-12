
using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerAudio : MonoBehaviourPun
{
    [SerializeField] private AudioSource playerSound;
    [SerializeField] private AudioSource playerRun;

    [Header("Player Sound Effects")]
    public AudioClip fainted1;
    public AudioClip fainted2;
    public AudioClip fainted3;
    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip hurt3;
    public AudioClip run;
    public AudioClip attack;

    private void Awake()
    {
        playerRun.clip = run;
    }
    public void PlayerHurt()
    {
        if (photonView.IsMine)
            photonView.RPC("_PlayerHurt", RpcTarget.AllBuffered);
    }

    public void PlayerFainted()
    {
        if (photonView.IsMine)
            StartCoroutine(Delay());
    }

    public void PlayerRunning(bool isTrue)
    {
        if (photonView.IsMine)
            photonView.RPC("_PlayerRunning", RpcTarget.AllBuffered, isTrue);
    }

    public void PlayerAttack()
    {
        if (photonView.IsMine)
            photonView.RPC("_PlayerAttack", RpcTarget.AllBuffered);
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.5f);
        photonView.RPC("_PlayerFainted", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void _PlayerHurt()
    {
        AudioClip clip;
        int i = Random.Range(1, 3);
        if (i == 1)
        {
            clip = hurt1;
        }
        else if (i == 2)
        {
            clip = hurt2;
        }
        else
        {
            clip = hurt3;
        }

        playerSound.PlayOneShot(clip);
    }
    [PunRPC]
    public void _PlayerFainted()
    {
        AudioClip clip;
        int i = Random.Range(1, 3);
        if (i == 1)
        {
            clip = fainted1;
        }
        else if (i == 2)
        {
            clip = fainted2;
        }
        else
        {
            clip = fainted3;
        }

        playerSound.PlayOneShot(clip);
    }
    [PunRPC]
    public void _PlayerRunning(bool isTrue)
    {
        if (isTrue)
        {
            if (!playerRun.isPlaying)
            {
                playerRun.Play();
            }
        }
        else
        {
            playerRun.Stop();
            
        }
    }
    [PunRPC]
    public void _PlayerAttack()
    {
        playerSound.PlayOneShot(attack);
    }
}
