using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllableCharacter : Character, IControllable
{
    /*
     
     This is the base class for controllable characters. One of the mechanics of this game is to switch between the main character and the main character's pet.
     This works by unsubscribing and subscribing functions to delegates in the input system (as seen in the functions AttachControlls and DetachControlls).
     Due to the use of the interface IControllable that contains the subscribeable functions it is very easy to create controllable characters and switch between them.
     
    */
    [SerializeField]
    protected float movementSpeed;

    [SerializeField]
    protected Rigidbody rigidbody;

    [SerializeField]
    private Interactable closestInteractable;

    protected Vector3 movementVector;

    protected Quaternion currentRotation;

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
    public virtual void RotateCharacter(Vector3 vector)
    {
        if (vector != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vector), 0.03f);
        }
        else
        {
            return;
        }
    }
    public virtual void RightMouseToggle(bool toggle)
    {

    }
    public virtual void ActivateAbility()
    {

    }
    public virtual void Sprint(bool sprinting)
    {

    }

}
