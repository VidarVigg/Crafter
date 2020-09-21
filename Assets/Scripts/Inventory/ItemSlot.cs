using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ItemSlot : MonoBehaviour
{

    [SerializeField]
    private string slotKey;

    [SerializeField]
    private int slotIndex;

    [SerializeField]
    private GameObject itemAmtDisplay;

    [SerializeField]
    private TextMeshProUGUI number;


    public void DisplayItemAmt()
    {
        itemAmtDisplay.SetActive(true);
    }
    public void HideItemAmt()
    {
        itemAmtDisplay.SetActive(false);
    }

    public void SetDisplayAmount(int amt)
    {
        number.text = amt.ToString();
    }

    public string SlotKey
    {
        get { return slotKey; }
        set { slotKey = value; }
    }

    public int SlotIndex
    {
        get { return slotIndex; }
        set { slotIndex = value; }
    }

    internal void ResetSlotValues()
    {
        SlotKey = "";
        HideItemAmt();
    }
}
