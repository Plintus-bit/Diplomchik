using System;
using System.Collections.Generic;
using Data;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerAbilities : MonoBehaviour
    {
        [SerializeField] private AbilityPanelUI _abilitiesUI;
        
        private Dictionary<string, Ability> _abilities;

        public static AbilityLoader _abilityLoader;

        private void Awake()
        {
            if (_abilityLoader == null) _abilityLoader = AbilityLoader.Instance;
            _abilities = new Dictionary<string, Ability>();
        }

        public bool Check(string abilId)
        {
            return _abilities.ContainsKey(abilId);
        }

        public void Use(string abilId)
        {
            if (!Check(abilId)) return;

            _abilities.TryGetValue(abilId, out Ability abil);
            abil.Use();
        }

        public bool TryAddAbility(string abilId)
        {
            Ability abil = _abilityLoader.GetDataById(abilId);
            if (abil == null) return false;
            
            bool isAdded = _abilities.TryAdd(abil.id, abil);
            if (isAdded)
            {
                _abilitiesUI.AddAbility(abil);   
            }

            return isAdded;
        }
        
    }
}