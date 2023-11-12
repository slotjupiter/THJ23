using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using THJ;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using System;
using Random = UnityEngine.Random;

public class ItemDrop : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public ItemType dropType;
    [ReadOnly] public ItemBox currentItemInSlot;
    public EquipmentDisplayActive equipmentDisplay;

    [Serializable]
    public class EquipmentDisplayActive
    {
        public Slider durableSlider;

        public GameObject headObject;
        public GameObject handsObject;
        public GameObject organsObject;
        public GameObject legsObject;

        public void SetSliderValue(float _targetVal)
        {
            if (durableSlider) durableSlider.value = _targetVal;
        }

        public void ResetSlider()
        {
            if (durableSlider) durableSlider.value = 0;
        }

        public void ActiveObject(GameObject targetObj)
        {
            if (targetObj) targetObj.SetActive(true);
            if (headObject != targetObj && headObject) headObject.SetActive(false);
            if (handsObject != targetObj && handsObject) handsObject.SetActive(false);
            if (organsObject != targetObj && organsObject) organsObject.SetActive(false);
            if (legsObject != targetObj && legsObject) legsObject.SetActive(false);
        }

        public void DeactiveAll()
        {
            if (headObject) headObject.SetActive(false);
            if (handsObject) handsObject.SetActive(false);
            if (organsObject) organsObject.SetActive(false);
            if (legsObject) legsObject.SetActive(false);
        }
    }

    public List<ItemType> allowDropType = new();
    public bool canDrop { get; set; } = false;
    public bool errorParts { get; private set; } = false;

    Image currentSlotImg;
    GameInfo gameInfo;

    private void Start()
    {
        gameInfo = FindObjectOfType<GameInfo>();
        currentSlotImg = GetComponent<Image>();
        canDrop = false;
        errorParts = false;
        equipmentDisplay.ResetSlider();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.gameObject.layer != 6)
        {
            ItemBox currentItemBox = eventData.pointerDrag.GetComponent<ItemBox>();
            if (currentItemBox)
            {
                ItemType currentType = currentItemBox.itemData.itemType;

                if (canDrop)
                {
                    HandleDropType(dropType, currentType, currentItemBox);
                    // Destroy(eventData.pointerDrag.gameObject);
                    eventData.pointerDrag.gameObject.SetActive(false);
                }
                else
                {
                    eventData.pointerDrag.GetComponent<ItemBox>().ReturnToPlace();
                }
            }
            else eventData.pointerDrag.GetComponent<ItemBox>().ReturnToPlace();

        }
    }

    private void HandleDropType(ItemType baseDropType, ItemType currentType, ItemBox refItembox)
    {
        switch (baseDropType)
        {
            case ItemType.HeadPart:
                if (gameInfo.equipmentSystem.equipHead && currentType == ItemType.Potion)
                {
                    if (gameInfo.equipmentSystem.equipOrgans)
                    {
                        gameInfo.UpdateSanityText(-10f);
                        AudioController.Instance.PlayFX("Drinking");
                        gameInfo.equipmentSystem.UpdateEquipmentsDurable("Head");
                        gameInfo.equipmentSystem.UpdateEquipmentsDurable("Organs");
                    }
                    else
                    {
                        float randomHeal = Random.Range(2f, 5f);
                        gameInfo.UpdateSanityText(-randomHeal);
                        AudioController.Instance.PlayFX("Puke");
                        gameInfo.equipmentSystem.UpdateEquipmentsDurable();
                    }
                    currentSlotImg.color = errorParts ? gameInfo.equipmentSystem.errorColor : gameInfo.equipmentSystem.defaultColor;
                }
                else
                {
                    EquipParts(currentType, refItembox);
                    gameInfo.equipmentSystem.equipHead = true;
                    gameInfo.equipmentSystem.HeadPart = refItembox;
                    currentItemInSlot = refItembox;
                }
                break;
            case ItemType.HandsPart:
                EquipParts(currentType, refItembox);
                gameInfo.equipmentSystem.equipHands = true;
                gameInfo.equipmentSystem.HandsPart = refItembox;
                currentItemInSlot = refItembox;
                break;
            case ItemType.LegsPart:
                EquipParts(currentType, refItembox);
                gameInfo.equipmentSystem.equipLegs = true;
                gameInfo.equipmentSystem.LegsPart = refItembox;
                currentItemInSlot = refItembox;
                break;
            case ItemType.OrgansPart:
                EquipParts(currentType, refItembox);
                gameInfo.equipmentSystem.equipOrgans = true;
                gameInfo.equipmentSystem.OrgansPart = refItembox;
                currentItemInSlot = refItembox;
                break;
            case ItemType.Potion:
                break;
            case ItemType.None:
                break;
        }
    }

    private void EquipParts(ItemType _currentType, ItemBox refItembox)
    {
        AudioController.Instance.PlayFX("EquipMeat");
        gameInfo.equipmentSystem.DetectDropSlot(ItemType.None, false);

        if (allowDropType.Contains(_currentType))
        {
            if (currentItemInSlot != null && _currentType != ItemType.Potion)
            {
                currentItemInSlot.gameObject.transform.SetAsLastSibling();
                currentItemInSlot.gameObject.SetActive(true);
                currentItemInSlot = null;
            }

            switch (_currentType)
            {
                case ItemType.HeadPart:
                    equipmentDisplay.ActiveObject(equipmentDisplay.headObject);
                    equipmentDisplay.SetSliderValue(refItembox.currentItemDurable);
                    if (dropType == ItemType.HandsPart) gameInfo.equipmentSystem.ActiveCursor(gameInfo.equipmentSystem.HeadCursor);
                    break;
                case ItemType.HandsPart:
                    equipmentDisplay.ActiveObject(equipmentDisplay.handsObject);
                    equipmentDisplay.SetSliderValue(refItembox.currentItemDurable);
                    if (dropType == ItemType.HandsPart) gameInfo.equipmentSystem.ActiveCursor(gameInfo.equipmentSystem.HandsCursor);
                    break;
                case ItemType.LegsPart:
                    equipmentDisplay.ActiveObject(equipmentDisplay.legsObject);
                    equipmentDisplay.SetSliderValue(refItembox.currentItemDurable);
                    if (dropType == ItemType.HandsPart) gameInfo.equipmentSystem.ActiveCursor(gameInfo.equipmentSystem.LegsCursor);
                    break;
                case ItemType.OrgansPart:
                    equipmentDisplay.ActiveObject(equipmentDisplay.organsObject);
                    equipmentDisplay.SetSliderValue(refItembox.currentItemDurable);
                    if (dropType == ItemType.HandsPart) gameInfo.equipmentSystem.ActiveCursor(gameInfo.equipmentSystem.OrgansCursor);
                    break;
            }

            if (_currentType != ItemType.Potion)
            {
                errorParts = (_currentType != dropType) ? true : false;
                if (errorParts) gameInfo.ErrorPartsEquip++;
                currentSlotImg.color = (_currentType != dropType) ? gameInfo.equipmentSystem.errorColor : gameInfo.equipmentSystem.defaultColor;
                gameInfo.inventorySystem.ResetItemPosition();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (currentItemInSlot)
            {
                currentItemInSlot.UpdateDurable(currentItemInSlot.currentItemDurable);
                currentItemInSlot.DoMaskable(true);
                currentItemInSlot.gameObject.transform.SetAsLastSibling();
                currentItemInSlot.gameObject.SetActive(true);

                AudioController.Instance.PlayFX("Unequip");

                equipmentDisplay.DeactiveAll();
                equipmentDisplay.ResetSlider();

                currentSlotImg.color = gameInfo.equipmentSystem.defaultColor;
                currentItemInSlot = null;

                if (errorParts && gameInfo.ErrorPartsEquip > 0)
                {
                    gameInfo.ErrorPartsEquip--;
                    errorParts = false;
                }
                ResetBoolEquip();
                if (dropType == ItemType.HandsPart) gameInfo.equipmentSystem.DeactiveCursor();
                gameInfo.inventorySystem.ResetItemPosition();
            }
        }
    }

    public void PartsDestroyed()
    {
        if (currentItemInSlot)
        {
            currentItemInSlot.UpdateDurable(currentItemInSlot.currentItemDurable);

            AudioController.Instance.PlayFX("MeatDestroyed");

            equipmentDisplay.DeactiveAll();
            equipmentDisplay.ResetSlider();

            currentSlotImg.color = gameInfo.equipmentSystem.defaultColor;
            currentItemInSlot = null;

            if (errorParts && gameInfo.ErrorPartsEquip > 0)
            {
                gameInfo.ErrorPartsEquip--;
                errorParts = false;
            }
            ResetBoolEquip();
            if (dropType == ItemType.HandsPart) gameInfo.equipmentSystem.DeactiveCursor();
            gameInfo.inventorySystem.ResetItemPosition();
        }
    }

    private void ResetBoolEquip()
    {
        switch (dropType)
        {
            case ItemType.HeadPart:
                gameInfo.equipmentSystem.equipHead = false;
                gameInfo.equipmentSystem.HeadPart = null;
                break;
            case ItemType.HandsPart:
                gameInfo.equipmentSystem.equipHands = false;
                gameInfo.equipmentSystem.HandsPart = null;
                break;
            case ItemType.LegsPart:
                gameInfo.equipmentSystem.equipLegs = false;
                gameInfo.equipmentSystem.LegsPart = null;
                break;
            case ItemType.OrgansPart:
                gameInfo.equipmentSystem.equipOrgans = false;
                gameInfo.equipmentSystem.OrgansPart = null;
                break;
        }
    }
}