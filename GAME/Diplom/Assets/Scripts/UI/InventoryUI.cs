using System.Collections.Generic;
using Interfaces;
using Interfaces.ReadOnly;
using UnityEngine;

namespace Inventory
{
    public class InventoryUI : MonoBehaviour, IInventoryUIInteractions
    {
        [SerializeField] private RectTransform inventoryPanel;
        [SerializeField] private RectTransform slotPrefab;
        [SerializeField] private ActiveItemUI activeItemUI;

        private List<SlotUI> _slots;
        private IInventory _inventory;
        
        private int inventorySize;
        private float slotWidth;
        private float betweenSlotInterval;
        private float slotStartPose;

        private void Awake()
        {
            _slots = new List<SlotUI>();
            slotStartPose = slotPrefab.localPosition.x;
            slotWidth = slotPrefab.rect.width;
            betweenSlotInterval = 14;
        }

        public void Change(int slotIndex, IReadOnlySlotData slot)
        {
            if (slot.IsEmpty())
            {
                _slots[slotIndex].RemoveItem();
                return;
            }
            _slots[slotIndex].AddItem(Resources.Load<Sprite>("ItemsIcon/" + slot.GetItemId()),
                slot);
        }

        public void SetSize(int newSize)
        {
            inventorySize = newSize;
            CreateSlots();
        }

        public void SetInventory(IInventory inventory)
        {
            _inventory = inventory;
        }
        
        private void CreateSlots()
        {
            for (int i = 0; i < inventorySize; i++)
            {
                RectTransform tempPanel = Instantiate(slotPrefab, inventoryPanel);
                float newXPos = slotStartPose + i * slotWidth + (i + 1) * betweenSlotInterval;
                tempPanel.localPosition = new Vector2(
                    newXPos, tempPanel.localPosition.y);
                SlotUI newSlot = tempPanel.GetComponentInChildren<SlotUI>();
                newSlot.SetSlotIndex(i);
                newSlot.SetParent(this);
                _slots.Add(newSlot);
            }
        }

        public void ShowActiveItemPanelWithItem(Sprite icon, IReadOnlySlotData slot)
        {
            activeItemUI.SetActive(icon, slot);
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