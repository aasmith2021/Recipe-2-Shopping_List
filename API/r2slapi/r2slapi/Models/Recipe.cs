using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace r2slapi.Models
{
    public class Recipe
    {
        public Recipe()
        {

        }
        
        public Recipe(Metadata metadata, CookingInstructions cookingInstructions, IngredientList ingredients)
        {
            this.Metadata = metadata;
            this.CookingInstructions = cookingInstructions;
            this.Ingredients = ingredients;
        }

        public int Id { get; set; }

        public int RecipeNumber { get; set; } = 0;

        public Metadata Metadata { get; set; } = new Metadata();

        public CookingInstructions CookingInstructions { get; set; } = new CookingInstructions();

        public IngredientList Ingredients { get; set; } = new IngredientList();
    }
}
