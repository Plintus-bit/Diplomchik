using Enums;
using Managers;
using Player;
using UnityEngine;

namespace Interfaces
{
    public interface ICharacterService
    {
        public string PlayerName { get; }
        public void TeleportToPos(string who, Vector3 where, Collider cameraArea = null);
        public void SetInputState(PlayerState state, string ownerId = null);
        public void PushCharacter(string who, Transform fromWhere);
        public Vector3 GetPlayerPosition();
        public bool HasPlayer();
        public void TryAddToInventory(string who, string whatId,
            int amount, out int amountNotAdded);
        public bool TryRemoveFromInventory(string who, string whatId, int amount);
        public bool TryAddAbility(string whatId);
        public bool TryAddProof(string whatId);
        public IInventorySlots GetInventory(string ownerId);
        public PlayerAbilities GetAbilities(string ownerId);
        public ProofFoundService GetProofService();
    }
}