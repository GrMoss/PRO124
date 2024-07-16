using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class RegionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private Vector2 initialPosition = new Vector2(292f, 20f);
    private Vector2 hoverPosition = new Vector2(430f, 20f);
    private Vector2 currentPosition;
    private bool isHovering = false;
    private bool isClicked = false;
    private Coroutine moveCoroutine;
    private TMP_Dropdown dropdown;
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        rectTransform = GetComponent<RectTransform>();
        // Set initial state (partially hidden)
        rectTransform.anchoredPosition = initialPosition;
    }
    private void Update()
    {
        currentPosition = rectTransform.anchoredPosition;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        if (!isClicked)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveButton(currentPosition, hoverPosition));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (!isClicked)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveButton(currentPosition, initialPosition));
            dropdown.Hide();
        }
        else if (isClicked && !isHovering) 
        {
            StartCoroutine(Delay());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isClicked = true;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveButton(currentPosition, hoverPosition));
    }
    public void PointerExit()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveButton(currentPosition, initialPosition));
    }

    private IEnumerator MoveButton(Vector2 fromPosition, Vector2 toPosition)
    {
        float elapsedTime = 0f;
        float duration = 0.5f; // Duration of the animation
        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(fromPosition, toPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = toPosition;
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.5f);
        if (!isHovering)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveButton(currentPosition, initialPosition));
            dropdown.Hide();
            isClicked = false;
        }
    }
}
