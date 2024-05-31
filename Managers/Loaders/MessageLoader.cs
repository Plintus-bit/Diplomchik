using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Managers
{
    public class MessageLoader : BaseLoader<string>
    {
        private Dictionary<string, string> _data;

        private static MessageLoader _instance;
        
        public static MessageLoader Instance
        {
            get
            {
                _instance ??= new MessageLoader();
                return _instance;
            }
        }
        
        public MessageLoader()
        {
            if (_instance != null) return;
            _instance = this;

            path = "Data/MessagesAndTextsData";
            _data = new Dictionary<string, string>();
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
                _data.Add(strokeData[0], strokeData[1]);
            }
        }
        
        public override string GetDataById(string id)
        {
            bool isFound = _data.TryGetValue(id, out string data);
            if (isFound) return data;
            return null;
        }
    }
}