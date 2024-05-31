using Interfaces.ReadOnly;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ActiveItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image icon;

        private int _tempSlotIndex;
        public int CurrIndex => _tempSlotIndex;
        
        private void Start()
        {
            SetInactive();
        }

        public void SetActive(IReadOnlySlotData slot, int slotIndex)
        {
            _tempSlotIndex = slotIndex;
            title.text = slot.GetItemName();
            description.text = slot.GetItemDescription();
            icon.sprite = slot.GetItemImage();
            gameObject.SetActive(true);
        }

        public void SetInactive()
        {
            gameObject.SetActive(false);
            _tempSlotIndex = -1;
            title.text = "";
            description.text = "";
            icon.sprite = null;
        }

        public bool Check(int index)
        {
            if (_tempSlotIndex == index) return true;
            return false;
        }
    }
}