using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using Player;
using UnityEngine;

namespace Managers
{
    public class CharacterService : MonoBehaviour, ICharacterService
    {
        private Dictionary<string, IInventorySlots> _inventoryMaps;
        private Dictionary<string, Character> _characters;

        private void Awake()
        {
            _inventoryMaps = new Dictionary<string, IInventorySlots>();
            _characters = new Dictionary<string, Character>();
        }

        private void Start()
        {
            var characters = FindObjectsOfType<Character>();
            foreach (var character in characters)
            {
                string tempCharName = character.GetName();
                _characters.Add(tempCharName, character);
                if (character.HasInventory() 
                    && !_inventoryMaps.ContainsKey(tempCharName))
                {
                    _inventoryMaps.Add(tempCharName,
                        character.GetComponentInChildren<Inventory.Inventory>());
                }
            }
        }

        public IInventorySlots GetInventory(string ownerId)
        {
            _inventoryMaps.TryGetValue(ownerId, out var inventory);
            return inventory;
        }

        public void TeleportToPos(string who, Transform where)
        {
            var tempChar = _characters[who];
            PlayerInput tempCharInput = null;
            if (tempChar.GetCharType() == CharacterTypes.Player)
            {
                tempCharInput = tempChar.GetComponent<PlayerInput>();
            }

            if (tempCharInput)
            {
                tempCharInput.State = PlayerState.Transition;
            }

            tempChar.transform.position = where.position;
            
            if (tempCharInput)
            {
                tempCharInput.State = PlayerState.Movable;
            }
        }

        public void Print()
        {
            foreach (var key in _inventoryMaps)
            {
                Debug.Log(key.Key);
            }
        }
    }
}