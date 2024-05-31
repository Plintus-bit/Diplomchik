using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using Managers;
using Managers.Notifiers;
using UnityEngine;

namespace Brainteasers
{
    [Serializable]
    public class Watch
    {
        [SerializeField] private List<WatchState> _states;
        [SerializeField] private Transform _hourHand;
        [SerializeField] private Transform _minuteHand;
        private ICharacterService _charService;
        private DialogService _dialogService;
        private DialogTransfer _dialogTransfer;
        private WatchState _currState;
        
        private Action _OnChangeCloseWindow;

        [SerializeField] private Informer _informer;

        public float angleValue = 15;

        public void Init(ICharacterService charService)
        {
            _charService = charService;
            _dialogTransfer = new DialogTransfer();
            _dialogService = new DialogService();
            _informer.Init();
            
            if (_states.Count > 0)
            {
                ChangeState();
            }
        }

        private void OnSolve()
        {
            if (!string.IsNullOrEmpty(_informer.InformerId))
            {
                _informer.Inform();
            }
        }

        public bool TryChangeState(Action closeWindow)
        {
            if (SetStateEndDialogs())
            {
                _OnChangeCloseWindow = closeWindow;
                return true;
            }
            ChangeState();
            return false;
        }

        public void ChangeState()
        {
            if (_currState == null)
            {
                _currState = _states[0];
            }
            else
            {
                _currState.OnEndState();
                _OnChangeCloseWindow?.Invoke();
                _OnChangeCloseWindow = null;
                int newIndex = _states.IndexOf(_currState) + 1;
                if (newIndex >= _states.Count)
                {
                    OnSolve();
                    return;
                }
                _currState = _states[newIndex];
            }
            _currState.Init();
            SetStartRotation();
        }

        public void SetStartRotation()
        {
            _hourHand.rotation = Quaternion
                .Euler(0, 0, _currState.startAngles.hourAngle);
            _minuteHand.rotation = Quaternion
                .Euler(0, 0, _currState.startAngles.minuteAngle);
        }

        public void RotateHands()
        {
            _hourHand.rotation = Quaternion
                .Euler(0, 0, _currState.currAngles.hourAngle);
            _minuteHand.rotation = Quaternion
                .Euler(0, 0, _currState.currAngles.minuteAngle);
        }

        public bool OnRotate(bool isRightDir)
        {
            bool status = false;
            if (isRightDir) status = _currState.OnChangeValue(-1 * angleValue);
            else status = _currState.OnChangeValue(angleValue);
            
            RotateHands();
            return status;
        }

        public bool SetStateEndDialogs()
        {
            List<string> dialogs = _currState.EndDialogs;
            if (_currState == null &&
                dialogs == null
                && dialogs.Count <= 0) return false;
            _dialogService.SetIds(dialogs);
            _dialogTransfer.SetAction(ChangeState);
            _dialogTransfer.StartDialog(
                _charService.PlayerName,
                _dialogService,
                DialogType.Basic);
            return true;
        }

        public bool GetActiveStateCurrStage()
        {
            if (_currState == null) return false;
            return _currState.IsActive;
        }

    }
}