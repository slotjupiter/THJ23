using System.Collections;
using System.Collections.Generic;
using THJ;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoldDropMinigame : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    GameInfo gameInfo;

    private void Start()
    {
        gameInfo = FindObjectOfType<GameInfo>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            GoldDragMinigame goldDrag = eventData.pointerDrag.GetComponent<GoldDragMinigame>();
            gameInfo.minigamesSystem.armsMinigame.UpdateNumbers(goldDrag);
            if (gameInfo.minigamesSystem.armsMinigame.currentIndex == 4) gameInfo.minigamesSystem.armsMinigame.CheckAnswer();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            gameInfo.minigamesSystem.armsMinigame.UpdateNumbers(null, true);
        }
    }
}
