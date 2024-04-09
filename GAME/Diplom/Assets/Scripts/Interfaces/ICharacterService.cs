using UnityEngine;

namespace Interfaces
{
    public interface ICharacterService
    {
        public IInventorySlots GetInventory(string ownerId);
        public void TeleportToPos(string who, Transform where);
    }
}