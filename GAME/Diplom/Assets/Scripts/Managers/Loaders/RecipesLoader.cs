using System.Collections.Generic;
using Data;
using Enums;
using UnityEngine;

namespace Managers
{
    public class RecipesLoader
    {
        private List<Recipe> _recipes;

        public RecipesLoader()
        {
            _recipes = new List<Recipe>();
            ReadDataFromFile();
        }

        private void ReadDataFromFile()
        {
            TextAsset data = Resources.Load<TextAsset>("Data/Recipes");
            
            string text = data.text;
            string[] textArray = DataCleaner.CleanAndGetStrokesData(text,
                cleanType: DataSepparType.Addition, splitType: DataSepparType.Stroke);
            foreach (var stroke in textArray)
            {
                string[] strokeData = DataCleaner.GetStrokesData(stroke, DataSepparType.Main);
                Recipe recipe = new Recipe(strokeData[0], int.Parse(strokeData[1]));
                for (int i = 2; i < strokeData.Length; i++)
                {
                    string[] strokeItemsData = DataCleaner
                        .GetStrokesData(strokeData[i], DataSepparType.Addition);
                    recipe.AddItem(strokeItemsData[0], int.Parse(strokeItemsData[1]));
                }
                _recipes.Add(recipe);
            }
        }

        public List<Recipe> GetRecipesWithItem(string itemId)
        {
            List<Recipe> resultList = new List<Recipe>();
            foreach (var recipe in _recipes)
            {
                if (recipe.HasItem(itemId)) resultList.Add(recipe);
            }
            return resultList;
        }
        
        public List<Recipe> GetRecipesWithItem(string itemId, int amount)
        {
            List<Recipe> resultList = new List<Recipe>();
            foreach (var recipe in _recipes)
            {
                if (recipe.HasItemAndAmount(itemId, amount)) resultList.Add(recipe);
            }
            return resultList;
        }

        public void Print()
        {
            foreach (var recipe in _recipes)
            {
                recipe.Print();
            }
        }
    }
}