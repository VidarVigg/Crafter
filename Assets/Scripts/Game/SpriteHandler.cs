using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    private static SpriteHandler instance;
    public static SpriteHandler GetInstance()
    {
        return instance;
    }
    [System.Serializable]
    private struct itemSpriteStruct
    {
        public Sprite sprite;
        public string key;
    }

    [System.Serializable]
    private struct backgroundSpriteStruct
    {
        public Sprite sprite;
        public string key;
    }

    [SerializeField]
    private itemSpriteStruct[] itemSprites;

    [SerializeField]
    private itemSpriteStruct[] backgroundSprites;

    private void Awake()
    {
        instance = this;
    }
    public Sprite GetItemSprite(string key)
    {
        Sprite sprite = null;
        for (int i = 0; i < itemSprites.Length; i++)
        {
            if (itemSprites[i].key == key)
            {

                sprite = itemSprites[i].sprite;

            }
        }
        return sprite;

    }
    private void Start()
    {
    }
    public Sprite GetBackgroundSprite(string key)
    {
        Sprite sprite;

        for (int i = 0; i < backgroundSprites.Length; i++)
        {
            if (backgroundSprites[i].key == key)
            {
                sprite = backgroundSprites[i].sprite;
                return sprite;
            }
            else
            {
                Debug.Log("Sprite Not Found");
            }
        }
        return null;

    }


}
