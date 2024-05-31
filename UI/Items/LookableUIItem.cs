using System.Collections.Generic;
using Enums;
using Managers;
using UnityEngine;

namespace UI.Items
{
    public class LookableUIItem : BasicUIItem
    {
        [SerializeField] private List<string> _dialogIds;
        
        private DialogService _dialogService;
        private DialogTransfer _dialogTransfer;

        public int startLoopIndex = 0;
        public int endLoopIndex = -1;
        
        protected override void Initialize()
        {
            base.Initialize();
            
            _dialogService = new DialogService(_dialogIds);
            _dialogTransfer = new DialogTransfer(OnEndInspect);
        }

        public override void Inspect()
        {
            if (_dialogTransfer.IsDialogPlay()) return;

            OnStartInspect();
            _dialogTransfer.StartDialog(_charService.PlayerName,
                _dialogService, DialogType.Basic);
        }

        public void OnEndDialog()
        {
            if (_dialogService.GetCurrDialogIndex() == startLoopIndex)
            {
                _dialogService.SetLoop(startLoopIndex, endLoopIndex);
                startLoopIndex = 0;
            }
        }

        public override void OnEndInspect()
        {
            base.OnEndInspect();

            if (startLoopIndex > 0) OnEndDialog();
        }
    }
}