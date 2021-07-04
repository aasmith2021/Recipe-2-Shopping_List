using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class IngredientList
    {
        public int Id { get; set; }

        public List<Ingredient> AllIngredients { get; set; } = new List<Ingredient>();

        public void AddIngredient(Ingredient newIngredient)
        {
            AllIngredients.Add(newIngredient);
        }

        public void DeleteIngredient(Ingredient ingredientToDelete)
        {
            AllIngredients.Remove(ingredientToDelete);
        }

        public string ProduceIngredientsText(bool printVersion, bool includeLineNumbers = false)
        {
            string ingredientsText = "";

            if (printVersion)
            {
                ingredientsText += $"INGREDIENTS:{Environment.NewLine}";

                for (int i = 0; i < this.AllIngredients.Count; i++)
                {
                    string ingredientLine = "";

                    if (includeLineNumbers)
                    {
                        ingredientLine += $"{i + 1}. ";
                    }
                        
                    ingredientLine += $"{this.AllIngredients[i].Quantity}";

                    if (this.AllIngredients[i].MeasurementUnit != "")
                    {
                        ingredientLine += $" {this.AllIngredients[i].MeasurementUnit}";
                    }

                    ingredientLine += $" {this.AllIngredients[i].Name}";

                    if (this.AllIngredients[i].PreparationNote != "")
                    {
                        ingredientLine += $", {this.AllIngredients[i].PreparationNote}";
                    }

                    ingredientsText += UserInterface.MakeStringConsoleLengthLines($"{ingredientLine}");

                    ingredientsText += $"{Environment.NewLine}";

                    if (i == this.AllIngredients.Count - 1)
                    {
                        ingredientsText += $"{Environment.NewLine}";
                    }
                }
            }
            else
            {
                ingredientsText += $"-START_OF_INGREDIENTS-{Environment.NewLine}";
                
                foreach (Ingredient ingredient in this.AllIngredients)
                {
                    ingredientsText += $"INGREDIENT_NAME:{ingredient.Name}{Environment.NewLine}";
                    ingredientsText += $"QTY:{ingredient.Quantity}{Environment.NewLine}";

                    if (ingredient.MeasurementUnit == "")
                    {
                        ingredientsText += $"UNIT:NONE{Environment.NewLine}";
                    }
                    else
                    {
                        ingredientsText += $"UNIT:{ingredient.MeasurementUnit}{Environment.NewLine}";
                    }

                    if (ingredient.PreparationNote == "")
                    {
                        ingredientsText += $"PREP_NOTE:NONE{Environment.NewLine}";
                    }
                    else
                    {
                        ingredientsText += $"PREP_NOTE:{ingredient.PreparationNote}{Environment.NewLine}";
                    }

                    if (ingredient.StoreLocation == "")
                    {
                        ingredientsText += $"STORE_LOC:NONE{Environment.NewLine}";
                    }
                    else
                    {
                        ingredientsText += $"STORE_LOC:{ingredient.StoreLocation}{Environment.NewLine}";
                    }
                }
            }

            return ingredientsText;
        }
    }
}
