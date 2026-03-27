using UnityEngine;

[CreateAssetMenu(menuName = "Wardrobe/ClothingItem")]
public class ClothingItemData : ScriptableObject
{
    public string itemName;
    public Sprite previewSprite;
    public string rigMeshName;
}