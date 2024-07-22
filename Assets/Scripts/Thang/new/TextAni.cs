using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAni : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;

    public string[] stringArray;

    [SerializeField] float timeBtwnChars; // Thời gian giữa mỗi ký tự
    [SerializeField] float timeBtwnWords; // Thời gian giữa mỗi từ
    int i = 0;

    void Start()
    {
        EndCheck();
    }

    public void EndCheck()
    {
        if (i <= stringArray.Length - 1)
        {
            _textMeshPro.text = stringArray[i];
            StartCoroutine(TextVisible());
        }
    }

    private IEnumerator TextVisible()
    {
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacter = _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacter + 1);
            _textMeshPro.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacter)
            {
                i += 1;
                Invoke("EndCheck", timeBtwnWords);
                break;
            }

            counter += 1;
            yield return new WaitForSeconds(timeBtwnChars / 10); // Tăng tốc độ hiệu ứng chữ bằng cách chia độ trễ
        }

    }
}