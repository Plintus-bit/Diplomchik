using Data;
using Enums;
using Managers;

namespace InteractableObjects.Characters
{
    public class GiverState : BaseDialogCharState
    {
    protected ItemDataForOperations _itemToGive;
    
    // FLAGS
    public bool showItemGivePanel;


    public GiverState(CharacterFSM fsm) : base(fsm)
    {
        _dialogService = new DialogService();
        dialogType = DialogType.Basic;
    }

    protected override void OnInteraction(string who)
    {
        if (!_itemToGive.HasReward()) return;
        if (ItemOperations
            .GiveItem(who, _itemToGive, showItemGivePanel))
        {
            _informer.Inform();
            if (!dialogBeforeAction)
            {
                StartDialog(who);
            }
        }
        
        _itemToGive.Clear();
    }

    public override void Set(CharStateData stateData)
    {
        base.Set(stateData);
        
        showItemGivePanel = stateData.showItemGivePanel;
        _dialogService.SetIds(stateData.dialogIds);
        _itemToGive = stateData.items[0];
    }
    }
}