using System.Collections.Generic;
using Data;
using Managers;
using UnityEngine;

namespace InteractableObjects.Characters
{
    public class BaseCharacter : BasicObject
    {
        private CharacterFSM _characterFsm;

        [SerializeField] private List<CharStateData> _statesData;

        public override bool Interact(string who)
        {
            return _characterFsm.Interact(who);
        }

        protected override void Initialize()
        {
            _characterFsm = new CharacterFSM(FindObjectOfType<DialogSystem>(),
                _statesData, transform, _charService);
        }
        
    }
}