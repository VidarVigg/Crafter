using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryParent : MonoBehaviour
{
    public Inventory itemInventory = new Inventory(0);
}
