using Data;
using Enums;
using InteractableObjects;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Brainteasers
{
    public abstract class BaseBrainTeaser : BasicDialogObject
    {
        private bool _isOpen;
        [SerializeField] protected Locker _locker;

        public BaseBrainTeaserUI brainTeaserUI;

        protected override void Initialize()
        {
            base.Initialize();
            ItemOperations.Init(_charService);
            _locker.Init();
        }

        public override bool Interact(string who)
        {
            if (_isOpen) return IsInteractable;
            
            Open();
            return IsInteractable;
        }

        public override void SetIconNames()
        {
            IconName = "BrainteaserIcon";
        }

        public void Open()
        {
            brainTeaserUI.Open();
            _isOpen = true;
        }

        public void Close()
        {
            brainTeaserUI.Close();
            _isOpen = false;
        }

        public virtual void OnUIAction(int index) { }
    }
}