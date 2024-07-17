
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Linq;
using Photon.Realtime;

public class PlayerAudio : MonoBehaviourPun
{
    [Header("Player Sound Effects")]
    public AudioClip fainted1;
    public AudioClip fainted2;
    public AudioClip fainted3;
    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip hurt3;
    public AudioClip run;
    public AudioClip attack;

    private AudioManager audioManager;
    private bool checkRun;
    private float runClipTime;

    [Header("Option")]
    [SerializeField] private float hearRange = 15f;
    private void Awake()
    {
        while (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        runClipTime = run.length;
    }
    public void PlayerHurt()
    {
        PlaySoundForOthers(0);
    }

    public void PlayerFainted()
    {

        PlaySoundForOthers(1);
    }

    public void PlayerRunning(bool isTrue)
    {
        PlaySoundForOthers(2, isTrue);
    }

    public void PlayerAttack()
    {
        PlaySoundForOthers(3);
    }

    public void PlayerEat()
    {
        PlaySoundForOthers(4);
    }

    private void PlaySoundForOthers(int index, bool isTrue = false)
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        var nearbyPlayers = allPlayers.Where(x => (Vector2.Distance(transform.position, x.transform.position) < hearRange));
        foreach (GameObject player in nearbyPlayers) 
        { 
            PhotonView ptView = player.GetComponent<PhotonView>();

            if (index == 0)
                ptView.RPC("_PlayerHurt", ptView.Owner);
            else if (index == 1)
                StartCoroutine(DelayDeath(ptView));
            else if (index == 2)
                ptView.RPC("_PlayerRunning", ptView.Owner, isTrue);
            else if (index == 3)
                ptView.RPC("_PlayerAttack", ptView.Owner);
            else
                ptView.RPC("_PlayerEat", ptView.Owner);

        }
    }
    IEnumerator DelayDeath(PhotonView ptView)
    {
        yield return new WaitForSeconds(0.2f);
        ptView.RPC("_PlayerFainted", ptView.Owner);
        Debug.Log("Die Sound");

    }

    [PunRPC]
    public void _PlayerHurt()
    {
        AudioClip clip;
        int i = Random.Range(1, 4);
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
        //AudioSource.PlayClipAtPoint(clip, pos, audioManager.playerSound);
        audioManager.ASPlayerSound.PlayOneShot(clip);
    }
    [PunRPC]
    public void _PlayerFainted()
    {
        AudioClip clip;
        int i = Random.Range(1, 4);
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
        //AudioSource.PlayClipAtPoint(clip, pos, audioManager.playerSound);
        audioManager.ASPlayerSound.PlayOneShot(clip);
    }
    [PunRPC]
    public void _PlayerRunning(bool isTrue)
    {
        if (isTrue)
        {
            if (!checkRun)
            {
                checkRun = true;
                //AudioSource.PlayClipAtPoint(run, pos, audioManager.playerFootstep);
                audioManager.ASPlayerFootstep.PlayOneShot(run);
            }
            else
            {
                runClipTime -= Time.deltaTime;
                if (runClipTime <= 0)
                {
                    checkRun = false;
                    runClipTime = run.length;
                }
            }
        }
    }
    [PunRPC]
    public void _PlayerAttack()
    {
        //AudioSource.PlayClipAtPoint(attack, pos, audioManager.playerSound);
        audioManager.ASPlayerSound.PlayOneShot(attack);
    }

    [PunRPC]
    public void _PlayerEat()
    {
        //AudioSource.PlayClipAtPoint(attack, pos, audioManager.playerSound);
        //audioManager.ASPlayerSound.PlayOneShot(attack);
        Debug.Log("Play Eating Sound!");
    }
}
