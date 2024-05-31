namespace Interfaces
{
    public interface IInventorySlots
    {
        public int Size { get; }
        
        public int AddItem(string itemId, int amount = 1,
            bool isUpdateUI = true);
        public bool TryRemoveItem(string itemId, int amountToRemove,
            bool isUpdateUI = true);
        public bool HasItem(string itemId);

        public void UpdateUI(int indexStart, int indexEnd);
        public bool HasEnoughAmount(string itemId, int amount);
    }
}