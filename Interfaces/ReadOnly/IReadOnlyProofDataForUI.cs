using System.Collections.Generic;
using Enums;

namespace Interfaces.ReadOnly
{
    public interface IReadOnlyProofDataForUI
    {
        public int GetPercentageAuthorGuilty(ProofType author);
        public List<IReadOnlyProofData> GetAuthorProofs(ProofType author);
        public bool ChangeAuthor(bool isNext);
        public bool ChangeAuthor(string authorName);
        public IReadOnlyAuthor GetCurrAuthor();
        public ProofType GetCurrAuthorType();
        public bool IsStartOrLastAuthor(out SwitcherStates state);
    }
}