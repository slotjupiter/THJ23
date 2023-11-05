using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace THJ
{
    public class EquipmentSystem : MonoBehaviour
    {
        [TabGroup("Current", "Equip")] public bool equipHead;
        [TabGroup("Current", "Equip")] public bool equipHands;
        [TabGroup("Current", "Equip")] public bool equipOrgans;
        [TabGroup("Current", "Equip")] public bool equipLegs;

        [TabGroup("Color Settings")] public Image dimBG;
        [TabGroup("Color Settings")] public Color canEquipColor;
        [TabGroup("Color Settings")] public Color canConsumeColor;
        [TabGroup("Color Settings")] public Color defaultColor;
        [TabGroup("Color Settings")] public Color errorColor;


        //*CURRENT INFO
        [Header("Slots")]
        [TabGroup("Slot Setup")] public Image HeadSlot;
        ItemDrop headDrop;
        [TabGroup("Slot Setup")] public Image HandsSlot;
        ItemDrop handsDrop;

        [TabGroup("Slot Setup")] public Image OrgansSlot;
        ItemDrop organsDrop;

        [TabGroup("Slot Setup")] public Image LegsSlot;
        ItemDrop legsDrop;

        [Header("Cursor")]
        [TabGroup("Slot Setup")] public GameObject HeadCursor;
        [TabGroup("Slot Setup")] public GameObject HandsCursor;
        [TabGroup("Slot Setup")] public GameObject OrgansCursor;
        [TabGroup("Slot Setup")] public GameObject LegsCursor;
        List<GameObject> cursorList = new();

        [TabGroup("Value Setup")] public float maxDecreseDurable = 8f;
        [TabGroup("Value Setup")] public float minDecreseDurable = 1f;

        public ItemBox HeadPart { get; set; }
        public ItemBox HandsPart { get; set; }
        public ItemBox OrgansPart { get; set; }
        public ItemBox LegsPart { get; set; }

        GameInfo gameInfo;

        private void Start()
        {
            headDrop = HeadSlot.transform.gameObject.GetComponent<ItemDrop>();
            handsDrop = HandsSlot.transform.gameObject.GetComponent<ItemDrop>();
            organsDrop = OrgansSlot.transform.gameObject.GetComponent<ItemDrop>();
            legsDrop = LegsSlot.transform.gameObject.GetComponent<ItemDrop>();
            gameInfo = FindObjectOfType<GameInfo>();

            cursorList.Add(HeadCursor);
            cursorList.Add(HandsCursor);
            cursorList.Add(LegsCursor);
            cursorList.Add(OrgansCursor);
        }

        public void ActiveCursor(GameObject targetCursor)
        {
            foreach (var cursor in cursorList)
            {
                if (cursor != targetCursor) cursor.SetActive(false);
                else cursor.SetActive(true);
            }
        }

        public void DeactiveCursor()
        {
            foreach (var cursor in cursorList)
            {
                cursor.SetActive(false);
            }
        }

        public void UpdateEquipmentsDurable()
        {
            SpecificPartDecrease(HeadPart, headDrop, headDrop.errorParts);
            SpecificPartDecrease(HandsPart, handsDrop, handsDrop.errorParts);
            SpecificPartDecrease(OrgansPart, organsDrop, organsDrop.errorParts);
            SpecificPartDecrease(LegsPart, legsDrop, legsDrop.errorParts);
        }

        private void SpecificPartDecrease(ItemBox targetParts, ItemDrop targetDrop, bool isErrorParts)
        {
            int extraValue = 0;
            if (isErrorParts) extraValue = Random.Range(2, 6);
            else extraValue = 0;

            float decDurable = Random.Range(minDecreseDurable, maxDecreseDurable);
            float sumValue = decDurable + extraValue + gameInfo.KeyitemsCollected / 2.5f;
            if (targetParts)
            {
                targetParts.currentItemDurable -= sumValue;
                if (targetParts.currentItemDurable <= 0f)
                {
                    targetParts.currentItemDurable = 0f;
                    targetDrop.PartsDestroyed();
                }

                if (targetDrop) targetDrop.equipmentDisplay.SetSliderValue(targetParts.currentItemDurable);
            }
        }

        public void DetectDropSlot(ItemType currentType, bool onDrag)
        {
            CanDrop(headDrop, HeadSlot, currentType, onDrag);
            CanDrop(handsDrop, HandsSlot, currentType, onDrag);
            CanDrop(organsDrop, OrgansSlot, currentType, onDrag);
            CanDrop(legsDrop, LegsSlot, currentType, onDrag);
        }

        private void CanDrop(ItemDrop dropSlot, Image imageSlot, ItemType currentType, bool onDrag)
        {
            if (dropSlot.allowDropType.Contains(currentType) && onDrag && currentType != ItemType.Potion)
            {
                dropSlot.canDrop = true;
                imageSlot.color = canEquipColor;
            }
            else if (dropSlot.allowDropType.Contains(currentType)
            && onDrag
            && currentType == ItemType.Potion
            && equipHead)
            {
                dropSlot.canDrop = true;
                imageSlot.color = canConsumeColor;
            }
            else if (!onDrag)
            {
                dropSlot.canDrop = false;

                if (dropSlot.errorParts)
                    imageSlot.color = errorColor;
                else
                    imageSlot.color = defaultColor;
            }
        }
    }

}
