using System;
using Interfaces;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private int maxTicksDelta;
        private IInteractManager interactManager;
        private Character _charData;

        private IInteractable _lastEnterInteractable;
        private IInteractable _lastExitInteractable;
        private int _lastEnterTicks;
        private int _lastExitTicks;

        private void Start()
        {
            interactManager = GetComponent<IInteractManager>();
            _charData = GetComponent<Character>();
        }

        private void OnTriggerEnter(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactManager.RegistrObj(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactManager.UnregistrObj(interactable, true);
            }
        }

        public void Interact()
        {
            interactManager.Interact(_charData.GetName());
        }
    }
}