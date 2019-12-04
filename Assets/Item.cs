using UnityEngine;

internal abstract class Item : ScriptableObject
{
    [SerializeField]
    internal string itemName;
    [SerializeField]
    internal Sprite swordSprite;

    internal virtual Item CreateInstance() {return null; }

}