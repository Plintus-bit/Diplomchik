using System.Collections.Generic;
using Enums;
using Interfaces;
using Managers;
using Player;
using UnityEngine;

namespace InteractableObjects
{
    public class ConstraintObject : MonoBehaviour, ILockable
    {
        private static DialogLoader _dialogLoader;
        private DialogService _dialog;
        private DialogTransfer _dialogTransfer;
        private ICharacterService _charService;
        private LockerInitializer _lockerInitializer;

        public List<string> dialogIds;
        public string whatNeedId;
        public string lockerId;

        public bool isEndDialogCycled;

        private void Awake()
        {
            if (_dialogLoader == null) _dialogLoader = new DialogLoader();
            _dialog = new DialogService(dialogIds);
            _dialogTransfer = new DialogTransfer();
            if (isEndDialogCycled) _dialogTransfer.SetAction(OnEndDialog);
        }

        private void Start()
        {
            _lockerInitializer ??= LockerInitializer.GetInstance();
            if (lockerId != null)
            {
                _lockerInitializer.AddItemToLocker(lockerId, this);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _charService ??= FindObjectOfType<CharacterService>();
                var playerName = _charService.PlayerName;
                
                PlayerAbilities abils = _charService.GetAbilities(playerName);
                bool hasNeededAbility = abils != null && abils.Check(whatNeedId);
                if (string.IsNullOrEmpty(whatNeedId)
                    || !hasNeededAbility)
                {
                    _charService.PushCharacter(playerName, transform);
                    
                    if (_dialog.IsEmpty()) return;
                    if (!_dialogTransfer.HasDialogSystem())
                    {
                        _dialogTransfer.DialogSystem = FindObjectOfType<DialogSystem>();
                    }
                    _dialogTransfer.StartDialog(playerName, _dialog, DialogType.Basic);
                    return;
                }

                if (hasNeededAbility)
                {
                    Unlock();
                }
            }
        }

        public void Lock()
        {
            GetComponentInChildren<Collider>().enabled = true;
            enabled = true;
        }

        public void Unlock()
        {
            GetComponentInChildren<Collider>().enabled = false;
            enabled = false;
        }

        public void OnEndDialog()
        {
            int currDialogIndex = _dialog.GetCurrDialogIndex();
            if (currDialogIndex == _dialog.GetDialogsCount() - 1)
            {
                _dialog.SetLoop(currDialogIndex, currDialogIndex);
                _dialogTransfer.ClearAction();
            }
        }
    }
}