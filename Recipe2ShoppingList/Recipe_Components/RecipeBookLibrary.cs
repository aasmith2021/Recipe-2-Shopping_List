using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recipe2ShoppingList
{
    public class RecipeBookLibrary
    {
        public RecipeBookLibrary()
        {
            AllMeasurementUnits.AddRange(MeasurementUnits.AllStandardMeasurementUnits());
        }

        public int? Id { get; set; }

        public DateTime LastSaved { get; set; }

        public List<RecipeBook> AllRecipeBooks { get; set; } = new List<RecipeBook>();

        public List<string> AllMeasurementUnits { get; set; } = new List<string>();

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
            AllMeasurementUnits.Add(measurementUnit);
        }

        public void DeleteMeasurementUnit(string measurementUnit)
        {
            AllMeasurementUnits.Remove(measurementUnit);
        }

        public void EditMeasurementUnit(string measurementUnit, string newMeasurementUnit)
        {
            AllMeasurementUnits[AllMeasurementUnits.IndexOf(measurementUnit)] = newMeasurementUnit;
        }

        public string ProduceLastSavedAndIdText()
        {
            string lastSavedAndIdText = $"LAST_SAVED:{this.LastSaved}{Environment.NewLine}";
            lastSavedAndIdText += $"RECIPE_BOOK_LIBRARY_ID:{this.Id}";

            return lastSavedAndIdText;
        }
    }
}
