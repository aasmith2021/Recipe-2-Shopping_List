using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace r2slapi.Models
{
    public class RecipeBookLibrary
    {
        private List<RecipeBook> allRecipeBooksList = new List<RecipeBook>();
        private List<string> allMeasurementUnits = new List<string>();
        
        public RecipeBookLibrary()
        {
            allMeasurementUnits.AddRange(MeasurementUnits.AllStandardMeasurementUnits());
        }

        public int Id { get; set; }

        public RecipeBook[] AllRecipeBooks
        {
            get
            {
                RecipeBook[] allRecipeBooksArray = allRecipeBooksList.ToArray();
                return allRecipeBooksArray;
            }
        }

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
            allRecipeBooksList.Add(recipeBook);
        }

        public void DeleteRecipeBook(RecipeBook recipeBook)
        {
            allRecipeBooksList.Remove(recipeBook);
        }

        public void AddMeasurementUnit(string measurementUnit)
        {
            allMeasurementUnits.Add(measurementUnit);
        }

        public void DeleteMeasurementUnit(string measurementUnit)
        {
            allMeasurementUnits.Remove(measurementUnit);
        }

        public void EditMeasurementUnit (string measurementUnit, string newMeasurementUnit)
        {
            allMeasurementUnits[allMeasurementUnits.IndexOf(measurementUnit)] = newMeasurementUnit;
        }
    }
}
