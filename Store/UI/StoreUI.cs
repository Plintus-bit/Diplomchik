using System.Collections.Generic;
using Interfaces;
using Interfaces.ReadOnly;
using Managers;
using TMPro;
using UI;
using UnityEngine;

namespace Store.UI
{
    public class StoreUI : BasicUIWindow
    {
        [SerializeField] private string _storeItemsId;
        [SerializeField] private StoreItemUI _prefab;
        [SerializeField] private TMP_Text _storeMessagePlace;
        private Vector2 _storeItemUISizes;
        
        private Store _store;

        private Dictionary<string, StoreItemUI> _itemsUI;

        public Transform _startTransformPoint;
        
        protected override void Initialize()
        {
            base.Initialize();
            
            _charService = FindObjectOfType<CharacterService>();
            _store = new Store(_storeItemsId, _charService);

            _storeItemUISizes = _prefab.GetComponent<RectTransform>().rect.size;
            CreateStoreItemsUI(_store.StoreItemsCount);

            _storeMessagePlace.text = string.Empty;
        }

        public void CreateStoreItemsUI(int count)
        {
            _itemsUI = new Dictionary<string, StoreItemUI>(count);

            int maxStrokeLength = 3;
            for (int i = 0; i < count; i++)
            {
                StoreItemUI tempItemUI = Instantiate(_prefab, transform);
                IReadOnlyStoreItem item = _store.GetItem(i);
                tempItemUI.SetStoreItem(item);
                tempItemUI.SetParent(this);
                tempItemUI.SetActiveBuyButton(true);

                float newXPos = (_storeItemUISizes.x + 20f) * (i % maxStrokeLength);
                float newYPos = (_storeItemUISizes.y + 40f) * (i / maxStrokeLength);

                tempItemUI.transform.localPosition = _startTransformPoint.localPosition;
                tempItemUI.transform.localPosition += new Vector3(
                    newXPos, -1 * newYPos, 0f
                );

                _itemsUI.Add(item.ItemId, tempItemUI);
                
                TryAddListener(tempItemUI);
            }

        }
        
        public void TryBuyItem(string itemid)
        {
            bool successBuy = _store.TryBuyItem(itemid, out string msg);
            _storeMessagePlace.text = msg;

            if (successBuy)
            {
                _itemsUI[itemid].SetActiveBuyButton(false);
            }
        }

        public override void OnCloseWindow()
        {
            _storeMessagePlace.text = string.Empty;
        }

        public override void OnOpenWindow()
        {
            _storeMessagePlace.text = _store.GetStartMessage();
        }
    }
}