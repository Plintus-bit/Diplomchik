using System.Collections.Generic;
using Data;
using Interfaces;
using Managers;
using UI;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour, IInventorySlots, IInventory
    {
        [SerializeField] private int inventorySize;
        [SerializeField] private InventoryUI inventoryUI;

        private SlotDataLoader _slotDataLoader;
        private List<Slot> _slots;
        private int _lastEmptySlotIndex;

        private Craft _craftSystem;

        public int Size => inventorySize;
        
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _slotDataLoader = SlotDataLoader.Instance;
            _craftSystem = new Craft();
            _slots = new List<Slot>(inventorySize);
            CreateSlots();
            inventoryUI.SetInventory(this);
        }

        private void CreateSlots()
        {
            for (int i = 0; i < inventorySize; i++)
            {
                _slots.Insert(i, new Slot());
            }
            _lastEmptySlotIndex = 0;
        }
        
        private int GetNotEmptySlotFromIndex(int index)
        {
            for (int i = index; i < _lastEmptySlotIndex; i++)
            {
                if (!_slots[i].IsEmpty()) return i;
            }
            return -1;
        }
        
        private int GetItemAmountInInventory(string itemId)
        {
            int amount = 0;
            foreach (var slot in _slots)
            {
                if (slot.IsEmpty()) break;
                if (slot.GetItemId() == itemId)
                    amount += slot.GetAmount();
            }
            return amount;
        }
        
        protected void MoveItems(int indexStart, int indexEnd)
        {
            for (int i = indexStart; i <= indexEnd; i++)
            {
                int nextIndex = GetNotEmptySlotFromIndex(i + 1);
                if (nextIndex >= inventorySize || nextIndex < 0) break;
                if (_slots[i].IsEmpty())
                {
                    _slots[i].SetSlot(_slots[nextIndex]);
                    _slots[nextIndex].ClearSlot();
                }
            }
        }
        
        protected SlotData LoadData(string itemId)
        {
            SlotData slot = _slotDataLoader.GetDataById(itemId);
            return slot;
        }

        protected Slot LoadDataAndAddSlot(string itemId)
        {
            SlotData slot = _slotDataLoader.GetDataById(itemId);
            _slots[_lastEmptySlotIndex].SetSlot(slot);
            return _slots[_lastEmptySlotIndex];
        }
        
        public void UpdateUI(int indexStart, int indexEnd)
        {
            for (int i = indexStart; i < indexEnd; i++)
            {
                inventoryUI.Change(i, _slots[i]);
            }
        }

        public int AddItem(string itemId, int amount = 1, bool isUpdateUI = true)
        {
            int amountToAdd = amount;
            Slot slot = GetSameSlot(itemId);
            if (slot == null && !HasFreeSlot()) return amountToAdd;
            if (slot == null && HasFreeSlot())
            {
                Slot newSlot = LoadDataAndAddSlot(itemId);
                amountToAdd = newSlot.SetAmount(newSlot.GetMaxAmount(), amountToAdd);
                inventoryUI.Change(_lastEmptySlotIndex, newSlot);
                _lastEmptySlotIndex += 1;
                if (amountToAdd > 0)
                {
                    _slots[_lastEmptySlotIndex] = LoadDataAndAddSlot(itemId);
                    slot = _slots[_lastEmptySlotIndex];
                    _lastEmptySlotIndex += 1;
                }
            }
            if (slot != null)
            {
                amountToAdd = slot.AddAmount(amountToAdd);
                while (amountToAdd > 0 && HasFreeSlot())
                {
                    amountToAdd = AddAmount(itemId, amountToAdd);
                }
                inventoryUI.Change(_slots.IndexOf(slot), slot);
                if (amountToAdd > 0) return amountToAdd;
            }
            return amountToAdd;
        }
        
        public int AddAmount(string itemId, int amount)
        {
            Slot slot = GetSameSlot(itemId);
            if (slot == null && HasFreeSlot())
            {
                slot = LoadDataAndAddSlot(itemId);
                inventoryUI.Change(_lastEmptySlotIndex, slot);
                _lastEmptySlotIndex += 1;
            }
            if (slot != null)
            {
                int amountToAdd = slot.AddAmount(amount);
                inventoryUI.Change(_slots.IndexOf(slot), slot);
                return amountToAdd;
            }
            return 0;
        }

        public Slot GetSameSlot(string itemId)
        {
            foreach (var slot in _slots)
            {
                if (slot.IsEmpty()) return null;
                if (slot.GetItemId() == itemId && slot.HasFreeRoom()) return slot;
            }
            return null;
        }

        public bool TryRemoveItem(string itemId, int amountToClear = 1,
            bool isUpdateUI = true)
        {
            if (IsEmpty() || !HasEnoughAmount(itemId, amountToClear)) return false;
            int needToClear = amountToClear;
            int slotCleared = 0;
            for (int i = 0; i < _lastEmptySlotIndex; i++)
            {
                if (_slots[i].GetItemId() == itemId)
                {
                    needToClear = _slots[i].DeleteAmount(needToClear);
                    if (_slots[i].IsEmpty())
                    {
                        slotCleared += 1;
                    }
                    if (needToClear <= 0)
                    {
                        break;
                    }
                }
            }
            MoveItems(0, _lastEmptySlotIndex);
            if (isUpdateUI)
            {
                UpdateUI(0, _lastEmptySlotIndex);
            }
            _lastEmptySlotIndex -= slotCleared;
            return true;
        }

        public bool HasEnoughAmount(string itemId, int amount)
        {
            int itemAmount = 0;
            foreach (var slot in _slots)
            {
                if (slot.IsEmpty()) continue;
                if (slot.GetItemId() == itemId) itemAmount += slot.GetAmount();
            }
            
            return amount > 0 && amount <= itemAmount;
        }

        public void OnItemSelect(int slotIndex)
        {
            Slot tempSlot = _slots[slotIndex];
            Slot newTempSlot = new Slot();
            int fullAmount = GetItemAmountInInventory(tempSlot.GetItemId());
            newTempSlot.SetSlot(tempSlot);
            newTempSlot.SetAmount(fullAmount, fullAmount);
            
            bool isInRecipes = _craftSystem.AddItemAndCheckRecipe(newTempSlot);
            if (!isInRecipes)
            {
                UpdateUISelected(0, _lastEmptySlotIndex, false);
                return;
            }
            bool isCrafted = _craftSystem.TryDoCraft(this);
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

        public bool HasFreeSlot()
        {
            if (_lastEmptySlotIndex == inventorySize)
            {
                return false;
            }
            return true;
        }

        public bool IsEmptySlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < inventorySize)
            {
                return _slots[slotIndex].IsEmpty();
            }
            return false;
        }

        public void OnItemDeselect(int slotIndex)
        {
            _craftSystem.RemoveItem(_slots[slotIndex]);
            UpdateUISelected(slotIndex, false);
        }

        public void ClearSelection()
        {
            _craftSystem.ClearCraft();
            UpdateUISelected(0, inventorySize, false);
        }
        
        public bool IsEmpty()
        {
            return _lastEmptySlotIndex == 0;
        }

        public bool HasItem(string itemId)
        {
            foreach (var item in _slots)
            {
                if (item.IsEmpty()) continue;
                if (item.GetItemId() == itemId) return true;
            }
            return false;
        }

        public void Print()
        {
            Debug.Log("начало");
            foreach (var slot in _slots)
            {
                if (!slot.IsEmpty())
                {
                    Debug.Log("Slot " + _slots.IndexOf(slot));
                    slot.Print();
                }
            }
        }
    }
}
