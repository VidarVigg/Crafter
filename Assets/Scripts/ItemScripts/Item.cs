using System;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField]
    internal string itemName;
    internal Sprite itemSprite;
    [SerializeField]
    internal InventoryTypes ownerInventory;
    [SerializeField]
    internal int inventoryIndex;
    internal Sprite background;
    [SerializeField]
    internal string key;
    [SerializeField]
    internal bool stackable = true;


    internal abstract Item CreateInstance(InventoryTypes inventory);
    internal abstract ItemSaveInfo GetInstanceSaveInfo(Item item);

}

public enum ItemType
{
    Consumable,
    Material,
    BluePrint,
    Weapon,
    None
}
