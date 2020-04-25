
using UnityEngine;

public class PlayerInventory : InventoryManager
{

    private bool activated = false;
    InventoryTypes type = InventoryTypes.PlayerInventory;


    protected override void Awake()
    {
        inventoryType = InventoryTypes.PlayerInventory;
        InputManager.INSTANCE.onTabPressed += ActivateInventory;

    }

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            if (itemInventory.items[i] != null)
            {
                AssignOwnerShip(InventoryTypes.PlayerInventory, itemInventory.items[i]);

            }
        }
    }

    public void ActivateInventory()
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
