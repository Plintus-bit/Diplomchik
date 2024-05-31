using Data;
using Enums;
using Interfaces;
using UnityEngine;

namespace Managers
{
    public class DialogSystem : MonoBehaviour
    {
        private DialogService _whatToPlay;
        private ChoiceDialogService _whatToPlayByChoice;
        
        private DialogTransfer _whoCallBack;
        private string _whoCall;

        public static ICharacterService charService;
        public DialogUIManager dialogUIManager;

        private void Start()
        {
            if (charService == null)
            {
                SetCharacterService();
            }
            dialogUIManager = FindObjectOfType<DialogUIManager>();
            DialogTransfer.dialogSystem = this;
        }

        public void Play()
        {
            if (_whatToPlay != null)
            {
                NextDialog();
                return;
            }

            if (_whatToPlayByChoice != null)
            {
                NextChoiceDialog();
            }
        }

        public void NextDialog()
        {
            if (_whatToPlay == null) return;

            if (!dialogUIManager.IsEndWrite())
            {
                dialogUIManager.EndWrite();
                return;
            }

            DialogStrokeData currDialog = _whatToPlay.NextDialog();
            if (currDialog == null)
            {
                DialogsClearOnly();
                dialogUIManager.onEndDialog();
                _whoCallBack.OnEndDialog();
                charService.SetInputState(PlayerState.Movable, _whoCall);
                if (!IsPlaying())
                {
                    Clear();
                }
            }
            else
            {
                DialogStatus status = _whatToPlay.GetStatus();
                if (status == DialogStatus.Process)
                {
                    dialogUIManager.Change(currDialog);
                } else if (status == DialogStatus.Start)
                {
                    dialogUIManager.Change(currDialog);
                    charService.SetInputState(PlayerState.InDialog, _whoCall);
                    dialogUIManager.OnStartDialog();
                }
            }
        }

        public void NextChoiceDialog()
        {
            DialogStrokeData currDialog = _whatToPlayByChoice.NextDialog();
            DialogStatus status = _whatToPlayByChoice.GetStatus();

            if (status == DialogStatus.Start)
            {
                if (!dialogUIManager.IsEndWrite())
                {
                    dialogUIManager.EndWrite();
                    return;
                }
                bool wasNotStarted = dialogUIManager.OnStartDialog();
                dialogUIManager.Change(currDialog);
                if (wasNotStarted)
                {
                    charService.SetInputState(
                        PlayerState.InDialog, _whoCall);
                }
                return;
            }
            if (status == DialogStatus.Choice)
            {
                bool wasNotStarted = dialogUIManager.OnStartDialog();
                dialogUIManager.Change(_whatToPlayByChoice.GetChoices());
                if (wasNotStarted)
                {
                    charService.SetInputState(
                        PlayerState.InDialog, _whoCall);
                }
                return;
            }
            if (currDialog == null) return;
            if (status == DialogStatus.Process)
            {
                if (!dialogUIManager.IsEndWrite())
                {
                    dialogUIManager.EndWrite();
                    return;
                }
                dialogUIManager.Change(currDialog);
            }
        }

        public void ChangeDialog(int index)
        {
            _whatToPlayByChoice.ChangeCurrentDialog(index);
            NextChoiceDialog();
        }
        
        public void SetAndStartDialog(string whoCall,
            DialogService whatToPlay, DialogTransfer whoCallBack)
        {
            if (_whatToPlay != null) return;
            
            _whatToPlay = whatToPlay;
            SetWhoCall(whoCall, whoCallBack);

            NextDialog();
        }
        
        public void SetAndStartDialog(string whoCall,
            ChoiceDialogService whatToPlay, DialogTransfer whoCallBack)
        {
            if (_whatToPlayByChoice != null) return;
            
            _whatToPlayByChoice = whatToPlay;
            SetWhoCall(whoCall, whoCallBack);
            
            NextChoiceDialog();
        }

        public void SetWhoCall(string whoCall, DialogTransfer whoCallBack)
        {
            _whoCall = whoCall;
            _whoCallBack = whoCallBack;
        }

        public bool IsPlaying()
        {
            return _whatToPlay != null || _whatToPlayByChoice != null;
        }
        
        public void SetCharacterService()
        {
            charService = FindObjectOfType<CharacterService>();
        }
        
        public void Interrupt()
        {
            if (_whatToPlayByChoice == null) return;
            charService.SetInputState(PlayerState.Movable, _whoCall);
            _whatToPlayByChoice.EndDialog();
            DialogsClearOnly();
            dialogUIManager.onEndDialog();
            if (!IsPlaying())
            {
                Clear();
            }
        }

        private void Clear()
        {
            _whatToPlayByChoice = null;
            _whatToPlay = null;
            _whoCallBack = null;
            _whoCall = null;
        }

        private void DialogsClearOnly()
        {
            _whatToPlayByChoice = null;
            _whatToPlay = null;
        }
    }
}