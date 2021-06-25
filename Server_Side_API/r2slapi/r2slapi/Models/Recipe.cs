using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace r2slapi.Models
{
    public class Recipe
    {
        public Recipe()
        {

        }
        
        public Recipe(Metadata metadata, CookingInstructions cookingInstructions, IngredientList ingredientList)
        {
            this.Metadata = metadata;
            this.CookingInstructions = cookingInstructions;
            this.IngredientList = ingredientList;
        }

        public int Id { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Recipe Number cannot be blank.")]
        public int? RecipeNumber { get; set; } = 0;

        public Metadata Metadata { get; set; } = new Metadata();

        public CookingInstructions CookingInstructions { get; set; } = new CookingInstructions();

        public IngredientList IngredientList { get; set; } = new IngredientList();
    }
}
