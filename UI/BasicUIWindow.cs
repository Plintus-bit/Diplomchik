using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using Managers;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BasicUIWindow : UIInputListener, IUIWindow
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        protected ICharacterService _charService;
        protected List<UIInputListener> _childrenInputListeners;
        protected override void Initialize()
        {
            _charService = FindObjectOfType<CharacterService>();
            _childrenInputListeners =
                GetComponentsInChildren<UIInputListener>().ToList();
            _canvasGroup = GetComponent<CanvasGroup>();
            Close();
        }

        public void Close(bool isForever = false)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            
            TurnOff(isForever);
            if (_childrenInputListeners != null)
            {
                TurnOffAllChildren(isForever);
            }
            
            OnCloseWindow();
            _charService.SetInputState(
                PlayerState.Movable);
        }

        public void TryRemoveListener(UIInputListener listener)
        {
            if (!_childrenInputListeners.Contains(listener)) return;
            _childrenInputListeners.Remove(listener);
        }
        
        public void TryAddListener(UIInputListener listener)
        {
            if (_childrenInputListeners.Contains(listener)) return;
            _childrenInputListeners.Add(listener);
        }

        public void Open()
        {
            _charService.SetInputState(
                PlayerState.InUIWindow);
            
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            
            TurnOn();
            if (_childrenInputListeners != null)
            {
                TurnOnAllChildren();
            }
            OnOpenWindow();
        }

        public void TurnOnAllChildren()
        {
            foreach (var listener in _childrenInputListeners)
            {
                listener.TurnOn();
            }
        }
        
        public void TurnOffAllChildren(bool isForever = false)
        {
            foreach (var listener in _childrenInputListeners)
            {
                listener.TurnOff(isForever);
            }
        }
        
        public virtual void OnOpenWindow()
        {
            
        }

        public virtual void OnCloseWindow()
        {
            
        }
        
        
    }
}