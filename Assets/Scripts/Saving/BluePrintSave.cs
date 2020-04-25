using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BluePrintSave : SaveObject
{
    public string[] bluePrintInventory;
    public BluePrintSave(List<string> bluePrintInventory)
    {
        this.bluePrintInventory = bluePrintInventory.ToArray();
    }
}
