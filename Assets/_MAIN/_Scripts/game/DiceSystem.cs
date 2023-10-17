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
        public TMP_Text diceRollText;

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
            diceRollText.text = randomNumber.ToString();
            gameInfo?.SetDiceValue(randomNumber);
            gameInfo.SetMovementRange(gameInfo.CurrentDiceValue);

            yield return new WaitForSeconds(1f);

            //Active path depends on values.
            gameInfo.tileCursorSystem.ActivePath();
            diceRollPanel.SetActive(false);
        }
    }

}
