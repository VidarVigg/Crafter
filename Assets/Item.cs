using UnityEngine;

internal abstract class Item : ScriptableObject
{
    [SerializeField]
    internal string itemName;
    [SerializeField]
    internal Sprite swordSprite;
    [SerializeField]
    internal Inventory ownerInventory;
    [SerializeField]
    internal int ownerInventoryIndex;

    internal virtual Item CreateInstance() {return null; }

}