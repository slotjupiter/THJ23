using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace THJ
{
    public class DialogueSystem : MonoBehaviour
    {
        [TabGroup("Dialogue")] public GameObject dialoguePanel;
        [TabGroup("Dialogue")] public Button closeBtn;
        [TabGroup("Dialogue")] public TMP_Text dialogueText;
        [TabGroup("Dialogue")] public TextAnimator_TMP dialogueAnimatorText;
        [TabGroup("Dialogue")] public bool ShowText { get; set; }
        bool activeDialogue = false;
        bool forceEnd = false;
        float forceTime;
        Action forceAction = null;
        GameInfo gameInfo;

        [TabGroup("Reset")] public GameObject resetPanel;

        private void Start()
        {
            ShowText = false;
            gameInfo = FindObjectOfType<GameInfo>();
        }

        private void Update()
        {
            if (activeDialogue && dialogueAnimatorText.allLettersShown && !forceEnd)
            {
                activeDialogue = false;
                ShowText = true;
                closeBtn.gameObject.SetActive(true);
            }
            else if (activeDialogue && dialogueAnimatorText.allLettersShown && forceEnd)
            {
                activeDialogue = false;
                ShowText = true;
                forceEnd = false;
                StartCoroutine(ClosePanelForceEnd());
            }
        }

        public void OpenResetPanel()
        {
            AudioController.Instance.PlayFX("Popup");
            resetPanel.SetActive(true);
        }

        public void CloseResetPanel()
        {
            AudioController.Instance.PlayFX("Popup");
            resetPanel.SetActive(false);
        }

        public void ResetGame()
        {
            AudioController.Instance.PlayFX("Click");
            gameInfo.uiController.ResetGame();
        }

        public void ClosePanel()
        {
            if (ShowText)
            {
                AudioController.Instance.PlayFX("Popup");
                dialoguePanel.SetActive(false);
                closeBtn.gameObject.SetActive(false);
                ShowText = false;
                activeDialogue = false;
            }
        }

        IEnumerator ClosePanelForceEnd()
        {
            yield return new WaitUntil(() => ShowText);
            yield return new WaitForSeconds(forceTime);
            AudioController.Instance.PlayFX("Popup");
            dialoguePanel.SetActive(false);
            closeBtn.gameObject.SetActive(false);
            ShowText = false;
            activeDialogue = false;

            if (forceAction != null)
            {
                forceAction.Invoke();
                forceAction = null;
            }
        }

        public void SetPopupText(string _text)
        {
            dialoguePanel.SetActive(true);
            closeBtn.gameObject.SetActive(true);
            dialogueText.text = _text;
            activeDialogue = true;
        }

        public void SetForceText(string _text)
        {
            dialoguePanel.SetActive(true);
            closeBtn.gameObject.SetActive(false);
            dialogueText.text = _text;
            activeDialogue = true;
        }

        public void SetForceTextWithTimeEnd(string _text, float _time, Action nextAction = null)
        {
            dialoguePanel.SetActive(true);
            closeBtn.gameObject.SetActive(false);
            dialogueText.text = _text;
            activeDialogue = true;
            forceEnd = true;
            forceTime = _time;
            if (nextAction != null) forceAction = nextAction;
        }
    }
}
