using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour, IPopupService
{
    public void Disable(InventoryManager inventory)
    {
        inventory.Disable();
    }

    public void Enable(InventoryManager inventory)
    {
        inventory.Activate();
    }
}
