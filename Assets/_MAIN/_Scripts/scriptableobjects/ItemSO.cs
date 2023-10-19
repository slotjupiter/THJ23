using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "APART/Create Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public bool KeyItem;
    public ItemType itemType;
    [PreviewField(Alignment = ObjectFieldAlignment.Center)] public Sprite itemSprite;
    public string itemName;
    [TextArea] public string itemDescription;
}

public enum ItemType
{
    HeadPart, HandsPart, LegsPart, OrgansPart, Potion, Book, Fuse
}
