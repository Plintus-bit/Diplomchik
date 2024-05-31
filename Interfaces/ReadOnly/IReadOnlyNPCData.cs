using UnityEngine;

namespace Interfaces.ReadOnly
{
    public interface IReadOnlyNPCData
    {
        public Sprite GetImage(string state);
        public string Name { get; }
        public Color Color { get; }
        public Color NameColor { get; }
    }
}