using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class Recipe
    {
        public string itemId;
        public List<IngredientData> itemsNeeded;
        public int amount;

        public Recipe(string itemId, int amount = 1)
        {
            this.itemId = itemId;
            this.amount = amount;
            itemsNeeded = new List<IngredientData>();
        }
        public void AddItem(string itemId, int amount)
        {
            itemsNeeded.Add(new IngredientData(itemId, amount));
        }
        
        public bool HasItem(string itemId)
        {
            foreach (var item in itemsNeeded)
            {
                if (item.itemId == itemId) return true;
            }
            return false;
        }
        
        public bool HasItemAndAmount(string itemId, int amount)
        {
            foreach (var item in itemsNeeded)
            {
                if (item.itemId == itemId && item.amount <= amount) return true;
            }
            return false;
        }
        
        public int GetIngCount(string itemId)
        {
            foreach (var item in itemsNeeded)
            {
                if (item.itemId == itemId)
                {
                    return item.amount;
                }
            }
            return 0;
        }

        public string GetItemId()
        {
            return itemId;
        }
        
        public bool CompareWith(List<IngredientData> itemsSelected)
        {
            if (itemsNeeded.Count != itemsSelected.Count) return false;
            List<string> itemsSelectedNames = new List<string>();
            foreach (var item in itemsSelected)
            {
                itemsSelectedNames.Add(item.itemId);
            }
            foreach (var item in itemsNeeded)
            {
                if (!itemsSelectedNames.Contains(item.itemId))
                {
                    itemsSelectedNames.Clear();
                    return false;
                }
            }
            itemsSelectedNames.Clear();
            return true;
        }
        
        public bool CompareWith(Dictionary<string, IngredientData> itemsSelected)
        {
            if (itemsNeeded.Count != itemsSelected.Count) return false;
            List<string> itemsSelectedNames = new List<string>();
            foreach (var item in itemsSelected)
            {
                itemsSelectedNames.Add(item.Value.itemId);
            }
            foreach (var item in itemsNeeded)
            {
                if (!itemsSelectedNames.Contains(item.itemId))
                {
                    itemsSelectedNames.Clear();
                    return false;
                }
            }
            itemsSelectedNames.Clear();
            return true;
        }
        
        public void Print()
        {
            Debug.Log("\n" + itemId);
            foreach (var ingData in itemsNeeded)
            {
                Debug.Log(ingData.itemId + ": " + ingData.amount);
            }
        }
    }
}