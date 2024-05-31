using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct ItemDataForOperations
    {
        public string id;
        public List<string> dialogIds;
        [Range(1, 10)]
        public int amount;

        public bool HasReward()
        {
            return !string.IsNullOrEmpty(id);
        }

        public void Clear()
        {
            amount = 0;
            id = null;
            dialogIds = null;
        }
    }
}