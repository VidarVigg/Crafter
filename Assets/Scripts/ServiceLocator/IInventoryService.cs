

public interface IInventoryService
{
    void AddToActiveInventories(InventoryManager inventory, InventoryTypes key);
    void RemoveFromActiveInventories(InventoryManager inventory, InventoryTypes key);
    InventoryManager GetInventoryByKey(InventoryTypes key);
}
