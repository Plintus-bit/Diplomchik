using System.Collections.Generic;
using Data;
using UnityEngine;

namespace InteractableObjects
{
    public class BasicReceiverObject : BasicDialogObject
    {
        [SerializeField] protected Locker _locker;
        [SerializeField] protected List<string> itemWasGetDialogIds;
        [SerializeField] protected bool _destroyOnConditionMet = false;
        [SerializeField] protected ItemDataForOperations _itemNeed;
        
        protected bool _isNeedItems = true;

        protected override void Initialize()
        {
            base.Initialize();
            _locker.Init();
        }

        public override bool Interact(string who)
        {
            if (CheckConditionMet(who))
            {
                OnConditionMet(who);
                if (_destroyOnConditionMet)
                {
                    DestroyObject();
                }
            }
            OnConditionNotMet(who);
            return IsInteractable;
        }

        public virtual bool CheckConditionMet(string who)
        {
            if (_isNeedItems
                && ItemOperations
                    .CheckItem(who, _itemNeed)) return true;
            return false;
        }

        public virtual void OnConditionMet(string who)
        {
            ItemOperations.RemoveItem(who, _itemNeed);
            _locker.Unlock(true);
            _isNeedItems = false;
            _dialogService.Clear();
            if (itemWasGetDialogIds != null)
            {
                _dialogService.SetIds(itemWasGetDialogIds);
            }
        }

        public virtual void OnConditionNotMet(string who)
        {
            StartDialog(who);
        }
    }
}