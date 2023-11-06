using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace THJ
{
    public class DialogueSystem : MonoBehaviour
    {
        public GameObject dialoguePanel;
        public Button closeBtn;
        public TMP_Text dialogueText;
        public TextAnimator_TMP dialogueAnimatorText;
        public bool ShowText { get; set; }
        bool activeDialogue = false;

        bool forceEnd = false;
        float forceTime;
        Action forceAction = null;

        private void Start()
        {
            ShowText = false;
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
