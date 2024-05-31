using System.Collections.Generic;
using Enums;
using Managers;
using UnityEngine;

namespace InteractableObjects
{
    public abstract class BasicDialogObject : BasicObject
    {
        [SerializeField] protected List<string> ids;
        protected DialogTransfer _dialogTransfer;
        protected DialogService _dialogService;

        [HideInInspector]
        public DialogType type;

        protected override void Initialize()
        {
            _dialogService = new DialogService(ids);
            _dialogTransfer = new DialogTransfer(OnEndDialog);
            type = DialogType.Basic;
        }
        
        public override bool Interact(string who)
        {
            OnInteraction();
            StartDialog(who);
            return IsInteractable;
        }

        public override void SetIconNames()
        {
            IconName = "DialogIcon";
        }

        public virtual void OnInteraction() {}

        public virtual bool StartDialog(string who)
        {
            if (_dialogService == null || _dialogService.IsEmpty()) return false;
            if (!_dialogTransfer.HasDialogSystem())
            {
                _dialogTransfer.DialogSystem = FindObjectOfType<DialogSystem>();
            }
            _dialogTransfer.StartDialog(who, _dialogService, type);
            return true;
        }

        public virtual void OnEndDialog()
        {
            _dialogTransfer.ClearAction();
        }
    }
}