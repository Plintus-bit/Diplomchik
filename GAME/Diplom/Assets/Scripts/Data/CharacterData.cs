using System;
using Enums;

namespace Data
{
    [Serializable]
    public struct CharacterData
    {
        public CharacterTypes type;
        public string name;
        public bool hasInventory;
    }
}