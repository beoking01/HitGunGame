using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public enum ItemType { Normal, Weapon, Consumable }

    [Header("Economy")]
    public float price;
    public float sellPrice;

    [Header("Behavior")]
    public ItemType itemType;

    [Header("Consumable")]
    public float healAmount;
}
