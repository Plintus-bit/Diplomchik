using UnityEngine;

namespace InteractableObjects
{
    public class AbilityObject : BasicDialogObject
    {
        [SerializeField] private string abilId;
        
        public GameObject item;

        public override bool Interact(string who)
        {
            bool isAdded = _charService.TryAddAbility(abilId);
            if (isAdded)
            {
                Destroy(item);
                StartDialog(who);
                IsInteractable = false;
            }
            return IsInteractable;
        }

        public override void OnEndDialog() {
            _dialogTransfer.ClearAction();
            Destroy(gameObject);
        }
    }
}