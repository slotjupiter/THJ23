using System.Collections;
using System.Collections.Generic;
using THJ;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace THJ
{
    public class ItemButton : MonoBehaviour
    {
        [SerializeField] Button itemBtn;
        ItemSO refItemSO;
        Image itemImage;
        bool isKeyItem;
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
            furniture = thisFurniture;
            isKeyItem = iskeyItem;
            refItemSO = targetItem;
            if (!itemImage) itemImage = GetComponent<Image>();
            itemImage.sprite = refItemSO.itemSprite;
            itemImage.SetNativeSize();
            transform.gameObject.SetActive(true);

        }

        public void SetButton()
        {
            if (!gameInfo)
                gameInfo = FindObjectOfType<GameInfo>();


            if (gameInfo && refItemSO && furniture)
            {
                if (isKeyItem)
                {
                    switch (refItemSO.itemType)
                    {
                        case ItemType.HeadPart:
                            if (!gameInfo.collectHead)
                            {
                                AudioController.Instance.PlayFX("EquipMeat");
                                gameInfo.collectHead = true;
                                gameInfo.HeadSlot.SetActive(true);
                                gameInfo.uiController.OpenLore("Head");
                                gameInfo.UpdateProgressText(25);
                                gameInfo.collectKeys++;
                                if (furniture.searchItemList.Contains(refItemSO)) furniture.searchItemList.Remove(refItemSO);
                                transform.gameObject.SetActive(false);
                            }
                            break;
                        case ItemType.HandsPart:
                            if (!gameInfo.collectHands)
                            {
                                AudioController.Instance.PlayFX("EquipMeat");

                                gameInfo.collectHands = true;
                                gameInfo.HandsSlot.SetActive(true);
                                gameInfo.uiController.OpenLore("Arms");
                                gameInfo.UpdateProgressText(25);
                                gameInfo.collectKeys++;
                                if (furniture.searchItemList.Contains(refItemSO)) furniture.searchItemList.Remove(refItemSO);
                                transform.gameObject.SetActive(false);
                            }
                            break;
                        case ItemType.LegsPart:
                            if (!gameInfo.collectLegs)
                            {
                                AudioController.Instance.PlayFX("EquipMeat");

                                gameInfo.collectLegs = true;
                                gameInfo.LegsSlot.SetActive(true);
                                gameInfo.uiController.OpenLore("Legs");
                                gameInfo.UpdateProgressText(25);
                                gameInfo.collectKeys++;
                                if (furniture.searchItemList.Contains(refItemSO)) furniture.searchItemList.Remove(refItemSO);
                                transform.gameObject.SetActive(false);
                            }
                            break;
                        case ItemType.OrgansPart:
                            if (!gameInfo.collectOrgans)
                            {
                                AudioController.Instance.PlayFX("EquipMeat");

                                gameInfo.collectOrgans = true;
                                gameInfo.OrgansSlot.SetActive(true);
                                gameInfo.uiController.OpenLore("Organs");
                                gameInfo.UpdateProgressText(25);
                                gameInfo.collectKeys++;
                                if (furniture.searchItemList.Contains(refItemSO)) furniture.searchItemList.Remove(refItemSO);
                                transform.gameObject.SetActive(false);
                            }
                            break;
                    }
                }
                else
                {
                    gameInfo.UpdateSanityText(-0.5f);
                    if (furniture.searchItemList.Contains(refItemSO)) furniture.searchItemList.Remove(refItemSO);

                    transform.gameObject.SetActive(false);
                }

                if (furniture.searchItemList.Count == 0) furniture.isFullySearch = true;
            }

        }
    }

}
