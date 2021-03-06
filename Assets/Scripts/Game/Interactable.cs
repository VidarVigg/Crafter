﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Interactable : MonoBehaviour, IInteractable
{
    public delegate void ActivateInteractable();
    public ActivateInteractable activateInteractable;
    public delegate void DeactivateInteractable();
    public DeactivateInteractable deactivateInteractable;

    public virtual void OnTriggerEnter(Collider collision)
    {
        PlayerManager player;
        if (player = (collision.GetComponent<PlayerManager>()))
        {
            player.InInteractingDistance = true;
            player.ProcessInteractable(this);
        }
    }
    public virtual void OnTriggerExit(Collider collision)
    {
        PlayerManager player;
        if (player = (collision.GetComponent<PlayerManager>()))
        {
            player.InInteractingDistance = false;
            player.ProcessInteractable(null);
            player.ClosestInteractable = null;
            deactivateInteractable?.Invoke();
        }
    }
    public virtual void Activate()
    {
        activateInteractable?.Invoke();
    }

}
