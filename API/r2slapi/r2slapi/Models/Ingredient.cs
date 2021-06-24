using System;
using System.Collections.Generic;
using System.Text;

namespace r2slapi.Models
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
    }
}
