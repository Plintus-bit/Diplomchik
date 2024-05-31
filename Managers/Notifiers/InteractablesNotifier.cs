using System.Collections.Generic;
using Interfaces;

namespace Managers.Notifiers
{
    public static class InteractablesNotifier
    {
        private static List<IInteractListener> _interactListeners;

        public static void RegistrListener(IInteractListener item)
        {
            if (_interactListeners == null)
                _interactListeners = new List<IInteractListener>();
            
            if(_interactListeners.Contains(item)) return;
            _interactListeners.Add(item);
        }

        public static void Notify(IInteractable obj, bool isInteractable = false)
        {
            for (int i = 0; i < _interactListeners.Count; i++)
            {
                int lenght = _interactListeners.Count;
                _interactListeners[i].ReactOnNotify(obj, isInteractable);
                if (lenght > _interactListeners.Count) i -= 1;
            }
        }

        public static void UnregistrListener(IInteractListener item)
        {
            if (_interactListeners.Contains(item))
                _interactListeners.Remove(item);
        }

    }
}