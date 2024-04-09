using System;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private IInteractManager interactManager;
        private Character _charData;

        private void Start()
        {
            interactManager = GetComponentInChildren<IInteractManager>();
            _charData = GetComponent<Character>();
        }

        private void OnTriggerEnter(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable is { IsInteractable: true })
            {
                interactManager.RegistrObj(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactManager.UnregistrObj(interactable);
            }
        }

        public void Interact()
        {
            interactManager.Interact(_charData.GetName());
        }
    }
}