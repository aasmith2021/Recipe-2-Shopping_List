using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace r2slapi.Models
{
    public class RecipeBook
    {
        public RecipeBook()
        {

        }
        
        public RecipeBook(string name = "")
        {
            this.Name = name;
        }

        public int Id { get; set; }

        [StringLength(120, MinimumLength = 1, ErrorMessage = "Recipe Book name cannot be blank, and cannot exceed 120 characters.")]
        public string Name { get; set; }

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();

        public void AddRecipe(Recipe newRecipe)
        {
            if (newRecipe.RecipeNumber == 0)
            {
                newRecipe.RecipeNumber = this.Recipes.Count + 1;
            }

            Recipes.Add(newRecipe);
        }

        public void DeleteRecipe(Recipe recipeToDelete)
        {
            Recipes.Remove(recipeToDelete);
        }
    }
}
