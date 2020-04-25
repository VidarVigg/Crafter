
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IGameStateObserver
{
    [SerializeField]
    protected Inventory itemInventory;

    [SerializeField]
    protected ItemVisualizer[] itemVisualizers;

    [SerializeField]
    protected GameObject[] itemSlotGameObject;

    [SerializeField]
    protected Item[] developementInventory; 

    [SerializeField]
    protected ItemSlot[] itemSlotData;

    [SerializeField]
    protected GameObject itemSlotPrefab;

    [SerializeField]
    private Transform inventoryPanel;

    [SerializeField]
    protected static Item pickedUpItem;

    [SerializeField]
    protected static int pickedUpAmt;

    [SerializeField]
    private Sprite emptySprite;

    [SerializeField]
    private Canvas inventoryCanvas;

    [SerializeField]
    protected InventoryTypes inventoryType;

    [SerializeField]
    private string[] keys;


    public delegate void OnPickedUpBluePrint();
    protected static OnPickedUpBluePrint onPickedUpBluePrint;
    protected static OnPickedUpBluePrint onUnloadedBluePrint;
    public delegate void InitializeInventoryRemotely(int size);
    public InitializeInventoryRemotely initializeInventoryRemotely; // Should be in the affected script

    public InventoryTypes GetInventoryType
    {
        get { return inventoryType; }
    }

    public Inventory Inventory
    {
        get { return this.itemInventory; }
        set { itemInventory = value; }
    }

    protected virtual void Awake()
    {
        initializeInventoryRemotely += InitializeInventory;

    }

    protected virtual void Start()
    {
        InitializeInventory(itemInventory.items.Length);
        GameManager.GetInstance().gameStateObservers.Add(this);
    }

    private void Update()
    {

    }

    internal void Activate()
    {
        inventoryCanvas.enabled = true;
        GameManager.GetInstance().HandleState(GameStates.Inventory);
    }

    internal void Disable()
    {
        inventoryCanvas.enabled = false;
        GameManager.GetInstance().HandleState(GameStates.Game);
    }

    protected void AssignOwnerShip(InventoryTypes inventory, Item item)
    {
        item.ownerInventory = inventory;
    }

    protected void InitializeInventory(int size)
    {
        itemVisualizers = new ItemVisualizer[size];
        itemSlotGameObject = new GameObject[size];
        itemSlotData = new ItemSlot[size];
        keys = new string[size];
        CloneItems();
        CreateItemSlots();
        UpdateItemslotValues();
        VisualizeItemSlots();
        CacheVisuals();
        AddEventToAllSlots();
    }

    internal void ClearInventory()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            Delete(i);
            DevisualizeAtIndex(i);
        }
    }

    protected void CloneItems()
    {
        for (int i = 0; i < developementInventory.Length; i++)
        {
            if (developementInventory[i] != null)
            {
                itemInventory.items[i] = developementInventory[i].CreateInstance(inventoryType);
                itemInventory.items[i].inventoryIndex = i;
            }
        }
    }

    protected virtual void CreateItemSlots()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            GameObject itemSlotClone = Instantiate(itemSlotPrefab, inventoryPanel);
            itemSlotGameObject[i] = itemSlotClone;
            itemSlotData[i] = itemSlotGameObject[i].GetComponent<ItemSlot>();
            itemSlotData[i].SlotIndex = i;
        }
    }

    internal void UpdateItemslotValues()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            if (itemInventory.items[i] != null)
            {
                itemSlotGameObject[i].GetComponent<ItemSlot>().SlotKey = itemInventory.items[i].key;
                itemSlotGameObject[i].GetComponent<ItemSlot>().ItemAmt += 1;
            }
        }
    }

    protected void CacheVisuals()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            itemVisualizers[i] = new ItemVisualizer();
            itemVisualizers[i].backgroundImage = itemSlotGameObject[i].GetComponentsInChildren<Image>()[0];
            itemVisualizers[i].itemImage = itemSlotGameObject[i].GetComponentsInChildren<Image>()[1];
        }
    }

    private void AddEventAtIndex(int index)
    {
        itemSlotGameObject[index].GetComponent<CustomButton>().leftClick.AddListener(() => OnClick(index, "Left"));
        itemSlotGameObject[index].GetComponent<CustomButton>().rightClick.AddListener(() => OnClick(index, "Right")); 
    }

    protected virtual void OnClick(int index, string key)
    {
        if (pickedUpItem == null)
        {
            if (itemInventory.items[index] != null)
            {
                Pickup(index, key);
            }
            else
            {
                //Debug.Log("There is nothing to pick up here");
            }
        }
        else
        {
            ItemSlot slot = itemSlotData[index];
            if (itemInventory.items[index] == null)
            {
                //Debug.Log(" Tried To Put Down Item");
                TryToPutDown(index, pickedUpItem); // don't repeat below
            }
            else
            {
                if (itemInventory.items[index].key == pickedUpItem.key && itemInventory.items[index].stackable)
                {

                    TryToPutDown(index, pickedUpItem);
                }
                else
                {
                    Debug.Log("Can't put Down: Slot Was Filled");
                }
            }
        }
    }

    private void VisualizeOnPickup(Item pickedUpItem)
    {

    }

    private void AddEventToAllSlots()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            AddEventAtIndex(i);
        }
    }

    private void Pickup(int index, string key)
    {

        ItemSlot slot = itemSlotGameObject[index].GetComponent<ItemSlot>();
        if (itemInventory.items[index] is BluePrint)
        {
            BluePrint bluePrint = itemInventory.items[index] as BluePrint;
            bluePrint.HandleBluePrint();
            DevisualizeAtIndex(index);
            slot.ResetSlotValues();
            Delete(index);
            return;
        }
        if (!pickedUpItem)
        {
            if (key == "Left")
            {
                pickedUpAmt = slot.ItemAmt;
                pickedUpItem = itemInventory.items[index];
                DevisualizeAtIndex(index);
                Delete(index);
                slot.ResetSlotValues();
                Debug.Log("Picked up all " + pickedUpAmt);
            }
            else if (key == "Right")
            {
                Debug.Log("Picked up one");
                if (slot.ItemAmt >= 1)
                {
                    pickedUpItem = itemInventory.items[index].CreateInstance(InventoryTypes.None);
                    slot.ItemAmt -= 1;
                    pickedUpAmt += 1;
                    slot.SetDisplayAmount(slot.ItemAmt);
                    if (slot.ItemAmt == 1)
                    {
                        slot.HideItemAmt();
                    }
                    if (slot.ItemAmt == 0)
                    {
                        slot.ResetSlotValues();
                        DevisualizeAtIndex(index);
                    }
                }
            }
        }
    }

    public void DevisualizeAtIndex(int index)
    {
        itemVisualizers[index].itemImage.sprite = emptySprite;
    }

    protected void VisualizeAtIndex(int index, Item item)
    {
        itemVisualizers[index].itemImage.sprite = item.itemSprite;
        itemVisualizers[index].backgroundImage.sprite = item.background;
    }

    public void Delete(int index)
    {
        itemInventory.items[index] = null;
    }

    public void AddItem(Item item, int index)
    {
        itemInventory.items[index] = item;
        VisualizeAtIndex(index, item);
        item.ownerInventory = inventoryType;
        item.inventoryIndex = itemSlotData[index].SlotIndex;
        //Maybe update slotAmt here.
    }

    protected virtual void TryToPutDown(int index, Item item)
    {
        ItemSlot slot = itemSlotGameObject[index].GetComponent<ItemSlot>();
        slot.SlotKey = pickedUpItem.key;
        slot.ItemAmt += pickedUpAmt;
        AddItem(item, index);
        pickedUpAmt = 0;
        pickedUpItem = null;
        if (slot.ItemAmt > 1)
        {
            slot.SetDisplayAmount(slot.ItemAmt);
            slot.DisplayItemAmt();
        }
    }

    public void VisualizeItemSlots()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            if (itemInventory.items[i])
            {
                itemSlotGameObject[i].GetComponentsInChildren<Image>()[1].sprite = itemInventory.items[i].itemSprite;
            }
            else
            {
                itemSlotGameObject[i].GetComponentsInChildren<Image>()[1].sprite = emptySprite;
            }
        }
    }

    public void AddDropEventForAllSlots()
    {
        for (int i = 0; i < itemSlotGameObject.Length; i++)
        {

            if (!itemInventory.items[i])
            {
                int index = i;
                CustomButton cb = itemSlotGameObject[i].GetComponent<CustomButton>();
                cb.leftClick.AddListener(() => TryToPutDown(index, pickedUpItem));
            }

        }
    }

    public void RemoveDropEventForAll()
    {
        for (int i = 0; i < itemSlotGameObject.Length; i++)
        {
            if (!itemInventory.items[i])
            {
                UnityEvent empty = new UnityEvent();
                CustomButton cb = itemSlotGameObject[i].GetComponent<CustomButton>();
                cb.leftClick = empty;

            }
        }
    }

    public void OnGameStateChanged(GameStates state)
    {
        Debug.Log(this.ToString() + " " + state);
    }




}



