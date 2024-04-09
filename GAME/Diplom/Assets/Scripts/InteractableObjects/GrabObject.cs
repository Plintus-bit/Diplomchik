using UnityEngine;

namespace InteractableObjects
{
    public class GrabObject : BasicObject
    {
        [SerializeField] private string itemId;
        [Range(1, 10)] public int amount;

        public override void SetIconNames()
        {
            IconName = "GrabIcon";
        }

        public override bool Interact(string who)
        {
            var charInventory = _charService.GetInventory(who);
            int amountStay = charInventory.AddItem(itemId, amount);
            if (amountStay <= 0)
            {
                Destroy(gameObject);
                return false;
            }
            amount = amountStay;
            return true;
        }
        
    }
}