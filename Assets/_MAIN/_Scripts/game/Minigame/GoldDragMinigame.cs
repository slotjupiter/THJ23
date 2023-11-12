using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace THJ
{
    public class GoldDragMinigame : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public int goldValue { get; set; }
        public Image currentImage;
        public Rigidbody2D rb;
        public PolygonCollider2D polygonCollider2D;
        public CanvasGroup canvasGroup;
        GameInfo gameInfo;
        RectTransform rectTransform;
        bool outOfScreen = false;

        private void OnEnable()
        {
            gameInfo = FindAnyObjectByType<GameInfo>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
            if ((screenPosition.y > Screen.height) || (screenPosition.y < 0f) || (screenPosition.x > Screen.width) || (screenPosition.x < 0f))
            {
                outOfScreen = true;
                if (outOfScreen)
                {
                    outOfScreen = false;
                    gameInfo.minigamesSystem.armsMinigame.ResetToSpawn(this.transform);
                }
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            rb.simulated = false;
            polygonCollider2D.enabled = false;
            canvasGroup.alpha = 0.5f;
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (gameInfo)
                rectTransform.anchoredPosition += eventData.delta / gameInfo.mainCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Reset();
        }
        
        public void Reset()
        {
            rb.simulated = true;
            polygonCollider2D.enabled = true;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }

}
