using Interfaces.ReadOnly;
using UnityEngine;

namespace Data
{
    public class Ability : IReadOnlyAbility
    {
        public string id;
        public Sprite icon;

        public Sprite AbilImage => icon;
        public Ability(string id)
        {
            this.id = id;
            icon = Resources.Load<Sprite>(
                "ItemsIcon/" + id);
        }
        
        public virtual void Use() {}
    }
}