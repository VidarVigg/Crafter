using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInventory : InventoryManager
{

    InventoryTypes type = InventoryTypes.CraftingInventory;

    protected override void Awake()
    {
        inventoryType = InventoryTypes.CraftingInventory;
    }

    [SerializeField]
    private List<string> requiredComponents = new List<string>();

    BluePrint activeBlueprint;

    protected override void Start()
    {
        //base.Start();
        onUnloadedBluePrint += ResetCraftingInventory;
    }

    public void Initialize(BluePrint bluePrint)
    {
        itemInventory = new Inventory(bluePrint.numberOfRequiredComponents);
        InitializeInventory(bluePrint.numberOfRequiredComponents);
        ServiceLocator.InventoryService.AddToActiveInventories(this, type);
        activeBlueprint = bluePrint;
        for (int i = 0; i < bluePrint.requiredComponents.Count; i++)
        {
            requiredComponents.Add(bluePrint.requiredComponents[i]);
        }
    }

    protected override void TryToPutDown(int index, Item item)
    {

        CraftingMaterial material = null;

        if (item is CraftingMaterial)
        {
            material = item as CraftingMaterial;
            for (int i = 0; i < requiredComponents.Count; i++)
            {
                if (material.key == requiredComponents[i])
                {
                    base.TryToPutDown(index, item);
                    requiredComponents.RemoveAt(i);

                    if (requiredComponents.Count == 0) 
                    {
                        Debug.Log("Done");
                        AbsorbMaterials();
                        return;
                    }

                    return;
                }

                else
                {
                    Debug.Log("Doesn't belong here");
                }
            }

        }
        else
        {
            Debug.Log("Was not CraftingMaterial");
        }

    }

    private void AbsorbMaterials()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            DevisualizeAtIndex(i);
            itemInventory.items[i] = null;
        }
    }

    private void ResetCraftingInventory()
    {

        InventoryManager playerInventoryManager = ServiceLocator.InventoryService.GetInventoryByKey(InventoryTypes.PlayerInventory);
        Inventory playerInventory = playerInventoryManager.Inventory;

        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            if (itemInventory.items[i])
            {
                for (int j = 0; j < playerInventory.items.Length; j++)
                {
                    if (playerInventory.items[j] == null)
                    {
                        playerInventoryManager.AddItem(itemInventory.items[i], j);
                        break;
                    }
                }
            }
            Destroy(itemSlotGameObject[i]);
        }
        ServiceLocator.InventoryService.RemoveFromActiveInventories(this, type);
    }

}
