using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace THJ
{
    public class OrgansMinigame : MonoBehaviour
    {
        [TabGroup("Setup")] public GameObject organsGamePanel;
        [TabGroup("Setup")] public GameObject closeBtn;
        [TabGroup("Setup")] public Image gameBG;
        [TabGroup("Setup")] public Sprite completedBG;
        [TabGroup("Setup")] public TMP_Text gameHeader;
        [TabGroup("Game")] public Slider slider_1;
        [TabGroup("Game")] public Slider slider_2;
        [TabGroup("Game")] public Slider slider_3;
        [TabGroup("Game")] public Light2D lightDisplay;
        [TabGroup("Game")] public Light2D poleLightDisplay;

        bool light1On, doLight1 = false;
        bool light2On, doLight2 = false;
        bool light3On, doLight3 = false;

        GameInfo gameInfo;

        private void Start()
        {
            gameInfo = FindObjectOfType<GameInfo>();
        }

        private void Update()
        {
            if (gameInfo)
                if (doLight1 && doLight2 && doLight3 && !gameInfo.minigamesSystem.organsWin)
                {
                    gameInfo.minigamesSystem.organsWin = true;
                    gameInfo.collectOrgansKey = true;
                    closeBtn.SetActive(false);
                    StartCoroutine(GiveOrgansSequences(gameInfo.minigamesSystem.organsWin));
                }
        }

        IEnumerator GiveOrgansSequences(bool _boolCheck)
        {
            yield return new WaitUntil(() => _boolCheck = true);
            slider_1.interactable = false;
            slider_2.interactable = false;
            slider_3.interactable = false;
            yield return new WaitForSeconds(1f);
            AudioController.Instance.PlayFX("SwitchClick");
            gameBG.sprite = completedBG;
            gameInfo.inventorySystem.CreateItemBox(gameInfo.minigamesSystem.organsKey);
            yield return new WaitForSeconds(1.5f);
            gameInfo.dialogueSystem.SetForceTextWithTimeEnd("You got 'An Organs'", 1.5f, () =>
            {
                DOVirtual.Float(0f, 1.8f, 0.5f, x => poleLightDisplay.pointLightOuterRadius = x);
                CloseOrgansPanel();
                gameInfo.uiController.OpenLore("Organs");
            });
        }

        public void CheckValues(int targetSlider)
        {
            switch (targetSlider)
            {
                case 1:
                    DisplayCorrectLight(slider_1, 0.32f, 0.45f, ref light1On, ref doLight1);
                    break;
                case 2:
                    DisplayCorrectLight(slider_2, 0.55f, 0.72f, ref light2On, ref doLight2);
                    break;
                case 3:
                    DisplayCorrectLight(slider_3, 0.125f, 0.2f, ref light3On, ref doLight3);
                    break;
                default:
                    break;
            }
        }

        private void DisplayCorrectLight(Slider _targetSlider, float minVal, float maxVal, ref bool lightOn, ref bool doLight)
        {
            if (_targetSlider.value <= maxVal && _targetSlider.value > minVal)
            {
                AudioController.Instance.PlayFX("SwitchClick");
                lightOn = true;
                if (lightOn && !doLight)
                {
                    doLight = true;
                    DOVirtual.Float(lightDisplay.intensity, lightDisplay.intensity + 4f, 0.25f, x => lightDisplay.intensity = x);
                }
            }
            else if (lightOn && doLight)
            {
                lightOn = false;
                doLight = false;
                DOVirtual.Float(lightDisplay.intensity, lightDisplay.intensity - 4f, 0.25f, x => lightDisplay.intensity = x);
            }
        }

        public void PlaySwitchSound()
        {
            AudioController.Instance.PlayFXWithTime("SwitchSlider", 0.175f);
        }

        public void CloseOrgansPanel()
        {
            AudioController.Instance.PlayFX("Popup");
            slider_1.value = 0;
            slider_2.value = 0;
            slider_3.value = 0;
            lightDisplay.intensity = 0;
            light1On = false;
            light2On = false;
            light3On = false;
            doLight1 = false;
            doLight2 = false;
            doLight3 = false;
            organsGamePanel.SetActive(false);
        }

    }
}