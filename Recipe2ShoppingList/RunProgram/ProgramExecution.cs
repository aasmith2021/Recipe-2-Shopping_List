using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    public class ProgramExecution
    {
        public static void RunProgram(IUserIO userIO, out bool exitProgram)
        {
            exitProgram = false;

            //Load <recipeBookLibrary> into the program from the database file
            RecipeBookLibrary recipeBookLibrary = ReadFromFile.GetRecipeBookLibraryFromFile();

            ShoppingList shoppingList = new ShoppingList();

            while (!exitProgram)
            {
                RunMainMenu(userIO, recipeBookLibrary, shoppingList, out exitProgram);
            }

            //Save <recipeBookLibrary> to the "write" database file before closing program
            recipeBookLibrary.WriteRecipeBookLibraryToFile(userIO);

            //Delete original database, then rename the "write" database file to become the new master database file
            DataHelperMethods.DeleteOldFileAndRenameNewFile(DataHelperMethods.GetReadDatabaseFilePath(), DataHelperMethods.GetWriteDatabaseFilePath());

            //Save the Shopping List to the Shopping List file before closing the program
            shoppingList.WriteShoppingListToFile(userIO);

            //Delete original Shopping List file, then rename the "write" Shopping List file to become the new master Shopping List
            DataHelperMethods.DeleteOldFileAndRenameNewFile(DataHelperMethods.GetReadShoppingListFilePath(), DataHelperMethods.GetWriteShoppingListFilePath());
        }

        private static void RunMainMenu(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, ShoppingList shoppingList, out bool exitProgram)
        {
            exitProgram = false;

            UserInterface.DisplayMainMenu(userIO, recipeBookLibrary, out List<string> mainMenuOptions);
            string userOption = GetUserInput.GetUserOption(userIO, mainMenuOptions);

            if (int.TryParse(userOption, out int userOptionNumber))
            {
                bool exitRecipeBookSection = false;

                while (!exitRecipeBookSection)
                {
                    RunRecipeBook(userIO, recipeBookLibrary, shoppingList, userOptionNumber, out exitRecipeBookSection, out exitProgram);
                }
            }
            else
            {
                switch (userOption)
                {
                    case "A":
                        ManageRecipeBooks.AddNewRecipeBook(userIO, recipeBookLibrary);
                        break;
                    case "R":
                        ManageRecipeBooks.RenameRecipeBook(userIO, recipeBookLibrary);
                        break;
                    case "D":
                        ManageRecipeBooks.DeleteRecipeBook(userIO, recipeBookLibrary);
                        break;
                    case "V":
                        UserInterface.DisplayShoppingList(userIO, shoppingList);
                        break;
                    case "M":
                        RunManageSavedMeasurementUnits(userIO, recipeBookLibrary);
                        break;
                    case "X":
                        exitProgram = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void RunRecipeBook(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, ShoppingList shoppingList, int recipeBookOptionNumber, out bool exitRecipeBookSection, out bool exitProgram)
        {
            exitProgram = false;
            exitRecipeBookSection = false;

            RecipeBook recipeBook = recipeBookLibrary.AllRecipeBooks[recipeBookOptionNumber - 1];
            UserInterface.DisplayOpenRecipeBook(userIO, recipeBook, out List<string> recipeBookOptions);
            string userOption = GetUserInput.GetUserOption(userIO, recipeBookOptions);

            if (int.TryParse(userOption, out int userOptionNumber))
            {
                bool exitRecipeSection = false;

                while (!exitRecipeSection)
                {
                    RunRecipe(userIO, recipeBookLibrary, shoppingList, recipeBook, userOptionNumber, out exitRecipeSection, out exitRecipeBookSection, out exitProgram);
                }
            }
            else
            {
                switch (userOption)
                {
                    case "A":
                        ManageRecipes.AddNewRecipe(userIO, recipeBookLibrary, recipeBook);
                        break;
                    case "E":
                        ManageRecipes.EditExistingRecipe(userIO, recipeBookLibrary, recipeBook);
                        break;
                    case "D":
                        ManageRecipes.DeleteExistingRecipe(userIO, recipeBook);
                        break;
                    case "S":
                        ManageRecipes.AddExistingRecipeToShoppingList(userIO, recipeBook, shoppingList);
                        break;
                    case "V":
                        UserInterface.DisplayShoppingList(userIO, shoppingList);
                        break;
                    case "R":
                        exitRecipeBookSection = true;
                        break;
                    case "X":
                        exitRecipeBookSection = true;
                        exitProgram = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void RunRecipe(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, ShoppingList shoppingList, RecipeBook recipeBook, int recipeOptionNumber, out bool exitRecipeSection, out bool exitRecipeBookSection, out bool exitProgram)
        {
            exitProgram = false;
            exitRecipeSection = false;
            exitRecipeBookSection = false;

            Recipe recipe = recipeBook.Recipes[recipeOptionNumber - 1];
            UserInterface.DisplayOpenRecipe(userIO, recipe, recipeBook, out List<string> recipeEditOptions);
            string userOption = GetUserInput.GetUserOption(userIO, recipeEditOptions);

            switch (userOption)
            {
                case "S":
                    ManageRecipes.AddRecipeToShoppingList(userIO, shoppingList, recipe);
                    break;
                case "E":
                    RunEditRecipe(userIO, recipeBookLibrary, recipe);
                    break;
                case "D":
                    ManageRecipes.DeleteOpenRecipe(userIO, recipeBook, recipe);
                    exitRecipeSection = true;
                    break;
                case "V":
                    UserInterface.DisplayShoppingList(userIO, shoppingList);
                    break;
                case "R":
                    exitRecipeSection = true;
                    break;
                case "X":
                    exitRecipeSection = true;
                    exitRecipeBookSection = true;
                    exitProgram = true;
                    break;
                default:
                    break;
            }
        }

        private static void RunManageSavedMeasurementUnits(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            bool exitMeasurementUnits = false;

            do
            {
                UserInterface.DisplayMenuHeader(userIO, "---------- MANAGE SAVED MEASUREMENT UNITS ----------");

                int allStandardMeasurementUnitsLength = MeasurementUnits.AllStandardMeasurementUnits().Count;
                string[] allMeasurementUnits = recipeBookLibrary.AllMeasurementUnits;

                List<string> userAddedMeasurementUnits = new List<string>();
                userAddedMeasurementUnits.AddRange(allMeasurementUnits);
                userAddedMeasurementUnits.RemoveRange(0, allStandardMeasurementUnitsLength);

                List<string[]> editOptions = new List<string[]>()
                {
                    new string[] { "A", "Add New Measurement Unit"},
                    new string[] { "E", "Edit Measurement Unit"},
                    new string[] { "D", "Delete Measurement Unit"},
                    new string[] { "R", "Return to Main Menu"},
                };
                List<string> options = new List<string>();

                if (userAddedMeasurementUnits.Count == 0)
                {
                    editOptions.RemoveAt(1);
                    editOptions.RemoveAt(1);
                }

                UserInterface.DisplayCurrentMeasurementUnits(userIO, userAddedMeasurementUnits, true);
                UserInterface.DisplayOptionsMenu(userIO, editOptions, out options);
                UserInterface.DisplayLitePrompt(userIO, "Select an editing option");

                string userOption = GetUserInput.GetUserOption(userIO, options);

                userIO.DisplayData();
                switch (userOption)
                {
                    case "A":
                        ManageMeasurementUnits.AddNewMeasurementUnit(userIO, recipeBookLibrary, userAddedMeasurementUnits);
                        break;
                    case "E":
                        ManageMeasurementUnits.EditExistingMeasurementUnit(userIO, recipeBookLibrary, userAddedMeasurementUnits);
                        break;
                    case "D":
                        ManageMeasurementUnits.DeleteExistingMeasurementUnit(userIO, recipeBookLibrary, userAddedMeasurementUnits);
                        break;
                    case "R":
                        exitMeasurementUnits = true;
                        break;
                    default:
                        break;
                }
            }
            while (!exitMeasurementUnits);
        }

        public static void RunEditRecipe(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            List<string[]> editRecipeOptions = new List<string[]>()
            {
                new string[] { "1", "Recipe Title" },
                new string[] { "2", "Notes" },
                new string[] { "3", "Preparation Times" },
                new string[] { "4", "Estimated Servings" },
                new string[] { "5", "Food Type & Genre"},
                new string[] { "6", "Ingredients" },
                new string[] { "7", "Instructions" },
                new string[] { "R", "Return to Previous Menu" }
            };

            string fieldToEdit = GetUserInput.GetTheFieldToEditFromUser(userIO, recipe, editRecipeOptions);

            switch (fieldToEdit)
            {
                case "1":
                    ManageMetadata.EditRecipeTitle(userIO, recipe);
                    break;
                case "2":
                    ManageMetadata.EditRecipeNotes(userIO, recipe);
                    break;
                case "3":
                    ManageMetadata.EditRecipePrepTimes(userIO, recipe);
                    break;
                case "4":
                    ManageMetadata.EditRecipeEstimatedServings(userIO, recipe);
                    break;
                case "5":
                    ManageMetadata.EditRecipeFoodTypeGenre(userIO, recipe);
                    break;
                case "6":
                    ManageIngredients.EditRecipeIngredients(userIO, recipeBookLibrary, recipe);
                    break;
                case "7":
                    ManageCookingInstructions.EditRecipeInstructions(userIO, recipe);
                    break;
                case "R":
                    return;
            }
        }
    }
}
