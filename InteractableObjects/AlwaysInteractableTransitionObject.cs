using System.Collections.Generic;
using Enums;
using Managers;
using UnityEngine;
namespace InteractableObjects
{
    public class AlwaysInteractableTransitionObject : TransitionObject
    {
        [SerializeField] protected List<string> _dialogIds;
        [SerializeField] protected List<string> _idsOnInform;
        protected DialogService _dialogService;
        protected DialogTransfer _dialogTransfer;

        protected bool _isLocked;

        protected override void Initialize()
        {
            base.Initialize();

            _dialogTransfer = new DialogTransfer();
            _dialogService = new DialogService(_dialogIds);
            _informerInitializer.RegistrResponcer(informerId, this);
        }

        public override void Lock()
        {
            _isLocked = true;
        }

        public override void Unlock()
        {
            _isLocked = false;
        }

        public override void SetIconNames()
        {
            IconName = "TransitionIcon";
        }

        public override bool Interact(string who)
        {
            if (_isLocked)
            {
                _dialogTransfer.StartDialog(who, _dialogService, DialogType.Basic);
                return IsInteractable;
            }
            
            _charService.TeleportToPos(who, teleportPos.position, cameraArea);
            return IsInteractable;
        }

        public override void Responce(
            string whoInformer,
            InformStatus status = InformStatus.End)
        {
            _informerInitializer.UnregistrResponcer(whoInformer, this);
            Lock();
            _dialogService.SetIds(_idsOnInform);
        }
    }
}