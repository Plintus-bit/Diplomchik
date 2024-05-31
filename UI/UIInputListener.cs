using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI
{
    public class UIInputListener : MonoBehaviour, IUIInputListener
    {
        protected PlayerInput _input;
        protected EventTrigger _eventTrigger;

        private void Start()
        {
            _input = GetComponent<PlayerInput>();
            if (_input == null)
                _input = GetComponentInChildren<PlayerInput>();
            _eventTrigger = GetComponent<EventTrigger>();
            if (_eventTrigger == null)
                _eventTrigger = GetComponentInChildren<EventTrigger>();
            Initialize();
        }
        
        protected virtual void Initialize() {}

        public void TurnOff(bool isForever = false)
        {
            SetInput(false);
            if (isForever)
            {
                Clear();
            }
        }
        
        public void TurnOn()
        {
            SetInput(true);
        }

        protected void SetInput(bool isActive)
        {
            if (_input != null)
            {
                _input.enabled = isActive;
            }

            if (_eventTrigger != null)
            {
                _eventTrigger.enabled = isActive;
            }
        }

        protected virtual void Clear()
        {
            if (_eventTrigger != null)
            {
                _eventTrigger.triggers.Clear();
            }
        }
    }
}