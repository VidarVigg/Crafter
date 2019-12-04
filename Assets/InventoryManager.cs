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

    private Item pickedUpItem;



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

    void Update()
    {

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
        }

    }

    private void DevisualizeAtIndex(int index)
    {
        itemVisualizers[index].itemImage.sprite = null;
    }

    private void LeftClick(int index)
    {
        Debug.Log("ClickedOnItem : " + itemInventory.items[index].itemName);
        DevisualizeAtIndex(index);
    }

    private void Pickup(int index)
    {
        if (!pickedUpItem)
        {
            pickedUpItem = itemInventory.items[index];
            DevisualizeAtIndex(index);
        }
        Debug.Log(itemInventory.items[index]);
    }

    private void PutDown(int index)
    {
        
    }



}
