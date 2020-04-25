using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PlayerSave: SaveObject
{
    public string playerName;
    public Vector2 playerPosition = Vector2.zero;
    public string[] playerInventory;


    public PlayerSave(string playerName, Vector2 playerPosition, List<string> playerInventory)
    {
        this.playerName = playerName;
        this.playerPosition = playerPosition;
        this.playerInventory = playerInventory.ToArray();
    }
}
