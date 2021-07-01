using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace r2slapi.Tests.Models
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

        [Range(.001, 1000, ErrorMessage = "Ingredient Quantity must be between .001 - 1,000")]
        public double Quantity { get; set; }

        [StringLength(30, MinimumLength = 0, ErrorMessage = "Ingredient Measurement Unit cannot exceed 30 characters.")]
        public string MeasurementUnit { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Ingredient Name cannot be blank, and cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(120, MinimumLength = 0, ErrorMessage = "Ingredient Preparation Notes cannot be null, and cannot exceed 120 characters.")]
        public string PreparationNote { get; set; }

        [StringLength(30, MinimumLength = 0, ErrorMessage = "Ingredient Store Location cannot be null, and cannot exceed 30 characters.")]
        public string StoreLocation { get; set; }
    }
}
