using System.Collections.Generic;
using Data;
using Enums;
using UnityEngine;

namespace Managers
{
    public class RecipesLoader : BaseLoader<Recipe>
    {
        private List<Recipe> _recipes;

        public RecipesLoader()
        {
            _recipes = new List<Recipe>();
            path = "Data/Recipes";
            ReadDataFromFile();
        }

        protected override void ReadDataFromFile()
        {
            TextAsset data = Resources.Load<TextAsset>(path);
            
            string text = data.text;
            string[] textArray = DataCleaner.CleanAndGetStrokesData(text,
                cleanType: DataSepparType.Addition, splitType: DataSepparType.Stroke);
            foreach (var stroke in textArray)
            {
                string[] strokeData = DataCleaner.GetStrokesData(stroke, DataSepparType.Main);
                Recipe recipe = new Recipe(strokeData[0], int.Parse(strokeData[1]));
                
                string[] recipeData = DataCleaner.GetStrokesData(strokeData[2], DataSepparType.AdditionMain);
                for (int i = 0; i < recipeData.Length; i++)
                {
                    string[] strokeItemsData = DataCleaner
                        .GetStrokesData(recipeData[i], DataSepparType.Addition);
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
                Debug.Log("recipe:");
                recipe.Print();
            }
        }
    }
}