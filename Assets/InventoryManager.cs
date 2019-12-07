using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryManager : InventoryParent
{
    [SerializeField]
    private ItemVisualizer[] itemVisualizers;

    [SerializeField]
    private GameObject[] itemSlotGameObject;

    [SerializeField]
    private GameObject itemSlotPrefab;

    [SerializeField]
    private Transform inventoryPanel;

    [SerializeField]
    private Item pickedUpItem;

    [SerializeField]
    private Sprite emptySprite;

    private void Awake()
    {

        itemVisualizers = new ItemVisualizer[itemInventory.items.Length];
        itemSlotGameObject = new GameObject[itemInventory.items.Length];

    }

    void Start()
    {
        CreateItemSlots();
        VisualizeAll();
        CacheVisuals();
        MakeInteractable();
    }

    private void CreateItemSlots()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            GameObject itemSlotClone = Instantiate(itemSlotPrefab, inventoryPanel);
            itemSlotGameObject[i] = itemSlotClone;
        }
    }
    private void CacheVisuals()
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
        UnityEvent leftClick = new UnityEvent();
        leftClick.AddListener(() => Pickup(index));
        itemSlotGameObject[index].GetComponent<CustomButton>().leftClick = leftClick;
    }
    private void MakeInteractable()
    {
        for (int i = 0; i < itemInventory.items.Length; i++)
        {

            if (itemInventory.items[i])
            {
                AddEventAtIndex(i);
            }

        }
    }
    private void VisualizeAll()
    {

        for (int i = 0; i < itemInventory.items.Length; i++)
        {
            if (itemInventory.items[i])
            {
                itemSlotGameObject[i].GetComponentsInChildren<Image>()[1].sprite = itemInventory.items[i].swordSprite;
            }
            else
            {
                itemSlotGameObject[i].GetComponentsInChildren<Image>()[1].sprite = emptySprite; 
            }
        }

    }
    private void Pickup(int index)
    {
        if (!pickedUpItem)
        {
            pickedUpItem = itemInventory.items[index];
            DevisualizeAtIndex(index);
            Delete(index);
            AddDropEvent(itemInventory.items[index]);
        }
        Debug.Log(itemInventory.items[index]);

    }
    private void DevisualizeAtIndex(int index)
    {
        itemVisualizers[index].itemImage.sprite = emptySprite; 
    }
    private void VisualizeAtIndex(int index, Item item)
    {
        Debug.Log(item);
        itemVisualizers[index].itemImage.sprite = item.swordSprite;
        itemVisualizers[index].backgroundImage.sprite = item.background;
    }
    private void Delete(int index)
    {
        itemInventory.items[index] = null;
    }
    private void AddItem(Item item, int index)
    {
        Debug.Log(item);
        itemInventory.items[index] = item;
        
    }
    private void AddDropEvent(Item item)
    {

        for (int i = 0; i < itemSlotGameObject.Length; i++)
        {

            int index = i;

            CustomButton cb = itemSlotGameObject[i].GetComponent<CustomButton>();

            UnityEvent ue = new UnityEvent();
            ue.AddListener(() => TryToPutDown(index, pickedUpItem));
            cb.leftClick = ue;

        }

    }
    private void TryToPutDown(int index, Item item)
    {
        Debug.Log(index);

        if (!itemInventory.items[index])
        {
            AddItem(item, index );
            VisualizeAtIndex(index, item);
            pickedUpItem = null;
            RemoveDropEventForAll();
            MakeInteractable();
            
        }
        else
        {
            Debug.Log("SlotWasFilled");
        }
        
    }
    private void RemoveDropEventForAll()
    {
        for (int i = 0; i < itemSlotGameObject.Length; i++)
        {
            UnityEvent empty = new UnityEvent();
            CustomButton cb = itemSlotGameObject[i].GetComponent<CustomButton>();
            cb.leftClick = empty;
        }
    }

}