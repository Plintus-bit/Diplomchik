using Data;
using Managers;

namespace InteractableObjects.Characters
{
    public class StayAndTalkState : BaseDialogCharState
    {
        public bool needInform;
        public StayAndTalkState(CharacterFSM fsm) : base(fsm)
        {
            _dialogTransfer = new DialogTransfer();
            _dialogService = new DialogService();
        }

        protected override void OnInteraction(string who)
        {
            StartDialog(who);
        }
        
        public override void Set(CharStateData stateData)
        {
            base.Set(stateData);

            if (stateData.needInform)
            {
                needInform = stateData.needInform;
                _informer = stateData.informer;
            }
            _dialogService.SetIds(stateData.dialogIds);
        }

        public override void OnEndDialog()
        {
            if (needInform) _informer.Inform();
            
            base.OnEndDialog();
        }
    }
}