using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Character, IGameStateObserver, ISavable
{

    private Rigidbody2D rigidbody;
    [SerializeField]
    private bool inInteractingDistance;
    [SerializeField]
    private Interactable closestInteractable;
    [SerializeField]
    private MagicGrid magicGrid;
    [SerializeField]
    private string playerName = "playerName";
    [SerializeField]
    private PlayerInventory inventory;

    public Interactable ClosestInteractable
    {
        get { return closestInteractable; }
        set { closestInteractable = value; }
    }
    public bool InInteractingDistance
    {
        get { return inInteractingDistance; }
        set { inInteractingDistance = value; }
    }

    private void Awake()
    {
        GameManager.GetInstance().Player = this;
        GameManager.GetInstance().gameStateObservers.Add(this);
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        InputManager.INSTANCE.onMoveKeyPressed += Move;
        InputManager.INSTANCE.onInteract += Interact;
        InputManager.INSTANCE.onSecondaryMousePressed += MagicGridToggle;
        SaveManager.INSTANCE.saveObserversDictionary.Add("PlayerInventory", this); //
    }

    internal void ProcessInteractable(Interactable interactable)
    {
        closestInteractable = interactable;
    }

    private void Move(Vector2 direction)
    {
        rigidbody.velocity = direction.normalized * 1.5f;
        movementDirection = direction;

    }

    private void Interact()
    {
        if (closestInteractable != null)
        {
            Debug.Log("Interacted With " + closestInteractable.name);
            closestInteractable.Activate();
        }
        else
        {
            Debug.Log("Interacted With None");
        }
    }

    private void MagicGridToggle(bool toggle)
    {
        magicGrid.magicCanvas.enabled = toggle;
        if (toggle)
        {
            magicGrid.ActivateGrid();
        }
        if (!toggle)
        {
            magicGrid.DisableGrid();
        }
    }

    public void OnGameStateChanged(GameStates state)
    {
        if (state == GameStates.Inventory)
        {
            InputManager.INSTANCE.onSecondaryMousePressed -= MagicGridToggle;
        }
        else if (state == GameStates.Game)
        {
            InputManager.INSTANCE.onSecondaryMousePressed += MagicGridToggle;
        }
    }

    void ISavable.SetSaveInfo(GlobalSave save)
    {
        List<string> inventorySave = new List<string>();
        for (int i = 0; i < inventory.Inventory.items.Length; i++)
        {
            if (inventory.Inventory.items[i] != null)
            {
                string itemSave = JsonUtility.ToJson(inventory.Inventory.items[i].GetInstanceSaveInfo(inventory.Inventory.items[i]));
                inventorySave.Add(itemSave);
            }
        }
        PlayerSave newPlayerSave = new PlayerSave(playerName, transform.position, inventorySave);
        save.player = newPlayerSave;
    }

    public void ApplySavedItems(Item item, int index)
    {
        inventory.AddItem(item, index);
        Debug.Log("Player Inventory Recieved " + item.itemName + " And Wanted to place it on " + index);
    }

    public void ClearInventory()
    {
        inventory.ClearInventory();
    }

    public void FinalizeLoad()
    {
        //inventory.UpdateItemslotValues();
    }
}
