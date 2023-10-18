using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using THJ;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureInteract : MonoBehaviour
{
    [TabGroup("Furniture Setup")] public FurnitureSO furnitureSO;
    [TabGroup("Furniture Setup")] public Transform interactBtnPos;
    string currentFaceDirection;

    GameInfo gameInfo;
    GameObject _interactBtn;
    bool playerInFront = false;
    bool setUpButton = false;
    int _storageCount;

    private void Start()
    {
        Initialized();
    }

    public void Initialized()
    {
        gameInfo = FindObjectOfType<GameInfo>();
        _interactBtn = gameInfo.mapManager.interactButton;

        if (furnitureSO)
            _storageCount = furnitureSO.storageCount;

        if (gameObject.transform.localScale.x == 1)
            currentFaceDirection = "Right";
        else if (gameObject.transform.localScale.x == -1)
            currentFaceDirection = "Left";

    }

    private void Update()
    {
        if (gameInfo.movingPhase && _interactBtn.activeSelf)
        {
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
                _interactBtn.SetActive(true);
        }

        if (playerInFront)
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
        if (_interactBtn)
            if (_interactBtn.activeSelf)
            {
                _interactBtn.SetActive(false);
            }

        if (playerInFront)
            playerInFront = false;

        if (!playerInFront)
        {
            setUpButton = false;
            _interactBtn.gameObject.GetComponent<Button>().onClick.RemoveListener(Interact);
        }
    }

    private void Interact()
    {
        Debug.Log("CLICK CLICK");

        switch (currentFaceDirection)
        {
            case "Left":
                gameInfo.tileCursorSystem.character.InteractBack();
                break;
            case "Right":
                gameInfo.tileCursorSystem.character.InteractLeft();
                break;
        }

    }
}
