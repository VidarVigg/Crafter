using UnityEngine;
using System;

[Serializable]
public class SpellDatabase
{
    [Serializable]
    public struct Spell
    {
        public string name;
        public string combination;
    }
    [SerializeField]
    public Spell[] spells;
}