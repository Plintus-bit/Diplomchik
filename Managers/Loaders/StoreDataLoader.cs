using System.Collections.Generic;
using Data;
using Enums;
using UnityEngine;

namespace Managers
{
    public class StoreDataLoader : BaseLoader<StoreItemData>
    {
        private static Dictionary<string, StoreItemData> _data;
        
        private static StoreDataLoader _instance;
        private static SlotDataLoader _slotDataLoader;
        
        public static StoreDataLoader Instance
        {
            get
            {
                _instance ??= new StoreDataLoader();
                _slotDataLoader = SlotDataLoader.Instance;
                return _instance;
            }
        }

        public StoreDataLoader()
        {
            if (_instance != null) return;
            _instance = this;

            path = "Data/itemsCost";
            if (_data != null) return;
            _data = new Dictionary<string, StoreItemData>();
            _slotDataLoader = SlotDataLoader.Instance;
            ReadDataFromFile();
        }

        protected override void ReadDataFromFile()
        {
            TextAsset itemsData = Resources.Load<TextAsset>(path);
            
            string text = itemsData.text;
            string[] textArray = DataCleaner.CleanAndGetStrokesData(text,
                cleanType: DataSepparType.Main, splitType: DataSepparType.Stroke);

            string[] tempItemData;
            foreach (var stroke in textArray)
            {
                tempItemData = DataCleaner.GetStrokesData(stroke, DataSepparType.Main);
                string storeItemName = _slotDataLoader
                    .GetDataById(tempItemData[0]).itemName;
                string costItemName = _slotDataLoader
                    .GetDataById(tempItemData[2]).itemName;
                _data.Add(tempItemData[0],
                    new StoreItemData(tempItemData, storeItemName, costItemName));
            }
        }

        public override StoreItemData GetDataById(string id)
        {
            _data.TryGetValue(id, out StoreItemData item);
            return item;
        }

        public Dictionary<string, StoreItemData> GetDataByIds(string[] ids)
        {
            if (ids == null) return null;
            var items = new Dictionary<string, StoreItemData>();
            foreach (var itemId in ids)
            {
                if (_data.TryGetValue(itemId, out StoreItemData data))
                {
                    items.Add(itemId, data);
                }
                
            }
            return items;
        }
    }
}