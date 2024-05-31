using System;
using Enums;

namespace Managers
{
    public class DialogTransfer
    {
        private event Action DialogEnd;
        
        public static DialogSystem dialogSystem;

        public DialogSystem DialogSystem
        {
            set => dialogSystem = value;
        }

        public bool HasDialogSystem()
        {
            return dialogSystem != null;
        }
        
        public DialogTransfer(Action action = null)
        {
            DialogEnd = action;
        }

        public void StartDialog(string whoCall, DialogService dialog,
            DialogType type)
        {
            switch (type)
            {
                case DialogType.Basic:
                    dialogSystem.SetAndStartDialog(
                        whoCall, dialog, this);
                    break;
                case DialogType.Choice:
                    dialogSystem.SetAndStartDialog(
                        whoCall, (ChoiceDialogService) dialog, this);
                    break;
            }
            
        }

        public bool IsDialogPlay()
        {
            if (dialogSystem != null) return dialogSystem.IsPlaying();
            return false;
        }

        public void SetAction(Action action)
        {
            DialogEnd = action;
        }

        public void ClearAction()
        {
            DialogEnd = null;
        }
        
        public void OnEndDialog()
        {
            DialogEnd?.Invoke();
        }
    }
}