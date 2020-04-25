using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SaveManager : MonoBehaviour
{
    public static SaveManager INSTANCE;
    string _filePath;

    public Dictionary<string, ISavable> saveObserversDictionary = new Dictionary<string, ISavable>();

    private void Awake()
    {
        if (!INSTANCE)
        {
            INSTANCE = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {

            SaveAll();

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LoadGame();
            Debug.Log("Loaded");
        }
    }

    private void LoadGame()
    {
        GlobalSave globalSave = new GlobalSave();

        if (File.Exists(Application.persistentDataPath + "/" + "GlobalSave.txt"))
        {
            //Debug.Log("Found File " + Application.persistentDataPath + "/" + "save.txt");
            string load = File.ReadAllText(Application.persistentDataPath + "/" + "GlobalSave.txt");
            globalSave = JsonUtility.FromJson<GlobalSave>(load);
            HandlePlayerSave(globalSave.player);
            HandleBluePrintSave(globalSave.bluePrintSave);
        }
        else
        {
            Debug.Log("Didn't find file");
        }

    }

    private void HandleBluePrintSave(BluePrintSave saveObject)
    {
        saveObserversDictionary["BluePrintInventory"].ClearInventory();
        for (int i = 0; i < saveObject.bluePrintInventory.Length; i++)
        {
            HandleBluePrintData(saveObject, i);
        }
        saveObserversDictionary["BluePrintInventory"].FinalizeLoad();
    }
    private void HandlePlayerSave(PlayerSave saveObject)
    {
        saveObserversDictionary["PlayerInventory"].ClearInventory();

        for (int i = 0; i < saveObject.playerInventory.Length; i++)
        {
            HandlePlayerItemData(saveObject, i);
        }
        saveObserversDictionary["PlayerInventory"].FinalizeLoad();
    }

    private void HandleBluePrintData(BluePrintSave saveObject, int index)
    {
        ItemSaveInfo itemSaveInfo = null;
        Item item = null;
        itemSaveInfo = JsonUtility.FromJson<ItemSaveInfo>(saveObject.bluePrintInventory[index]);
        BluePrintSaveInfo bluePrintSaveInfo = new BluePrintSaveInfo();
        bluePrintSaveInfo = JsonUtility.FromJson<BluePrintSaveInfo>(saveObject.bluePrintInventory[index]);
        BluePrint bluePrint = ScriptableObject.CreateInstance<BluePrint>();
        bluePrint = bluePrint.GetSavedBluePrint(bluePrintSaveInfo);
        item = bluePrint;
        saveObserversDictionary["BluePrintInventory"].ApplySavedItems(item, itemSaveInfo.INDEX);
    }


    private void HandlePlayerItemData(PlayerSave loadedSave, int index)
    {
        ItemSaveInfo itemSaveInfo = null;
        Item item = null;

        itemSaveInfo = JsonUtility.FromJson<ItemSaveInfo>(loadedSave.playerInventory[index]);

        if (itemSaveInfo != null)
        {
            switch (itemSaveInfo.ITEMTYPE)
            {
                case "Weapon":
                    WeaponSaveInfo weaponSaveInfo = new WeaponSaveInfo();
                    weaponSaveInfo = JsonUtility.FromJson<WeaponSaveInfo>(loadedSave.playerInventory[index]);
                    Weapon weapon = ScriptableObject.CreateInstance<Weapon>();
                    weapon = weapon.GetSavedWeapon(weaponSaveInfo);
                    item = weapon;
                    break;
                case "Material":
                    CraftingMaterialSaveInfo craftingMaterialSaveInfo = new CraftingMaterialSaveInfo();
                    craftingMaterialSaveInfo = JsonUtility.FromJson<CraftingMaterialSaveInfo>(loadedSave.playerInventory[index]);
                    CraftingMaterial craftingMaterial = ScriptableObject.CreateInstance<CraftingMaterial>();
                    craftingMaterial = craftingMaterial.GetSavedCraftingMaterial(craftingMaterialSaveInfo);
                    item = craftingMaterial;
                    break;
                case "BluePrint":
                    BluePrintSaveInfo bluePrintSaveInfo = new BluePrintSaveInfo();
                    bluePrintSaveInfo = JsonUtility.FromJson<BluePrintSaveInfo>(loadedSave.playerInventory[index]);
                    BluePrint bluePrint = ScriptableObject.CreateInstance<BluePrint>();
                    bluePrint = bluePrint.GetSavedBluePrint(bluePrintSaveInfo);
                    item = bluePrint;
                    break;

            }
                saveObserversDictionary["PlayerInventory"].ApplySavedItems(item, itemSaveInfo.INDEX);
        }
    }

    public void SaveAll()
    {
        Debug.Log("SavedAll");
        GlobalSave save = new GlobalSave();
        foreach (var item in saveObserversDictionary)
        {
            item.Value.SetSaveInfo(save);
        }
        _filePath = Application.persistentDataPath + "/" + save + ".txt";
        string saveObject = JsonUtility.ToJson(save);
        File.WriteAllText(_filePath, saveObject);
        File.AppendText(_filePath);
    }



}
