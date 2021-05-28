using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class IngredientsList
    {
        private List<Ingredient> allIngredients = new List<Ingredient>();

        public Ingredient[] AllIngredients
        {
            get { return this.allIngredients.ToArray(); }
        }

        public void AddIngredient(Ingredient newIngredient)
        {
            allIngredients.Add(newIngredient);
        }

        public void DeleteIngredient(Ingredient ingredientToDelete)
        {
            allIngredients.Remove(ingredientToDelete);
        }

        public string ProduceIngredientsText(bool printVersion)
        {
            string ingredientsText = "";

            if (printVersion)
            {
                ingredientsText += $"INGREDIENTS:{Environment.NewLine}";

                foreach (Ingredient ingredient in this.AllIngredients)
                {
                    ingredientsText += $"{ingredient.Quantity} {ingredient.MeasurementUnit} {ingredient.Name}{Environment.NewLine}";
                }

                ingredientsText += $"{Environment.NewLine}";
            }
            else
            {
                ingredientsText += $"-START_OF_INGREDIENTS-{Environment.NewLine}";
                
                foreach (Ingredient ingredient in this.AllIngredients)
                {
                    ingredientsText += $"INGREDIENT_NAME:{ingredient.Name}{Environment.NewLine}";
                    ingredientsText += $"QTY:{ingredient.Quantity}{Environment.NewLine}";
                    ingredientsText += $"UNIT:{ingredient.MeasurementUnit}{Environment.NewLine}";
                }
            }

            return ingredientsText;
        }
    }
}
