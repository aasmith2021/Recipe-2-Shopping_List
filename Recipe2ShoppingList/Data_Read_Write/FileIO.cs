using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    //This FileIO is used to read and write program data to a local file in the event a user does not want to
    //use the API to load and save program data with a database.
    //This FileIO is also used as a way to save a backup copy of the program data if the API server is down,
    //and it also saves a copy of the program's shopping list to a local file.
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

            recipeBookLibrary.LastSaved = GetLastSavedFromText(allTextFromFile);
            recipeBookLibrary.Id = GetRecipeBookLibraryIdFromText(allTextFromFile);

            string[] allCustomMeasurementUnits = GetCustomMeasurementUnitsFromText(allTextFromFile);
            for (int i = 0; i < allCustomMeasurementUnits.Length; i++)
            {
                recipeBookLibrary.AddMeasurementUnit(allCustomMeasurementUnits[i]);
            }

            string[] allPossibleRecipeBooksTextFromFile = allTextFromFile.Split("-END_OF_MEASUREMENT_UNITS-", StringSplitOptions.RemoveEmptyEntries);
            string allRecipeBooksTextFromFile = "";

            if (allPossibleRecipeBooksTextFromFile.Length > 1)
            {
                allRecipeBooksTextFromFile = allPossibleRecipeBooksTextFromFile[1];
            }

            //Only add recipe books if the database file has recipe book data in it
            if (allRecipeBooksTextFromFile != "")
            {
                string[] separateRecipeBooks = allRecipeBooksTextFromFile.Split("-NEW_RECIPE_BOOK-", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < separateRecipeBooks.Length; i++)
                {
                    RecipeBook newRecipeBookToAdd = new RecipeBook();
                    string recipeBookText = separateRecipeBooks[i];

                    newRecipeBookToAdd.Id = GetRecipeBookIdFromData(recipeBookText);
                    newRecipeBookToAdd.Name = GetRecipeBookNameFromData(recipeBookText);

                    newRecipeBookToAdd.AddAllRecipesToRecipeBook(recipeBookText);

                    recipeBookLibrary.AddRecipeBook(newRecipeBookToAdd);
                }
            }

            return recipeBookLibrary;
        }

        private int GetRecipeBookIdFromData(string recipeBookText)
        {
            string regexExpression = @"RECIPE_BOOK_ID:(.*?)RECIPE_BOOK_NAME:";

            int recipeBookId = Convert.ToInt32(Regex.Match(recipeBookText, regexExpression).Groups[1].Value.ToString());

            return recipeBookId;
        }

        private string GetRecipeBookNameFromData(string recipeBookText)
        {
            string regexExpression = @"RECIPE_BOOK_NAME:(.*?)(-START_OF_RECIPE-|-END_OF_RECIPE_BOOK-)";

            string recipeBookName = Regex.Match(recipeBookText, regexExpression).Groups[1].Value.ToString();

            return recipeBookName;
        }

        private DateTime GetLastSavedFromText(string allTextFromFile)
        {
            string startMaker = "LAST_SAVED:";
            string endMaker = "RECIPE_BOOK_LIBRARY_ID:";
            string lastSavedText = GetDataFromStartAndEndMarkers(allTextFromFile, startMaker, endMaker);
            DateTime lastSaved = Convert.ToDateTime("1800-01-01");

            try
            {
                lastSaved = Convert.ToDateTime(lastSavedText);
            }
            catch (Exception)
            {
            }

            return lastSaved;
        }

        private int GetRecipeBookLibraryIdFromText(string allTextFromFile)
        {
            string startMaker = "RECIPE_BOOK_LIBRARY_ID:";
            string endMaker = "-START_OF_MEASUREMENT_UNITS-";
            string idText = GetDataFromStartAndEndMarkers(allTextFromFile, startMaker, endMaker);
            int id = Convert.ToInt32(idText);

            return id;
        }

        private string[] GetCustomMeasurementUnitsFromText(string allTextFromFile)
        {
            string startMaker = "-START_OF_MEASUREMENT_UNITS-";
            string endMaker = "-END_OF_MEASUREMENT_UNITS-";
            string allMeasurementUnitsText = GetDataFromStartAndEndMarkers(allTextFromFile, startMaker, endMaker);

            string[] splitMeasurementUnits = allMeasurementUnitsText.Split("MU:", StringSplitOptions.RemoveEmptyEntries);

            return splitMeasurementUnits;
        }

        public bool WriteRecipeBookLibraryToDataSource(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, string alternateFilePath = "")
        {
            bool writeSuccessful = false;
            RecipeBook[] allRecipeBooks = recipeBookLibrary.AllRecipeBooks.ToArray();

            recipeBookLibrary.LastSaved = DateTime.Now;

            try
            {
                using (StreamWriter sw = new StreamWriter(GetWriteDatabaseFilePath()))
                {
                    sw.WriteLine(recipeBookLibrary.ProduceLastSavedAndIdText());
                    sw.WriteLine(MeasurementUnits.ProduceMeasurementUnitsText(recipeBookLibrary));

                    foreach (RecipeBook recipeBook in allRecipeBooks)
                    {
                        sw.WriteLine(recipeBook.ProduceRecipeBookText(false));
                    }
                }
            }
            catch (IOException exception)
            {
                UserInterface.DisplayInformation(userIO, "Error: Unable to write Recipe Database to a file. Unfortunately, all current changes will be lost.");
                UserInterface.DisplayInformation(userIO, "Press \"Enter\" to continue...", false);
                GetUserInput.GetEnterFromUser(userIO);
            }

            //Delete original database file, then rename the "write" database file to become the new master database file
            DeleteOldFileAndRenameNewFile(GetReadDatabaseFilePath(alternateFilePath), GetWriteDatabaseFilePath());

            writeSuccessful = true;

            return writeSuccessful;
        }

        public bool WriteShoppingListToDataSource(IUserIO userIO, ShoppingList shoppingList, string alternateFilePath = "")
        {
            bool writeSuccessful = false;
            string entireShoppingList = shoppingList.GetEntireShoppingList(true);

            try
            {
                using (StreamWriter sw = new StreamWriter(GetWriteShoppingListFilePath()))
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
            DeleteOldFileAndRenameNewFile(GetReadShoppingListFilePath(alternateFilePath), GetWriteShoppingListFilePath());

            writeSuccessful = true;

            return writeSuccessful;
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

        public string GetAllDatabaseText(string alternateFilePath = "")
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
