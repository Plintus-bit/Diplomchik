using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Managers
{
    public class UIMessageService : MonoBehaviour
    {
        [SerializeField] private ItemGetUIPanel _itemGetPanel;
        
        private List<string> _itemsToGetPanel;

        private static UIMessageService _instance;
        public static UIMessageService Instance => _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = GetComponent<UIMessageService>();
            
            Init();
        }

        private void Init()
        {
            _itemsToGetPanel = new List<string>();
        }

        public void StartItemGetAnim(string title) {
            if (_itemGetPanel.IsAnimPlayed)
            {
                _itemsToGetPanel.Add(title);
                _itemGetPanel.SetAction(SetItemFromQueue);
                return;
            }
            _itemGetPanel.SetItem(title);
        }

        public void SetItemFromQueue()
        {
            if (_itemsToGetPanel.Count > 0)
            {
                StartItemGetAnim(_itemsToGetPanel[0]);
                _itemsToGetPanel.RemoveAt(0);
                return;
            }
            
            _itemGetPanel.ClearAction();
        }
    }
}