using System;
using System.Collections.Generic;
using Cinemachine;
using Enums;
using Interfaces;
using Player;
using UnityEngine;

namespace Managers
{
    public class CharacterService : MonoBehaviour, ICharacterService
    {
        [SerializeField] private GameSceneManager _gameSceneManager;
        [SerializeField] private ProofFoundService _playerProofFoundService;
        [SerializeField] private CinemachineConfiner cameraConfiner;
        [SerializeField] private PlayerCharacter _currPlayer;
        
        private Dictionary<string, IInventorySlots> _inventoryMaps;
        private Dictionary<string, Character> _characters;

        private string _playerName;
        public String PlayerName => _playerName;

        [Min(0)]
        public float pushSpeed;

        public Vector3 tempTeleportPoint;
        public Collider tempCameraArea;

        private void Awake()
        {
            _inventoryMaps = new Dictionary<string, IInventorySlots>();
            _characters = new Dictionary<string, Character>();
            _playerName = "";
        }

        private void Start()
        {
            var characters = FindObjectsOfType<Character>();
            foreach (var character in characters)
            {
                string tempCharName = character.GetName();
                if (string.IsNullOrEmpty(tempCharName)) continue;
                
                _characters.Add(tempCharName, character);
                if (character.HasInventory() 
                    && !_inventoryMaps.ContainsKey(tempCharName))
                {
                    _inventoryMaps.Add(tempCharName, character.CharacterInventory);
                }

                if (character.GetCharType() == CharacterTypes.Player)
                {
                    _currPlayer = character.GetComponent<PlayerCharacter>();
                    _playerName = _currPlayer.GetName();
                }
            }
        }

        public bool HasPlayer()
        {
            if (_currPlayer == null) return false;
            return true;
        }
        
        private void Push(Rigidbody rb, Vector3 direction)
        {
            if (pushSpeed == 0) pushSpeed = 2.55f;
            rb.AddForce(direction * pushSpeed, ForceMode.Impulse);
        }

        public IInventorySlots GetInventory(string ownerId)
        {
            _inventoryMaps.TryGetValue(PlayerName, out IInventorySlots inventory);
            return inventory;
        }

        public PlayerAbilities GetAbilities(string ownerId)
        {
            return _currPlayer.Abilities;
        }

        public void TeleportToPos(string who, Vector3 where, Collider cameraArea = null)
        {
            if (cameraArea != null)
            {
                tempCameraArea = cameraArea;
            }

            tempTeleportPoint = where;
            _currPlayer.PlayerInput.State = PlayerState.Transition;
            _gameSceneManager.CloseScreen(Teleport);
        }

        public void Teleport()
        {
            cameraConfiner.m_BoundingVolume = tempCameraArea;
            _currPlayer.transform.position = tempTeleportPoint;
            _gameSceneManager.OpenScreen(EndTeleport);
        }

        public void EndTeleport()
        {
            _currPlayer.PlayerInput.State = PlayerState.Movable;
        }
        
        public Vector3 GetPlayerPosition()
        {
            return _currPlayer.PlayerRB.transform.position;
        }

        public void SetInputState(PlayerState state, string ownerId = null)
        {
            if (string.IsNullOrEmpty(ownerId) &&
                string.IsNullOrEmpty(_playerName)) return;
            
            if (_currPlayer == null) return;
            _currPlayer.PlayerInput.State = state;
        }

        public bool CheckPlayer()
        {
            if (string.IsNullOrEmpty(PlayerName)) return false;
            if (_currPlayer.PlayerInput == null) return false;
            if (_currPlayer.PlayerRB == null) return false;
            return true;
        }

        public void PushCharacter(string who, Transform fromWhere)
        {
            Character tempChar = _characters[who];
            var direction = tempChar.transform.position.x - fromWhere.position.x;
            var normalize = direction / MathF.Abs(direction);
            var dirVector = new Vector3(normalize, 0, 0);

            if (who == PlayerName)
            {
                Push(_currPlayer.PlayerRB, dirVector);
            }
            else Push(tempChar.GetComponent<Rigidbody>(), dirVector);
        }

        public ProofFoundService GetProofService()
        {
            return _playerProofFoundService;
        }

        public void TryAddToInventory(
            string who, string whatId,
            int amount, out int amountNotAdded)
        {
            IInventorySlots inventory = GetInventory(who);
            amountNotAdded = inventory.AddItem(whatId, amount);
        }

        public bool TryRemoveFromInventory(
            string who, string whatId, int amount)
        {
            IInventorySlots inventory = GetInventory(who);
            if (inventory.HasEnoughAmount(whatId, amount))
            {
                return inventory.TryRemoveItem(whatId, amount);
            }

            return false;
        }

        public bool TryAddAbility(string whatId)
        {
            return _currPlayer.Abilities.TryAddAbility(whatId);
        }

        public bool TryAddProof(string whatId)
        {
            return _playerProofFoundService.AddProof(whatId);
        }
    }
}