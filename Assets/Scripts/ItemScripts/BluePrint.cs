﻿using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BluePrint : Item
{

    public int numberOfRequiredComponents;
    public List<string> requiredComponents = new List<string>();
    public string output;
    public bool learnt;
    public enum BluePrintType
    {
        Torch,
        Sword,
    }
    public BluePrintType bluePrintType;
    internal string itemType = ItemType.BluePrint.ToString();


    internal override Item CreateInstance(InventoryTypes inventory)
    {
        BluePrint newBluePrint = CreateInstance<BluePrint>();
        newBluePrint.itemName = itemName;
        newBluePrint.itemType = itemType;
        newBluePrint.bluePrintType = bluePrintType;
        newBluePrint.itemSprite = SpriteHandler.GetInstance().GetItemSprite("BluePrint");// Should be specific
        newBluePrint.background = SpriteHandler.GetInstance().GetBackgroundSprite("Common");
        newBluePrint.ownerInventory = inventory;
        newBluePrint.inventoryIndex = inventoryIndex;
        newBluePrint.key = key;
        newBluePrint.stackable = stackable;
        newBluePrint.requiredComponents = requiredComponents;
        newBluePrint.numberOfRequiredComponents = numberOfRequiredComponents;
        newBluePrint.output = output;
        return newBluePrint;
    }

    internal override ItemSaveInfo GetInstanceSaveInfo(Item item)
    {
        BluePrint bluePrint = item as BluePrint;
        BluePrintSaveInfo newSaveInfo = new BluePrintSaveInfo();

        newSaveInfo.NAME = bluePrint.itemName;
        newSaveInfo.KEY = bluePrint.key;
        newSaveInfo.ITEMTYPE = itemType;
        newSaveInfo.OWNER = bluePrint.ownerInventory.ToString();
        Debug.Log("Got Save Info " + newSaveInfo.OWNER);
        newSaveInfo.INDEX = bluePrint.inventoryIndex;
        newSaveInfo.STACKABLE = bluePrint.stackable;
        newSaveInfo.REQUIREDCOMPONENTS = bluePrint.requiredComponents.ToArray();
        newSaveInfo.NUMBEROFREQUIREDCOMPONENTS = bluePrint.numberOfRequiredComponents;
        newSaveInfo.OUTPUT = bluePrint.output;
        return newSaveInfo;
    }

    internal BluePrint GetSavedBluePrint(BluePrintSaveInfo itemSaveInfo)
    {
        BluePrint savedBluePrint = CreateInstance<BluePrint>();
        savedBluePrint.itemName = itemSaveInfo.NAME;
        savedBluePrint.inventoryIndex = itemSaveInfo.INDEX;
        InventoryTypes owner;
        Enum.TryParse("Active", out owner);
        savedBluePrint.ownerInventory = owner;
        savedBluePrint.key = itemSaveInfo.KEY;
        savedBluePrint.stackable = itemSaveInfo.STACKABLE;
        savedBluePrint.numberOfRequiredComponents = itemSaveInfo.NUMBEROFREQUIREDCOMPONENTS;
        savedBluePrint.requiredComponents = new List<string>();   
        for (int i = 0; i < itemSaveInfo.REQUIREDCOMPONENTS.Length; i++)
        {
            savedBluePrint.requiredComponents.Add(itemSaveInfo.REQUIREDCOMPONENTS[i]);
        }
        savedBluePrint.output = itemSaveInfo.OUTPUT;
        savedBluePrint.itemSprite = SpriteHandler.GetInstance().GetItemSprite("BluePrint");//Should be specific based on type
        savedBluePrint.background = SpriteHandler.GetInstance().GetBackgroundSprite("Common");
        return savedBluePrint;
    }

    public virtual void HandleBluePrint()
    {
        if (!learnt)
        {
            InventoryManager blueprintInventory = ServiceLocator.InventoryService.GetInventoryByKey(InventoryTypes.BluePrintInventory);
            for (int i = 0; i < blueprintInventory.Inventory.items.Length; i++)
            {
                if (blueprintInventory.Inventory.items[i] == null)
                {
                    blueprintInventory.AddItem(this, i);
                    ownerInventory = InventoryTypes.BluePrintInventory;
                    return;
                }
            }

        }
    }

}