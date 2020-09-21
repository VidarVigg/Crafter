using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ControllableCharacter, IGameStateObserver, ISavable
{

    [SerializeField]
    private bool inInteractingDistance;

    [SerializeField]
    private MagicGrid magicGrid;

    [SerializeField]
    private string playerName = "playerName";

    [SerializeField]
    private PlayerInventory inventory;

    [SerializeField]
    private float sprintAddition;

    [SerializeField]
    private bool sprinting;

    [SerializeField]
    private RavenMaster pet;

    [SerializeField]
    private CameraFollow cameraFollow;
    private float defaultMovementSpeed;

    public bool InInteractingDistance
    {
        get { return inInteractingDistance; }
        set { inInteractingDistance = value; }
    }

    public void ProcessInteractable(Interactable interactable)
    {
        ClosestInteractable = interactable;
    }

    private void Start()
    {
        GameManager.GetInstance().Player = this;
        GameManager.GetInstance().gameStateObservers.Add(this);
        rigidbody = GetComponent<Rigidbody>();
        AttachControls();
        SaveManager.INSTANCE.saveObserversDictionary.Add("PlayerInventory", this);
        defaultMovementSpeed = movementSpeed;
    }

    private void Update()
    {
        rigidbody.velocity = movementVector.normalized * movementSpeed;
    }

    public void OnGameStateChanged(GameStates state)
    {
        if (state == GameStates.Inventory)
        {
            InputManager.INSTANCE.onSecondaryMousePressed -= RightMouseToggle;
        }
        else if (state == GameStates.Game)
        {
            InputManager.INSTANCE.onSecondaryMousePressed += RightMouseToggle;
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
        //
    }

    public override void ActivateAbility()
    {
        movementVector = Vector3.zero;
        DetachControls();
        pet.Activate();
        cameraFollow.SwithObjectToFollow(pet.transform);
    }

    public override void RightMouseToggle(bool toggle)
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

    public override void Sprint(bool sprinting)
    {
        if (sprinting)
        {
            movementSpeed += sprintAddition;
        }
        else
        {
            movementSpeed = defaultMovementSpeed;
        }
    }
}
