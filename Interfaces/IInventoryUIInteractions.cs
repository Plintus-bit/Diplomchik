using Interfaces.ReadOnly;

namespace Interfaces
{
    public interface IInventoryUIInteractions
    {
        public void ShowActiveItemPanelWithItem(IReadOnlySlotData slot, int slotIndex);
        public void HideActiveItemPanel();
        public void OnItemSelect(int slotIndex);
        public void OnItemDeselect(int slotIndex);
        public void ClearSelection();
    }
}