using Interfaces.ReadOnly;
using UnityEngine;

namespace Data
{
    public class StoreItemData : IReadOnlyStoreItem
    {
        public Sprite itemImage;
        public string itemId;
        public string itemTitle;
        public int amount;
        
        public Sprite costItemImage;
        public string costItemId;
        public string costItemTitle;
        public int costItemAmount;

        public Sprite ItemImage => itemImage;
        public Sprite CostItemImage => costItemImage;
        public string ItemTitle => itemTitle;
        public string CostItemTitle => costItemTitle;
        public string ItemId => itemId;
        public string CostItemId => costItemId;
        public int ItemAmount => amount;
        public int CostItemAmount => costItemAmount;

        public StoreItemData(
            string itemId, int amount,
            string costItemId, int costItemAmount,
            string itemName, string costItemName)
        {
            this.itemId = itemId;
            this.itemTitle = itemName;
            this.amount = amount;
            
            this.costItemId = costItemId;
            this.costItemTitle = costItemName;
            this.costItemAmount = costItemAmount;

            LoadSprites(itemId, costItemId);
        }

        public StoreItemData(
            string[] data, string itemName, string costItemName)
        {
            itemId = data[0];
            itemTitle = itemName;
            int.TryParse(data[1], out amount);

            costItemId = data[2];
            costItemTitle = costItemName;
            int.TryParse(data[3], out costItemAmount);
            
            LoadSprites(itemId, costItemId);
        }

        private void LoadSprites(string itemId, string costItemId)
        {
            itemImage = Resources.Load<Sprite>("ItemsIcon/" + itemId);
            costItemImage = Resources.Load<Sprite>("ItemsIcon/" + costItemId);
        }
    }
}