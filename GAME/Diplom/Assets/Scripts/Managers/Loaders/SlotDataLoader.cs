using System.Collections.Generic;
using Data;
using Enums;
using UnityEngine;

namespace Managers
{
    public class DataLoader
    {
        private Dictionary<string, SlotData> _itemsData;

        public DataLoader()
        {
            _itemsData = new Dictionary<string, SlotData>();
            ReadDataFromFile();
        }
        
        private void ReadDataFromFile()
        {
            TextAsset itemsData = Resources.Load<TextAsset>("Data/ItemsData");
            
            string text = itemsData.text;
            string[] textArray = DataCleaner.CleanAndGetStrokesData(text,
                cleanType: DataSepparType.Main, splitType: DataSepparType.Stroke);
            foreach (string stroke in textArray)
            {
                string[] strokeData = DataCleaner.GetStrokesData(stroke, DataSepparType.Main);
                SlotData data = new SlotData(strokeData[0], strokeData[1],
                    strokeData[2], int.Parse(strokeData[3]));
                _itemsData.Add(strokeData[0], data);
            }
        }
        
        public SlotData GetDataByItemId(string itemId)
        {
            if (_itemsData.TryGetValue(itemId, out var resultSlotData))
            {
                return resultSlotData;
            }

            return null;
        }
    }
}