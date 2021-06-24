using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class Ingredient
    {
        public Ingredient()
        {

        }
        
        public Ingredient(double quantity, string measurementUnit, string name, string preparationNote = "", string storeLocation = "")
        {
            this.Quantity = quantity;
            this.MeasurementUnit = measurementUnit;
            this.Name = name;
            this.PreparationNote = preparationNote;
            this.StoreLocation = storeLocation;
        }

        public int Id { get; set; }

        public double Quantity { get; set; }

        public string MeasurementUnit { get; set; }

        public string Name { get; set; }

        public string PreparationNote { get; set; }

        public string StoreLocation { get; set; }

        public Ingredient CombineIngredientsForShoppingList(Ingredient ingredient)
        {
            Ingredient combinedIngredient = new Ingredient(0, "", this.Name);

            bool thisIngredientHasVolumeMeasurementUnit = MeasurementUnits.IngredientHasVolumeMeasurementUnit(this);
            bool newIngredientHasVolumeMeasurementUnit = MeasurementUnits.IngredientHasVolumeMeasurementUnit(ingredient);

            Tuple<double, string> combinedQuantityAndMeasurementUnit = MeasurementUnits.CombineMeasurementUnits(this.Quantity, this.MeasurementUnit, thisIngredientHasVolumeMeasurementUnit, ingredient.Quantity, ingredient.MeasurementUnit, newIngredientHasVolumeMeasurementUnit);

            combinedIngredient.Quantity = combinedQuantityAndMeasurementUnit.Item1;
            combinedIngredient.MeasurementUnit = combinedQuantityAndMeasurementUnit.Item2;

            return combinedIngredient;
        }

    }
}
