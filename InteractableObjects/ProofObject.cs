using Data;
using Enums;

namespace InteractableObjects
{
    public class ProofObject : BasicDialogObject
    {
        public ItemDataForOperations proof;

        public bool dialogBeforeGive = true;
        public bool showItemGetPopup = false;
        public bool deleteAfterGive = true;

        public override void SetIconNames()
        {
            IconName = "ProofIcon";
        }

        public override bool Interact(string who)
        {
            if (!proof.HasReward())
            {
                StartDialog(who);
                return IsInteractable;
            }
            
            if (!dialogBeforeGive)
            {
                OnInteraction();
            }
            
            if (!StartDialog(who))
            {
                OnEndDialog();
            }
            return IsInteractable;
        }

        public override void OnInteraction()
        {
            bool proofAdded = ItemOperations
                .ProofItemOperations(
                    _charService.PlayerName,
                    proof, OperationsType.Add,
                    showItemGetPopup);
            
            if (!proofAdded) return;

            proof.Clear();
            if (!deleteAfterGive) return;
            if (visualItem != null) Destroy(visualItem);
            IsInteractable = false;
        }

        public override void OnEndDialog()
        {
            if (dialogBeforeGive)
            {
                OnInteraction();
            }
            _dialogTransfer.ClearAction();
            if (deleteAfterGive)
            {
                DestroyObject();
                return;
            }

            _dialogService.SetLoop(1);
        }
    }
}