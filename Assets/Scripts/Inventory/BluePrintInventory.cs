using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrintInventory : InventoryManager, ISavable
{
    InventoryTypes type;
    protected override void Awake()
    {
        type = InventoryTypes.BluePrintInventory;
    }

    protected override void Start()
    {
        base.Start();
        ServiceLocator.InventoryService.AddToActiveInventories(this, InventoryTypes.BluePrintInventory);
        SaveManager.INSTANCE.saveObserversDictionary.Add("BluePrintInventory", this);
    }
    protected override void OnClick(int index, string key)
    {
        if (pickedUpItem == null)
        {
            if (itemInventory.items[index] != null)
            {
                Item blueprint = itemInventory.items[index].CreateInstance(InventoryTypes.BluePrintInput) as BluePrint;
                BluePrintInput bluePrintInput = ServiceLocator.InventoryService.GetInventoryByKey(InventoryTypes.BluePrintInput) as BluePrintInput;
                if (bluePrintInput.Inventory.items[0] == null)
                {
                    bluePrintInput.AddItem(blueprint, 0);
                    bluePrintInput.LoadCraftingInventory(itemInventory.items[index] as BluePrint);
                }
                else
                {
                    Debug.Log("Slot Was Filled");
                }

            }
            else
            {
                //Debug.Log("There is nothing to pick up here");
            }
        }
        else
        {
            return;
        }
    }

    void ISavable.SetSaveInfo(GlobalSave save)
    {
        List<string> inventorySave = new List<string>();
        for (int i = 0; i < Inventory.items.Length; i++)
        {
            if (Inventory.items[i] != null)
            {
                string itemSave = JsonUtility.ToJson(Inventory.items[i].GetInstanceSaveInfo(Inventory.items[i]));
                inventorySave.Add(itemSave);

            }
        }
        BluePrintSave bluePrintSave = new BluePrintSave(inventorySave);
        save.bluePrintSave = bluePrintSave;
    }


    public void ApplySavedItems(Item item, int index)
    {
        AddItem(item, index);
        Debug.Log("BluePrint Inventory Recieved " + item.itemName);
    }

    void ISavable.ClearInventory()
    {
        ClearInventory();
    }

    public void FinalizeLoad()
    {
        Debug.Log("Finalized");
    }
}
