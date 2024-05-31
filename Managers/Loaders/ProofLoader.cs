using System;
using System.Collections.Generic;
using Data;
using Enums;
using UnityEngine;

namespace Managers
{
    public class ProofLoader : BaseLoader<ProofData>
    {
        private static Dictionary<string, Author> _authors;
        private static Dictionary<string, ProofData> _data;
        
        private static ProofLoader _instance;

        public static ProofLoader Instance
        {
            get
            {
                _instance ??= new ProofLoader();
                return _instance;
            }
        }
        public ProofLoader()
        {
            if (_instance != null) return;
            _instance = this;
            
            path = "Data/ProofsData";
            if (_data != null) return;
            _authors = new Dictionary<string, Author>();
            _data = new Dictionary<string, ProofData>();
            LoadAuthors();
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
                bool isFoundAuthor = _authors.TryGetValue(strokeData[3], out Author author);
                if (!isFoundAuthor) continue;
                ProofData data = new ProofData(strokeData[0],
                    strokeData[1], strokeData[2], author,
                    Int32.Parse(strokeData[4]));
                _data.Add(strokeData[0], data);
            }
        }

        private void LoadAuthors()
        {
            TextAsset data = Resources.Load<TextAsset>("Data/ProofAuthors");
            
            string[] textArray = DataCleaner.CleanAndGetStrokesData(data.text,
                cleanType: DataSepparType.Main,
                splitType: DataSepparType.Stroke);
            
            foreach (string stroke in textArray)
            {
                string[] strokeData = DataCleaner.GetStrokesData(stroke, DataSepparType.Main);
                ProofType.TryParse(strokeData[1], out ProofType tempType);
                _authors.Add(strokeData[0], new Author(tempType,
                    strokeData[2], strokeData[3]));
            }
        }

        public int GetAuthorProofsCount(ProofType author)
        {
            List<string> itemsTitles = new List<string>();
            int authorProofCounter = 0;
            foreach (var item in _data)
            {
                if (item.Value.author.AuthorType == author &&
                    !itemsTitles.Contains(item.Value.title))
                {
                    authorProofCounter += item.Value.maxAmount;
                    itemsTitles.Add(item.Value.title);
                }
            }
            itemsTitles.Clear();
            return authorProofCounter;
        }

        public override ProofData GetDataById(string id)
        {
            bool isFound = _data.TryGetValue(id, out ProofData data);
            if (isFound) return data;
            return null;
        }
        
        public override bool HasItem(string itemId)
        {
            return _data.ContainsKey(itemId);
        }
    }
}