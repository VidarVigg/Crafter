using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllableCharacter : Character, IControllable
{

    [SerializeField]
    protected float movementSpeed;
    [SerializeField]
    protected Rigidbody rigidbody;
    protected Vector3 movementVector;
    protected Quaternion currentRotation;
    [SerializeField]
    private Interactable closestInteractable;
    public Interactable ClosestInteractable
    {
        get { return closestInteractable; }
        set { closestInteractable = value; }
    }
    public void AttachControls()
    {
        InputManager.INSTANCE.onMoveKeyPressed += Move;
        InputManager.INSTANCE.onInteract += Interact;
        InputManager.INSTANCE.onSecondaryMousePressed += RightMouseToggle;
        InputManager.INSTANCE.onShiftPressed += Sprint;
        InputManager.INSTANCE.onChangedRotation += RotateCharacter;
        InputManager.INSTANCE.onActivate += ActivateAbility;
    }
    public void DetachControls()
    {
        InputManager.INSTANCE.onMoveKeyPressed -= Move;
        InputManager.INSTANCE.onInteract -= Interact;
        InputManager.INSTANCE.onSecondaryMousePressed -= RightMouseToggle;
        InputManager.INSTANCE.onShiftPressed -= Sprint;
        InputManager.INSTANCE.onChangedRotation -= RotateCharacter;
        InputManager.INSTANCE.onActivate -= ActivateAbility;
    }
    public virtual void Move(Vector3 direction)
    {
        direction.y = rigidbody.velocity.y;
        movementVector = direction.normalized * movementSpeed;
    }
    public virtual void Interact()
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
    public virtual void RightMouseToggle(bool toggle)
    {

    }
    public virtual void RotateCharacter(Vector3 vector)
    {
        if (vector != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vector), 0.07f);
        }
        else
        {
            return;
        }
    }
    public virtual void ActivateAbility()
    {

    }
    public virtual void Sprint(bool sprinting)
    {

    }

}
