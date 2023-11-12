using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace THJ
{
    public class DiceSystem : MonoBehaviour
    {
        GameInfo gameInfo;

        [TabGroup("Panel")] public Button diceButton;
        [TabGroup("Panel")] public GameObject diceRollPanel;
        [TabGroup("Panel")] public Sprite normalSprite;
        [TabGroup("Panel")] public Sprite meatSprite;

        [TabGroup("Dice")] public GameObject NormalDiceObject;
        [TabGroup("Dice")] public GameObject MeatDiceObject;

        [TabGroup("Text")] public TMP_Text diceValueText;
        [TabGroup("Text")] public Color normalColor;
        [TabGroup("Text")] public Color meatColor;

        public bool OnRollingDice { get; private set; } = false;
        public bool usingMeatDice { get; private set; } = false;
        GameObject currentDiceType;
        public int diceCount { get; private set; } = 0;

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

        private void Update()
        {
            if (gameInfo.ErrorPartsEquip == 4 && !usingMeatDice)
            {
                usingMeatDice = true;
                diceButton.image.sprite = meatSprite;
                diceButton.image.SetNativeSize();
            }
            else if (gameInfo.ErrorPartsEquip < 4 && usingMeatDice)
            {
                usingMeatDice = false;
                diceButton.image.sprite = normalSprite;
                diceButton.image.SetNativeSize();
            }
        }

        private void RollDice()
        {
            if (gameInfo.canRollDice)
            {
                gameInfo.equipmentSystem.UpdateEquipmentsDurable();
                gameInfo.UpdateSanityText(1f);
                diceCount++;
                OnRollingDice = true;
                diceButton.transform.DOShakeScale(0.15f, 1, 6, 0, true, ShakeRandomnessMode.Harmonic);
                gameInfo.movingPhase = true;
                gameInfo.canRollDice = false;
                if (!usingMeatDice)
                    StartCoroutine(StartRoll(DiceType.NormalType));
                else
                    StartCoroutine(StartRoll(DiceType.MeatType));
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
                    currentDiceType = MeatDiceObject;
                    break;
            }
            currentDiceType.SetActive(true);
            diceRollPanel.SetActive(true);

            if (!gameInfo.equipmentSystem.equipLegs)
            {
                diceValueText.text = "-2 Move";
                diceValueText.color = normalColor;
            }
            else if (gameInfo.equipmentSystem.equipLegs && !usingMeatDice)
            {
                diceValueText.text = null;
                diceValueText.color = normalColor;
            }
            else if (gameInfo.equipmentSystem.equipLegs && usingMeatDice)
            {
                diceValueText.text = "+2 Move";
                diceValueText.color = meatColor;
            }

            //*Random Num
            int randomNumber = Random.Range(1, 6);
            int extraMove = GetExtraMove();
            gameInfo?.SetDiceValue(randomNumber + extraMove);
            gameInfo.SetMovementRange(gameInfo.CurrentDiceValue);
            //*Set Sprite
            currentDiceType.GetComponent<Animator>().SetInteger("RollValue", randomNumber);
            if (currentDiceType == NormalDiceObject)
                AudioController.Instance.PlayFX("DiceRolling");
            else
                AudioController.Instance.PlayFXWithDelay("MeatDiceRolling", 0.25f);
            yield return new WaitUntil(() => currentDiceType.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !currentDiceType.GetComponent<Animator>().IsInTransition(0));
            yield return new WaitForSeconds(0.35f);

            //*Active path depends on values.
            gameInfo.tileCursorSystem.ActivePath();
            currentDiceType.GetComponent<Animator>().SetInteger("RollValue", 0);
            diceRollPanel.SetActive(false);
            currentDiceType.SetActive(false);
            OnRollingDice = false;
        }

        private int GetExtraMove()
        {
            if (gameInfo.equipmentSystem.equipLegs && !usingMeatDice)
            {
                return 0;
            }
            else if (gameInfo.equipmentSystem.equipLegs && usingMeatDice)
            {
                return 2;
            }
            else return -2;
        }

        public enum DiceType
        {
            NormalType, MeatType
        }
    }

}
