using Interfaces;
using UnityEngine;

namespace InteractableObjects
{
    public class PlaceObject : BasicDialogObject
    {
        [SerializeField] private string _whatCanPlaceID;
        [SerializeField] private BasicObject _prefab;
        [SerializeField] private Transform _point;
        
        private BasicObject _placedObject;

        public override bool Interact(string who)
        {
            IInventorySlots tempInventory;
            if (_placedObject == null)
            {
                tempInventory = _charService.GetInventory(who);
                if (tempInventory.HasItem(_whatCanPlaceID))
                {
                    tempInventory.TryRemoveItem(_whatCanPlaceID, 1);
                    
                    _placedObject = Instantiate(_prefab);
                    _placedObject.transform.position = _point.position;

                    if (_placedObject.GetType() == typeof(GrabObject))
                    {
                        IsInteractable = false;
                        RegistrListener();
                    }
                    else
                    {
                        DestroyObject();
                    }
                }
            }
            return IsInteractable;
        }

        public override void ReactOnNotify(IInteractable obj, bool isInteractable)
        {
            if ((BasicObject)obj == _placedObject
                && isInteractable == false)
            {
                UnregistrListener();
                IsInteractable = true;
                Notify();
            }
        }
    }
}