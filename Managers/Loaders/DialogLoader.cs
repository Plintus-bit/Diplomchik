using System.Collections.Generic;
using Data;
using Enums;
using Interfaces.ReadOnly;
using UnityEngine;

namespace Managers
{
    public class DialogLoader : BaseLoader<DialogStrokeData>, IReadOnlyDialogs
    {
        private List<DialogStrokeData> _dialogsData;
        private Dictionary<string, NPCData> _Characters;
        private Dictionary<string, string> _characterMoods;
        private string[] _choicesData;

        public DialogLoader()
        {
            _dialogsData = new List<DialogStrokeData>();
            _characterMoods = new Dictionary<string, string>();
            _Characters = new Dictionary<string, NPCData>();
            InitializePath();
            LoadNPC();
            LoadMoods();
            ReadDataFromFile();
        }

        public virtual void InitializePath()
        {
            path = "Data/Dialogs";
        }

        private void LoadNPC()
        {
            TextAsset NPC = Resources.Load<TextAsset>("Data/Characters");

            string[] textArray = DataCleaner.CleanAndGetStrokesData(NPC.text,
                cleanType: DataSepparType.Main,
                splitType: DataSepparType.Stroke);
            foreach (string stroke in textArray)
            {
                string[] strokeData = DataCleaner.GetStrokesData(stroke, DataSepparType.Main);
                _Characters.Add(strokeData[0], GetNPCData(strokeData));
            }
        }
        
        private void LoadMoods() {
            TextAsset NPC = Resources.Load<TextAsset>("Data/CharacterMoods");
            
            string[] textArray = DataCleaner.CleanAndGetStrokesData(NPC.text,
                cleanType: DataSepparType.Main,
                splitType: DataSepparType.Stroke);
            foreach (string stroke in textArray)
            {
                string[] strokeData = DataCleaner.GetStrokesData(stroke, DataSepparType.Main);
                _characterMoods.Add(strokeData[0], strokeData[1]);
            }
        }

        protected override void ReadDataFromFile()
        {
            TextAsset dialogText = Resources.Load<TextAsset>(path);
            TextAsset choicesText = Resources.Load<TextAsset>("Data/ChoiceDialogs");
            
            string data = dialogText.text;
            string choices = choicesText.text;
            
            _choicesData = DataCleaner.GetStrokesData(choices, DataSepparType.Stroke);
            string[] textArray = DataCleaner.CleanAndGetStrokesData(data,     
                cleanType: DataSepparType.Main, splitType: DataSepparType.Stroke);
            foreach (string stroke in textArray)
            {
                string[] strokeData = DataCleaner.GetStrokesData(
                    stroke, DataSepparType.Main);
                bool isFound = _Characters.TryGetValue(strokeData[0], out NPCData author);
                if (!isFound) continue;
                if (_characterMoods.TryGetValue(strokeData[2], out string state))
                {
                    author.TryAddImageState(state);
                    _dialogsData.Add(new DialogStrokeData(author, strokeData[1], state));
                }
            }
        }

        private NPCData GetNPCData(string[] strokeNPCData)
        {
            NPCData NPC = new NPCData(strokeNPCData[1],
                DataCleaner.GetColorFromString(strokeNPCData[2]),
                DataCleaner.GetColorFromString(strokeNPCData[3]));
            return NPC;
        }

        public DialogStrokeData GetDataById(int id)
        {
            if (id <= 0 || id > _dialogsData.Count) return null;
            return _dialogsData[id - 1];
        }

        public string GetChoiceDataById(int id)
        {
            if (id <= 0 || id > _choicesData.Length) return null;
            return _choicesData[id - 1];
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