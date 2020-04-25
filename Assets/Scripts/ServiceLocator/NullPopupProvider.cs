using UnityEngine;

internal class NullPopupProvider : MonoBehaviour, IPopupService
{
    public void Disable(InventoryManager inventory)
    {
       
        Debug.Log("Null Provider");
    }

    public void Enable(InventoryManager inventory)
    {
        Debug.Log("Null Provider");
    }
}