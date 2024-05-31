using UnityEngine;
namespace InteractableObjects
{
    public class TransitionObject : BasicObject
    {
        [SerializeField] protected Transform teleportPos;
        [SerializeField] protected Collider cameraArea;

        public override void SetIconNames()
        {
            IconName = "TransitionIcon";
        }

        public override bool Interact(string who)
        {
            _charService.TeleportToPos(who, teleportPos.position, cameraArea);
            return IsInteractable;
        }
        
    }
}