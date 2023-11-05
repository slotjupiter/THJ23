using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace THJ
{
    public class FurnitureInteract : MonoBehaviour
    {
        [TabGroup("Furniture Setup")] public string InteractSide = "Right";
        [TabGroup("Furniture Setup")] public SpriteRenderer furnitureImage;
        [TabGroup("Furniture Setup")] public FurnitureSO furnitureSO;
        [TabGroup("Furniture Setup")] public Transform interactBtnPos;
        [TabGroup("Furniture Setup"), ReadOnly] public List<ItemSO> searchItemList;
        [TabGroup("Furniture Setup")] public int maxStorageCount;
        int storageCount;
        public bool isOpen = false;
        public bool isFullySearch = false;
        public bool onlyNoneItems { get; set; }
        bool _changeToOpenSprite = false;
        bool canOpen = true;

        string currentFaceDirection;

        GameInfo gameInfo;
        GameObject _interactBtn;
        bool playerInFront = false;
        bool setUpButton = false;

        private void Awake()
        {
            gameInfo = FindObjectOfType<GameInfo>();

            storageCount = maxStorageCount;

            searchItemList = new();
            isOpen = false;
        }

        private void Start()
        {
            Initialized();
        }

        public void Initialized()
        {
            _interactBtn = gameInfo.mapManager.interactButton;

            if (gameObject.transform.localScale.x == 1 && InteractSide != "Left" || InteractSide == "Right")
                currentFaceDirection = "Right";
            else if (gameObject.transform.localScale.x == -1 || InteractSide == "Left")
                currentFaceDirection = "Left";
        }

        public void InitItems(ItemSO item)
        {
            if (storageCount > 0)
            {
                if (searchItemList.Count == storageCount) return;
                searchItemList.Add(item);
            }
        }

        private void Update()
        {
            if (gameInfo.movingPhase && _interactBtn.activeSelf
            || gameInfo.inventorySystem.openInventory && _interactBtn.activeSelf && playerInFront)
            {
                _interactBtn.SetActive(false);
            }
            else if (!gameInfo.inventorySystem.openInventory && !_interactBtn.activeSelf && playerInFront)
            {
                _interactBtn.SetActive(true);
            }

            if (isFullySearch && furnitureSO && furnitureImage && !_changeToOpenSprite)
            {
                furnitureImage.sprite = furnitureSO.openSprite[0];
                _changeToOpenSprite = true;
                _interactBtn.SetActive(false);
            }
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "Player" && _interactBtn && !gameInfo.movingPhase)
            {
                if (!playerInFront)
                    playerInFront = true;

                if (!_interactBtn.activeSelf)
                {
                    _interactBtn.SetActive(true);
                }
            }

            if (playerInFront && canOpen)
            {
                if (!setUpButton)
                {
                    setUpButton = true;
                    _interactBtn.transform.position = interactBtnPos.transform.position;
                    _interactBtn.gameObject.GetComponent<Button>().onClick.AddListener(Interact);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (playerInFront)
            {
                canOpen = true;
                playerInFront = false;
            }

            if (!playerInFront)
            {
                setUpButton = false;
                _interactBtn.gameObject.GetComponent<Button>().onClick.RemoveListener(Interact);
            }

            if (_interactBtn)
                if (_interactBtn.activeSelf)
                {
                    _interactBtn.SetActive(false);
                }
        }

        private void Interact()
        {
            canOpen = false;
            _interactBtn.SetActive(false);
            switch (currentFaceDirection)
            {
                case "Left":
                    gameInfo.tileCursorSystem.character.InteractBack();
                    break;
                case "Right":
                    gameInfo.tileCursorSystem.character.InteractLeft();
                    break;
            }

            AudioController.Instance.PlayFX("Popup");

            switch (furnitureSO.searchFurnitureType)
            {
                case FurnitureSO.SearchFurnitureType.Locker:
                    gameInfo.searchSystem.OpenLocker(searchItemList, this);
                    break;
                case FurnitureSO.SearchFurnitureType.Cabinet_A:
                    gameInfo.searchSystem.OpenCabinetA(searchItemList, this);
                    break;
                case FurnitureSO.SearchFurnitureType.Cabinet_B:
                    gameInfo.searchSystem.OpenCabinetB(searchItemList, this);
                    break;
                case FurnitureSO.SearchFurnitureType.Cabinet_C:
                    gameInfo.searchSystem.OpenCabinetC(searchItemList, this);
                    break;
                case FurnitureSO.SearchFurnitureType.Cabinet_D:
                    gameInfo.searchSystem.OpenCabinetD(searchItemList, this);
                    break;
            }

        }
    }

}

