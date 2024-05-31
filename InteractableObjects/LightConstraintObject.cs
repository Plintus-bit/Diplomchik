using System.Collections.Generic;
using Enums;
using Interfaces;
using Managers;
using UnityEngine;

namespace InteractableObjects
{
    public class LightConstraintObject : MonoBehaviour, ILockable
    {
        protected static DialogLoader _dialogLoader;
        protected DialogService _dialog;
        protected DialogTransfer _dialogTransfer;
        protected ICharacterService _charService;
        protected LockerInitializer _lockerInitializer;

        public List<string> dialogIds;
        public List<string> lockerIds;

        public int startLoopIndex = 0;
        public int endLoopIndex = -1;
        
        private void Awake()
        {
            if (_dialogLoader == null) _dialogLoader = new DialogLoader();
            _dialog = new DialogService();
            if (dialogIds.Count > 0)
            {
                _dialog.SetIds(dialogIds);
            }
            if (startLoopIndex > 0)
            {
                _dialogTransfer = new DialogTransfer(OnEndDialog);
            }
            else
            {
                _dialogTransfer = new DialogTransfer();
            }
        }
        
        private void Start()
        {
            if (lockerIds.Count > 0)
            {
                _lockerInitializer ??= LockerInitializer.GetInstance();
                foreach (var lockerId in lockerIds)
                {
                    _lockerInitializer.AddItemToLocker(lockerId, this);
                }
            }
            _charService = FindObjectOfType<CharacterService>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _charService.PushCharacter(_charService.PlayerName, transform);
                _dialogTransfer.StartDialog(
                    _charService.PlayerName, _dialog, DialogType.Basic);
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
            if (_dialog.GetCurrDialogIndex() == startLoopIndex)
            {
                _dialog.SetLoop(startLoopIndex, endLoopIndex);
                _dialogTransfer.ClearAction();
            }
        }
    }
}