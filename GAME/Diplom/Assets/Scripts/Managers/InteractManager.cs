using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace Managers
{
    public class InteractManager : MonoBehaviour, IInteractManager
    {
        private List<IInteractable> _activeObjs;
        private IInteractable _selectedObj;
        
        private int _currSelectedIndex;

        private void Start()
        {
            _activeObjs = new List<IInteractable>();
            _selectedObj = null;
        }

        public bool Interact(string who)
        {
            if (_selectedObj == null || !_selectedObj.IsInteractable) return false;
            bool status = _selectedObj.Interact(who);
            if (!status)
            {
                UnregistrObj(_selectedObj);
            }
            return status;
        }

        public void RegistrObj(IInteractable obj)
        {
            if (!_activeObjs.Contains(obj))
            {
                _activeObjs.Add(obj);
                obj.SetActive(true);
                if (_activeObjs.Count == 1)
                {
                    SetSelectedObj(obj);
                }
            }
        }

        public void UnregistrObj(IInteractable obj)
        {
            if (_activeObjs.Contains(obj))
            {
                _activeObjs.Remove(obj);
                obj.SetActive(false);
                DropSelectedObj(obj);
            }
        }

        public void SetSelectedObj(IInteractable obj, int index = 0)
        {
            if (_selectedObj == null)
            {
                _selectedObj = obj;
                obj.SetSelected(true);
            } else if (_selectedObj != obj)
            {
                _selectedObj.SetSelected(false);
                _selectedObj = obj;
                obj.SetSelected(true);
            }
            
            if (_currSelectedIndex == -1)
            {
                _currSelectedIndex = _activeObjs.IndexOf(obj);
            }
            else
            {
                _currSelectedIndex = index;
            }
        }

        public void DropSelectedObj(IInteractable obj)
        {
            if (_selectedObj == obj)
            {
                _selectedObj = null;
                obj.SetSelected(false);
                if (_activeObjs.Count > 0)
                {
                    SetSelectedObj(_activeObjs[0]);
                }
                else
                {
                    _currSelectedIndex = -1;
                }
            }
        }

        public void ChangeSelected(bool isRightDirect)
        {
            if (_activeObjs.Count < 2) return;
            int currCount = _activeObjs.Count;
            if (isRightDirect)
            {
                _currSelectedIndex += 1;
                if (_currSelectedIndex >= currCount)
                {
                    _currSelectedIndex = 0;
                }
            }
            else
            {
                _currSelectedIndex = _currSelectedIndex - 1;
                if (_currSelectedIndex < 0)
                {
                    _currSelectedIndex = currCount - 1;
                }
            }
            SetSelectedObj(_activeObjs[_currSelectedIndex], _currSelectedIndex);
        }
    }
}