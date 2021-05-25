using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class RecipeBook
    {
        public RecipeBook(string name = "")
        {
            this.Name = name;
        }
        
        public string Name { get; set; }

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
            newRecipe.RecipeId = this.Recipes.Length + 1;
            recipes.Add(newRecipe);
        }

        public void DeleteRecipe(Recipe recipeToDelete)
        {
            recipes.Remove(recipeToDelete);
        }

        public string ProduceRecipeBookText()
        {
            string recipeBookText = "";

            recipeBookText += $"RECIPE BOOK NAME: {this.Name}{Environment.NewLine}{Environment.NewLine}";
            
            for (int i = 0; i < this.Recipes.Length; i++)
            {
                recipeBookText += $"Recipe #{this.Recipes[i].RecipeId}{Environment.NewLine}";
                recipeBookText += this.Recipes[i].ProduceRecipeText();
                
                if (i != this.Recipes.Length - 1)
                {
                    recipeBookText += $"{Environment.NewLine}{Environment.NewLine}";
                }
            }

            return recipeBookText;
        }
    }
}
