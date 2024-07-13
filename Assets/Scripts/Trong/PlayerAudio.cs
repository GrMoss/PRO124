
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Linq;
using Photon.Realtime;

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
        playerRun.volume = audioManager.playerFootstep;
        playerSound.volume = audioManager.playerSound;
        playerRun.clip = run;

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

    private void PlaySoundForOthers(int index, bool isTrue = false)
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        var nearbyPlayers = allPlayers.Where(x => (Vector2.Distance(transform.position, x.transform.position) < hearRange));
        foreach (GameObject player in nearbyPlayers) 
        { 
            PhotonView ptView = player.GetComponent<PhotonView>();

            if (index == 0)
                ptView.RPC("_PlayerHurt", ptView.Owner, transform.position);
            else if (index == 1)
                StartCoroutine(DelayDeath(ptView));
            else if (index == 2)
                ptView.RPC("_PlayerRunning", ptView.Owner, transform.position, isTrue);
            else
                ptView.RPC("_PlayerAttack", ptView.Owner, transform.position);
        }
    }
    IEnumerator DelayDeath(PhotonView ptView)
    {
        yield return new WaitForSeconds(0.35f);
        ptView.RPC("_PlayerFainted", ptView.Owner, transform.position);

    }

    [PunRPC]
    public void _PlayerHurt(Vector3 pos)
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
        AudioSource.PlayClipAtPoint(clip, pos, audioManager.playerSound);
    }
    [PunRPC]
    public void _PlayerFainted(Vector3 pos)
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
        Debug.Log("ok");
        AudioSource.PlayClipAtPoint(clip, pos, audioManager.playerSound);
    }
    [PunRPC]
    public void _PlayerRunning(Vector3 pos, bool isTrue)
    {
        if (isTrue)
        {
            if (!checkRun)
            {
                checkRun = true;
                AudioSource.PlayClipAtPoint(run, pos, audioManager.playerFootstep);
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
    public void _PlayerAttack(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(attack, pos, audioManager.playerSound);
    }
}
