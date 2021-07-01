using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recipe2ShoppingList
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

        public void EditMeasurementUnit (string measurementUnit, string newMeasurementUnit)
        {
            allMeasurementUnits[allMeasurementUnits.IndexOf(measurementUnit)] = newMeasurementUnit;
        }

        public void WriteRecipeBookLibraryToFile(IUserIO userIO, string alternateFilePath = "")
        {
            string[] allMeasurementUnits = this.AllMeasurementUnits;
            RecipeBook[] allRecipeBooks = this.AllRecipeBooks.ToArray();

            try
            {
                using (StreamWriter sw = new StreamWriter(DataHelperMethods.GetWriteDatabaseFilePath(alternateFilePath)))
                {
                    sw.WriteLine(MeasurementUnits.ProduceMeasurementUnitsText(this));

                    foreach (RecipeBook recipeBook in allRecipeBooks)
                    {
                        sw.WriteLine(recipeBook.ProduceRecipeBookText(false));
                    }
                }
            }
            catch (IOException exception)
            {
                userIO.DisplayData("Cannot open Recipe Database file to save data.");
                userIO.DisplayData();
                userIO.DisplayData("Press \"Enter\" to continue...");
                userIO.GetInput();
            }
        }
    }
}
