using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace UI
{
    public class ChoiceDialogUI : MonoBehaviour
    {
        [SerializeField] private List<ChoiceUI> choices;
        [SerializeField] private ChoiceUI choicePrefab;
        [SerializeField] private float _height;

        private static DialogSystem _dialogSystem;

        public int verticalMargin = 20;

        private void Awake()
        {
            _height = choicePrefab.GetComponent<RectTransform>().rect.height;
        }

        private void Start()
        {
            if (_dialogSystem == null) GetDialogSystem();
            InitChoices();
        }

        public void Change(List<string> data)
        {
            FillChoicePlaces(data);
        }

        public void InitChoices()
        {
            for (int i = 0; i < choices.Count; i++)
            {
                choices[i].Subscribe(OnSelect);
                choices[i].SetIndex(i);
            }
        }
        
        public void GetDialogSystem()
        {
            _dialogSystem = FindObjectOfType<DialogSystem>();
        }

        public void CreateAndFillChoicePlaces(List<string> data)
        {
            choices = new List<ChoiceUI>();
            for (int i = 0; i < data.Count; i++)
            {
                ChoiceUI newTextPlace = Instantiate(choicePrefab, transform);
                newTextPlace.transform.localPosition += new Vector3(
                    0,
                    -1 * i * (_height + verticalMargin),
                    0);
                newTextPlace.Subscribe(OnSelect);
                newTextPlace.Set(data[i], i);
                choices.Add(newTextPlace);
            }
        }

        public void FillChoicePlaces(List<string> data)
        {
            int lastFilledIndex = -1;
            for (int i = 0; i < data.Count; i++)
            {
                lastFilledIndex = i;
                choices[i].Set(data[i]);
                choices[i].Show();
            }

            if (lastFilledIndex < choices.Count - 1)
            {
                for (int i = lastFilledIndex + 1; i < choices.Count; i++)
                {
                    choices[i].Clear();
                    choices[i].Hide();
                }
            }
        }

        public void HideChoices()
        {
            foreach (var choice in choices)
            {
                choice.Clear();
                choice.Hide();
            }
        }
        
        public void Clear()
        {
            for (int i = 0; i < choices.Count; i++)
            {
                Destroy(choices[i]);
                i -= 1;
            }
            choices.Clear();
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        public void OnSelect(int index)
        {
            if (_dialogSystem == null) GetDialogSystem();
            _dialogSystem.ChangeDialog(index);
        }
    }
}