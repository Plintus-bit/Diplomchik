using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace InteractableObjects
{
    public class RewardReceiverObject : BasicReceiverObject
    {
        [SerializeField] protected ItemDataForOperations _reward;

        protected override void Initialize()
        {
            base.Initialize();
            ItemOperations.Init(_charService);
        }

        public override void OnConditionMet(string who)
        {
            ItemOperations.RemoveItem(who, _itemNeed);
            _locker.Unlock(true);
            _dialogService.Clear();
            ItemOperations.GiveItem(who, _reward);
        }
    }
}