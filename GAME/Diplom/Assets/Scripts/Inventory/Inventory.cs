using System.Collections.Generic;
using Data;
using Interfaces;
using Managers;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour, IInventorySlots, IInventory
    {
        [SerializeField] private int inventorySize;
        [SerializeField] private InventoryUI inventoryUI;

        private DataLoader dataLoader;
        private List<Slot> slots;
        private int lastEmptySlotIndex;

        private Craft craftSystem;
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            dataLoader = new DataLoader();
            craftSystem = new Craft();
            slots = new List<Slot>(inventorySize);
            CreateSlots();
            inventoryUI.SetSize(inventorySize);
            inventoryUI.SetInventory(this);
        }

        private void CreateSlots()
        {
            for (int i = 0; i < inventorySize; i++)
            {
                slots.Insert(i, new Slot());
            }
            lastEmptySlotIndex = 0;
        }


        public int AddItem(string itemId, int amount = 1)
        {
            int amountToAdd = amount;
            Slot slot = GetSameSlot(itemId);
            if (slot == null && !HasFreeSlot()) return amountToAdd;
            if (slot == null && HasFreeSlot())
            {
                Slot newSlot = LoadData(itemId);
                amountToAdd = newSlot.SetAmount(newSlot.GetMaxAmount(), amountToAdd);
                inventoryUI.Change(lastEmptySlotIndex, newSlot);
                lastEmptySlotIndex += 1;
                if (amountToAdd > 0)
                {
                    slots[lastEmptySlotIndex] = LoadData(itemId);
                    slot = slots[lastEmptySlotIndex];
                    lastEmptySlotIndex += 1;
                }
            }
            if (slot != null)
            {
                amountToAdd = slot.AddAmount(amountToAdd);
                while (amountToAdd > 0 && HasFreeSlot())
                {
                    amountToAdd = AddAmount(itemId, amountToAdd);
                }
                inventoryUI.Change(slots.IndexOf(slot), slot);
                if (amountToAdd > 0) return amountToAdd;
            }
            return amountToAdd;
        }
        
        public int AddAmount(string itemId, int amount)
        {
            Slot slot = GetSameSlot(itemId);
            if (slot == null && HasFreeSlot())
            {
                slot = LoadData(itemId);
                inventoryUI.Change(lastEmptySlotIndex, slot);
                lastEmptySlotIndex += 1;
            }
            if (slot != null)
            {
                int amountToAdd = slot.AddAmount(amount);
                inventoryUI.Change(slots.IndexOf(slot), slot);
                return amountToAdd;
            }
            return 0;
        }

        public Slot GetSameSlot(string itemId)
        {
            foreach (var slot in slots)
            {
                if (slot.IsEmpty()) return null;
                if (slot.GetItemId() == itemId && slot.HasFreeRoom()) return slot;
            }
            return null;
        }

        public bool TryRemoveItem(string itemId, int amountToClear = 1)
        {
            if (IsEmpty() || !HasEnoughAmount(itemId, amountToClear)) return false;
            int needToClear = amountToClear;
            int slotCleared = 0;
            for (int i = 0; i < lastEmptySlotIndex; i++)
            {
                if (slots[i].GetItemId() == itemId)
                {
                    needToClear = slots[i].DeleteAmount(needToClear);
                    if (slots[i].IsEmpty())
                    {
                        slotCleared += 1;
                    }
                    if (needToClear <= 0)
                    {
                        break;
                    }
                }
            }
            MoveItems(0, lastEmptySlotIndex);
            UpdateUI(0, lastEmptySlotIndex);
            lastEmptySlotIndex -= slotCleared;
            return true;
        }

        protected void MoveItems(int indexStart, int indexEnd)
        {
            for (int i = indexStart; i <= indexEnd; i++)
            {
                int nextIndex = GetNotEmptySlotFromIndex(i + 1);
                if (nextIndex >= inventorySize || nextIndex < 0) break;
                if (slots[i].IsEmpty())
                {
                    slots[i].SetSlot(slots[nextIndex]);
                    slots[nextIndex].ClearSlot();
                }
            }
        }
        
        protected void UpdateUI(int indexStart, int indexEnd)
        {
            for (int i = indexStart; i < indexEnd; i++)
            {
                inventoryUI.Change(i, slots[i]);
            }
        }

        public bool HasEnoughAmount(string itemId, int amount)
        {
            int itemAmount = 0;
            foreach (var slot in slots)
            {
                if (slot.IsEmpty()) continue;
                if (slot.GetItemId() == itemId) itemAmount += slot.GetAmount();
            }
            return itemAmount >= amount;
        }

        private int GetNotEmptySlotFromIndex(int index)
        {
            for (int i = index; i < lastEmptySlotIndex; i++)
            {
                if (!slots[i].IsEmpty()) return i;
            }
            return -1;
        }

        public void OnItemSelect(int slotIndex)
        {
            bool isInRecipes = craftSystem.AddItemAndCheckRecipe(slots[slotIndex]);
            if (!isInRecipes)
            {
                UpdateUISelected(0, lastEmptySlotIndex, false);
                return;
            }
            bool isCrafted = craftSystem.TryDoCraft(this);
            if (isCrafted)
            {
                UpdateUISelected(0, inventorySize, false);
            }
            else
            {
                UpdateUISelected(slotIndex, true);
            }
        }

        public void UpdateUISelected(int indexStart, int indexEnd, bool status = false)
        {
            for (int i = indexStart; i < indexEnd; i++)
            {
                inventoryUI.ChangeItemSelect(i, status);
            }
        }

        public void UpdateUISelected(int index, bool status = false)
        {
            inventoryUI.ChangeItemSelect(index, status);
        }

        public Slot LoadData(string itemId)
        {
            SlotData slot = dataLoader.GetDataByItemId(itemId);
            slots[lastEmptySlotIndex].SetSlot(slot);
            return slots[lastEmptySlotIndex];
        }

        public bool HasFreeSlot()
        {
            if (lastEmptySlotIndex == inventorySize)
            {
                return false;
            }
            return true;
        }

        public bool IsEmptySlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < inventorySize)
            {
                return slots[slotIndex].IsEmpty();
            }
            return false;
        }

        public void OnItemDeselect(int slotIndex)
        {
            craftSystem.RemoveItem(slots[slotIndex]);
            UpdateUISelected(slotIndex, false);
        }

        public void ClearSelection()
        {
            craftSystem.ClearCraft();
            UpdateUISelected(0, inventorySize, false);
        }
        
        public bool IsEmpty()
        {
            return lastEmptySlotIndex == 0;
        }

        public void Print()
        {
            Debug.Log("начало");
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty())
                {
                    Debug.Log("Slot " + slots.IndexOf(slot));
                    slot.Print();
                }
            }
        }
    }
}
