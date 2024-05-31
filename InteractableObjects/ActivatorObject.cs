using Data;
using UnityEngine;

namespace InteractableObjects
{
    public class ActivatorObject : BasicDialogObject
    {
        [SerializeField] protected Locker _locker;
        [SerializeField] protected ItemDataForOperations itemNeed;

        protected override void Initialize()
        {
            base.Initialize();
            
            _locker.Init();
        }

        public override bool Interact(string who)
        {
            if (ItemOperations.CheckItem(who, itemNeed))
            {
                ItemOperations.RemoveItem(who, itemNeed);
                _locker.Unlock(true);

                DestroyObject();
                return false;
            }
            
            return IsInteractable;
        }
    }
}