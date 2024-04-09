using Data;
using Interfaces.ReadOnly;
using UnityEngine;

namespace Inventory
{
    public class Slot : IReadOnlySlotData
    {
        private SlotData slotData;
        private int amount;

        public Slot()
        {
            ClearSlot();
        }

        public SlotData GetSlotData()
        {
            return slotData;
        }
        
        public int AddAmount(int itemsAmount)
        {
            int newItemAmount = itemsAmount + amount;
            return SetAmount(slotData.maxAmount, newItemAmount);
        }

        public int SetSlot(SlotData newSlotData, int itemsAmount = 0)
        {
            slotData = newSlotData;
            return SetAmount(newSlotData.maxAmount, itemsAmount);
        }

        public int SetAmount(int maxAmount, int newAmount)
        {
            if (newAmount > maxAmount)
            {
                amount = maxAmount;
                return newAmount - maxAmount;
            }
            amount = newAmount;
            return 0;
        }

        public int DeleteAmount(int amountToDelete)
        {
            if (amountToDelete >= amount)
            {
                int needToDeleteAmount = amountToDelete - amount;
                ClearSlot();
                return needToDeleteAmount;
            }
            amount -= amountToDelete;
            return 0;
        }

        public void SetSlot(Slot slot)
        {
            slotData = slot.GetSlotData();
            amount = slot.amount;
        }

        public void ClearSlot()
        {
            slotData = null;
            amount = 0;
        }

        public bool IsEmpty()
        {
            return slotData == null || amount == 0;
        }

        public bool HasFreeRoom()
        {
            return slotData.maxAmount - amount > 0;
        }
        
        public Slot GetSlot()
        {
            return this;
        }

        public string GetItemId()
        {
            return slotData.itemId;
        }

        public string GetItemName()
        {
            return slotData.itemName;
        }

        public string GetItemDescription()
        {
            return slotData.itemDescript;
        }

        public int GetAmount()
        {
            return amount;
        }

        public int GetMaxAmount()
        {
            return slotData.maxAmount;
        }

        public void Print()
        {
            Debug.Log(slotData.itemName + ": " + amount);
        }
    }
}