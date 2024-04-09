using Interfaces.ReadOnly;
using Inventory;
using UnityEngine;

namespace Interfaces
{
    public interface IInventoryUIInteractions
    {
        public void ShowActiveItemPanelWithItem(Sprite icon, IReadOnlySlotData slot);
        public void HideActiveItemPanel();
        public void OnItemSelect(int slotIndex);
        public void OnItemDeselect(int slotIndex);
        public void ClearSelection();
    }
}