using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordHandler : MonoBehaviour
{
    public WordDatabase wordDatabase = new WordDatabase();

    public string GenerateCombination()
    {
        string output;
        string adjective;
        string noun;
        adjective = wordDatabase.highTierAdjective[Random.Range(0, wordDatabase.highTierAdjective.Length)];
        noun = wordDatabase.indefiniteNoun[Random.Range(0, wordDatabase.indefiniteNoun.Length)];
        output = "Legendary Sword of " + adjective + " " + noun;
        return output;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(GenerateCombination());
        }
    }

}
