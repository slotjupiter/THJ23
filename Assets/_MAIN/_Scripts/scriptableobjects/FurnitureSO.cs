using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "FurnitureSO", menuName = "Create Furniture", order = 0)]
public class FurnitureSO : ScriptableObject
{
    public string furnitureName;

    [TabGroup("Setup")]
    public FurnitureType furnitureType;

    [TabGroup("Setup"), ShowIf("@furnitureType == FurnitureType.SearchType")]
    public int storageCount = 0;

    [TabGroup("Setup"), PreviewField(Alignment = ObjectFieldAlignment.Center)]
    public Sprite defaultSprite;
    [TabGroup("Setup"), ShowIf("@furnitureType == FurnitureType.KeyType")]
    public Sprite finishedSprite;

    [TabGroup("Setup"), ShowIf("@furnitureType == FurnitureType.SearchType")]
    public List<Sprite> openSprite;

    public enum FurnitureType
    {
        SearchType, KeyType
    }
}
