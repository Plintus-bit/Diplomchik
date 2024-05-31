using System.Collections.Generic;
using Data;
using Factory;
using Interfaces;
using Managers;
using UnityEngine;

namespace InteractableObjects.Characters
{
    public class CharacterFSM
    {
        private List<ICharState> _states;
        private static CharStateFactory _charStateFactory;

        private Transform _parentTransform;
        private ICharacterService _characterService;
        private LockerInitializer _lockerInitializer;
        
        protected ICharState _currentState;
        protected int _currIndex;

        public CharacterFSM(DialogSystem system, List<CharStateData> _statesData,
            Transform parent, ICharacterService characterService)
        {
            _charStateFactory ??= new CharStateFactory(system);
            _parentTransform = parent;
            _characterService = characterService;
            
            Initialize(_statesData);
        }
        
        public bool Interact(string who)
        {
            if (_currentState != null) return _currentState.Interact(who);
            return true;
        }

        private void Initialize(List<CharStateData> _statesData)
        {
            _states = new List<ICharState>();
            foreach (CharStateData stateData in _statesData)
            {
                ICharState state = _charStateFactory.CreateState(this, stateData,
                    _parentTransform, _characterService);
                _states.Add(state);
            }

            if (_states.Count > 0)
            {
                ChangeState(0);
            }
        }
        
        public void ChangeState(int newStateIndex)
        {
            if (newStateIndex >= 0 && newStateIndex < _states.Count)
            {
                if (_currentState != null) _currentState.OnEndState();
                _currentState = _states[newStateIndex];
                _currIndex = newStateIndex;
                _currentState.OnStartState();
            }
        }

        public void NextState()
        {
            ChangeState(_currIndex + 1);
        }

        public void ChangeState(string stateId)
        {
            for (int i = 0; i < _states.Count; i++)
            {
                if (_states[i].ID == stateId)
                {
                    ChangeState(i);
                }
            }
        }
    }
}