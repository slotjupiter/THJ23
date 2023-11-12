using System.Collections;
using System.Collections.Generic;
using THJ;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeadMinigameDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public Image choiceImg;
    public bool isCorrectAnswer { get; set; } = false;
    public bool isDropping { get; set; } = false;
    Vector2 startPlace;
    RectTransform imageRect;
    public CanvasGroup canvasGroup { get; private set; }
    public GameInfo gameInfo { get; set; }

    private void Start()
    {
        choiceImg = GetComponent<Image>();
        gameInfo = FindObjectOfType<GameInfo>();
        startPlace = GetComponent<RectTransform>().anchoredPosition;
        imageRect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void BackToStartPlace()
    {
        if (imageRect)
            imageRect.anchoredPosition = startPlace;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameInfo)
            imageRect.anchoredPosition += eventData.delta / gameInfo.mainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameInfo && !isDropping)
            BackToStartPlace();
        else if (isDropping)
        {

        }

        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
