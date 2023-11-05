using System.Collections;
using System.Collections.Generic;
using THJ;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform inventoryContent;
    public GameObject inventoryBoxPrefab;
    public List<ItemBox> itemsInInventory = new();

    public bool openInventory { get; private set; } = false;

    public void ActiveInventoryPanel()
    {
        bool active = inventoryPanel.activeSelf;
        active = !active;
        inventoryPanel.SetActive(active);
        openInventory = active;
        AudioController.Instance.PlayFX("Popup");
    }

    public void CreateItemBox(ItemSO targetItem)
    {
        GameObject _itemBOX = Instantiate(inventoryBoxPrefab, inventoryContent);
        ItemBox itemInfo = _itemBOX.GetComponent<ItemBox>();
        itemInfo.Initialized(targetItem, true);
        itemsInInventory.Add(itemInfo);
    }

    public void ResetItemPosition()
    {
        if (itemsInInventory.Count > 0)
            foreach (var item in itemsInInventory)
            {
                if (item.gameObject.activeSelf)
                    StartCoroutine(item.UpdateNewPositionOnGrid());
            }
    }
}
