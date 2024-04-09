using Inventory;

namespace Interfaces
{
    public interface IInventory
    {
        public void OnItemSelect(int slotIndex);
        public void OnItemDeselect(int slotIndex);
        public void ClearSelection();
    }
}