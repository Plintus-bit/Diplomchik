using Data;
using Enums;
using Managers;

namespace InteractableObjects.Characters
{
    public class RecieveState : BaseDialogCharState
    {
        protected ItemDataForOperations _itemNeed;

        public bool changeStateOnSuccessInteract;

        public RecieveState(CharacterFSM fsm) : base(fsm)
        {
            _dialogService = new DialogService();
            dialogType = DialogType.Basic;
        }
        
        public override bool Interact(string who)
        {
            if (ItemOperations.CheckItem(who, _itemNeed))
            {
                if (changeStateOnSuccessInteract)
                {
                    if (_dialogIds != null
                        && _dialogIds.Count > 0)
                    {
                        _dialogTransfer.SetAction(OnEndDialog);
                        _dialogService.SetIds(_dialogIds);
                        StartDialog(who);
                        return true;
                    }
                    OnEndState();
                }
                return true;
            }

            StartDialog(who);
            return true;
        }

        public override void Set(CharStateData stateData)
        {
            base.Set(stateData);
            
            _dialogService.SetIds(stateData.dialogIds);
            _dialogIds = stateData.additionDialogIds;
            _itemNeed = stateData.items[0];
            changeStateOnSuccessInteract = stateData.changeStateOnSuccessInteract;
        }

        public override void OnEndDialog()
        {
            ItemOperations.RemoveItem(_charService.PlayerName, _itemNeed);
            _dialogTransfer.ClearAction();
            _fsm.NextState();
        }
        
        public override void OnStartState()
        {
            base.OnStartState();

            if (dialogEndEqualStateEnd)
            {
                _dialogTransfer.SetAction(OnEndDialog);
            }
            else
            {
                _dialogTransfer.ClearAction();
            }
            
            if (startDialogOnStartState)
            {
                StartDialog(_charService.PlayerName);
            }
        }
        
        public override void OnEndState()
        {
            _dialogService.Clear();
        }
    }
}