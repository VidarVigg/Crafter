using System.Collections.Generic;

public interface ISavable
{
    void SetSaveInfo(GlobalSave save);
    void ApplySavedItems(Item item, int index);
    void ClearInventory();
    void FinalizeLoad();
}