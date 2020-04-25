using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BluePrintInput : InventoryManager
{
    private bool activated;
    private CustomButton customButton;
    InventoryTypes type = InventoryTypes.BluePrintInput;

    [SerializeField]
    private CraftingInventory craftingRequirementsInventory;

    [SerializeField]
    private BluePrintInventory bluePrintInventory;

    private BluePrint bluePrin;

    [SerializeField]
    protected bool blueprintLoaded = false;

    private bool loaded;

    protected override void Awake()
    {
        inventoryType = InventoryTypes.BluePrintInventory;
    }

    protected override void Start()
    {
        base.Start();
        customButton = itemSlotGameObject[0].GetComponent<CustomButton>();
        AddDropEventToBluePrintSlot();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!activated)
            {
                ServiceLocator.PopupService.Enable(this);
                ServiceLocator.InventoryService.AddToActiveInventories(this, type);

                activated = true;
            }
            else
            {
                ServiceLocator.PopupService.Disable(this);
                ServiceLocator.InventoryService.RemoveFromActiveInventories(this, type);
                activated = false;
            }
        }
    }

    protected override void OnClick(int index, string key)
    {
        onUnloadedBluePrint.Invoke();
        DevisualizeAtIndex(0);
        Delete(0);
    }


    public void LoadCraftingInventory(BluePrint bluePrint)
    {
        AssignOwnerShip(InventoryTypes.BluePrintInventory, bluePrint);
        craftingRequirementsInventory.Initialize(bluePrint);
    }

    private void AddDropEventToBluePrintSlot()
    {
        UnityEvent ue = new UnityEvent();
        ue.AddListener(() => OnClick(0, "Left"));
        customButton.leftClick = ue;
        Debug.Log("AddedDropEventToBluePrintSlot");
    }
}
