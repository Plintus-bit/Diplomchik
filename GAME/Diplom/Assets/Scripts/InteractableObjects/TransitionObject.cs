using Enums;
using Player;
using UnityEngine;

namespace InteractableObjects
{
    public class TransitionObject : BasicObject
    {
        [SerializeField] private Transform teleportPos;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Transform playerTransform;

        protected override void Initialize()
        {
            GameObject temp = GameObject.FindWithTag("Player");
            _playerInput = temp.GetComponent<PlayerInput>();
            playerTransform = temp.transform;
        }
        
        public override void SetIconNames()
        {
            IconName = "TransitionIcon";
        }

        public override bool Interact(string who)
        {
            _charService.TeleportToPos(who, teleportPos);
            return IsInteractable;
        }
        
    }
}