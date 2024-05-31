using System.Collections.Generic;
using Data;
using Enums;
using UnityEngine;

namespace Managers
{
    public class SlotDataLoader : BaseLoader<SlotData>
    {
        private static Dictionary<string, SlotData> _itemsData;
        private static SlotDataLoader _instance;

        public static SlotDataLoader Instance
        {
            get
            {
                _instance ??= new SlotDataLoader();
                return _instance;
            }
        }
        public SlotDataLoader()
        {
            if (_instance != null) return;
            _instance = this;
            path = "Data/ItemsData";
            
            if (_itemsData != null) return;
            _itemsData = new Dictionary<string, SlotData>();
            ReadDataFromFile();
        }
        
        protected override void ReadDataFromFile()
        {
            TextAsset itemsData = Resources.Load<TextAsset>(path);
            
            string text = itemsData.text;
            string[] textArray = DataCleaner.CleanAndGetStrokesData(text,
                cleanType: DataSepparType.Main, splitType: DataSepparType.Stroke);
            foreach (string stroke in textArray)
            {
                string[] strokeData = DataCleaner.GetStrokesData(stroke, DataSepparType.Main);
                SlotData data = new SlotData(strokeData[0], strokeData[1],
                    strokeData[2], Resources.Load<Sprite>("ItemsIcon/" + strokeData[0]),
                    int.Parse(strokeData[3]));
                _itemsData.Add(strokeData[0], data);
            }
        }
        
        public override SlotData GetDataById(string itemId)
        {
            if (_itemsData.TryGetValue(itemId, out var resultSlotData))
            {
                return resultSlotData;
            }
            return null;
        }

        public override bool HasItem(string itemId)
        {
            return _itemsData.ContainsKey(itemId);
        }
    }
}