using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace THJ
{
    public class DiceSystem : MonoBehaviour
    {
        GameInfo gameInfo;

        public Button diceButton;
        public GameObject diceRollPanel;
        public GameObject NormalDiceObject;
        GameObject currentDiceType;

        private void Awake()
        {
            diceRollPanel.SetActive(false);
            if (!gameInfo) gameInfo = FindObjectOfType<GameInfo>();
        }

        private void Start()
        {
            if (diceButton)
                diceButton.onClick.AddListener(RollDice);
        }

        private void RollDice()
        {
            if (gameInfo.canRollDice)
            {
                diceButton.transform.DOShakeScale(0.15f, 1, 6, 0, true, ShakeRandomnessMode.Harmonic);
                gameInfo.movingPhase = true;
                gameInfo.canRollDice = false;
                StartCoroutine(StartRoll(DiceType.NormalType));
            }
        }

        IEnumerator StartRoll(DiceType diceType)
        {
            switch (diceType)
            {
                case DiceType.NormalType:
                    currentDiceType = NormalDiceObject;
                    break;
                case DiceType.MeatType:
                    //*Meat type
                    break;
            }

            diceRollPanel.SetActive(true);
            //*Random Num
            int randomNumber = Random.Range(1, 6);
            gameInfo?.SetDiceValue(randomNumber);
            gameInfo.SetMovementRange(gameInfo.CurrentDiceValue);
            //*Set Sprite
            currentDiceType.GetComponent<Animator>().SetInteger("RollValue", gameInfo.CurrentDiceValue);

            yield return new WaitUntil(() => currentDiceType.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !currentDiceType.GetComponent<Animator>().IsInTransition(0));
            yield return new WaitForSeconds(0.35f);

            //*Active path depends on values.
            gameInfo.tileCursorSystem.ActivePath();
            currentDiceType.GetComponent<Animator>().SetInteger("RollValue", 0);
            diceRollPanel.SetActive(false);
        }

        public enum DiceType
        {
            NormalType, MeatType
        }
    }

}
