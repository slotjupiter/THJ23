using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using THJ;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SearchSystem : MonoBehaviour
{

    [TabGroup("UI")] public TMP_Text textHeader;
    [TabGroup("UI")] public List<string> randomTextHeader;


    public GameObject SearchPanelUI;

    public GameObject LockerSearchUI;
    SearchFurniture _lockerSearch;

    public GameObject CabinetASearchUI;
    SearchFurniture _cabinetASearch;

    public GameObject CabinetBSearchUI;
    SearchFurniture _cabinetBSearch;

    public GameObject CabinetCSearchUI;
    SearchFurniture _cabinetCSearch;

    public GameObject CabinetDSearchUI;
    SearchFurniture _cabinetDSearch;

    [TabGroup("Book")] public GameObject bookPanel;
    [TabGroup("Book")] public Image bookImage;
    [TabGroup("Book")] public Sprite book1SpriteOpen;
    [TabGroup("Book")] public Sprite book2SpriteOpen;
    [TabGroup("Book")] public Sprite book3SpriteOpen;


    SearchFurniture currentSearch;
    GameObject currentObjectOpen;
    GameInfo gameInfo;

    private void Start()
    {
        SearchPanelUI.SetActive(false);
        _lockerSearch = LockerSearchUI.gameObject.GetComponent<SearchFurniture>();
        _cabinetASearch = CabinetASearchUI.gameObject.GetComponent<SearchFurniture>();
        _cabinetBSearch = CabinetBSearchUI.gameObject.GetComponent<SearchFurniture>();
        _cabinetCSearch = CabinetCSearchUI.gameObject.GetComponent<SearchFurniture>();
        _cabinetDSearch = CabinetDSearchUI.gameObject.GetComponent<SearchFurniture>();
        gameInfo = FindObjectOfType<GameInfo>();
    }

    public void ClosePanel()
    {
        if (currentObjectOpen && currentSearch)
        {
            currentSearch.CloseFurniture();
            currentObjectOpen.SetActive(false);
            currentObjectOpen = null;
        }

        AudioController.Instance.PlayFX("Popup");
        SearchPanelUI.SetActive(false);
    }

    public void CloseBook()
    {
        AudioController.Instance.PlayFX("CloseBook");
        bookPanel.SetActive(false);
    }

    public void OpenBook(ItemType bookType)
    {
        switch (bookType)
        {
            case ItemType.Book1:
                bookImage.sprite = book1SpriteOpen;
                bookPanel.SetActive(true);
                break;
            case ItemType.Book2:
                bookImage.sprite = book2SpriteOpen;
                bookPanel.SetActive(true);
                break;
            case ItemType.Book3:
                bookImage.sprite = book3SpriteOpen;
                bookPanel.SetActive(true);
                break;
        }
    }

    private void RandomTextHeader()
    {
        if (randomTextHeader.Count > 0)
        {
            int indexRandom = Random.Range(0, randomTextHeader.Count - 1);
            textHeader.text = randomTextHeader[indexRandom];
        }
    }

    private void RandomTextHeader(TMP_Text header)
    {
        if (randomTextHeader.Count > 0)
        {
            int indexRandom = Random.Range(0, randomTextHeader.Count - 1);
            header.text = randomTextHeader[indexRandom];
        }
    }

    public void OpenLocker(List<ItemSO> searchItemList, FurnitureInteract furniture)
    {
        RandomTextHeader();
        _lockerSearch.InitItemsPosition(searchItemList, furniture);
        currentSearch = _lockerSearch;
        SearchPanelUI.SetActive(true);
        LockerSearchUI.SetActive(true);
        currentObjectOpen = LockerSearchUI;
    }

    public void OpenCabinetA(List<ItemSO> searchItemList, FurnitureInteract furniture)
    {
        RandomTextHeader();
        _cabinetASearch.InitItemsPosition(searchItemList, furniture);
        currentSearch = _cabinetASearch;
        SearchPanelUI.SetActive(true);
        CabinetASearchUI.SetActive(true);
        currentObjectOpen = CabinetASearchUI;
    }

    public void OpenCabinetB(List<ItemSO> searchItemList, FurnitureInteract furniture)
    {
        RandomTextHeader();

        _cabinetBSearch.InitItemsPosition(searchItemList, furniture);
        currentSearch = _cabinetBSearch;
        SearchPanelUI.SetActive(true);
        CabinetBSearchUI.SetActive(true);
        currentObjectOpen = CabinetBSearchUI;
    }

    public void OpenCabinetC(List<ItemSO> searchItemList, FurnitureInteract furniture)
    {
        RandomTextHeader();

        _cabinetCSearch.InitItemsPosition(searchItemList, furniture);
        currentSearch = _cabinetCSearch;
        SearchPanelUI.SetActive(true);
        CabinetCSearchUI.SetActive(true);
        currentObjectOpen = CabinetCSearchUI;
    }

    public void OpenCabinetD(List<ItemSO> searchItemList, FurnitureInteract furniture)
    {
        RandomTextHeader();

        _cabinetDSearch.InitItemsPosition(searchItemList, furniture);
        currentSearch = _cabinetDSearch;
        SearchPanelUI.SetActive(true);
        CabinetDSearchUI.SetActive(true);
        currentObjectOpen = CabinetDSearchUI;
    }

    public void OpenElectricPole()
    {
        if (gameInfo.collectOrgansKey) return;

        if (gameInfo.equipmentSystem.equipHands)
        {
            RandomTextHeader(gameInfo.minigamesSystem.organsMinigame.gameHeader);
            gameInfo.minigamesSystem.organsMinigame.organsGamePanel.SetActive(true);
        }
        else
            gameInfo.dialogueSystem.SetForceText("I need 'Hands'");
    }

    public void OpenBookShelf()
    {
        if (gameInfo.CollectHeadKey) return;

        if (gameInfo.equipmentSystem.equipHands)
        {
            RandomTextHeader(gameInfo.minigamesSystem.headMinigame.gameHeader);
            gameInfo.minigamesSystem.headMinigame.OpenHeadMinigame();
        }
        else
            gameInfo.dialogueSystem.SetForceText("I need 'Hands'");
    }

    public void OpenTiltBed()
    {
        if (gameInfo.collectLegsKey) return;

        if (gameInfo.equipmentSystem.equipHands)
        {
            RandomTextHeader(gameInfo.minigamesSystem.legsMinigame.gameHeader);
            gameInfo.minigamesSystem.legsMinigame.OpenLegsMinigame();
        }
        else
            gameInfo.dialogueSystem.SetForceText("I need 'Hands'");
    }

    public void OpenScales()
    {
        if (gameInfo.collectHandsKey) return;

        if (gameInfo.equipmentSystem.equipHands)
        {
            RandomTextHeader(gameInfo.minigamesSystem.armsMinigame.gameHeader);
            gameInfo.minigamesSystem.armsMinigame.OpenArmsMinigame();
        }
        else
            gameInfo.dialogueSystem.SetForceText("I need 'Hands'");
    }
}