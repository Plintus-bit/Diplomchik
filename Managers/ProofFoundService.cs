using System;
using System.Collections.Generic;
using Data;
using Enums;
using Interfaces.ReadOnly;
using UnityEngine;

namespace Managers
{
    public class ProofFoundService : MonoBehaviour, IReadOnlyProofDataForUI
    {
        [SerializeField] private BookmarksManager _bookmarksManager;
        
        private ProofLoader _proofLoader;
        private List<Proof> _proofs;
        private Dictionary<ProofType, int> _authorsProofCountMap;
        
        private Dictionary<int, Author> _authorOrder;
        private int _currAuthorIndex;

        private void Awake()
        {
            _proofLoader = ProofLoader.Instance;
            _authorsProofCountMap = new Dictionary<ProofType, int>();
            _authorOrder = new Dictionary<int, Author>();
            _proofs = new List<Proof>();
        }

        public int GetPercentageAuthorGuilty(ProofType author)
        {
            int allAuthorProofsCount = _proofLoader.GetAuthorProofsCount(author);
            bool isAuthorFound = _authorsProofCountMap.TryGetValue(author, out int value);
            if (!isAuthorFound) return 0;
            double result = value / (double) allAuthorProofsCount * 100;
            return (int) Math.Ceiling(result);
        }

        public bool AddProof(string id)
        {
            ProofData proofData = _proofLoader.GetDataById(id);
            if (proofData == null) return false;

            bool isContainAuthor = _authorsProofCountMap
                .ContainsKey(proofData.author.AuthorType);
            if (isContainAuthor)
            {
                _authorsProofCountMap[proofData.author.AuthorType] += 1;
            }
            else
            {
                _authorOrder.Add(_authorsProofCountMap.Count,
                    proofData.author);
                _bookmarksManager.CreateBookmark(proofData.author);
                _authorsProofCountMap.Add(proofData.author.author, 1);
            }

            if (proofData.maxAmount > 1)
            {
                foreach (var proof in _proofs)
                {
                    if (proof.IsEqual(proofData))
                    {
                        return proof.TryAddProof(proofData);
                    }
                }
            }
            
            _proofs.Add(new Proof(proofData));
            if (_proofs.Count == 1) _currAuthorIndex = 0;
            return true;
        }

        public List<IReadOnlyProofData> GetAuthorProofs(ProofType author)
        {
            if (_proofs.Count <= 0) return null;
            
            List<IReadOnlyProofData> dataToReturn = new List<IReadOnlyProofData>();
            foreach (var proof in _proofs)
            {
                if (proof.author.AuthorType == author)
                    dataToReturn.Add(proof);
            }

            return dataToReturn;
        }

        public bool ChangeAuthor(bool isNext)
        {
            int index;
            if (isNext) index = _currAuthorIndex + 1;
            else index = _currAuthorIndex - 1;

            if (index < 0 || index >= _authorOrder.Count) return false;
            _currAuthorIndex = index;
            return true;
        }

        public bool ChangeAuthor(string authorName)
        {
            if (_authorOrder[_currAuthorIndex].Name == authorName) return false;

            foreach (var authorKeyValue in _authorOrder)
            {
                if (authorKeyValue.Value.Name == authorName)
                {
                    _currAuthorIndex = authorKeyValue.Key;
                    return true;
                }
            }
            return false;
        }

        public IReadOnlyAuthor GetCurrAuthor()
        {
            if (_authorOrder.TryGetValue(_currAuthorIndex,
                    out Author author)) return author;
            return null;
        }

        public ProofType GetCurrAuthorType()
        {
            IReadOnlyAuthor tempAuthor = GetCurrAuthor();
            if (tempAuthor == null) return ProofType.Undefined;
            return tempAuthor.AuthorType;
        }

        public bool IsStartOrLastAuthor(out SwitcherStates state)
        {
            if (_currAuthorIndex == 0)
            {
                if (_currAuthorIndex == _authorOrder.Count - 1)
                {
                    state = SwitcherStates.StartEnd;
                } else
                {
                    state = SwitcherStates.Start;
                }
                return true;
            }
            if (_currAuthorIndex == _authorOrder.Count - 1)
            {
                state = SwitcherStates.End;
                return true;
            }

            state = SwitcherStates.Start;
            return false;
        }

        public void Print()
        {
            if (_proofs == null || _proofs.Count == 0) return;
            Debug.Log("Start");
            foreach (var proof in _proofs)
            {
                proof.Print();
            }
            Debug.Log("End");
        }
    }
}