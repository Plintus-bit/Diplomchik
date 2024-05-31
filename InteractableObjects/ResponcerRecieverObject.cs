using System.Collections.Generic;
using Data;
using Enums;
using Managers.Notifiers;
using UnityEngine;

namespace InteractableObjects
{
    public class ResponcerRecieverObject : BasicReceiverObject
    {
        [SerializeField] protected Informer _informer;
        [SerializeField] protected List<string> _informers;

        protected override void Initialize()
        {
            base.Initialize();
            
            _informer.Init();
            if (_informers.Count > 0)
            {
                foreach (var informId in _informers)
                {
                    _informerInitializer.RegistrResponcer(informId, this);
                }
            }
        }
        
        public override bool Interact(string who)
        {
            if (CheckConditionMet(who))
            {
                OnConditionMet(who);
                return IsInteractable;
            }
            OnConditionNotMet(who);
            return IsInteractable;
        }

        public override bool CheckConditionMet(string who)
        {
            if (_informers.Count > 0) return false;
            return base.CheckConditionMet(who);
        }
        
        public override void OnConditionMet(string who)
        {
            _locker.Unlock(true);
            _isNeedItems = false;
            _dialogService.Clear();
            if (itemWasGetDialogIds != null)
            {
                _dialogService.SetIds(itemWasGetDialogIds);
                _dialogTransfer.SetAction(OnEndDialog);
                StartDialog(who);
            }
        }
        
        public override void OnConditionNotMet(string who)
        {
            _dialogTransfer.ClearAction();
            StartDialog(who);
        }

        public override void OnEndDialog()
        {
            base.OnEndDialog();
            
            ItemOperations.RemoveItem(_charService.PlayerName, _itemNeed);
            _informer.Inform();
            DestroyObject();
        }

        public override void Responce(string whoInformer, InformStatus status = InformStatus.End)
        {
            _informers.Remove(whoInformer);
            _informerInitializer.UnregistrResponcer(whoInformer, this);
        }
    }
}