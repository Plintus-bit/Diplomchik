using Interfaces.ReadOnly;

namespace Data
{
    public class SlotData
    {
        public string itemId;
        public string itemName;
        public string itemDescript;
        public int maxAmount;
        
        public SlotData(string itemId, string itemName,
                        string itemDescript, int maxItemCount = 1)
        {
            SetSlot(itemId, itemName, itemDescript, maxItemCount);
        }

        public void SetSlot(string itemId, string itemName,
                            string itemDescript, int maxItemCount = 1)
        {
            this.itemId = itemId;
            this.itemName = itemName;
            this.itemDescript = itemDescript;
            this.maxAmount = maxItemCount;
        }
    }
}