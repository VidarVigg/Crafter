
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IGameStateObserver
{

    /*
     
     This is entire project is a work in progress that started with the building of this inventory system.
     This script is the base class of that inventory system and it is something I have worked on through many iterations.
     Since this game project has a lot of focus on item handling, crafting and looting, the goal was always to create a solid dynamic 
     and expandable system that can easily be handled from the inspector, and derived from. 
     I am proud of this system because as of now I have succeeded in that.
     To see how inheritance ties in to this system I recommend looking at the scripts PlayerInventory and BluePrintInput. 
     They are two very different scripts that derives from this one.    

    */

    [SerializeField]
    protected Item[] developementInventory;

    [SerializeField]
    protected Inventory itemInventory;

    protected ItemVisualizer[] itemVisualizers;

    protected GameObject[] itemSlotGameObject;

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
    private int inventoryLength = 10;

    public delegate void OnPickedUpBluePrint();
    protected static OnPickedUpBluePrint onPickedUpBluePrint;
    protected static OnPickedUpBluePrint onUnloadedBluePrint;
    public delegate void InitializeInventoryRemotely(int size);
    public InitializeInventoryRemotely initializeInventoryRemotely;

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
        InitializeInventory(inventoryLength);
        GameManager.GetInstance().gameStateObservers.Add(this);
    }

    // Sets up all functionality for base inventory.
    protected void InitializeInventory(int size)
    {
        itemVisualizers = new ItemVisualizer[size];
        itemSlotGameObject = new GameObject[size];
        itemSlotData = new ItemSlot[size];
        CloneItems();
        CreateItemSlots();
        UpdateItemslotValues();
        VisualizeAndCacheSprites();
        AddEventToAllSlots();
    }

    //Activates inventory canvas.
    internal void Activate()
    {
        inventoryCanvas.enabled = true;
        GameManager.GetInstance().HandleState(GameStates.Inventory);
    }

    //Deactivates inventory canvas.
    internal void Disable()
    {
        inventoryCanvas.enabled = false;
        GameManager.GetInstance().HandleState(GameStates.Game);
    }

    // Used in the derived classes to set the owner of Item. This is primarily used for the saving system. 
    protected void AssignOwnerShip(InventoryTypes inventory, Item item)
    {
        item.ownerInventory = inventory;
    }

    // For development purposes only. Items are added from the inspector to the development inventory. This function make clones of the items and adds them to the ingame inventory.
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

    // Creating the slots that contain item data as well as input listeners.
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

    // Used to assign values to the item slots containing items.
    internal void UpdateItemslotValues()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            if (itemInventory.items[i] != null)
            {
                itemSlotGameObject[i].GetComponent<ItemSlot>().SlotKey = itemInventory.items[i].key;
            }
        }
    }

    // VIsualizes the items in the inventory after which the sprites of the Items are cached in to the item visualizers. This makes it so we don´t have to get the sprites every time we want to update them.
    protected void VisualizeAndCacheSprites()
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
            itemVisualizers[i] = new ItemVisualizer();
            itemVisualizers[i].backgroundImage = itemSlotGameObject[i].GetComponentsInChildren<Image>()[0];
            itemVisualizers[i].itemImage = itemSlotGameObject[i].GetComponentsInChildren<Image>()[1];
        }
    }

    // Adding listeners to one specific itemslot.
    private void AddEventAtIndex(int index)
    {
        itemSlotGameObject[index].GetComponent<CustomButton>().leftClick.AddListener(() => OnClick(index, "Left"));
        itemSlotGameObject[index].GetComponent<CustomButton>().rightClick.AddListener(() => OnClick(index, "Right"));
    }

    //Adding listeners to all itemslots of the inventory.
    private void AddEventToAllSlots()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            AddEventAtIndex(i);
        }
    }

    // Controlls what happens when you click a slot in the inventory. The parameter index is the index of the clicked Itemslot. The "Key" is uset to controll stacking of items.
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
                Debug.Log("There is nothing to pick up here");
            }
        }
        else
        {
            ItemSlot slot = itemSlotData[index];
            if (itemInventory.items[index] == null)
            {
                TryToPutDown(index, pickedUpItem);
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

    // Behaviour for when item is picked up from the inventory. Picked up item and how many Items that are picked up are cached.
    private void Pickup(int index, string key)
    {
        ItemSlot slot = itemSlotGameObject[index].GetComponent<ItemSlot>();
        if (itemInventory.items[index] is BluePrint)
        {
            BluePrint bluePrint = itemInventory.items[index] as BluePrint;
            bluePrint.HandleBluePrint();
            DevisualizeAtIndex(index);
            Delete(index);
            return;
        }
        if (!pickedUpItem)
        {
            if (key == "Left")
            {
                pickedUpAmt = itemInventory.items[index].itemCopies;
                pickedUpItem = itemInventory.items[index];
                DevisualizeAtIndex(index);
                Delete(index);
                slot.HideItemAmt();
                Debug.Log("Picked up all " + pickedUpAmt);
            }
            else if(key == "Right")
            {

            }
        }
    }

    // "Devisualizes" a  specific item visualizer
    public void DevisualizeAtIndex(int index)
    {
        itemVisualizers[index].itemImage.sprite = emptySprite;
    }

    // Updating the item visualizer at specific index.
    protected void VisualizeAtIndex(int index, Item item)
    {
        itemVisualizers[index].itemImage.sprite = item.itemSprite;
        itemVisualizers[index].backgroundImage.sprite = item.background;
    }

    // Removes Item from inventory.
    public void Delete(int index)
    {
        itemInventory.items[index] = null;
    }

    // Clears entire inventory
    internal void ClearInventory()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            Delete(i);
            DevisualizeAtIndex(i);
        }
    }

    // Behaviour for when item is added to inventory. This function also controls wether item in question should be stacked or not.
    public void AddItem(Item item, int index, bool stack = false)
    {
        if (stack)
        {
            itemInventory.items[index].itemCopies += pickedUpAmt;
        }
        else
        {
            itemInventory.items[index] = item;
        }
        VisualizeAtIndex(index, item);
        item.ownerInventory = inventoryType;
        item.inventoryIndex = itemSlotData[index].SlotIndex;
    }

    // Function called if an Item is picked up I.E. picked up item is cached, and user presses inventory slot to put it down.
    protected virtual void TryToPutDown(int index, Item item)
    {
        ItemSlot slot = itemSlotGameObject[index].GetComponent<ItemSlot>();
        bool stacking = itemInventory.items[index];
        AddItem(item, index, stacking);
        Debug.Log("The  picked up item " + pickedUpItem + " was added to index " + index + " Copies on index " + index + " = " + itemInventory.items[index].itemCopies);
        pickedUpAmt = 0;
        pickedUpItem = null;
    }

    // Logs changes in game states. This will be used for behaviour in future development.
    public void OnGameStateChanged(GameStates state)
    {
        Debug.Log(this.ToString() + " " + state);
    }

}