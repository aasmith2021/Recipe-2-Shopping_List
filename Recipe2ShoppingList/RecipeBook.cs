using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class RecipeBook
    {
        private List<Recipe> recipes = new List<Recipe>();

        public Recipe[] Recipes
        {
            get
            {
                Recipe[] allRecipes = recipes.ToArray();
                return allRecipes;
            }
        }

        public void AddRecipe(Recipe newRecipe)
        {
            recipes.Add(newRecipe);
        }

        public void DeleteRecipe(Recipe recipeToDelete)
        {
            recipes.Remove(recipeToDelete);
        }

    }
}
