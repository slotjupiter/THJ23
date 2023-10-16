using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace THJ
{
    public class DiceSystem : MonoBehaviour
    {
        GameInfo gameInfo;

        public Button diceButton;

        private void Awake()
        {
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
            int randomNumber = Random.Range(1, 6);
            gameInfo?.SetDiceValue(randomNumber);
        }
    }

}
