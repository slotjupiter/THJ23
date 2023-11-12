using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using THJ;

public class HeadMinigameDrop : MonoBehaviour, IDropHandler
{
    HeadMinigameDrag currentDrag;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            currentDrag = eventData.pointerDrag.GetComponent<HeadMinigameDrag>();
            if (currentDrag && currentDrag.isCorrectAnswer)
            {
                currentDrag.isDropping = true;
                currentDrag.gameInfo.minigamesSystem.headMinigame.WinSequence(currentDrag);
                currentDrag = null;
            }
            else if (currentDrag && !currentDrag.isCorrectAnswer)
            {
                currentDrag.isDropping = true;
                currentDrag.gameInfo.minigamesSystem.headMinigame.WrongSequence(currentDrag);
                currentDrag = null;
            }
        }
    }

}
