using System;
using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    [SerializeField] private int _totalSlots = 16;
    [SerializeField] private InventorySlot[] _slots;

    public Action<int, InventorySlot> OnInventoryUpdate;

    private void Awake()
    {
        // Make new array with length of Total Slots
        _slots = new InventorySlot[_totalSlots];
        
        // Make new Inventory Slot until the slot is full
        for (int i = 0; i <= _totalSlots; i++)
        {
            _slots[i] = new InventorySlot();
        }
    }

    public bool AddItem(ItemSO item, int amount = 1)
    {
        // Check if there is a same slot with same item and not empty also not full yet
        for (int i = 0; i <= _totalSlots; i++)
        {
            if (!_slots[i].IsEmpty && _slots[i].Item == item && !_slots[i].IsFull)
            {
                int spaceLeft = item.MaxStack -  _slots[i].Quantity;
                int addAmount = Math.Min(spaceLeft, amount);
                _slots[i].AddQuantity(addAmount);
                amount -= addAmount;

                OnInventoryUpdate?.Invoke(i, _slots[i]);
                if (amount <= 0) return true;
            }
        }

        // Check if there is another slot available
        for (int i = 0; i <= _totalSlots; i++)
        {
            if (!_slots[i].IsEmpty)
            {
                int addAmount = Math.Min(item.MaxStack, amount);
                _slots[i].AssignItem(item, addAmount);
                amount -= addAmount;
                OnInventoryUpdate?.Invoke(i, _slots[i]);
                if (amount <= 0) return true;
            }
        }

        // Say if inventory slots is full
        Debug.Log("Inventory is full");
        return false;
    }
}