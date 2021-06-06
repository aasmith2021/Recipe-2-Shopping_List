using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class Ingredient
    {
        public Ingredient(double quantity, string measurementUnit, string name, string preparationNote = "")
        {
            this.Quantity = quantity;
            this.MeasurementUnit = measurementUnit;
            this.Name = name;
            this.PreparationNote = preparationNote;
        }

        public double Quantity { get; set; }

        public string MeasurementUnit { get; set; }

        public string Name { get; set; }

        public string PreparationNote { get; set; }
    }
}
