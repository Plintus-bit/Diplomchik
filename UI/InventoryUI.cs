using System.Collections.Generic;
using Interfaces;
using Interfaces.ReadOnly;
using UnityEngine;

namespace UI
{
    public class InventoryUI : MonoBehaviour, IInventoryUIInteractions
    {
        [SerializeField] private ActiveItemUI activeItemUI;
        [SerializeField] private List<SlotUI> _slots;
        
        private IInventory _inventory;

        public void Change(int slotIndex, IReadOnlySlotData slot)
        {
            bool isEqualWithActive = activeItemUI.Check(slotIndex);

            if (slot.IsEmpty())
            {
                _slots[slotIndex].RemoveItem(isEqualWithActive);
                return;
            }
            _slots[slotIndex].AddItem(slot, isEqualWithActive);
        }

        public void SetInventory(IInventory inventory)
        {
            _inventory = inventory;
            FillSlots();
        }
        
        private void FillSlots()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].SetSlotIndex(i);
                _slots[i].SetParent(this);
            }
        }

        public void ShowActiveItemPanelWithItem(IReadOnlySlotData slot, int slotIndex)
        {
            activeItemUI.SetActive(slot, slotIndex);
        }

        public void HideActiveItemPanel()
        {
            activeItemUI.SetInactive();
        }

        public void OnItemSelect(int slotIndex)
        {
            _inventory.OnItemSelect(slotIndex);
        }

        public void OnItemDeselect(int slotIndex)
        {
            _inventory.OnItemDeselect(slotIndex);
        }

        public void ClearSelection()
        {
            _inventory.ClearSelection();
        }

        public void ChangeItemSelect(int slotIndex, bool status = false)
        {
            _slots[slotIndex].ChangeIsSelect(status);
        }
    };
}