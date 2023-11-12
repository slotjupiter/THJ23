using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using THJ;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeadMinigame : MonoBehaviour
{
    [TabGroup("Setup")] public GameObject headGamePanel;
    [TabGroup("Setup")] public GameObject closeBtn;
    [TabGroup("Setup")] public TMP_Text gameHeader;
    [TabGroup("Choices")] public List<Sprite> choicesSprite;
    [TabGroup("Choices")] public List<HeadMinigameDrag> dragChoices;

    GameInfo gameInfo;

    private void Start()
    {
        gameInfo = FindObjectOfType<GameInfo>();
    }

    public void OpenHeadMinigame()
    {
        SetChoices();
        AudioController.Instance.PlayFX("FlipBook");
        headGamePanel.SetActive(true);
    }

    public void CloseHeadMinigamePanel()
    {
        AudioController.Instance.PlayFX("Popup");
        headGamePanel.SetActive(false);
    }

    public void WinSequence(HeadMinigameDrag currentDrag)
    {
        StartCoroutine(GiveHeadSequences(gameInfo.minigamesSystem.headWin, currentDrag));
    }

    public void WrongSequence(HeadMinigameDrag currentDrag)
    {
        StartCoroutine(GiveWrongSequence(currentDrag));
    }

    IEnumerator GiveHeadSequences(bool _boolCheck, HeadMinigameDrag currentDrag)
    {
        yield return new WaitUntil(() => _boolCheck = true);
        closeBtn.gameObject.GetComponent<Button>().interactable = false;
        gameInfo.CollectHeadKey = true;
        gameInfo.UpdateProgressText(25);
        foreach (var drag in dragChoices)
            drag.canvasGroup.interactable = false;

        gameInfo.inventorySystem.CreateItemBox(gameInfo.minigamesSystem.headKey);
        yield return new WaitForSeconds(0.25f);
        if (currentDrag)
            if (currentDrag.isCorrectAnswer)
            {
                currentDrag.choiceImg.DOColor(Color.black, 0.25f);
                currentDrag.canvasGroup.DOFade(0f, 0.25f);
            }

        AudioController.Instance.PlayFX("SmokeOut");
        yield return new WaitForSeconds(0.25f);
        AudioController.Instance.PlayEventBGM("Chanting", 0.5f);
        yield return new WaitForSeconds(1.5f);
        gameInfo.dialogueSystem.SetForceTextWithTimeEnd("You acquired head as a reward.", 2.5f, () =>
        {
            CloseHeadMinigamePanel();
            gameInfo.uiController.OpenLore("Head");
        });
    }

    IEnumerator GiveWrongSequence(HeadMinigameDrag currentDrag)
    {
        closeBtn.gameObject.GetComponent<Button>().interactable = false;
        foreach (var drag in dragChoices)
            drag.canvasGroup.interactable = false;
        yield return new WaitForSeconds(0.25f);
        currentDrag.choiceImg.DOColor(Color.black, 0.25f);
        currentDrag.canvasGroup.DOFade(0f, 0.25f);
        AudioController.Instance.PlayFX("SmokeOut");
        yield return new WaitForSeconds(1f);
        gameInfo.dialogueSystem.SetForceTextWithTimeEnd("No... try again", 2.5f, () =>
        {
            gameInfo.mapManager.MovePlayerToSpawn();
            CloseHeadMinigamePanel();
            ResetChoices();
        });
    }

    private void ResetChoices()
    {
        foreach (var drag in dragChoices)
        {
            drag.canvasGroup.alpha = 1f;
            drag.choiceImg.color = Color.white;
            drag.canvasGroup.interactable = true;
            drag.BackToStartPlace();
        }
    }

    private void SetChoices()
    {
        if (choicesSprite.Count > 0 && dragChoices.Count > 0)
        {
            Utils.ShuffleList(dragChoices);

            for (int i = 0; i < dragChoices.Count; i++)
            {
                dragChoices[i].choiceImg.sprite = choicesSprite[i];
                if (i == dragChoices.Count - 1)
                    dragChoices[i].isCorrectAnswer = true;
                else
                    dragChoices[i].isCorrectAnswer = false;
            }
        }
    }
}
