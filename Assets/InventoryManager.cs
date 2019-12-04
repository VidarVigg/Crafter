using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    [SerializeField]
    private Item[] playerInventory;

    [SerializeField]
    private ItemVisualizer[] itemVisualizers;

    [SerializeField]
    private GameObject[] itemSlotGameObject;

    [SerializeField]
    private GameObject itemSlotPrefab;

    [SerializeField]
    private Transform inventoryPanel;

    private void Awake()
    {

        itemVisualizers = new ItemVisualizer[playerInventory.Length];
        itemSlotGameObject = new GameObject[playerInventory.Length];


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
        for (int i = 0; i < playerInventory.Length; i++)
        {
            GameObject itemSlotClone = Instantiate(itemSlotPrefab, inventoryPanel);
            itemSlotGameObject[i] = itemSlotClone;
        }
    }
    private void CacheVisuals()
    {
        for (int i = 0; i < playerInventory.Length; i++)
        {
            itemVisualizers[i] = new ItemVisualizer();
            itemVisualizers[i].backgroundImage = itemSlotGameObject[i].GetComponentsInChildren<Image>()[0];
            itemVisualizers[i].itemImage = itemSlotGameObject[i].GetComponentsInChildren<Image>()[1];
        }
    }
    private void AddEventAtIndex(int index)
    {
        UnityEvent leftClick = new UnityEvent();
        leftClick.AddListener(() => LeftClick(index));
        itemSlotGameObject[index].GetComponent<CustomButton>().leftClick = leftClick;
    }

    private void MakeInteractable()
    {
        for (int i = 0; i < playerInventory.Length; i++)
        {
            if (playerInventory[i])
            {
                AddEventAtIndex(i);
            }
        }
    }
    private void VisualizeAll()
    {

        for (int i = 0; i < playerInventory.Length; i++)
        {
            if (playerInventory[i])
            {
                itemSlotGameObject[i].GetComponentsInChildren<Image>()[1].sprite = playerInventory[i].swordSprite;
            }
        }

    }

    private void DevisualizeAtIndex(int index)
    {
        itemVisualizers[index].itemImage.sprite = null;
    }

    private void LeftClick(int index)
    {
        Debug.Log("ClickedOnItem : " + playerInventory[index].itemName);
        DevisualizeAtIndex(index);
    }



}
