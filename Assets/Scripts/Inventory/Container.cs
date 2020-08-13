using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Container : InventoryManager
{
    [SerializeField]
    private ContainerData containerData;
    private Interactable interactable;

    private bool opened;

    public ContainerData ContainerData
    {
        get { return containerData; }
    }

    protected override void Awake()
    {
        inventoryType = InventoryTypes.Container;
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        interactable = GetComponent<Interactable>();
        interactable.activateInteractable += OpenContainer;
        interactable.deactivateInteractable += CloseContainer;
    }

    public void OpenContainer()
    {
        if (!opened)
        {
            Inventory = new Inventory(ContainerData.capacity);
            initializeInventoryRemotely.Invoke(ContainerData.capacity);

            for (int i = 0; i < Inventory.items.Length; i++)
            {
                int index = i;
                Inventory.items[index] = ContainerData.possibleLoot[Random.Range(0, ContainerData.possibleLoot.Length)].CreateInstance(inventoryType);

            }
            VisualizeItemSlots();
            UpdateItemslotValues();
            opened = true;
        }

        ServiceLocator.PopupService.Enable(this);
    }
    public void CloseContainer()
    {
        ServiceLocator.PopupService.Disable(this);
    }
}
