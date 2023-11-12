using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "FurnitureSO", menuName = "APART/Create Furniture", order = 0)]
public class FurnitureSO : SerializedScriptableObject
{
    public string furnitureName;

    [TabGroup("Setup")]
    public FurnitureType furnitureType;

    [TabGroup("Setup")]
    public SearchFurnitureType searchFurnitureType = SearchFurnitureType.Locker;

    [TabGroup("Setup"), PreviewField(Alignment = ObjectFieldAlignment.Center)]
    public Sprite defaultSprite;
    [TabGroup("Setup"), ShowIf("@furnitureType == FurnitureType.KeyType"), PreviewField(Alignment = ObjectFieldAlignment.Center)]
    public Sprite finishedSprite;

    [TabGroup("Setup"), ShowIf("@furnitureType == FurnitureType.SearchType"), PreviewField(Alignment = ObjectFieldAlignment.Center)]
    public List<Sprite> openSprite;

    public enum FurnitureType
    {
        SearchType, KeyType
    }

    public enum SearchFurnitureType
    {
        Locker, Cabinet_A, Cabinet_B, Cabinet_C, Cabinet_D, ElectricPole, BookShelf, TiltBed, Scales
    }
}
