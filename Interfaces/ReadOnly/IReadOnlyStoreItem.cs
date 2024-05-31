using UnityEngine;

namespace Interfaces.ReadOnly
{
    public interface IReadOnlyStoreItem
    {
        public Sprite ItemImage { get; }
        public Sprite CostItemImage { get; }
        public string ItemTitle { get; }
        public string CostItemTitle { get; }
        public string ItemId { get; }
        public string CostItemId { get; }
        public int ItemAmount { get; }
        public int CostItemAmount { get; }

    }
}