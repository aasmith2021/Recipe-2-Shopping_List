using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class Ingredient
    {
        public Ingredient(string quantity, string measurementUnit, string name)
        {
            this.Quantity = quantity;
            this.MeasurementUnit = measurementUnit;
            this.Name = name;
        }

        public string Quantity { get; set; }

        public string MeasurementUnit { get; set; }

        public string Name { get; set; }
    }
}
