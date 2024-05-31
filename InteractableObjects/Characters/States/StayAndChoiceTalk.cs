using Data;
using Enums;
using Managers;
using UnityEngine;

namespace InteractableObjects.Characters
{
    public class StayAndChoiceTalk : BaseCharState
    {
        protected DialogTransfer _dialogTransfer;
        protected ChoiceDialogService _dialogService;

        public StayAndChoiceTalk(CharacterFSM fsm) : base(fsm)
        {
            _dialogTransfer = new DialogTransfer();
            _dialogService = new ChoiceDialogService();
        }

        public override bool Interact(string who)
        {
            StartDialog(who);
            return true;
        }
        
        public virtual void StartDialog(string who)
        {
            if (_dialogService == null || _dialogService.IsEmpty()) return;
            _dialogTransfer.StartDialog(who, _dialogService, DialogType.Choice);
        }
        
        public override void OnEndState()
        {
            _dialogTransfer.ClearAction();
            _dialogService.ChangeCurrentDialogId(_dialogService.DialogEndIndex);
        }

        public override void Set(CharStateData stateData)
        {
            base.Set(stateData);
            
            _dialogService.Set(
                stateData.choiceIds,
                stateData.dialogIds,
                stateData.startDialogs);
            
            whereStay = stateData.transform;
        }
    }
}