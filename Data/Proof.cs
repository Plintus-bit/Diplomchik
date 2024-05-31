using System.Collections.Generic;
using Interfaces.ReadOnly;
using UnityEngine;

namespace Data
{
    public class Proof : IReadOnlyProofData
    {
        public List<ProofData> proofDatas;
        public int amount;

        public int maxAmount;
        public string title;
        public Author author;
        
        
        public Proof(ProofData data, int amount = 1)
        {
            proofDatas = new List<ProofData>();
            proofDatas.Add(data);
            this.amount = amount;
            maxAmount = data.maxAmount;
            author = data.author;
            title = data.title;
        }

        public int Amount => amount;
        public int MaxAmount => maxAmount;
        public IReadOnlyAuthor Author => author;
        public string Title => title;
        public int ProofVariantsAmount => proofDatas.Count;

        public string GetProofDescription(int index)
        {
            if (index >= 0 && index < proofDatas.Count)
                return proofDatas[index].description;
            return null;
        }
        
        public ProofData GetProofData(string id)
        {
            foreach (var item in proofDatas)
            {
                if (item.id == id) return item;
            }
            return null;
        }

        public bool IsEqual(ProofData proof)
        {
            if (author.AuthorType == proof.author.AuthorType
                && title == proof.title) return true;
            return false;
        }

        public bool TryAddProof(ProofData data)
        {
            if (amount == maxAmount) return false;
            if (GetProofData(data.id) == null) proofDatas.Add(data);
            amount += 1;
            return true;
        }

        public List<string> GetDescriptions()
        {
            List<string> dataToReturn = new List<string>(proofDatas.Count);
            foreach (var data in proofDatas)
            {
                dataToReturn.Add(data.description);
            }

            return dataToReturn;
        }

        public void Print()
        {
            Debug.Log("”À» ¿");
            Debug.Log(title + "; " + Author.Name
                      + "; maxAmount: " + maxAmount
                      + "; amount: " + amount);
            Debug.Log("Items:");
            foreach (var data in proofDatas)
            {
                Debug.Log(data.description);
            }
        }
    }
}