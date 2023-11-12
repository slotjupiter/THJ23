using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace THJ
{
    public class LegsMinigame : MonoBehaviour
    {
        [TabGroup("Setup"), SerializeField] private GameObject legsGamePanel;
        [TabGroup("Setup")] public TMP_Text gameHeader;
        [TabGroup("Setup"), SerializeField] private RectTransform topRect;
        [TabGroup("Setup"), SerializeField] private RectTransform centerRect;
        [TabGroup("Setup"), SerializeField] private RectTransform BottomRect;
        [TabGroup("Setup"), SerializeField] private List<Button> buttonList;

        Image topImg;
        Image centerImg;
        Image bottomImg;

        [TabGroup("Sprite"), SerializeField] private List<Sprite> topSprite;
        [TabGroup("Sprite"), SerializeField] private List<Sprite> centerSprite;
        [TabGroup("Sprite"), SerializeField] private List<Sprite> bottomSprite;

        int indexTop, indexCenter, indexBottom;

        GameInfo gameInfo;

        float delayTime;
        bool preventClick = false;

        private void Start()
        {
            gameInfo = FindObjectOfType<GameInfo>();
            topImg = topRect.gameObject.GetComponent<Image>();
            centerImg = centerRect.gameObject.GetComponent<Image>();
            bottomImg = BottomRect.gameObject.GetComponent<Image>();
        }

        private void Update()
        {
            if (delayTime > 0f)
            {
                delayTime -= Time.deltaTime * 2.5f;
                if (delayTime <= 0f) delayTime = 0f;
            }
        }

        public void NextItem(int item)
        {
            CheckWinSequence();
            if (delayTime > 0 || preventClick) return;
            delayTime = 1f;
            AudioController.Instance.PlayFX("Click");
            switch (item)
            {
                case 0:
                    indexTop++;
                    if (indexTop > topSprite.Count - 1) indexTop = 0;
                    topImg.sprite = topSprite[indexTop];
                    break;
                case 1:
                    indexCenter++;
                    if (indexCenter > centerSprite.Count - 1) indexCenter = 0;
                    centerImg.sprite = centerSprite[indexCenter];
                    break;
                case 2:
                    indexBottom++;
                    if (indexBottom > bottomSprite.Count - 1) indexBottom = 0;
                    bottomImg.sprite = bottomSprite[indexBottom];
                    break;
            }
        }

        public void PreviousItem(int item)
        {
            CheckWinSequence();
            if (delayTime > 0 || preventClick) return;
            delayTime = 1f;

            AudioController.Instance.PlayFX("Click");
            switch (item)
            {
                case 0:
                    indexTop--;
                    if (indexTop < 0) indexTop = topSprite.Count - 1;
                    topImg.sprite = topSprite[indexTop];
                    break;
                case 1:
                    indexCenter--;
                    if (indexCenter < 0) indexCenter = centerSprite.Count - 1;
                    centerImg.sprite = centerSprite[indexCenter];
                    break;
                case 2:
                    indexBottom--;
                    if (indexBottom < 0) indexBottom = bottomSprite.Count - 1;
                    bottomImg.sprite = bottomSprite[indexBottom];
                    break;
            }
        }

        public void OpenLegsMinigame()
        {
            AudioController.Instance.PlayFX("Popup");

            RandomStarterSet();
            legsGamePanel.SetActive(true);
        }

        public void ClosePanel()
        {
            AudioController.Instance.PlayFX("Popup");

            legsGamePanel.SetActive(false);
        }

        private void CheckWinSequence()
        {
            if (indexTop == 0 && indexCenter == 0 && indexBottom == 0)
            {
                preventClick = true;
                StartCoroutine(GiveLegsSequences(true));
            }
        }

        IEnumerator GiveLegsSequences(bool _boolCheck)
        {
            yield return new WaitUntil(() => _boolCheck = true);
            foreach (var btn in buttonList)
            {
                btn.interactable = false;
            }

            gameInfo.inventorySystem.CreateItemBox(gameInfo.minigamesSystem.legsKey);
            gameInfo.collectLegsKey = true;
            gameInfo.UpdateProgressText(25);

            yield return new WaitForSeconds(0.5f);
            BottomRect.DOAnchorPosY(-518f, 0.25f);
            topRect.DOAnchorPosY(-350f, 0.25f);
            AudioController.Instance.PlayFX("EquipMeat");
            yield return new WaitForSeconds(1.5f);
            gameInfo.dialogueSystem.SetForceTextWithTimeEnd("You acquired legs as a reward.", 1.5f, () =>
            {
                ClosePanel();
                gameInfo.uiController.OpenLore("Legs");
            });
        }

        private void RandomStarterSet()
        {
            indexTop = Random.Range(0, topSprite.Count);
            indexCenter = Random.Range(0, centerSprite.Count);
            indexBottom = Random.Range(0, bottomSprite.Count);

            while (indexCenter == indexTop)
            {
                indexCenter = Random.Range(0, centerSprite.Count);
            }

            while (indexBottom == indexTop || indexBottom == indexCenter)
            {
                indexBottom = Random.Range(0, bottomSprite.Count);
            }

            topImg.sprite = topSprite[indexTop];
            centerImg.sprite = centerSprite[indexCenter];
            bottomImg.sprite = bottomSprite[indexBottom];
        }

    }

}
