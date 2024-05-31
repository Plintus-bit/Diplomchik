using Enums;
using InteractableObjects;
using Managers;
using UI;
using UnityEngine;

namespace Brainteasers
{
    public class WatchBrainTeaser : BasicDialogObject
    {
        [SerializeField] private WatchUI _ui;
        [SerializeField] private Watch watch;

        public override void SetIconNames()
        {
            IconName = "BrainteaserIcon";
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            watch.Init(_charService);
            _ui.WatchBrain = watch;
            
            _informerInitializer ??= InformerInitializer.Instance;
            _informerInitializer.RegistrResponcer(informerId, this);
        }

        public override bool Interact(string who)
        {
            if (watch.GetActiveStateCurrStage())
            {
                _ui.Open();
                return IsInteractable;
            }
            
            _dialogTransfer.StartDialog(who, _dialogService, type);
            return IsInteractable;
        }

        public override void Responce(string whoInformer,
            InformStatus status = InformStatus.End)
        {
            DestroyObject();
        }
    }
}