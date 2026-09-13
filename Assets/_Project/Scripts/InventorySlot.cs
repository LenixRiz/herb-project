using System;

public class InventorySlot
{
    public ItemSO Item { get; private set; }
    public int Quantity { get; private set; }

    public bool IsEmpty => Item == null && Quantity <= 0;
    public bool IsFull => Item != null && Quantity <= Item.MaxStack;

    public void AssignItem(ItemSO newItem, int amount)
    {
        Item = newItem;
        Quantity += amount;
    }

    public void AddQuantity(int amount) => Quantity += amount;

    public void RemoveQuantity(int amount)
    {
        Quantity -= amount;
        if (Quantity <= 0) clear();
    }

    public void clear()
    {
        Item = null;
        Quantity = 0;
    }
}