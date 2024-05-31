using Enums;
using UnityEngine;

namespace Interfaces.ReadOnly
{
    public interface IReadOnlyAuthor
    {
        public ProofType AuthorType { get; }
        public string Name { get; }
        public string Description { get; }
        public Sprite AuthorImage { get; }
    }
}