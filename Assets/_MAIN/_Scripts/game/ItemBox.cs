using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace THJ
{
    public class ItemBox : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField, TabGroup("Item UI")] private Image itemimage;
        [SerializeField, TabGroup("Item UI")] private TMP_Text itemName;
        [SerializeField, TabGroup("Item UI")] private RectTransform itemboxRect;
        [SerializeField, TabGroup("Item UI")] private Slider itemSlider;
        [SerializeField, TabGroup("Item BG")] private Image itemBoxBG1;
        [SerializeField, TabGroup("Item BG")] private Image itemBoxBG2;

        [TabGroup("Item UI")] public List<Image> maskImage;
        [TabGroup("Color")] public Color defaultColor;
        [TabGroup("Color")] public Color keyItemColor;
        [TabGroup("Color")] public Color potionColor;

        public float currentItemDurable { get; set; } = 100f;

        public ItemSO itemData { get; private set; }
        public bool isKeyItem { get; private set; } = false;
        public bool isPotion { get; private set; } = false;

        GameInfo gameInfo;
        Vector2 startPlace;

        private void Awake()
        {
            gameInfo = FindObjectOfType<GameInfo>();
        }

        private void Start()
        {
            DoMaskable(true);
            StartCoroutine(UpdateNewPositionOnGrid());
        }

        public void Initialized(ItemSO itemSO, bool firstTime = false)
        {
            itemData = itemSO;
            itemimage.sprite = itemData.itemSprite;
            itemName.text = itemData.itemName;
            isKeyItem = itemData.KeyItem;
            DoMaskable(true);

            if (!firstTime)
                itemSlider.value = itemData.itemType != ItemType.Potion ? currentItemDurable : 0f;
            else
            {
                currentItemDurable = 100f;
                itemSlider.value = itemData.itemType != ItemType.Potion ? currentItemDurable : 0f;
            }

            SetBGColor(itemData.itemType);
        }

        public void UpdateDurable(float value)
        {
            itemSlider.value = value;
        }

        public IEnumerator UpdateNewPositionOnGrid()
        {
            yield return new WaitUntil(() => itemboxRect.GetComponent<RectTransform>().anchoredPosition != Vector2.zero);
            startPlace = itemboxRect.GetComponent<RectTransform>().anchoredPosition;
        }

        public void ReturnToPlace()
        {
            DoMaskable(true);
            itemboxRect.anchoredPosition = startPlace;
        }

        private void SetBGColor(ItemType itemType)
        {
            if (itemType == ItemType.Potion)
            {
                itemBoxBG1.color = potionColor;
                itemBoxBG2.color = potionColor;
            }
            else if (isKeyItem)
            {
                itemBoxBG1.color = keyItemColor;
                itemBoxBG2.color = keyItemColor;
            }
            else
            {
                itemBoxBG1.color = defaultColor;
                itemBoxBG2.color = defaultColor;
            }
        }

        public void DoMaskable(bool val)
        {
            foreach (var img in maskImage)
            {
                img.maskable = val;

            }
            itemName.maskable = val;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (gameInfo) gameInfo.CurrentItemBox = this;

            DoMaskable(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (gameInfo)
            {
                itemboxRect.anchoredPosition += eventData.delta / gameInfo.mainCanvas.scaleFactor;
                if (eventData.pointerDrag != null)
                {
                    ItemType currentType = eventData.pointerDrag.GetComponent<ItemBox>().itemData.itemType;
                    gameInfo.equipmentSystem.DetectDropSlot(currentType, true);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (gameInfo)
                gameInfo.equipmentSystem.DetectDropSlot(ItemType.None, false);

            ReturnToPlace();
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }
    }

}
