using System.Collections.Generic;

namespace Interfaces.ReadOnly
{
    public interface IReadOnlyProofData
    {
        public int Amount { get; }
        public int MaxAmount { get; }
        public string Title { get; }
        public IReadOnlyAuthor Author { get; }
        public int ProofVariantsAmount { get; }
        public string GetProofDescription(int index);
        public List<string> GetDescriptions();
        
    }
}