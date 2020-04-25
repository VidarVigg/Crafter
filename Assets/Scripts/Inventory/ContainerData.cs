using UnityEngine;

[CreateAssetMenu]
public class ContainerData : ScriptableObject
{

    public InteractableType interactableType;
    public int capacity;
    public Item[] possibleLoot;
}
public enum InteractableType
{
    None,
    Chest,
    Barrel,
}