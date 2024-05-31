using UI;
using UnityEngine;

namespace InteractableObjects
{
    public class OpenObject : BasicObject
    {
        [SerializeField] private BasicUIWindow _UIWindow;

        public override bool Interact(string who)
        {
            _UIWindow.Open();

            return IsInteractable;
        }
    }
}