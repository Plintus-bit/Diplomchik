using Inventory;

namespace Data
{
    public struct IngredientData
    {
        public string itemId;
        public int amount;

        public IngredientData(string itemId, int amount)
        {
            this.itemId = itemId;
            this.amount = amount;
        }
        public IngredientData(Slot slot)
        {
            itemId = slot.GetItemId();
            amount = slot.GetAmount();
        }
    }
}