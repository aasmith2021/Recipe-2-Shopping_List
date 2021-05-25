using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class Recipe
    {
        public Recipe(Metadata metadata, CookingInstructions cookingInstructions, Ingredients ingredients)
        {
            this.Metadata = metadata;
            this.CookingInstructions = cookingInstructions;
            this.Ingredients = ingredients;
        }

        public int RecipeId { get; set; }

        public Metadata Metadata { get; set; }

        public CookingInstructions CookingInstructions { get; set; }

        public Ingredients Ingredients { get; set; }

        public string ProduceRecipeText()
        {
            string recipeText = "";
            recipeText += this.Metadata.ProduceMetadataText();
            recipeText += this.Ingredients.ProduceIngredientsText();
            recipeText += this.CookingInstructions.ProduceInstructionsText();

            return recipeText;
        }
    }
}
