using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    public class FileIO : IDataIO
    {
         public RecipeBookLibrary GetRecipeBookLibraryFromDataSource(string alternateFilePath = "")
         {
            RecipeBookLibrary recipeBookLibrary = new RecipeBookLibrary();
            string allTextFromFile = GetAllDatabaseText(alternateFilePath);

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

        private string GetRecipeBookNameFromData(string recipeBookText)
        {
            string regexExpression = @"RECIPE_BOOK_NAME:(.*?)(-START_OF_RECIPE-|-END_OF_RECIPE_BOOK-)";

            string recipeBookName = Regex.Match(recipeBookText, regexExpression).Groups[1].Value.ToString();

            return recipeBookName;
        }

        private string[] GetCustomMeasurementUnitsFromText(string allTextFromFile)
        {
            string startMaker = "-START_OF_MEASUREMENT_UNITS-";
            string endMaker = "-END_OF_MEASUREMENT_UNITS-";
            string allMeasurementUnitsText = GetDataFromStartAndEndMarkers(allTextFromFile, startMaker, endMaker);

            string[] splitMeasurementUnits = allMeasurementUnitsText.Split("MU:", StringSplitOptions.RemoveEmptyEntries);

            return splitMeasurementUnits;
        }

        public void WriteRecipeBookLibraryToDataSource(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, string alternateFilePath = "")
        {
            string[] allMeasurementUnits = recipeBookLibrary.AllMeasurementUnits;
            RecipeBook[] allRecipeBooks = recipeBookLibrary.AllRecipeBooks.ToArray();

            try
            {
                using (StreamWriter sw = new StreamWriter(GetWriteDatabaseFilePath(alternateFilePath)))
                {
                    sw.WriteLine(MeasurementUnits.ProduceMeasurementUnitsText(recipeBookLibrary));

                    foreach (RecipeBook recipeBook in allRecipeBooks)
                    {
                        sw.WriteLine(recipeBook.ProduceRecipeBookText(false));
                    }
                }
            }
            catch (IOException exception)
            {
                UserInterface.DisplayInformation(userIO, "Cannot open Recipe Database file to save data.");
                UserInterface.DisplayInformation(userIO, "Press \"Enter\" to continue...", false);
                GetUserInput.GetEnterFromUser(userIO);
            }

            //Delete original database, then rename the "write" database file to become the new master database file
            DeleteOldFileAndRenameNewFile(GetReadDatabaseFilePath(), GetWriteDatabaseFilePath());
        }

        public void WriteShoppingListToDataSource(IUserIO userIO, ShoppingList shoppingList, string alternateFilePath = "")
        {
            string entireShoppingList = shoppingList.GetEntireShoppingList(true);

            try
            {
                using (StreamWriter sw = new StreamWriter(GetWriteShoppingListFilePath(alternateFilePath)))
                {
                    sw.WriteLine(entireShoppingList);
                }
            }
            catch (IOException exception)
            {
                UserInterface.DisplayInformation(userIO, "Cannot open Shopping List file to save data.");
                UserInterface.DisplayInformation(userIO, "Press \"Enter\" to continue...", false);
                GetUserInput.GetEnterFromUser(userIO);
            }

            //Delete original Shopping List file, then rename the "write" Shopping List file to become the new master Shopping List
            DeleteOldFileAndRenameNewFile(GetReadShoppingListFilePath(), GetWriteShoppingListFilePath());
        }

        private string GetReadDatabaseFilePath(string alternateFilePath = "")
        {
            string filePath = Directory.GetCurrentDirectory();

            if (alternateFilePath == "")
            {
                filePath = Path.Combine(filePath, "Recipe_Database.txt");
            }
            else
            {
                filePath = Path.Combine($"{alternateFilePath}.txt");
            }

            return filePath;
        }

        private string GetWriteDatabaseFilePath(string alternateFilePath = "")
        {
            string filePath = Directory.GetCurrentDirectory();

            if (alternateFilePath == "")
            {
                filePath = Path.Combine(filePath, "Recipe_Database_write.txt");
            }
            else
            {
                filePath = Path.Combine(filePath, $"{alternateFilePath}.txt");
            }

            return filePath;
        }

        private string GetReadShoppingListFilePath(string alternateFilePath = "")
        {
            string filePath = Directory.GetCurrentDirectory();

            if (alternateFilePath == "")
            {
                filePath = Path.Combine(filePath, "Shopping_List.txt");
            }
            else
            {
                filePath = Path.Combine(filePath, $"{alternateFilePath}.txt");
            }

            return filePath;
        }

        private string GetWriteShoppingListFilePath(string alternateFilePath = "")
        {
            string filePath = Directory.GetCurrentDirectory();

            if (alternateFilePath == "")
            {
                filePath = Path.Combine(filePath, "Shopping_List_write.txt");
            }
            else
            {
                filePath = Path.Combine(filePath, $"{alternateFilePath}.txt");
            }

            return filePath;
        }

        private string GetAllDatabaseText(string alternateFilePath = "")
        {
            string databaseText = "";
            string currentLineOfText = "";

            //Try-Catch the exception for starting the program when the database file doesn't
            //exist yet.
            try
            {
                using (StreamReader sr = new StreamReader(GetReadDatabaseFilePath(alternateFilePath)))
                {

                    while (!sr.EndOfStream)
                    {
                        currentLineOfText = sr.ReadLine();
                        databaseText += currentLineOfText;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
            }

            return databaseText;
        }

        private void DeleteOldFileAndRenameNewFile(string oldFilePath, string newFilePath)
        {
            File.Delete(oldFilePath);
            File.Move(newFilePath, oldFilePath);
        }

        public static string GetDataFromStartAndEndMarkers(string data, string startMarker, string endMarker)
        {
            int startIndexOfReturnData = data.IndexOf(startMarker) + startMarker.Length;
            int startIndexOfEndMarker = data.IndexOf(endMarker);
            int lengthOfReturnData = startIndexOfEndMarker - startIndexOfReturnData;

            string returnData = "";

            if (lengthOfReturnData >= 0)
            {
                returnData = data.Substring(startIndexOfReturnData, lengthOfReturnData);
            }

            return returnData;
        }
    }
}
