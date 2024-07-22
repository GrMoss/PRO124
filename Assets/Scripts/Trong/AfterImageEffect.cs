using UnityEngine;
using System.Collections;

public class AfterImageEffect : MonoBehaviour
{
    public GameObject afterImagePrefab;
    public float afterImageLifetime = 0.5f;
    public float afterImageSpawnRate = 0.1f;
    public Color afterImageColor = new Color(1f, 1f, 1f, 0.5f);

    private float spawnCooldown;
    PlayerController playerController;

    TestMovementByTrong testMove;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        testMove = GetComponent<TestMovementByTrong>();
    }
    void LateUpdate()
    {
        if (testMove.isMoving)
        {
            if (spawnCooldown > 0)
            {
                spawnCooldown -= Time.deltaTime;
            }
            else
            {
                SpawnAfterImage();
                spawnCooldown = afterImageSpawnRate;
            }
        }
        
    }

    void SpawnAfterImage()
    {
        GameObject afterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
        SpriteRenderer sr = afterImage.GetComponent<SpriteRenderer>();
        SpriteRenderer playerSR = GetComponent<SpriteRenderer>();

        sr.sprite = playerSR.sprite;
        sr.color = afterImageColor;
        if (testMove.moveX < 0) sr.flipX = true;
        else sr.flipX = false;

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
