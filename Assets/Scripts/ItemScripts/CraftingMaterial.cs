using UnityEngine;
using System;

[CreateAssetMenu]
public class CraftingMaterial : Item
{

    [SerializeField]
    private string itemType = ItemType.Material.ToString();

    public enum CraftingMaterialTypes
    {
        Resin,
        CoalPowder,
        DeadWood
    }

    public CraftingMaterialTypes craftingMaterialType;

    internal override Item CreateInstance(InventoryTypes inventory = InventoryTypes.None)
    {
        CraftingMaterial newCraftingMaterial = CreateInstance<CraftingMaterial>();
        newCraftingMaterial.craftingMaterialType = craftingMaterialType;
        newCraftingMaterial.itemName = name;
        newCraftingMaterial.ownerInventory = inventory;
        newCraftingMaterial.itemSprite = SpriteHandler.GetInstance().GetItemSprite(craftingMaterialType.ToString());
        newCraftingMaterial.background = SpriteHandler.GetInstance().GetBackgroundSprite("Common");
        newCraftingMaterial.inventoryIndex = inventoryIndex;
        newCraftingMaterial.key = craftingMaterialType.ToString();
        newCraftingMaterial.itemType = itemType;
        newCraftingMaterial.stackable = stackable;
        return newCraftingMaterial;
    }

    internal override ItemSaveInfo GetInstanceSaveInfo(Item item)
    {
        CraftingMaterialSaveInfo craftingMaterialSaveInfo = new CraftingMaterialSaveInfo();
        CraftingMaterial craftingMaterial = item as CraftingMaterial;
        craftingMaterialSaveInfo.NAME = craftingMaterial.itemName;
        craftingMaterialSaveInfo.KEY = craftingMaterial.key;
        craftingMaterialSaveInfo.ITEMTYPE = craftingMaterial.itemType;
        craftingMaterialSaveInfo.OWNER = craftingMaterial.ownerInventory.ToString();
        craftingMaterialSaveInfo.INDEX = craftingMaterial.inventoryIndex;
        craftingMaterialSaveInfo.STACKABLE = craftingMaterial.stackable;
        return craftingMaterialSaveInfo;
    }

    internal CraftingMaterial GetSavedCraftingMaterial(CraftingMaterialSaveInfo itemSaveInfo)
    {
        CraftingMaterial craftingMaterial = CreateInstance<CraftingMaterial>();
        craftingMaterial.itemName = itemSaveInfo.NAME;
        InventoryTypes owner;
        Enum.TryParse("Active", out owner);
        craftingMaterial.ownerInventory = owner;
        craftingMaterial.key = itemSaveInfo.KEY;
        craftingMaterial.stackable = itemSaveInfo.STACKABLE;
        craftingMaterial.itemSprite = SpriteHandler.GetInstance().GetItemSprite(craftingMaterialType.ToString());
        craftingMaterial.background = SpriteHandler.GetInstance().GetBackgroundSprite("Common");
        return craftingMaterial;
    }
    
}

public class CraftingMaterialSaveInfo : ItemSaveInfo { }