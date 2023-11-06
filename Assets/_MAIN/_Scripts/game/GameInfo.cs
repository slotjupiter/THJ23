using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [TabGroup("GameStatus")] public Canvas mainCanvas;

        [TabGroup("Game Database")] public List<ItemsSetup> allItems = new();
        List<ItemSO> ingameItemList = new();

        [Serializable]
        public class ItemsSetup
        {
            public ItemSO targetItem;
            public int itemCount;
        }
        [TabGroup("Game Database")] public List<FurnitureInteract> allFurniture;

        [TabGroup("Current Game Info"), ReadOnly] public int KeyitemsCollected = 0;
        [TabGroup("Current Game Info"), ReadOnly] public int ErrorPartsEquip = 0;
        [TabGroup("Current Game Info"), ReadOnly] public ItemBox CurrentItemBox;

        public bool CollectHeadKey = false;
        public bool collectHandsKey = false;
        public bool collectOrgansKey = false;
        public bool collectLegsKey = false;


        [Header("Items")]
        [TabGroup("Current Game Info")] public List<ItemSO> currentItems;

        [TabGroup("character", "Character Status")] public GameObject characterPrefab;

        public TileCursorSystem tileCursorSystem { get; set; }
        public MapManager mapManager { get; set; }
        public DiceSystem diceSystem { get; set; }
        public SearchSystem searchSystem { get; set; }
        public UIController uiController { get; set; }
        public InventorySystem inventorySystem { get; set; }
        public EquipmentSystem equipmentSystem { get; set; }
        public DialogueSystem dialogueSystem { get; set; }
        public MinigamesSystem minigamesSystem { get; set; }

        //Game Vibe Controller
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
            inventorySystem = FindObjectOfType<InventorySystem>();
            equipmentSystem = FindObjectOfType<EquipmentSystem>();
            dialogueSystem = FindObjectOfType<DialogueSystem>();
            minigamesSystem = FindObjectOfType<MinigamesSystem>();

            sanityText.text = sanityValues.ToString();
            progressText.text = progressValues.ToString();
            KeyitemsCollected = 0;
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

        }

        public void UpdateSanityText(float value)
        {
            if (value > 0) value += (diceSystem.diceCount / 10f) + (ErrorPartsEquip * 0.75f);
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
            foreach (var item in allItems)
            {
                for (int i = 0; i < item.itemCount; i++)
                {
                    ingameItemList.Add(item.targetItem);
                }
            }

            Utils.ShuffleList(ingameItemList);

            int itemIndex = 0;
            int furnitureCount = 0;
            // if (allFurniture.Count > 0 && ingameItemList.Count > 0)
            // {
            //     for (furnitureCount = 0; furnitureCount < allFurniture.Count; furnitureCount++)
            //     {
            //         if (allFurniture[furnitureCount].searchItemList.Count == allFurniture[furnitureCount].maxStorageCount) return;
            //         allFurniture[furnitureCount].InitItems(ingameItemList[itemIndex]);
            //         itemIndex++;
            //         if (furnitureCount > allFurniture.Count) furnitureCount = 0;
            //     }
            // }

            if (allFurniture.Count > 0 && ingameItemList.Count > 0)
            {
                for (int i = 0; i < allFurniture.Count; i++)
                {
                    // Utils.ShuffleList(allFurniture);

                    if (allFurniture[i].searchItemList.Count >= allFurniture[i].maxStorageCount) return;
                    else if (allFurniture[i].searchItemList.Count < allFurniture[i].maxStorageCount)
                    {
                        // int randomCount = Random.Range(1, allFurniture[i].maxStorageCount);

                        for (int j = 0; j < allFurniture[i].maxStorageCount; j++)
                        {
                            if (itemIndex >= ingameItemList.Count) return;
                            allFurniture[i].InitItems(ingameItemList[itemIndex]);
                            itemIndex++;
                        }
                    }
                }
            }

            // if (itemIndex > 0 && itemIndex < ingameItemList.Count)
            // {
            //     Utils.ShuffleList(allFurniture);

            //     for (int i = 0; i < allFurniture.Count; i++)
            //     {
            //         if (allFurniture[i].searchItemList.Count == allFurniture[i].maxStorageCount) return;
            //         allFurniture[i].InitItems(ingameItemList[itemIndex]);
            //         itemIndex++;
            //     }
            // }
        }

        public void SetDiceValue(int value)
        {
            if (value <= 0) value = 1;
            currentDiceValue = value;
        }

        public void SetMovementRange(int value)
        {
            MovementRange = value;
        }
    }
}

