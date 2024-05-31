using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ChoiceUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public event Action<int> Select;
        private int _index;
        
        public void Set(string text, int index)
        {
            this.text.text = text;
            _index = index;
        }
        
        public void Set(string text)
        {
            this.text.text = text;
        }

        public void SetIndex(int index)
        {
            _index = index;
        }
        
        public void Clear(bool isFullClear = false)
        {
            text.text = string.Empty;
            if (isFullClear)
            {
                UnSubscribe();
                _index = -1;
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void Subscribe(Action<int> action)
        {
            Select = action;
        }

        public void UnSubscribe()
        {
            Select = null;
        }

        public void OnSelect()
        {
            Select?.Invoke(_index);
        }
    }
}