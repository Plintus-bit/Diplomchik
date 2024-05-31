using System.Collections.Generic;
using Data;
using Enums;
using Interfaces;
using Inventory;
using Managers;
using UnityEngine;

public class Craft
{
    private static RecipesLoader _recipesLoader;
    private List<Recipe> tempRecipes;
    private Dictionary<string, IngredientData> itemsSelected;

    private RecipeItemStatus craftStatus;

    public Craft()
    {
        _recipesLoader = new RecipesLoader();
        tempRecipes = null;
        itemsSelected = new Dictionary<string, IngredientData>();
        craftStatus = RecipeItemStatus.Waiting;
    }

    public bool AddItemAndCheckRecipe(Slot slot)
    {
        if (itemsSelected.ContainsKey(slot.GetItemId())) return true;
        IngredientData newIng = new IngredientData(slot);
        itemsSelected.Add(newIng.itemId, newIng);
        if (tempRecipes == null || tempRecipes.Count == 0)
        {
            tempRecipes = _recipesLoader.GetRecipesWithItem(
                slot.GetItemId(), slot.GetAmount());
        }
        else
        {
            RemoveRecipes(slot.GetItemId(), slot.GetAmount(), RecipeItemStatus.Without);
        }
        if (tempRecipes.Count != 0) return true;
        ClearCraft();
        return false;
    }

    public void RemoveItem(Slot slot)
    {
        IngredientData data;
        if (!itemsSelected
                .TryGetValue(slot.GetItemId(), out data)) return;

        RemoveRecipes(data.itemId,
            data.amount, RecipeItemStatus.With);
        itemsSelected.Remove(data.itemId);
    }
    
    public bool CanCraft()
    {
        if (tempRecipes == null) return false;
        if (tempRecipes.Count == 1
            && tempRecipes[0].CompareWith(itemsSelected)) return true;
        return false;
    }

    private void RemoveRecipes(string itemId, int amount, RecipeItemStatus conditionToRemove)
    {
        var recipeToDelete = new List<Recipe>();
        foreach (var recipe in tempRecipes)
        {
            if (!recipe.HasItemAndAmount(itemId, amount))
            {
                if (conditionToRemove == RecipeItemStatus.Without)
                {
                    recipeToDelete.Add(recipe);
                }
            }
            else
            {
                if (conditionToRemove == RecipeItemStatus.With)
                {
                    recipeToDelete.Add(recipe);
                }
            }
        }

        for (int i = 0; i < recipeToDelete.Count; i++)
        {
            tempRecipes.Remove(recipeToDelete[i]);
        }
        recipeToDelete.Clear();
    }
    
    public void ClearCraft()
    {
        if (craftStatus != RecipeItemStatus.InCraft)
        {
            tempRecipes = null;
            itemsSelected.Clear();
        }
    }

    public bool TryDoCraft(IInventorySlots inventory)
    {
        if (!CanCraft())
        {
            return false;
        }
        if (itemsSelected.Count <= 0) return false;
        craftStatus = RecipeItemStatus.InCraft;
        var recipe = tempRecipes[0];
        foreach (var ingData in itemsSelected)
        {
            bool status = inventory.TryRemoveItem(ingData.Value.itemId,
                recipe.GetIngCount(ingData.Value.itemId), false);
        }
        inventory.AddItem(recipe.itemId, recipe.amount, false);
        inventory.UpdateUI(0, inventory.Size);
        craftStatus = RecipeItemStatus.Waiting;
        ClearCraft();
        return true;
    }

    public void Print()
    {
        Debug.Log("Выбранное");
        foreach (var item in itemsSelected)
        {
            Debug.Log(item.Value.itemId);
            Debug.Log(item.Value.amount);
        }
    }
}