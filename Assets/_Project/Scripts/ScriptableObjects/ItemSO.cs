using UnityEngine;

public class ItemSO : ScriptableObject
{
    [field: SerializeField] public string ItemId { get; private set; }
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public Sprite ItemIcon { get; private set; }
    [field: SerializeField] public int MaxStack { get; private set; } = 99;
}