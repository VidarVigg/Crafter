using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalInventoryController :MonoBehaviour, IInventoryService
{

    [SerializeField]
    private InventoryManager[] activeInventories;

    private Dictionary<InventoryTypes, InventoryManager> inventoryDictionary = new Dictionary<InventoryTypes, InventoryManager>();

    internal void AddInventory(InventoryManager inventory, InventoryTypes key)
    {
        for (int i = 0; i < activeInventories.Length; i++)
        {
            if (!activeInventories[i])
            {
                activeInventories[i] = inventory;
                break;
            }
        }
        inventoryDictionary.Add(key, inventory);
    }

    internal void RemoveInventory(InventoryManager inventory, InventoryTypes key)
    {
        for (int i = 0; i < activeInventories.Length; i++)
        {
            if (activeInventories[i] == inventory)
            {
                activeInventories[i] = null;
                break;
            }
        }
        inventoryDictionary.Remove(key);
    }

    internal InventoryManager GetSpecificInventory(InventoryTypes key)
    {
        InventoryManager inventory = inventoryDictionary[key];
        return inventory;
    }

    public void AddToActiveInventories(InventoryManager inventory, InventoryTypes key)
    {
        AddInventory(inventory, key);
    }

    public void RemoveFromActiveInventories(InventoryManager inventory, InventoryTypes key)
    {
        RemoveInventory(inventory, key);
    }

    InventoryManager IInventoryService.GetInventoryByKey(InventoryTypes key)
    {
       return GetSpecificInventory(key);
    }
}
public enum InventoryTypes
{

    PlayerInventory,
    BluePrintInventory,
    CraftingInventory,
    None,
    Container,
    BluePrintInput,
}
