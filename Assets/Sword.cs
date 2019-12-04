using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
internal class Sword : Weapon
{

    [SerializeField]
    internal int sharpness;


    internal override Item CreateInstance()
    {

        Sword newSword = CreateInstance<Item>() as Sword;
        newSword.itemName = itemName;
        newSword.damage = damage;
        newSword.sharpness = sharpness;
        newSword.swordSprite = swordSprite;
        return newSword;

    }
}
