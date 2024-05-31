using System.Collections.Generic;
using Data;
using Enums;
using Managers;

namespace InteractableObjects.Characters
{
    public abstract class BaseDialogCharState : BaseCharState
    {
        protected DialogTransfer _dialogTransfer;
        protected DialogService _dialogService;
        protected List<string> _dialogIds;

        public DialogType dialogType = DialogType.Basic;
        
        // FLAGS
        public bool dialogBeforeAction;
        public bool dialogEndEqualStateEnd;
        public bool startDialogOnStartState;

        protected BaseDialogCharState(CharacterFSM fsm) : base(fsm)
        {
            _dialogTransfer = new DialogTransfer();
        }
        
        public override bool Interact(string who)
        {
            if (dialogBeforeAction)
            {
                StartDialog(who);
            }
            else
            {
                OnInteraction(who);
            }
            return true;
        }

        protected virtual void OnInteraction(string who) { }

        public virtual void OnEndDialog()
        {
            if (dialogBeforeAction)
            {
                OnInteraction(_charService.PlayerName);
            }
            
            if (dialogEndEqualStateEnd) {
                _dialogTransfer.ClearAction();
                _fsm.NextState();
                return;
            }
        }

        public override void OnStartState()
        {
            base.OnStartState();

            _dialogTransfer.SetAction(OnEndDialog);
            
            if (startDialogOnStartState)
            {
                StartDialog(_charService.PlayerName);
            }
        }

        public override void OnEndState()
        {
            _dialogService.Clear();
        }
        
        public override void Set(CharStateData stateData)
        {
            base.Set(stateData);

            dialogBeforeAction = stateData.dialogBeforeAction;
            dialogEndEqualStateEnd = stateData.dialogEndEqualStateEnd;
            startDialogOnStartState = stateData.startDialogOnStartState;
        }

        public virtual bool StartDialog(string who)
        {
            if (_dialogService == null || _dialogService.IsEmpty()) return false;
            _dialogTransfer.StartDialog(who, _dialogService, dialogType);
            return true;
        }
    }
}