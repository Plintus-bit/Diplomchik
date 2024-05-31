using Inventory;
using UnityEngine;

namespace Interfaces.ReadOnly
{
    public interface IReadOnlySlotData
    {
        public Slot GetSlot();
        public string GetItemId();
        public string GetItemName();
        public string GetItemDescription();
        public int GetAmount();
        public int GetMaxAmount();
        public bool IsEmpty();
        public Sprite GetItemImage();
    }
}