namespace Interfaces
{
    public interface IInventorySlots
    {
        public int AddItem(string itemId, int amount = 1);
        public bool TryRemoveItem(string itemId, int amountToRemove);
    }
}