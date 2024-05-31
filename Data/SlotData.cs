using Interfaces.ReadOnly;
using UnityEngine;

namespace Data
{
    public class SlotData
    {
        public string itemId;
        public Sprite itemImage;
        public string itemName;
        public string itemDescript;
        public int maxAmount;
        
        public SlotData(string itemId, string itemName,
                        string itemDescript, Sprite itemImage,
                        int maxItemAmount = 1)
        {
            SetSlot(itemId, itemName, itemDescript, itemImage, maxItemAmount);
        }

        public void SetSlot(string itemId, string itemName,
                            string itemDescript, Sprite itemImage,
                            int maxItemAmount = 1)
        {
            this.itemId = itemId;
            this.itemName = itemName;
            this.itemDescript = itemDescript;
            this.itemImage = itemImage;
            this.maxAmount = maxItemAmount;
        }
    }
}