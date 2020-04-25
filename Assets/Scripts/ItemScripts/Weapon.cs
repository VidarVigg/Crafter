using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
internal class Weapon : Item
{
    public enum WeaponTypes
    {
        Sword,
        Axe,
        Bow,
        None,
    }

    [SerializeField]
    internal int damage;

    [SerializeField]
    internal string itemType = ItemType.Weapon.ToString();

    public WeaponTypes weaponType;

    internal override Item CreateInstance(InventoryTypes inventory = InventoryTypes.None)
    {

        Weapon newWeapon = CreateInstance<Weapon>();
        newWeapon.itemType = itemType;
        newWeapon.weaponType = weaponType;
        newWeapon.itemName = itemName;
        newWeapon.damage = damage;
        newWeapon.ownerInventory = inventory;
        newWeapon.inventoryIndex = inventoryIndex;
        newWeapon.key = weaponType.ToString();
        newWeapon.stackable = stackable;
        newWeapon.itemSprite = SpriteHandler.GetInstance().GetItemSprite(weaponType.ToString());
        newWeapon.background = SpriteHandler.GetInstance().GetBackgroundSprite("Common");
        return newWeapon;
    }

    internal override ItemSaveInfo GetInstanceSaveInfo(Item item)
    {
        Weapon sword = item as Weapon;
        WeaponSaveInfo newSaveInfo = new WeaponSaveInfo();
        newSaveInfo.ITEMTYPE = itemType;
        newSaveInfo.NAME = sword.itemName;
        newSaveInfo.DAMAGE = sword.damage;
        newSaveInfo.OWNER = sword.ownerInventory.ToString();
        newSaveInfo.INDEX = sword.inventoryIndex;
        newSaveInfo.KEY = sword.key;
        newSaveInfo.STACKABLE = sword.stackable;
        return newSaveInfo;
    }

    internal Weapon GetSavedWeapon(WeaponSaveInfo itemSaveInfo)
    {
        Weapon savedWeapon = CreateInstance<Weapon>();
        savedWeapon.itemType = itemSaveInfo.ITEMTYPE;
        WeaponTypes wT;
        Enum.TryParse("Active", out wT);
        savedWeapon.weaponType = wT;
        savedWeapon.itemName = itemSaveInfo.NAME;
        savedWeapon.damage = itemSaveInfo.DAMAGE;
        savedWeapon.inventoryIndex = itemSaveInfo.INDEX;
        InventoryTypes owner;
        Enum.TryParse("Active", out owner);
        savedWeapon.ownerInventory = owner;
        savedWeapon.key = itemSaveInfo.KEY;
        savedWeapon.stackable = itemSaveInfo.STACKABLE;
        savedWeapon.itemSprite = SpriteHandler.GetInstance().GetItemSprite(weaponType.ToString());
        savedWeapon.background = SpriteHandler.GetInstance().GetBackgroundSprite("Common");
        return savedWeapon;
    }

}

