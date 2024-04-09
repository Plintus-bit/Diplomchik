using System.Collections.Generic;
using Data;
using Enums;
using Interfaces.ReadOnly;
using UnityEngine;

namespace Managers
{
    public class DialogLoader : IReadOnlyDialogs
    {
        private List<DialogStrokeData> _dialogsData;
        private Dictionary<string, string> _NPCs;
        private string[] _choicesData;

        public DialogLoader()
        {
            _dialogsData = new List<DialogStrokeData>();
            _NPCs = new Dictionary<string, string>();
            LoadNPC();
            ReadDataFromFile();
        }

        private void LoadNPC()
        {
            TextAsset NPC = Resources.Load<TextAsset>("Data/NPC");
            
            // NPC.text.Replace("; ", ";");
            string[] textArray = DataCleaner.CleanAndGetStrokesData(NPC.text,
                cleanType: DataSepparType.Main,
                splitType: DataSepparType.Stroke);
            foreach (string stroke in textArray)
            {
                string[] strokeData = DataCleaner.GetStrokesData(stroke, DataSepparType.Main);
                _NPCs.Add(strokeData[0], strokeData[1]);
            }
        }

        private void ReadDataFromFile()
        {
            TextAsset dialogText = Resources.Load<TextAsset>("Data/DialogsLoc1");
            TextAsset choicesText = Resources.Load<TextAsset>("Data/ChoiceDialogs");
            
            string data = dialogText.text;
            string choices = choicesText.text;
            
            _choicesData = DataCleaner.GetStrokesData(choices, DataSepparType.Stroke);
            string[] textArray = DataCleaner.CleanAndGetStrokesData(data,     
                cleanType: DataSepparType.Main, splitType: DataSepparType.Stroke);
            foreach (string stroke in textArray)
            {
                string[] strokeData = DataCleaner.GetStrokesData(stroke, DataSepparType.Main);
                _NPCs.TryGetValue(strokeData[0], out string authorName);
                _dialogsData.Add(new DialogStrokeData(authorName, strokeData[1]));
            }
        }

        public DialogStrokeData GetDataById(int id)
        {
            return _dialogsData[id];
        }

        public string GetChoiceDataById(int id)
        {
            return _choicesData[id];
        }

        public List<DialogStrokeData> GetDataByIds(List<int> ids)
        {
            int count = ids.Count;
            List<DialogStrokeData> resultDialogData = new List<DialogStrokeData>(count);
            for (int i = 0; i < count; i++)
            {
                resultDialogData.Add(GetDataById(ids[i]));
            }
            return resultDialogData;
        }
        
        public List<string> GetChoiceDataByIds(List<int> ids)
        {
            int count = ids.Count;
            List<string> choices = new List<string>();
            for (int i = 0; i < count; i++)
            {
                choices.Add(_choicesData[i]);
            }

            return choices;
        }
    }
}