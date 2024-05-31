using Data;
using UnityEngine;

namespace Interfaces
{
    public interface ICharState : ILockable, IResponcer
    {
        public string ID { get; }
        
        public void SetStateBasics(
            Transform parent,
            ICharacterService characterService);

        public void OnEndState();

        public void OnStartState();

        public bool Interact(string who);

        public void Set(CharStateData stateData);
    }
}