using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{

    public static int index = -1;
    public int indexForShow;
    public string letter;
    public bool hover;
    private INodeObserver observer;
    public bool activated;

    public INodeObserver Observer
    {
        get { return observer; }
        set { observer = value; }
    }

    private void Start()
    {
    }

    internal void ShowIndex(int test)
    {
        indexForShow = test;
    }

    private void OnMouseOver()
    {
        hover = true;
        if (InputManager.INSTANCE.held)
        {
            if (!activated)
            {
                observer.Interacted(this);
                activated = true;
            }
        }
    }
    private void OnMouseExit()
    {
        hover = false;
    }
}
