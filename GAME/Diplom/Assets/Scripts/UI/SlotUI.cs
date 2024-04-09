using System;
using Interfaces;
using Interfaces.ReadOnly;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] private Image slotImage;
        [SerializeField] private Image amountPlace;
        [SerializeField] private TMP_Text amountText;

        private IReadOnlySlotData _currSlotData;
        private IInventoryUIInteractions _inventory;
        private int _slotIndex;

        private static Sprite _emptySlot;
        private bool _isSelected;

        private void Awake()
        {
            _emptySlot = Resources.Load<Sprite>("UI/EmptySlotIcon");
        }

        private void Start()
        {
            slotImage = GetComponentInChildren<Image>();
            RemoveItem();
        }

        public void SetSlotIndex(int index)
        {
            _slotIndex = index;
        }

        public void SetParent(IInventoryUIInteractions inventory)
        {
            _inventory = inventory;
        }

        public void AddItem(Sprite itemImage, IReadOnlySlotData slot)
        {
            ChangeIsSelect(false);
            if (_currSlotData != null && _currSlotData == slot)
            {
                _inventory.ClearSelection();
            }
            _currSlotData = slot;
            slotImage.sprite = itemImage;
            if (slot.GetMaxAmount() == slot.GetAmount() && slot.GetMaxAmount() == 1)
            {
                amountPlace.enabled = false;
                amountText.text = "";
            }
            else
            {
                amountPlace.enabled = true;
                amountText.text = $"{slot.GetAmount()}";
            }
        }

        public void TrySelect()
        {
            if (_currSlotData == null) return;
            if (_isSelected)
            {
                _inventory.OnItemDeselect(_slotIndex);
                return;
            }
            _inventory.OnItemSelect(_slotIndex);
        }

        public void ChangeIsSelect(bool status)
        {
            _isSelected = status;
            if (status)
            {
                OnSelect();
            }
            else
            {
                OnDeselect();
            }
        }

        public void EnterSlot()
        {
            if(_currSlotData != null) _inventory
                .ShowActiveItemPanelWithItem(slotImage.sprite, _currSlotData);
        }

        public void ExitSlot()
        {
            if(_currSlotData != null) _inventory.HideActiveItemPanel();
        }

        public void OnSelect()
        {
            slotImage.color = Color.green;
        }

        public void OnDeselect()
        {
            slotImage.color = Color.white;
        }

        public void RemoveItem()
        {
            slotImage.sprite = _emptySlot;
            _currSlotData = null;
            amountText.text = "";
            amountPlace.enabled = false;
            _isSelected = false;
            OnDeselect();
        }
        
    }
}