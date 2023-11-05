using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace THJ
{
    public class SearchFurniture : MonoBehaviour
    {
        [TabGroup("Close Setup")] public Button openButton;
        [TabGroup("Close Setup")] public GameObject closeObject;
        [TabGroup("Open Setup")] public GameObject openObject;
        [TabGroup("Open Setup")] public List<GameObject> itemsPositionButton;
        [TabGroup("Open Setup")] List<Button> itemsPositionBtn;

        FurnitureInteract currentFurniture;

        public void InitItemsPosition(List<ItemSO> itemList, FurnitureInteract furniture)
        {
            itemsPositionBtn = new();
            currentFurniture = furniture;
            if (currentFurniture)
            {
                if (!currentFurniture.isOpen && !currentFurniture.isFullySearch)
                {
                    openButton.onClick.AddListener(() =>
                       {
                           switch (currentFurniture.furnitureSO.searchFurnitureType)
                           {
                               case FurnitureSO.SearchFurnitureType.Locker:
                                   AudioController.Instance.PlayFX("LockerOpen");
                                   break;
                               default:
                                   AudioController.Instance.PlayFX("CabinetOpen");
                                   break;
                           }
                           if (currentFurniture != null) currentFurniture.isOpen = true;
                           if (itemList.Count == 0 && !currentFurniture.isFullySearch
                           || !currentFurniture.isFullySearch && currentFurniture.onlyNoneItems) currentFurniture.isFullySearch = true;
                           closeObject.SetActive(false);
                           openObject.SetActive(true);
                       });
                }

                foreach (var btn in itemsPositionButton)
                {
                    btn.SetActive(false);
                }

                if (!currentFurniture.isOpen && itemList.Count > 0)
                {
                    int itemIndex = 0;

                    closeObject.SetActive(true);

                    if (itemsPositionButton.Count > 0)
                    {
                        for (int i = 0; i < itemsPositionButton.Count; i++)
                        {
                            if (itemIndex < itemList.Count)
                            {
                                itemsPositionButton[i].gameObject.GetComponent<ItemButton>().Initialize(itemList[itemIndex], itemList[itemIndex].KeyItem, currentFurniture);
                                itemIndex++;
                            }
                        }
                    }
                }
                else if (!currentFurniture.isOpen && itemList.Count == 0)
                {
                    closeObject.SetActive(true);
                    openObject.SetActive(true);

                    if (itemsPositionButton.Count > 0)
                    {
                        for (int j = 0; j < itemsPositionButton.Count; j++)
                        {
                            itemsPositionButton[j].SetActive(false);
                        }
                    }
                }
                else if (currentFurniture.isOpen && itemList.Count > 0)
                {
                    closeObject.SetActive(false);
                    openObject.SetActive(true);
                    int itemIndex = 0;

                    if (itemsPositionButton.Count > 0)
                    {
                        for (int i = 0; i < itemsPositionButton.Count; i++)
                        {
                            if (itemIndex < itemList.Count)
                            {
                                itemsPositionButton[i].gameObject.GetComponent<ItemButton>().Initialize(itemList[itemIndex], itemList[itemIndex].KeyItem, currentFurniture);
                                itemIndex++;
                            }
                        }
                    }
                }
                else if (currentFurniture.isOpen && itemList.Count == 0 || currentFurniture.isOpen && furniture.onlyNoneItems)
                {
                    currentFurniture.isFullySearch = true;

                    closeObject.SetActive(false);
                    openObject.SetActive(true);

                    if (itemsPositionButton.Count > 0)
                    {
                        for (int j = 0; j < itemsPositionButton.Count; j++)
                        {
                            itemsPositionButton[j].SetActive(false);
                        }
                    }
                }
            }
        }

        public void CloseFurniture()
        {
            currentFurniture = null;
        }

    }
}
