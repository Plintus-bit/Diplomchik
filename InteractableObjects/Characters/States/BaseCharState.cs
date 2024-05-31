using System;
using Data;
using Enums;
using Interfaces;
using Managers;
using Managers.Notifiers;
using UnityEngine;

namespace InteractableObjects.Characters
{
    public abstract class BaseCharState : ICharState
    {
        private string _id;

        protected CharacterFSM _fsm;
        
        protected Transform _parent;
        
        protected static ICharacterService _charService;
        protected static LockerInitializer _lockerInitializer;
        protected static InformerInitializer _informerInitializer;

        protected Informer _informer;
        protected Locker _locker;
        
        protected string iconName;
        protected string _informerId;
        
        public Transform whereStay;

        public BaseCharState(CharacterFSM fsm)
        {
            _fsm = fsm;
        }
        
        public string ID => _id;
        
        public void SetStateBasics(
            Transform parent,
            ICharacterService characterService)
        {
            _parent = parent;
            _charService ??= characterService;
            _lockerInitializer ??= LockerInitializer.GetInstance();
            _informerInitializer ??= InformerInitializer.Instance;

            if (!string.IsNullOrEmpty(_informerId))
            {
                _informerInitializer.RegistrResponcer(_informerId, this);
            }
        }
        
        public virtual bool Interact(string who)
        {
            return true;
        }

        public virtual void OnEndState() {}

        public virtual void OnStartState()
        {
            if (whereStay != null)
            {
                _parent.transform.position = whereStay.position;   
            }
        }

        public virtual void Set(CharStateData stateData)
        {
            if (stateData.stateId != String.Empty)
            {
                _id = stateData.stateId;
            }
            whereStay = stateData.transform;
            _locker = stateData.locker;
            _locker.Init();

            _informer = stateData.informer;
            _informer.Init();
        }

        public virtual void Lock() {}

        public virtual void Unlock() {}

        public virtual void Responce(string whoInformer,
            InformStatus status = InformStatus.End) { }
    }
}