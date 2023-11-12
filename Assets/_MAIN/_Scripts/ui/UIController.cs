using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Febucci.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace THJ
{
    public class UIController : MonoBehaviour
    {
        GameInfo gameInfo;
        public CanvasGroup menuCanvasgroup;
        public TextAnimator_TMP pressAnykeyAnim;
        public TextAnimator_TMP winQuotesText;
        public TextAnimator_TMP winText;
        public GameObject loseObj;
        public GameObject logo;

        public GameObject HeadLore;
        public GameObject LegsLore;
        public GameObject ArmsLore;
        public GameObject OrgansLore;

        GameObject currentLoreOpen;

        private void Awake()
        {
            menuCanvasgroup.alpha = 1f;
            gameInfo = FindObjectOfType<GameInfo>();
            currentLoreOpen = null;
        }

        private void Update()
        {
            if (gameInfo)
            {
                if (!gameInfo.gameStart && pressAnykeyAnim.allLettersShown)
                {
                    if (Input.anyKey)
                    {
                        gameInfo.gameStart = true;
                        menuCanvasgroup.DOFade(0f, 0.5f).OnComplete(() => menuCanvasgroup.gameObject.SetActive(false));
                    }
                }

                if (winQuotesText.allLettersShown && winQuotesText.gameObject.activeSelf)
                {
                    winText.gameObject.SetActive(true);
                }
            }
        }

        [Button("TestWin")]
        public void GameWin()
        {
            AudioController.Instance.PlayFX("Click");
            pressAnykeyAnim.gameObject.SetActive(false);
            menuCanvasgroup.alpha = 0f;
            menuCanvasgroup.gameObject.SetActive(true);
            menuCanvasgroup.DOFade(1f, 0.8f).OnComplete(
                () =>
                {
                    AudioController.Instance.FadeToStopBGM();
                    winQuotesText.gameObject.SetActive(true);
                });
        }

        [Button("TestOver")]
        public void GameOver()
        {
            AudioController.Instance.FadeToStopBGM();
            AudioController.Instance.PlayFX("Demon");

            pressAnykeyAnim.gameObject.SetActive(false);
            loseObj.SetActive(true);
            menuCanvasgroup.alpha = 0f;
            menuCanvasgroup.gameObject.SetActive(true);
            menuCanvasgroup.DOFade(1f, 0.8f).OnComplete(
                () =>
                {
                    DOVirtual.DelayedCall(4f, () =>
                    {
                        Scene scene = SceneManager.GetActiveScene();
                        SceneManager.LoadScene(scene.name);
                    });
                });
        }

        public void ResetGame()
        {
            menuCanvasgroup.alpha = 0f;
            logo.SetActive(false);
            pressAnykeyAnim.gameObject.SetActive(false);
            menuCanvasgroup.gameObject.SetActive(true);
            menuCanvasgroup.DOFade(1f, 0.25f).OnComplete(
                () =>
                {
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        Scene scene = SceneManager.GetActiveScene();
                        SceneManager.LoadScene(scene.name);
                    });
                });
        }

        public void OpenLore(string parts)
        {
            switch (parts)
            {
                case "Head":
                    HeadLore.GetComponent<LoreActived>().ActiveBorder();
                    currentLoreOpen = HeadLore;
                    break;
                case "Arms":
                    ArmsLore.GetComponent<LoreActived>().ActiveBorder();
                    currentLoreOpen = ArmsLore;
                    break;
                case "Legs":
                    LegsLore.GetComponent<LoreActived>().ActiveBorder();
                    currentLoreOpen = LegsLore;
                    break;
                case "Organs":
                    OrgansLore.GetComponent<LoreActived>().ActiveBorder();
                    currentLoreOpen = OrgansLore;
                    break;
            }
        }

        public void CloseLore(string parts)
        {
            switch (parts)
            {
                case "Head":
                    HeadLore.GetComponent<LoreActived>().CloseBorder();
                    break;
                case "Arms":
                    ArmsLore.GetComponent<LoreActived>().CloseBorder();
                    break;
                case "Legs":
                    LegsLore.GetComponent<LoreActived>().CloseBorder();
                    break;
                case "Organs":
                    OrgansLore.GetComponent<LoreActived>().CloseBorder();
                    break;
            }
        }
    }
}

