using System.Collections.Generic;
using Data;
using Enums;
using Interfaces;
using Interfaces.ReadOnly;
using Managers;
using Randomizable;

namespace Store
{
    public class Store
    {
        private Dictionary<string, StoreItemData> _itemsData;
        private List<string> _keys;
        
        private StoreDataLoader _storeDataLoader;
        private ICharacterService _charService;
        private MessageLoader _messageLoader;

        private List<MessageType> _startMessages;

        public int StoreItemsCount => _itemsData.Count;
        
        public Store(string itemsId, ICharacterService characterService)
        {
            _charService = characterService;
            _storeDataLoader = StoreDataLoader.Instance;
            _messageLoader = MessageLoader.Instance;

            string[] idsData = DataCleaner.CleanAndGetStrokesData(itemsId,
                cleanType: DataSepparType.Main,
                splitType: DataSepparType.Main);
            _itemsData = _storeDataLoader.GetDataByIds(idsData);
            _keys = new List<string>(_itemsData.Count);
            foreach (var keyValue in _itemsData)
            {
                _keys.Add(keyValue.Key);    
            }
            
            SetStartMessages();
        }

        private void SetStartMessages()
        {
            _startMessages = new List<MessageType>();
            _startMessages.Add(new MessageType(
                "store-hello-1", 100));
            _startMessages.Add(new MessageType(
                "store-hello-2", 20));
            _startMessages.Add(new MessageType(
                "store-hello-3", 10));
        }

        public string GetStartMessage()
        {
            return Randomizer<MessageType>
                .Randomize(_startMessages)
                .GetText();
        }

        public IReadOnlyStoreItem GetItem(int index)
        {
            return _itemsData[_keys[index]];
        }

        public bool TryBuyItem(string storeItemId, out string msg)
        {
            msg = "";

            StoreItemData itemToBuy = _itemsData[storeItemId];

            IInventorySlots inventory = 
                _charService.GetInventory(_charService.PlayerName);

            if (!inventory.HasItem(itemToBuy.CostItemId))
            {
                msg = _messageLoader.GetDataById("has-not-item");
                return false;
            }

            if (!inventory.HasEnoughAmount(
                    itemToBuy.CostItemId,
                    itemToBuy.costItemAmount))
            {
                msg = _messageLoader.GetDataById("not-enough-amount-to-buy");
                return false;
            }

            if (inventory.TryRemoveItem(
                    itemToBuy.costItemId, itemToBuy.costItemAmount))
            {
                inventory.AddItem(itemToBuy.itemId, itemToBuy.amount);
                msg = _messageLoader.GetDataById("success-buy");
                return true;
            }
    
            return false;
        }
    }
}