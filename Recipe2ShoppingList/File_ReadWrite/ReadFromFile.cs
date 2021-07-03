using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    public class ReadFromFile
    {
         public static RecipeBookLibrary GetRecipeBookLibraryFromFile(string alternateFilePath = "")
        {
            RecipeBookLibrary recipeBookLibrary = new RecipeBookLibrary();
            string allTextFromFile = FileDataHelperMethods.GetAllDatabaseText(alternateFilePath);

            if (allTextFromFile == "")
            {
                return recipeBookLibrary;
            }

            string[] allCustomMeasurementUnits = GetCustomMeasurementUnitsFromText(allTextFromFile);
            for (int i = 0; i < allCustomMeasurementUnits.Length; i++)
            {
                recipeBookLibrary.AddMeasurementUnit(allCustomMeasurementUnits[i]);
            }

            string allRecipeBooksTextFromFile = allTextFromFile.Split("-END_OF_MEASUREMENT_UNITS-", StringSplitOptions.RemoveEmptyEntries)[1];

            //Only add recipe books if the database file has data in it
            if (allTextFromFile != "")
            {
                string[] separateRecipeBooks = allRecipeBooksTextFromFile.Split("-NEW_RECIPE_BOOK-", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < separateRecipeBooks.Length; i++)
                {
                    RecipeBook newRecipeBookToAdd = new RecipeBook();
                    string recipeBookText = separateRecipeBooks[i];

                    newRecipeBookToAdd.Name = GetRecipeBookNameFromData(recipeBookText);

                    newRecipeBookToAdd.AddAllRecipesToRecipeBook(recipeBookText);

                    recipeBookLibrary.AddRecipeBook(newRecipeBookToAdd);
                }
            }
            
            return recipeBookLibrary;
        }

        private static string GetRecipeBookNameFromData(string recipeBookText)
        {
            string regexExpression = @"RECIPE_BOOK_NAME:(.*?)(-START_OF_RECIPE-|-END_OF_RECIPE_BOOK-)";

            string recipeBookName = Regex.Match(recipeBookText, regexExpression).Groups[1].Value.ToString();

            return recipeBookName;
        }

        private static string[] GetCustomMeasurementUnitsFromText(string allTextFromFile)
        {
            string startMaker = "-START_OF_MEASUREMENT_UNITS-";
            string endMaker = "-END_OF_MEASUREMENT_UNITS-";
            string allMeasurementUnitsText = FileDataHelperMethods.GetDataFromStartAndEndMarkers(allTextFromFile, startMaker, endMaker);

            string[] splitMeasurementUnits = allMeasurementUnitsText.Split("MU:", StringSplitOptions.RemoveEmptyEntries);

            return splitMeasurementUnits;
        }
    }
}
