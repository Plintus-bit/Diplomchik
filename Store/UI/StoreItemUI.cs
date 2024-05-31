using Interfaces.ReadOnly;
using TMPro;
using UI;
using UnityEngine.UI;

namespace Store.UI
{
    public class StoreItemUI : UIInputListener
    {
        private StoreUI _parent;
        private string _storeItemId;
        
        public Image storeItemImage;
        public TMP_Text storeItemTitle;
        
        public Image costItemImage;
        public TMP_Text costItemTitle;
        public TMP_Text costItemAmount;

        public Button btnBuy;

        public void SetStoreItem(IReadOnlyStoreItem data)
        {
            _storeItemId = data.ItemId;

            storeItemTitle.text = data.ItemTitle;
            costItemTitle.text = data.CostItemTitle;
            
            storeItemImage.sprite = data.ItemImage;
            costItemImage.sprite = data.CostItemImage;
            costItemAmount.text = data.CostItemAmount.ToString();
        }

        public void SetParent(StoreUI parent)
        {
            this._parent = parent;
        }
        
        public void TryBuy()
        {
            _parent.TryBuyItem(_storeItemId);
        }

        public void SetActiveBuyButton(bool isActive)
        {
            btnBuy.interactable = isActive;
        }
    }
}