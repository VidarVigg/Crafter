using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPopupService
{
    void Enable(InventoryManager inventory);
    void Disable(InventoryManager inventory);
}
