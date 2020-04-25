using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{

    private static IPopupService popupProvider = null;
    private static IInventoryService inventoryProvider = null;


    public static IPopupService PopupService
    {
        get { return popupProvider; }
        set
        {
            if (popupProvider == null)
            {
                popupProvider = new NullPopupProvider();
            }
            popupProvider = value;
        }
    }

    internal static IInventoryService InventoryService
    {
        get { return inventoryProvider; }
        set
        {
            inventoryProvider = value;
        }
    }



    public static void Initialize()
    {
        popupProvider = new NullPopupProvider();
    }

}
