using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace THJ
{
    public class GameInfo : MonoBehaviour
    {
        [ReadOnly, ShowInInspector, TabGroup("Info")] public int CurrentDiceValue { get => currentDiceValue; }
        private int currentDiceValue;
        [ReadOnly, ShowInInspector, TabGroup("Info")] public int MovementRange { get; set; }

        [TabGroup("Info")] public float MoveSpeed = 3f;
        [TabGroup("Info"), ShowInInspector, ReadOnly] public bool isMoving { get; set; }
        [TabGroup("Info"), ShowInInspector, ReadOnly] public bool movingPhase { get; set; }
        [TabGroup("Info"), ShowInInspector, ReadOnly] public bool canRollDice { get; set; }

        [TabGroup("GameStatus"), ShowInInspector, ReadOnly] public bool gameStart { get; set; }
        [TabGroup("GameStatus")] public TMP_Text progressText;
        int progressValues = 0;
        [TabGroup("GameStatus")] public TMP_Text sanityText;
        float sanityValues = 100;

        [TabGroup("Game Database")] public List<ItemSO> allItems = new();
        [TabGroup("Game Database")] public List<FurnitureInteract> allFurniture;

        [TabGroup("Current Game Info")] public int collectKeys { get; set; } = 0;
        [TabGroup("Current Game Info")] public ItemSO HeadPart { get; set; }
        [TabGroup("Current Game Info")] public GameObject HeadCursor;
        [TabGroup("Current Game Info")] public GameObject HeadSlot;
        public bool collectHead = false;

        [TabGroup("Current Game Info")] public ItemSO HandsPart { get; set; }
        [TabGroup("Current Game Info")] public GameObject HandsCursor;
        [TabGroup("Current Game Info")] public GameObject HandsSlot;
        public bool collectHands = false;

        [TabGroup("Current Game Info")] public ItemSO OrgansPart { get; set; }
        [TabGroup("Current Game Info")] public GameObject OrgansCursor;
        [TabGroup("Current Game Info")] public GameObject OrgansSlot;
        public bool collectOrgans = false;

        [TabGroup("Current Game Info")] public ItemSO LegsPart { get; set; }
        [TabGroup("Current Game Info")] public GameObject LegsCursor;
        [TabGroup("Current Game Info")] public GameObject LegsSlot;
        public bool collectLegs = false;

        [Header("Items")]
        [TabGroup("Current Game Info")] public List<ItemSO> currentItems;

        public GameObject characterPrefab;

        public TileCursorSystem tileCursorSystem { get; set; }
        public MapManager mapManager { get; set; }
        public DiceSystem diceSystem { get; set; }
        public SearchSystem searchSystem { get; set; }
        public UIController uiController { get; set; }
        bool phase2 = false;
        bool lose = false;

        private void Awake()
        {
            gameStart = false;
            tileCursorSystem = FindObjectOfType<TileCursorSystem>();
            mapManager = FindObjectOfType<MapManager>();
            diceSystem = FindObjectOfType<DiceSystem>();
            searchSystem = FindObjectOfType<SearchSystem>();
            uiController = FindObjectOfType<UIController>();

            sanityText.text = sanityValues.ToString();
            progressText.text = progressValues.ToString();
            collectKeys = 0;
        }

        private void Start()
        {
            isMoving = false;
            movingPhase = false;
            canRollDice = true;

            Utils.ShuffleList(allFurniture);
            Utils.ShuffleList(allItems);

            InitItemToFurniture();
        }

        private void Update()
        {
            if (gameStart)
            {
                if (progressValues >= 100f && !lose)
                {
                    progressText.text = "100";
                    uiController.GameWin();
                }
                else if (sanityValues <= 0)
                {
                    if (!lose)
                    {
                        lose = true;
                        sanityText.text = "0";
                        uiController.GameOver();
                    }
                }

                if (sanityValues <= 50f && !phase2)
                {
                    phase2 = true;
                    AudioController.Instance.PlayBGM("UnsafeBG");
                }
            }


            if (collectHead && !HeadCursor.activeSelf)
            {
                HeadCursor.SetActive(true);
                HeadSlot.SetActive(true);
            }
            if (collectHands && !HandsCursor.activeSelf)
            {
                HandsCursor.SetActive(true);
                HandsSlot.SetActive(true);
            }
            if (collectLegs && !LegsCursor.activeSelf)
            {
                LegsCursor.SetActive(true);
                LegsSlot.SetActive(true);
            }
            if (collectOrgans && !OrgansCursor.activeSelf)
            {
                OrgansCursor.SetActive(true);
                OrgansSlot.SetActive(true);
            }
        }

        public void UpdateSanityText(float value)
        {
            if (value > 0) value += diceSystem.diceCount / 2f;
            sanityValues -= value;
            if (sanityValues >= 100) sanityValues = 100f;
            sanityText.text = ((int)sanityValues).ToString();
        }

        public void UpdateProgressText(int value)
        {
            progressValues += value;
            progressText.text = progressValues.ToString();
        }


        private void InitItemToFurniture()
        {
            int itemIndex = 0;
            if (allFurniture.Count > 0 && allItems.Count > 0)
            {
                for (int i = 0; i < allFurniture.Count; i++)
                {
                    if (itemIndex >= allItems.Count) return;
                    allFurniture[i].InitItems(allItems[itemIndex]);
                    itemIndex++;
                }
            }

            if (itemIndex > 0 && itemIndex < allItems.Count)
            {
                Utils.ShuffleList(allFurniture);

                for (int i = 0; i < allFurniture.Count; i++)
                {
                    if (itemIndex >= allItems.Count) return;
                    allFurniture[i].InitItems(allItems[itemIndex]);
                    itemIndex++;
                }
            }
        }

        public void SetDiceValue(int value)
        {
            if (value > 0)
                currentDiceValue = value;
        }

        public void SetMovementRange(int value)
        {
            MovementRange = value;
        }
    }
}

