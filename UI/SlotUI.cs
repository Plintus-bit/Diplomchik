using Interfaces;
using Interfaces.ReadOnly;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] private Transform slot;
        [SerializeField] private Image slotImage;
        [SerializeField] private Image amountPlace;
        [SerializeField] private TMP_Text amountText;

        private IReadOnlySlotData _currSlotData;
        private IInventoryUIInteractions _inventory;
        private int _slotIndex;

        private static Sprite _emptySlot;
        private bool _isSelected;

        private Vector3 _startPos;

        public IReadOnlySlotData CurrSlotData => _currSlotData;

        private void Awake()
        {
            _startPos = slot.transform.localPosition;
            _emptySlot = Resources.Load<Sprite>("UI/EmptySlotIcon");
        }

        private void Start()
        {
            slotImage = GetComponentInChildren<Image>();
            RemoveItem(false);
        }

        public void PlayAnim(bool isSelect)
        {
            Vector3 pos;
            if (isSelect)
            {
                pos = _startPos + Vector3.down * 18f;
                if (slot.transform.localPosition == pos) return;

                slot.transform
                    .LeanMoveLocal(pos, 0.15f)
                    .setEase(LeanTweenType.easeOutSine);
                return;
            }

            pos = _startPos;
            if (slot.transform.localPosition == pos) return;

            slot.transform
                .LeanMoveLocal(_startPos, 0.15f)
                .setEase(LeanTweenType.easeOutSine);
        }

        public void SetSlotIndex(int index)
        {
            _slotIndex = index;
        }

        public void SetParent(IInventoryUIInteractions inventory)
        {
            _inventory = inventory;
        }

        public void AddItem(IReadOnlySlotData slot, bool isSelected)
        {
            ChangeIsSelect(false);
            if (_currSlotData != null && _currSlotData == slot)
            {
                _inventory.ClearSelection();
            }

            _currSlotData = slot;
            slotImage.sprite = slot.GetItemImage();
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
            
            if (isSelected)
            {
                EnterSlot();
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
                .ShowActiveItemPanelWithItem(_currSlotData, _slotIndex);
        }

        public void ExitSlot()
        {
            _inventory.HideActiveItemPanel();
        }

        public void OnSelect()
        {
            // slotImage.color = Color.green;
            PlayAnim(true);
        }

        public void OnDeselect()
        {
            // slotImage.color = Color.white;
            PlayAnim(false);
        }

        public void RemoveItem(bool isSelected)
        {
            slotImage.sprite = _emptySlot;
            _currSlotData = null;
            amountText.text = "";
            amountPlace.enabled = false;
            _isSelected = false;
            OnDeselect();

            if (isSelected)
            {
                ExitSlot();
            }
        }
        
    }
}