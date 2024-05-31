using Managers.Notifiers;
using UnityEngine;

namespace InteractableObjects
{
    public class GrabObject : BasicDialogObject
    {
        [SerializeField] protected Informer _informer;
        [SerializeField] private string _itemId;

        [Range(1, 10)] public int amount;

        public override void SetIconNames()
        {
            IconName = "GrabIcon";
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            _informer.Init();
        }

        public override bool Interact(string who)
        {
            _charService.TryAddToInventory(
                who, _itemId, amount, out int amountStay);
            if (amountStay <= 0)
            {
                if (visualItem != null)
                {
                    Destroy(visualItem);   
                }

                if (!StartDialog(who))
                {
                    OnEndDialog();
                }
                IsInteractable = false;
                return IsInteractable;
            }
            amount = amountStay;
            return IsInteractable;
        }

        public override void OnEndDialog()
        {
            _informer.Inform();
            _dialogTransfer.ClearAction();
            DestroyObject();
        }
    }
}