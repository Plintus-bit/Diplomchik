using System.Collections.Generic;
using Data;
using Enums;
using UnityEngine;

namespace Managers
{
    public class AbilityLoader : BaseLoader<Ability>
    {
        private static Dictionary<string, Ability> _data;
        private static AbilityLoader _instance;

        public static AbilityLoader Instance
        {
            get
            {
                _instance ??= new AbilityLoader();
                return _instance;
            }
        }
        
        public AbilityLoader()
        {
            if (_instance != null) return;
            _instance = this;
            path = "Data/AbilsData";
            
            if (_data != null) return;
            _data = new Dictionary<string, Ability>();
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
                Ability tempData = new Ability(strokeData[0]);
                
                _data.Add(strokeData[0], tempData);
            }
        }
        
        public override Ability GetDataById(string itemId)
        {
            _data.TryGetValue(itemId, out Ability data);
            return data;
        }
        
        public override bool HasItem(string itemId)
        {
            return _data.ContainsKey(itemId);
        }
    }
}