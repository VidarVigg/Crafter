using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WordDatabase
{

    [SerializeField]
    public string[] highTierAdjective = new string[] 
    {"Gleaming", "Rotting", "Glowing", "Ferocious", "Scorching", "Ominous", "Serene", "Enduring", "Forceful",
     "Deep", "Sorrowful", "Graceful"
    };

    [SerializeField]
    public string[] lowTierAdjective = new string[] { "Cold", "Dark", "Bright", "Honed", "Sturdy", "Jagged",
     "Rough", "Light", "Reinforced", "Fleeting"
    };

    [SerializeField]
    public string[] indefiniteNoun = new string[] 
    {"Glow", "Embers", "Twilight", "Stones", "Doom", "Abyss", "Mountains", "Dawn"};

}
