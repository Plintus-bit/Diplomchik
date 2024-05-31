using System;
using System.Collections.Generic;
using Data;
using Enums;
using Interfaces;
using Managers;
using Managers.Notifiers;
using UnityEngine;

namespace Brainteasers
{
    [Serializable]
    public class WatchState : IResponcer
    {
        [SerializeField] private Informer _informer;
        [SerializeField] private Locker _locker;
        [SerializeField] private List<string> _informerIds;
        [SerializeField] private List<string> _dialogIdsOnEnd;
        private static InformerInitializer _informerInitializer;
        private bool _isActiveState = false;

        public bool IsActive => _isActiveState;
        public List<string> EndDialogs => _dialogIdsOnEnd;

        [Serializable]
        public struct Angles
        {
            public float minuteAngle;
            public float hourAngle;
        }

        public Angles trueAngles;
        public Angles startAngles;
        public Angles currAngles;

        public void Init()
        {
            _informer.Init();
            _locker.Init();
            if (_informerIds.Count > 0)
            {
                _informerInitializer ??= InformerInitializer.Instance;
                foreach (var informerId in _informerIds)
                {
                    _informerInitializer
                        .RegistrResponcer(informerId, this);
                }
                _isActiveState = false;
            }
            else
            {
                Activate();
            }
        }

        private void OnStartState()
        {
            currAngles.minuteAngle = startAngles.minuteAngle;
            currAngles.hourAngle = startAngles.hourAngle;
        }

        private bool CheckAngles()
        {
            if ((int) Math.Ceiling(currAngles.hourAngle)
                != (int) Math.Ceiling(trueAngles.hourAngle)) return false;
            
            if ((int) Math.Ceiling(currAngles.minuteAngle)
                != (int) Math.Ceiling(trueAngles.minuteAngle)) return false;
            
            return true;
        }

        public bool OnChangeValue(float angle)
        {
            currAngles.minuteAngle += angle;
            if (Math.Abs(currAngles.minuteAngle) >= 360)
                currAngles.minuteAngle = 0;
            currAngles.hourAngle += angle / 12;
            if (Math.Abs(currAngles.hourAngle) >= 360)
                currAngles.hourAngle = 0;
            return CheckAngles();
        }

        public void OnEndState()
        {
            _informer.Inform();
            _informer.Unregistr();
            _locker.Unlock(isForever: true);
        }

        public void Responce(string whoInformer,
            InformStatus status = InformStatus.End)
        {
            if (_informerIds.Remove(whoInformer))
            {
                _informerInitializer
                    .UnregistrResponcer(whoInformer, this);
                if (_informerIds.Count == 0) Activate();
            }
        }

        private void Activate()
        {
            _informer.Inform(InformStatus.Start);
            _locker.Lock();
            _isActiveState = true;
            OnStartState();
        }
    }
}