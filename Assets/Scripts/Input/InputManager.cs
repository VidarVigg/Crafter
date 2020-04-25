using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager INSTANCE;

    public delegate void OnInteract();
    public OnInteract onInteract;

    public delegate void OnMainMouse();
    public OnMainMouse onMainMousePressed;
    public OnMainMouse onMainMouseClick;

    public delegate void OnMoveKeyPressed(Vector2 direction);
    public OnMoveKeyPressed onMoveKeyPressed;

    public delegate void OnMouseMoved(Vector2 mousePosition);
    public OnMouseMoved onMouseMoved;

    public delegate void OnSecondaryMouse(bool activated);
    public OnSecondaryMouse onSecondaryMousePressed;

    public delegate void OnTabPressed();
    public OnTabPressed onTabPressed;

    public Dictionary<KeyCode, Delegate> inputDictionary = new Dictionary<KeyCode, Delegate>();

    public bool held;

    public KeyCode interact = KeyCode.E;
    public KeyCode mainMouse = KeyCode.Mouse0;
    public KeyCode secondaryMouse = KeyCode.Mouse1;
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode jump = KeyCode.Space;
    public KeyCode tabPressed = KeyCode.Tab;

    private void Awake()
    {
        if (INSTANCE)
        {
            Destroy(gameObject);
        }
        else
        {
            INSTANCE = this;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {

        MainMouse();
        SecondaryMouse();
        TabPressed();
        Interact();
        Jump();
        Move();
        held = MainMouseHeld();
    }


    public void Interact()
    {
        if (Input.GetKeyDown(interact))
        {
            onInteract?.Invoke();
        }
    }


    public void MainMouse()
    {
        bool clicked = Input.GetKeyDown(mainMouse);
        if (clicked)
        {
            onMainMouseClick?.Invoke();
        }
    }
    public bool MainMouseHeld()
    {
        bool pressed = Input.GetKey(mainMouse);
        if (pressed)
        {
            onMainMousePressed?.Invoke();
        }
        return pressed;
    }
    public void SecondaryMouse()
    {

        bool keyDown = Input.GetKeyDown(secondaryMouse);
        bool keyUp = Input.GetKeyUp(secondaryMouse);

        if (keyDown)
        {
            onSecondaryMousePressed?.Invoke(true);
        }
        if (keyUp)
        {
            onSecondaryMousePressed?.Invoke(false);
        }

    }
    public void Move()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(moveUp))
        {
            direction += new Vector2(0, 1);
        }
        if (Input.GetKey(moveLeft))
        {
            direction += new Vector2(-1, 0);
        }
        if (Input.GetKey(moveDown))
        {
            direction += new Vector2(0, -1);
        }
        if (Input.GetKey(moveRight))
        {
            direction += new Vector2(1, 0);
        }

        onMoveKeyPressed.Invoke(direction);

    }

    public void Jump()
    {

    }

    public Vector3 GetMousePosition()
    {
        Vector3 VScreen = new Vector3();
        Vector3 positionPoint = new Vector3();

        VScreen.x = Input.mousePosition.x;
        VScreen.y = Input.mousePosition.y;
        VScreen.z = 10;
        positionPoint = Camera.main.ScreenToWorldPoint(VScreen);

        return positionPoint;
    }

    public void TabPressed()
    {

        if (Input.GetKeyDown(tabPressed))
        {

                onTabPressed.Invoke();
  
        }
    }

}
public enum InputModes
{
    Inventory,
    Game
}
