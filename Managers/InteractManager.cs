using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using Managers.Notifiers;

namespace Managers
{
    public class InteractManager : MonoBehaviour, IInteractManager, IInteractListener
    {
        private List<IInteractable> _activeObjsMap;
        private IInteractable _selectedObj;

        private List<IInteractable> _tempCloseObjects;
        
        private int _currSelectedIndex;
        private int activeObjCount;

        private void Start()
        {
            _activeObjsMap = new List<IInteractable>();
            _tempCloseObjects = new List<IInteractable>();
            activeObjCount = 0;
            _selectedObj = null;
            RegistrListener();
        }

        public bool Interact(string who)
        {
            if (_selectedObj == null ||
                !_selectedObj.IsInteractable) return false;
            bool status = _selectedObj.Interact(who);

            if (_selectedObj == null) return status;
            if (!status
                && status == _selectedObj.IsInteractable)
            {
                UnregistrObj(_selectedObj);
            }
            return status;
        }

        public void RegistrObj(IInteractable obj)
        {
            if (!_tempCloseObjects.Contains(obj))
                _tempCloseObjects.Add(obj);
            
            if (obj.IsInteractable
                && !_activeObjsMap.Contains(obj))
            {
                AddToMap(obj);
                activeObjCount += 1;
                obj.SetActive(true);
                if (activeObjCount == 1)
                {
                    SetSelectedObj(obj);
                }
            }
        }
        
        public void UnregistrObj(IInteractable obj, bool exitForNearBy = false)
        {
            if (_activeObjsMap.Contains(obj))
            {
                _activeObjsMap.Remove(obj);
                activeObjCount -= 1;
                obj.SetActive(false);
                DropSelectedObj(_selectedObj);
            }
            
            if (!exitForNearBy) return;
            if (_tempCloseObjects.Contains(obj))
                _tempCloseObjects.Remove(obj);
        }

        private void AddToMap(IInteractable objToAdd)
        {
            int newIndex = RecursiveAddObjToMap(
                0, activeObjCount - 1, objToAdd);

            if (newIndex >= activeObjCount)
            {
                _activeObjsMap.Add(objToAdd);
            }
            else
            {
                _activeObjsMap.Insert(newIndex, objToAdd);
            }

            _currSelectedIndex = _activeObjsMap.IndexOf(_selectedObj);
        }

        private int RecursiveAddObjToMap(
            int startIndex, int endIndex,
            IInteractable objToAdd)
        {
            // callRec += 1;
            int deltaIndex = endIndex - startIndex;
            if (deltaIndex < 0) return 0;
            if (deltaIndex <= 1)
            {
                bool isLessThenStart = objToAdd.XPos < _activeObjsMap[startIndex].XPos;
                bool isLessThenEnd = objToAdd.XPos < _activeObjsMap[endIndex].XPos;
                if (isLessThenStart) return startIndex;
                if (!isLessThenEnd) return endIndex + 1;
                return endIndex;
            }
            
            int middleIndex = startIndex + deltaIndex / 2;

            if (objToAdd.XPos < _activeObjsMap[middleIndex].XPos)
            {
                return RecursiveAddObjToMap(
                    startIndex, middleIndex, objToAdd);
            }
            if (objToAdd.XPos > _activeObjsMap[middleIndex].XPos)
            {
                return RecursiveAddObjToMap(
                    middleIndex, endIndex, objToAdd);
            }
            return middleIndex;
        }

        public void SetSelectedObj(IInteractable objToSelect)
        {
            if (_selectedObj == null)
            {
                _selectedObj = objToSelect;
            } else if (_selectedObj != objToSelect)
            {
                _selectedObj.SetSelected(false);
                _selectedObj = objToSelect;
            }
            _selectedObj.SetSelected(true);
            _currSelectedIndex = _activeObjsMap.IndexOf(objToSelect);
        }

        public void DropSelectedObj(IInteractable obj)
        {
            if (_selectedObj == obj)
            {
                _selectedObj = null;
                if (obj != null)
                {
                    obj.SetSelected(false);
                }
                if (_activeObjsMap.Count > 0)
                {
                    SetSelectedObj(_activeObjsMap[0]);
                }
                else
                {
                    _currSelectedIndex = -1;
                }
            }
        }

        public void ChangeSelected(bool isRightDirect)
        {
            if (activeObjCount < 2) return;
            
            if (isRightDirect)
            {
                _currSelectedIndex += 1;
                if (_currSelectedIndex >= activeObjCount)
                {
                    _currSelectedIndex = 0;
                }
            }
            else
            {
                _currSelectedIndex -= 1;
                if (_currSelectedIndex < 0)
                {
                    _currSelectedIndex = activeObjCount - 1;
                }
            }
            SetSelectedObj(_activeObjsMap[_currSelectedIndex]);
        }

        public void ReactOnNotify(IInteractable obj, bool isInteractable)
        {
            if (isInteractable && _tempCloseObjects.Contains(obj)) RegistrObj(obj);
            else UnregistrObj(obj);
        }

        public void RegistrListener()
        {
            InteractablesNotifier.RegistrListener(this);
        }
        
        public void UnregistrListener()
        {
            InteractablesNotifier.UnregistrListener(this);
        }
    }
}