using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class RecipeBook
    {
        private List<Recipe> masterRecipeBook = new List<Recipe>();

        public Recipe[] GetAllRecipes()
        {
            Recipe[] allRecipes = masterRecipeBook.ToArray();
            return allRecipes;
        }

        public void AddRecipe(Recipe newRecipe)
        {
            masterRecipeBook.Add(newRecipe);
        }

        public void DeleteRecipe(Recipe recipeToDelete)
        {
            masterRecipeBook.Remove(recipeToDelete);
        }

    }
}
