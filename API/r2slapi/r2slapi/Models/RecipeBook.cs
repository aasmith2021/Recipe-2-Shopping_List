using System;
using System.Collections.Generic;
using System.Text;

namespace r2slapi.Models
{
    public class RecipeBook
    {
        private List<Recipe> recipes = new List<Recipe>();

        public RecipeBook()
        {

        }
        
        public RecipeBook(string name = "")
        {
            this.Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

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
            if (newRecipe.RecipeNumber == 0)
            {
                newRecipe.RecipeNumber = this.Recipes.Length + 1;
            }

            recipes.Add(newRecipe);
        }

        public void DeleteRecipe(Recipe recipeToDelete)
        {
            recipes.Remove(recipeToDelete);
        }
    }
}
