using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory
{
    [SerializeField]
    internal Item[] items;

    public Inventory(int capacity)
    {
        items = new Item[capacity];
    }
}
