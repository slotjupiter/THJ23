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
        public GameObject normalDiceImage;

        private void Awake()
        {
            diceRollPanel.SetActive(false);
            if (!gameInfo) gameInfo = FindObjectOfType<GameInfo>();
        }

        private void Start()
        {
            if (diceButton)
            {
                diceButton.onClick.AddListener(RollDice);
            }
        }

        private void RollDice()
        {
            if (gameInfo.canRollDice)
            {
                gameInfo.canRollDice = false;
                StartCoroutine(StartRoll());
            }
        }

        IEnumerator StartRoll()
        {
            diceRollPanel.SetActive(true);
            int randomNumber = Random.Range(1, 6);
            gameInfo?.SetDiceValue(randomNumber);
            gameInfo.SetMovementRange(gameInfo.CurrentDiceValue);
            normalDiceImage.GetComponent<Animator>().SetInteger("RollValue", gameInfo.CurrentDiceValue);
            yield return new WaitUntil(() => normalDiceImage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !normalDiceImage.GetComponent<Animator>().IsInTransition(0));
            //Active path depends on values.
            gameInfo.tileCursorSystem.ActivePath();
            normalDiceImage.GetComponent<Animator>().SetInteger("RollValue", 0);
            diceRollPanel.SetActive(false);
        }
    }

}
