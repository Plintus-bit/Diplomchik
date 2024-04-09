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
    private List<IngredientData> itemsSelected;

    private RecipeItemStatus craftStatus;

    public Craft()
    {
        _recipesLoader = new RecipesLoader();
        tempRecipes = null;
        itemsSelected = new List<IngredientData>();
        craftStatus = RecipeItemStatus.Waiting;
    }

    public bool AddItemAndCheckRecipe(Slot slot)
    {
        IngredientData newIng = new IngredientData(slot);
        if (itemsSelected.Contains(newIng)) return true;
        itemsSelected.Add(newIng);
        if (tempRecipes == null)
        {
            tempRecipes = _recipesLoader.GetRecipesWithItem(
                slot.GetItemId(), slot.GetAmount());
        }
        else
        {
            RemoveRecipes(slot.GetItemId(), RecipeItemStatus.Without);
        }
        if (tempRecipes.Count != 0) return true;
        ClearCraft();
        return false;
    }

    public void RemoveItem(Slot slot)
    {
        int index = -1;
        for (int i = 0; i < itemsSelected.Count; i++)
        {
            if (itemsSelected[i].itemId == slot.GetItemId())
            {
                index = i;
                return;
            }
        }
        if (index >= 0)
        {
            RemoveRecipes(slot.GetItemId(), RecipeItemStatus.With);
            itemsSelected.RemoveAt(index);
        }
    }

    public bool CanCraft()
    {
        if (tempRecipes == null) return false;
        if (tempRecipes.Count == 1
            && tempRecipes[0].CompareWith(itemsSelected)) return true;
        return false;
    }

    private void RemoveRecipes(string itemId, RecipeItemStatus condition)
    {
        var recipeToDelete = new List<Recipe>();
        foreach (var recipe in tempRecipes)
        {
            if (!recipe.HasItem(itemId))
            {
                if (condition == RecipeItemStatus.Without)
                {
                    recipeToDelete.Add(recipe);
                }
            }
            else
            {
                if (condition == RecipeItemStatus.With)
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
            bool status = inventory.TryRemoveItem(ingData.itemId,
                recipe.GetIngCount(ingData.itemId));
        }
        inventory.AddItem(recipe.itemId, recipe.amount);
        craftStatus = RecipeItemStatus.Waiting;
        ClearCraft();
        return true;
    }

    public void Print()
    {
        Debug.Log("Выбранное");
        for (int i = 0; i < itemsSelected.Count; i++)
        {
            Debug.Log(itemsSelected[i].itemId);
        }
    }
}