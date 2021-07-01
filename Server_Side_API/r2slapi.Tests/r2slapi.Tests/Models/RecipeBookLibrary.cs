using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace r2slapi.Tests.Models
{
    public class RecipeBookLibrary
    {
        private List<string> allMeasurementUnits = new List<string>();

        public RecipeBookLibrary()
        {
            allMeasurementUnits.AddRange(MeasurementUnits.AllStandardMeasurementUnits());
        }

        public int? Id { get; set; }

        public List<RecipeBook> AllRecipeBooks { get; set; } = new List<RecipeBook>();

        public string[] AllMeasurementUnits
        {
            get
            {
                string[] allMeasurementUnitsArray = allMeasurementUnits.ToArray();
                return allMeasurementUnitsArray;
            }
        }

        public void AddRecipeBook(RecipeBook recipeBook)
        {
            AllRecipeBooks.Add(recipeBook);
        }

        public void DeleteRecipeBook(RecipeBook recipeBook)
        {
            AllRecipeBooks.Remove(recipeBook);
        }

        public void AddMeasurementUnit(string measurementUnit)
        {
            allMeasurementUnits.Add(measurementUnit);
        }

        public void DeleteMeasurementUnit(string measurementUnit)
        {
            allMeasurementUnits.Remove(measurementUnit);
        }

        public void EditMeasurementUnit(string measurementUnit, string newMeasurementUnit)
        {
            allMeasurementUnits[allMeasurementUnits.IndexOf(measurementUnit)] = newMeasurementUnit;
        }
    }
}
