using System.Collections;
using System.Collections.Generic;
using System.Linq;
using THJ;
using UnityEngine;
using UnityEngine.UI;

namespace THJ
{
    public class ItemButton : MonoBehaviour
    {
        [SerializeField] Button itemBtn;

        bool isKeyItem;
        ItemSO refItemSO;
        Image itemImage;
        GameInfo gameInfo;
        FurnitureInteract furniture;

        private void Start()
        {
            if (!gameInfo)
                gameInfo = FindObjectOfType<GameInfo>();

            itemImage = GetComponent<Image>();
        }

        public void Initialize(ItemSO targetItem, bool iskeyItem, FurnitureInteract thisFurniture)
        {
            refItemSO = targetItem;
            furniture = thisFurniture;
            if (refItemSO.itemType != ItemType.None)
            {
                isKeyItem = iskeyItem;
                if (!itemImage) itemImage = GetComponent<Image>();
                itemImage.sprite = refItemSO.itemSprite;
                itemImage.SetNativeSize();
                transform.gameObject.SetActive(true);
            }
        }

        public void SetButton()
        {
            if (refItemSO && refItemSO.itemType != ItemType.None && !IsBook(refItemSO.itemType))
            {
                if (!gameInfo)
                    gameInfo = FindObjectOfType<GameInfo>();

                if (gameInfo && refItemSO && furniture)
                {
                    AudioController.Instance.PlayFX("Pickup");
                    gameInfo.inventorySystem.CreateItemBox(refItemSO);

                    if (furniture.searchItemList.Contains(refItemSO))
                        furniture.searchItemList.Remove(refItemSO);

                    transform.gameObject.SetActive(false);

                    furniture.onlyNoneItems = furniture.searchItemList.All(item => item.itemType == ItemType.None);

                    if (furniture.searchItemList.Count == 0 || furniture.onlyNoneItems)
                        furniture.isFullySearch = true;
                }
            }
            else if (IsBook(refItemSO.itemType) && refItemSO)
            {
                if (!gameInfo)
                    gameInfo = FindObjectOfType<GameInfo>();

                AudioController.Instance.PlayFX("FlipBook");
                gameInfo.searchSystem.OpenBook(refItemSO.itemType);
            }
        }

        public bool IsBook(ItemType itemType)
        {
            if (itemType == ItemType.Book1 || itemType == ItemType.Book2 || itemType == ItemType.Book3)
                return true;
            else return false;
        }
    }
}
