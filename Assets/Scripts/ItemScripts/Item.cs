using System;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField]
    internal string itemName;
    [SerializeField]
    internal string itemType = "";
    [SerializeField]
    internal string key;
    internal InventoryTypes ownerInventory;
    [SerializeField]
    internal int inventoryIndex;
    [SerializeField]
    internal int itemCopies = 1;
    [SerializeField]
    internal bool stackable = true;
    internal Sprite itemSprite;
    internal Sprite background;

    internal abstract Item CreateInstance(InventoryTypes inventory);
    internal abstract ItemSaveInfo GetInstanceSaveInfo(Item item);

}
