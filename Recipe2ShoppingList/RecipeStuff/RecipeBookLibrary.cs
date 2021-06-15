using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recipe2ShoppingList
{
    public class RecipeBookLibrary
    {
        private List<RecipeBook> allRecipeBooksList = new List<RecipeBook>();
        private List<string> allMeasurementUnits = new List<string>();

        public RecipeBookLibrary()
        {
            allMeasurementUnits.AddRange(MeasurementUnits.AllStandardMeasurementUnits());
        }
        
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

        public void WriteRecipeBookLibraryToFile(string alternateFilePath = "")
        {
            string[] allMeasurementUnits = this.AllMeasurementUnits;
            RecipeBook[] allRecipeBooks = this.AllRecipeBooks;

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
                Console.WriteLine("Cannot open Recipe Database file to save data.");
                Console.WriteLine();
                Console.WriteLine("Press \"Enter\" to continue...");
                Console.ReadLine();
            }
        }
    }
}
