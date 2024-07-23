using UnityEngine;
using System.Collections;
using Photon.Pun;

public class AfterImageEffect : MonoBehaviourPun
{
    public GameObject afterImagePrefab;
    public float afterImageLifetime = 0.5f;
    public float afterImageSpawnRate = 0.1f;
    public Color afterImageColor = new Color(1f, 1f, 1f, 0.5f);
    public GameObject parentPlayer;

    private float spawnCooldown;
    PlayerController playerController;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }
    void LateUpdate()
    {
        if (photonView.IsMine)
        {
            if (playerController.moveSpeed > 4.1f)
            {
                if (Mathf.Abs(playerController.moveVector.x) > 0.1f || Mathf.Abs(playerController.moveVector.y) > 0.1f)
                {
                    if (spawnCooldown > 0)
                    {
                        spawnCooldown -= Time.deltaTime;
                    }
                    else
                    {
                        photonView.RPC("SpawnAfterImage", RpcTarget.AllBuffered);
                        spawnCooldown = afterImageSpawnRate;
                    }
                }
            }
        }
        
    }

    [PunRPC]
    void SpawnAfterImage()
    {
        GameObject afterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
        SpriteRenderer sr = afterImage.GetComponent<SpriteRenderer>();
        SpriteRenderer playerSR = GetComponent<SpriteRenderer>();
        sr.sprite = playerSR.sprite;
        sr.color = afterImageColor;
        afterImage.transform.localScale = parentPlayer.transform.localScale;
        StartCoroutine(FadeAfterImage(sr));
        Destroy(afterImage, afterImageLifetime + 0.5f);
    }

    IEnumerator FadeAfterImage(SpriteRenderer sr)
    {
        Color originalColor = sr.color;
        float fadeDuration = afterImageLifetime;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            sr.color = Color.Lerp(originalColor, new Color(originalColor.r, originalColor.g, originalColor.b, 0), normalizedTime);
            yield return null;
        }
        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}
