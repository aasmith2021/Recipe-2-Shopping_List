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
                        AddNewRecipeBook(userIO, recipeBookLibrary);
                        break;
                    case "R":
                        RenameRecipeBook(userIO, recipeBookLibrary);
                        break;
                    case "D":
                        DeleteRecipeBook(userIO, recipeBookLibrary);
                        break;
                    case "V":
                        UserInterface.DisplayShoppingList(userIO, shoppingList);
                        break;
                    case "M":
                        ManageSavedMeasurementUnits(userIO, recipeBookLibrary);
                        break;
                    case "X":
                        exitProgram = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public static void ManageSavedMeasurementUnits(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            bool exitMeasurementUnits = false;

            do
            {
                string header = "---------- MANAGE SAVED MEASUREMENT UNITS ----------";
                UserInterface.DisplayMenuHeader(userIO, header);

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

                if (userAddedMeasurementUnits.Count == 0)
                {
                    userIO.DisplayData("There are currently no user-added measurement units saved.");
                    userIO.DisplayData();
                }
                else
                {
                    userIO.DisplayData("<<< Current Measurement Units >>>");
                    for (int i = 0; i < userAddedMeasurementUnits.Count; i++)
                    {
                        userIO.DisplayData($"{i + 1}. {userAddedMeasurementUnits[i]}");
                    }
                    userIO.DisplayData();
                }

                UserInterface.DisplayOptionsMenu(userIO, editOptions, out options);
                userIO.DisplayData();
                userIO.DisplayDataLite("Select an editing option: ");
                string userOption = GetUserInput.GetUserOption(userIO, options);

                userIO.DisplayData();
                switch (userOption)
                {
                    case "A":
                        AddNewMeasurementUnit(userIO, recipeBookLibrary, userAddedMeasurementUnits);
                        break;
                    case "E":
                        EditExistingMeasurementUnit(userIO, recipeBookLibrary, userAddedMeasurementUnits);
                        break;
                    case "D":
                        DeleteExistingMeasurementUnit(userIO, recipeBookLibrary, userAddedMeasurementUnits);
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

        public static void AddNewMeasurementUnit(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            string header = "---------- MANAGE SAVED MEASUREMENT UNITS ----------";
            UserInterface.DisplayMenuHeader(userIO, header);

            if (userAddedMeasurementUnits.Count != 0)
            {
                userIO.DisplayData("<<< Current Measurement Units >>>");
                for (int i = 0; i < userAddedMeasurementUnits.Count; i++)
                {
                    userIO.DisplayData($"{i + 1}. {userAddedMeasurementUnits[i]}");
                }
            }

            string measurementUnit = "";
            GetUserInput.GetNewMeasurementUnitFromUser(userIO, out measurementUnit);

            GetUserInput.AreYouSure(userIO, "add this measurement unit", out bool isSure);

            if (isSure)
            {
                recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                UserInterface.SuccessfulChange(userIO, true, "measurement unit", "added");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "measurement unit", "added");
            }
        }

        public static void EditExistingMeasurementUnit(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            string header = "---------- MANAGE SAVED MEASUREMENT UNITS ----------";
            UserInterface.DisplayMenuHeader(userIO, header);

            List<string> userOptions = new List<string>();

            userIO.DisplayData("<<< Current Measurement Units >>>");
            for (int i = 0; i < userAddedMeasurementUnits.Count; i++)
            {
                userIO.DisplayData($"{i + 1}. {userAddedMeasurementUnits[i]}");
                userOptions.Add((i + 1).ToString());
            }

            userIO.DisplayData();
            userIO.DisplayData("Select the measurement unit to edit:");
            string userOption = GetUserInput.GetUserOption(userIO, userOptions);

            userIO.DisplayData();
            userIO.DisplayData("Enter the new name for the measurement unit:");
            string newName = GetUserInput.GetUserInputString(userIO, false, 30);

            recipeBookLibrary.EditMeasurementUnit(userAddedMeasurementUnits[int.Parse(userOption) - 1], newName);
            UserInterface.SuccessfulChange(userIO, true, "measurement unit name", "updated");
        }

        public static void DeleteExistingMeasurementUnit(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            string header = "---------- MANAGE SAVED MEASUREMENT UNITS ----------";
            UserInterface.DisplayMenuHeader(userIO, header);

            List<string> userOptions = new List<string>();

            userIO.DisplayData("<<< Current Measurement Units >>>");
            for (int i = 0; i < userAddedMeasurementUnits.Count; i++)
            {
                userIO.DisplayData($"{i + 1}. {userAddedMeasurementUnits[i]}");
                userOptions.Add((i + 1).ToString());
            }

            userIO.DisplayData();
            userIO.DisplayData("Select the measurement unit to delete:");
            string userOption = GetUserInput.GetUserOption(userIO, userOptions);

            GetUserInput.AreYouSure(userIO, "delete this measurement unit", out bool isSure);

            if (isSure)
            {
                recipeBookLibrary.DeleteMeasurementUnit(userAddedMeasurementUnits[int.Parse(userOption) - 1]);
                UserInterface.SuccessfulChange(userIO, true, "measurement unit", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "measurement unit", "deleted");
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
                        AddNewRecipe(userIO, recipeBookLibrary, recipeBook);
                        break;
                    case "E":
                        EditExistingRecipe(userIO, recipeBookLibrary, recipeBook);
                        break;
                    case "D":
                        DeleteExistingRecipe(userIO, recipeBook);
                        break;
                    case "S":
                        AddExistingRecipeToShoppingList(userIO, recipeBook, shoppingList);
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
                    AddRecipeToShoppingList(userIO, shoppingList, recipe);
                    break;
                case "E":
                    EditRecipe(userIO, recipeBookLibrary, recipe);
                    break;
                case "D":
                    DeleteOpenRecipe(userIO, recipeBook, recipe);
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

        private static void AddNewRecipeBook(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            userIO.ClearDisplay();
            userIO.DisplayData("---------- ADD NEW RECIPE BOOK ----------");
            userIO.DisplayData();
            userIO.DisplayDataLite("Enter a name for the new recipe book: ");
            string bookName = GetUserInput.GetUserInputString(userIO, false, 120);

            GetUserInput.AreYouSure(userIO, $"create a new recipe book named {bookName}", out bool isSure);

            if (isSure)
            {
                RecipeBook newRecipeBook = new RecipeBook(bookName);
                recipeBookLibrary.AddRecipeBook(newRecipeBook);
                UserInterface.SuccessfulChange(userIO, true, $"new recipe book, {bookName},", "created");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "new recipe book", "created");
            }
        }

        private static void RenameRecipeBook(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            userIO.ClearDisplay();
            userIO.DisplayData("---------- RENAME RECIPE BOOK ----------");
            userIO.DisplayData();

            List<string[]> recipeBooksToDisplay = new List<string[]>();
            List<string> recipeBookOptions = new List<string>();

            for (int i = 0; i < recipeBookLibrary.AllRecipeBooks.Length; i++)
            {
                recipeBooksToDisplay.Add(new string[] { (i + 1).ToString(), recipeBookLibrary.AllRecipeBooks[i].Name });
            }

            UserInterface.DisplayOptionsMenu(userIO, recipeBooksToDisplay, out recipeBookOptions);
            userIO.DisplayData();
            userIO.DisplayDataLite("Enter the recipe book you would like to rename: ");
            int userOption = int.Parse(GetUserInput.GetUserOption(userIO, recipeBookOptions));

            RecipeBook recipeBookToRename = recipeBookLibrary.AllRecipeBooks[userOption - 1];
            string oldName = recipeBookToRename.Name;

            userIO.DisplayData("");
            userIO.DisplayDataLite($"Enter the new name for the {oldName} recipe book: ");
            string newName = GetUserInput.GetUserInputString(userIO, false, 120);

            GetUserInput.AreYouSure(userIO, $"rename the {oldName} recipe book to \"{newName}\"", out bool isSure);

            if (isSure)
            {
                recipeBookToRename.Name = newName;
                UserInterface.SuccessfulChange(userIO, true, $"{oldName} recipe book", $"renamed {newName}");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "recipe book", "renamed");
            }
        }

        private static void DeleteRecipeBook(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            userIO.ClearDisplay();
            userIO.DisplayData("---------- DELETE RECIPE BOOK ----------");
            userIO.DisplayData();

            List<string[]> recipeBooksToDisplay = new List<string[]>();
            List<string> recipeBookOptions = new List<string>();

            for (int i = 0; i < recipeBookLibrary.AllRecipeBooks.Length; i++)
            {
                recipeBooksToDisplay.Add(new string[] { (i + 1).ToString(), recipeBookLibrary.AllRecipeBooks[i].Name });
            }

            UserInterface.DisplayOptionsMenu(userIO, recipeBooksToDisplay, out recipeBookOptions);
            userIO.DisplayData();
            userIO.DisplayDataLite("Enter the recipe book you would like to delete: ");
            int userOption = int.Parse(GetUserInput.GetUserOption(userIO, recipeBookOptions));

            RecipeBook recipeBookToDelete = recipeBookLibrary.AllRecipeBooks[userOption - 1];

            GetUserInput.AreYouSure(userIO, $"delete the {recipeBookToDelete.Name} recipe book", out bool isSure);

            if (isSure)
            {
                recipeBookLibrary.DeleteRecipeBook(recipeBookToDelete);
                UserInterface.SuccessfulChange(userIO, true, $"{recipeBookToDelete.Name} recipe book", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "recipe book", "deleted");
            }
        }

        private static void AddNewRecipe(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, RecipeBook recipeBook)
        {
            Metadata recipeMetadata = GetUserInput.GetMetadataFromUser(userIO);
            IngredientList recipeIngredientList = GetUserInput.GetIngredientsFromUser(userIO, recipeBookLibrary);
            CookingInstructions recipeCookingInstructions = GetUserInput.GetCookingInstructionsFromUser(userIO);

            Recipe newRecipe = new Recipe(recipeMetadata, recipeCookingInstructions, recipeIngredientList);

            GetUserInput.AreYouSure(userIO, $"add this new recipe", out bool isSure);

            if (isSure)
            {
                recipeBook.AddRecipe(newRecipe);
                UserInterface.SuccessfulChange(userIO, true, $"new recipe", "added to the recipe book");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "new recipe", "added");
            }
        }

        private static void EditRecipe(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, Recipe recipe)
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
                    EditRecipeTitle(userIO, recipe);
                    break;
                case "2":
                    EditRecipeNotes(userIO, recipe);
                    break;
                case "3":
                    EditRecipePrepTimes(userIO, recipe);
                    break;
                case "4":
                    EditRecipeEstimatedServings(userIO, recipe);
                    break;
                case "5":
                    EditRecipeFoodTypeGenre(userIO, recipe);
                    break;
                case "6":
                    EditRecipeIngredients(userIO, recipeBookLibrary, recipe);
                    break;
                case "7":
                    EditRecipeInstructions(userIO, recipe);
                    break;
                case "R":
                    return;
            }
        }

        private static void EditExistingRecipe(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, RecipeBook recipeBook)
        {
            string header = "---------- EDIT RECIPE ----------";
            UserInterface.DisplayMenuHeader(userIO, header);

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string> recipeOptions = new List<string>();

            for (int i = 0; i < recipeBook.Recipes.Length; i++)
            {
                recipesToDisplay.Add(new string[] { (i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title });
            }

            UserInterface.DisplayOptionsMenu(userIO, recipesToDisplay, out recipeOptions);
            userIO.DisplayData();
            userIO.DisplayDataLite("Enter the recipe you would like to edit: ");
            int userOption = int.Parse(GetUserInput.GetUserOption(userIO, recipeOptions));

            Recipe recipeToEdit = recipeBook.Recipes[userOption - 1];

            EditRecipe(userIO, recipeBookLibrary, recipeToEdit);
        }

        private static void DeleteOpenRecipe(IUserIO userIO, RecipeBook recipeBook, Recipe recipeToDelete)
        {
            userIO.ClearDisplay();
            userIO.DisplayData("---------- DELETE RECIPE ----------");
            userIO.DisplayData();
            userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines($"Recipe Title: {recipeToDelete.Metadata.Title}"));

            GetUserInput.AreYouSure(userIO, $"delete this recipe", out bool isSure);

            if (isSure)
            {
                recipeBook.DeleteRecipe(recipeToDelete);
                UserInterface.SuccessfulChange(userIO, true, $"recipe", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "recipe", "deleted");
            }
        }

        public static void DeleteExistingRecipe(IUserIO userIO, RecipeBook recipeBook)
        {
            string header = "---------- DELETE RECIPE ----------";
            UserInterface.DisplayMenuHeader(userIO, header);

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string> recipeOptions = new List<string>();

            for (int i = 0; i < recipeBook.Recipes.Length; i++)
            {
                recipesToDisplay.Add(new string[] { (i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title });
            }

            UserInterface.DisplayOptionsMenu(userIO, recipesToDisplay, out recipeOptions);
            userIO.DisplayData();
            userIO.DisplayDataLite("Enter the recipe you would like to delete: ");
            int userOption = int.Parse(GetUserInput.GetUserOption(userIO, recipeOptions));

            Recipe recipeToDelete = recipeBook.Recipes[userOption - 1];
            string recipeTitle = recipeToDelete.Metadata.Title;

            GetUserInput.AreYouSure(userIO, $"delete the {recipeTitle} recipe", out bool isSure);

            if (isSure)
            {
                recipeBook.DeleteRecipe(recipeToDelete);
                UserInterface.SuccessfulChange(userIO, true, $"recipe", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "recipe", "deleted");
            }
        }

        public static void EditRecipeTitle(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            userIO.DisplayDataLite("Enter the new title for this recipe: ");
            string newTitle = GetUserInput.GetUserInputString(userIO, false, 200);

            GetUserInput.AreYouSure(userIO, $"change the name of this recipe to {newTitle}", out bool isSure);

            if (isSure)
            {
                recipe.Metadata.Title = newTitle;
                UserInterface.SuccessfulChange(userIO, true, "recipe", "renamed");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "recipe", "renamed");
            }
        }

        public static void EditRecipeNotes(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            string userInput = "";
            if (recipe.Metadata.Notes != "")
            {
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Do you want to add to the existing notes or delete the old notes and add a new note?"));
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Enter \"A\" to Add to the existing notes or \"D\" to Delete the old notes and add a new note:"));
                List<string> options = new List<string>() { "A", "D" };
                userInput = GetUserInput.GetUserOption(userIO, options);
            }

            userIO.DisplayData();
            string newNotes = GetUserInput.GetNewUserNotes(userIO, recipe);

            GetUserInput.AreYouSure(userIO, "add these new notes to the recipe", out bool isSure);

            if (isSure)
            {
                if (userInput == "D")
                {
                    recipe.Metadata.Notes = "";
                }
                recipe.Metadata.Notes += " " + newNotes;
                UserInterface.SuccessfulChange(userIO, true, "recipe notes", "updated", true);
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "recipe notes", "updated", true);
            }
        }

        public static void EditRecipePrepTimes(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Do you want to change the Prep Time, the Cook Time, or Both?"));
            userIO.DisplayData();
            userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Enter \"P\" to change the Prep Time, \"C\" to change the Cook Time, or \"B\" to change both:"));
            List<string> options = new List<string>() { "P", "C", "B" };
            string userInput = GetUserInput.GetUserOption(userIO, options);

            int changeStartIndex = 0;
            int changeEndIndex = 0;

            switch (userInput)
            {
                case "B":
                    changeStartIndex = 0;
                    changeEndIndex = 1;
                    break;
                case "P":
                    changeStartIndex = 0;
                    changeEndIndex = 0;
                    break;
                case "C":
                    changeStartIndex = 1;
                    changeEndIndex = 1;
                    break;
                default:
                    break;
            }

            int newPrepTime = 0;
            int newCookTime = 0;

            for (int i = changeStartIndex; i <= changeEndIndex; i++)
            {
                if (i == 0)
                {
                    userIO.DisplayData();
                    userIO.DisplayDataLite("Enter the new Prep Time in minutes: ");
                    newPrepTime = GetUserInput.GetUserInputInt(userIO, 1);
                    while (newPrepTime > 2880)
                    {
                        userIO.DisplayData();
                        userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("A recipe cannot have a prep time of more than 2,880 minutes. Please enter a valid prep time:"));
                        newPrepTime = GetUserInput.GetUserInputInt(userIO, 1);
                    }
                }
                else
                {
                    userIO.DisplayData();
                    userIO.DisplayDataLite("Enter the new Cook Time in minutes: ");
                    newCookTime = GetUserInput.GetUserInputInt(userIO, 1);
                    while (newCookTime > 1440)
                    {
                        userIO.DisplayData();
                        userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("A recipe cannot have a cook time of more than 1,440 minutes. Please enter a valid cook time:"));
                        newCookTime = GetUserInput.GetUserInputInt(userIO, 1);
                    }
                }
            }

            GetUserInput.AreYouSure(userIO, $"update the recipe preparation times", out bool isSure);

            if (isSure)
            {
                if (userInput == "B" || userInput == "P")
                {
                    recipe.Metadata.PrepTimes.PrepTime = newPrepTime;
                }

                if (userInput == "B" || userInput == "C")
                {
                    recipe.Metadata.PrepTimes.CookTime = newCookTime;
                }

                UserInterface.SuccessfulChange(userIO, true, "recipe preparation times", "updated", true);
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "recipe preparation times", "updated", true);
            }
        }

        public static void EditRecipeEstimatedServings(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Do you want to change the Low # of Servings, High # of Servings, or Both?"));
            userIO.DisplayData();
            userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Enter \"L\" to change the Low # of Servings, \"H\" to change the High # of Servings, or \"B\" to change both:"));
            List<string> options = new List<string>() { "L", "H", "B" };
            string userInput = GetUserInput.GetUserOption(userIO, options);

            int changeStartIndex = 0;
            int changeEndIndex = 0;

            switch (userInput)
            {
                case "B":
                    changeStartIndex = 0;
                    changeEndIndex = 1;
                    break;
                case "L":
                    changeStartIndex = 0;
                    changeEndIndex = 0;
                    break;
                case "H":
                    changeStartIndex = 1;
                    changeEndIndex = 1;
                    break;
                default:
                    break;
            }

            int newLowServings = 0;
            int newHighServings = 0;

            for (int i = changeStartIndex; i <= changeEndIndex; i++)
            {
                if (i == 0)
                {
                    userIO.DisplayData();
                    userIO.DisplayDataLite("Enter the new Low # of Servings: ");
                    newLowServings = GetUserInput.GetUserInputInt(userIO, 1);
                    while (newLowServings > 500)
                    {
                        userIO.DisplayData();
                        userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("A recipe cannot have more than 500 servings. Please enter a valid number of servings:"));
                        newLowServings = GetUserInput.GetUserInputInt(userIO, 1);
                    }
                }
                else
                {
                    userIO.DisplayData();
                    userIO.DisplayDataLite("Enter the new High # of Servings: ");
                    newHighServings = GetUserInput.GetUserInputInt(userIO, 1);
                    while (newHighServings > 500)
                    {
                        userIO.DisplayData();
                        userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("A recipe cannot have more than 500 servings. Please enter a valid number of servings:"));
                        newHighServings = GetUserInput.GetUserInputInt(userIO, 1);
                    }
                }
            }

            GetUserInput.AreYouSure(userIO, $"update the recipe estimated servings", out bool isSure);

            if (isSure)
            {
                if (userInput == "B" || userInput == "L")
                {
                    recipe.Metadata.Servings.LowNumberOfServings = newLowServings;
                }

                if (userInput == "B" || userInput == "H")
                {
                    recipe.Metadata.Servings.HighNumberOfServings = newHighServings;
                }

                UserInterface.SuccessfulChange(userIO, true, "recipe estimated servings", "updated", true);
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "recipe estimated servings", "updated", true);
            }
        }

        public static void EditRecipeFoodTypeGenre(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Do you want to change the Food Type, Food Genre, or Both?"));
            userIO.DisplayData();
            userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Enter \"T\" to change the Food Type, \"G\" to change the Food Genre, or \"B\" to change both:"));
            List<string> options = new List<string>() { "T", "G", "B" };
            string userInput = GetUserInput.GetUserOption(userIO, options);

            int changeStartIndex = 0;
            int changeEndIndex = 0;

            switch (userInput)
            {
                case "B":
                    changeStartIndex = 0;
                    changeEndIndex = 1;
                    break;
                case "T":
                    changeStartIndex = 0;
                    changeEndIndex = 0;
                    break;
                case "G":
                    changeStartIndex = 1;
                    changeEndIndex = 1;
                    break;
                default:
                    break;
            }

            string newFoodType = "";
            string newFoodGenre = "";

            for (int i = changeStartIndex; i <= changeEndIndex; i++)
            {
                if (i == 0)
                {
                    userIO.DisplayData();
                    userIO.DisplayDataLite("Enter the new Food Type: ");
                    newFoodType = GetUserInput.GetUserInputString(userIO, true, 100);
                }
                else
                {
                    userIO.DisplayData();
                    userIO.DisplayDataLite("Enter the new Food Genre: ");
                    newFoodGenre = GetUserInput.GetUserInputString(userIO, true, 100);
                }
            }

            GetUserInput.AreYouSure(userIO, $"update the recipe food type/genre", out bool isSure);

            if (isSure)
            {
                if (userInput == "B" || userInput == "T")
                {
                    recipe.Metadata.Tags.FoodType = newFoodType;
                }

                if (userInput == "B" || userInput == "G")
                {
                    recipe.Metadata.Tags.FoodGenre = newFoodGenre;
                }

                UserInterface.SuccessfulChange(userIO, true, "recipe food type/genre", "updated", false);
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "recipe food type/genre", "updated", false);
            }
        }

        public static void EditRecipeIngredients(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);
            userIO.DisplayDataLite(recipe.Ingredients.ProduceIngredientsText(true, true));

            List<string[]> menuOptions = new List<string[]>()
            {
                new string[] { "A", "Add a New Igredient" },
                new string[] { "E", "Edit an Ingredient"},
                new string[] { "D", "Delete an Ingredient"},
                new string[] { "R", "Return to Previous Menu"},
            };
            List<string> options = new List<string>();

            if (recipe.Ingredients.AllIngredients.Count == 0)
            {
                menuOptions.RemoveAt(1);
                menuOptions.RemoveAt(1);
            }

            userIO.DisplayData();
            UserInterface.DisplayOptionsMenu(userIO, menuOptions, out options);
            userIO.DisplayData();
            userIO.DisplayDataLite("Select an editing option: ");
            string userOption = GetUserInput.GetUserOption(userIO, options);

            userIO.DisplayData();
            switch (userOption)
            {
                case "A":
                    AddNewIngredient(userIO, recipeBookLibrary, recipe);
                    break;
                case "E":
                    EditExistingIngredient(userIO, recipeBookLibrary, recipe);
                    break;
                case "D":
                    DeleteExistingIngredient(userIO, recipe);
                    break;
                case "R":
                    return;
            }
        }

        public static void EditExistingIngredient(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            List<Ingredient> allRecipeIngredients = recipe.Ingredients.AllIngredients;
            List<string> ingredientLineOptions = new List<string>();
            for (int i = 0; i < allRecipeIngredients.Count; i++)
            {
                ingredientLineOptions.Add((i + 1).ToString());
            }

            Ingredient ingredientToEdit;

            if (allRecipeIngredients.Count > 1)
            {
                userIO.DisplayData("Select the ingredient line you would like to edit:");
                string userOption = GetUserInput.GetUserOption(userIO, ingredientLineOptions);
                userIO.DisplayData();
                ingredientToEdit = allRecipeIngredients[int.Parse(userOption) - 1];
            }
            else
            {
                ingredientToEdit = allRecipeIngredients[0];
            }

            List<string[]> ingredientComponentsForMenu = new List<string[]>()
            {
                new string[] { "1", "Quantity" },
                new string[] { "2", "Measurement Unit" },
                new string[] { "3", "Ingredient Name" },
                new string[] { "4", "Preparation Note" }
            };
            List<string> ingredientComponentOptions = new List<string>();

            userIO.DisplayData("Select the part of the ingredient you would like to edit:");
            UserInterface.DisplayOptionsMenu(userIO, ingredientComponentsForMenu, out ingredientComponentOptions);
            string ingredientComponentToEdit = GetUserInput.GetUserOption(userIO, ingredientComponentOptions);

            userIO.DisplayData();
            string measurementUnit = "";
            string newPrepNote;
            switch (ingredientComponentToEdit)
            {
                case "1":
                    userIO.DisplayDataLite("Enter the new quantity of the ingredient (ex: 1.5): ");
                    double newQuantity = GetUserInput.GetUserInputDouble(userIO, 2);
                    while (newQuantity > 1000)
                    {
                        userIO.DisplayData();
                        userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("An ingredient quantity cannot be more than 1000. Please enter a valid ingredient quantity:"));
                        newQuantity = GetUserInput.GetUserInputDouble(userIO, 2);
                    }
                    ingredientToEdit.Quantity = newQuantity;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient quantity", "updated");
                    break;
                case "2":
                    List<string[]> measurementUnits = MeasurementUnits.AllMeasurementUnitsForUserInput(recipeBookLibrary);
                    List<string> options = new List<string>();
                    userIO.DisplayData("Select the new ingredient measurement unit from the list of options:");
                    UserInterface.DisplayOptionsMenu(userIO, measurementUnits, out options);

                    int userOptionNumber = int.Parse(GetUserInput.GetUserOption(userIO, options));

                    if (userOptionNumber == options.Count)
                    {
                        GetUserInput.GetNewMeasurementUnitFromUser(userIO, out measurementUnit);
                        recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                        userIO.DisplayData();
                        userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines($"Success! New measurement unit, {measurementUnit}, was added and will be used for this ingredient."));
                    }
                    else if (measurementUnits[userOptionNumber - 1][1] != "None")
                    {
                        measurementUnit = measurementUnits[userOptionNumber - 1][1];
                    }
                    ingredientToEdit.MeasurementUnit = measurementUnit;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient meaurement unit", "updated");
                    break;
                case "3":
                    userIO.DisplayData("Enter the new ingredient name:");
                    string newName = GetUserInput.GetUserInputString(userIO, false, 100);
                    ingredientToEdit.Name = newName;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient name", "updated");
                    break;
                case "4":
                    userIO.DisplayData("Enter the new ingredient preparation note (or press \"Enter\" to leave blank):");
                    newPrepNote = GetUserInput.GetUserInputString(userIO, true, 120);
                    ingredientToEdit.PreparationNote = newPrepNote;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient preparation note", "updated");
                    break;
                default:
                    break;
            }
        }

        public static void AddNewIngredient(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            Ingredient ingredientToAdd = GetUserInput.GetIngredientFromUser(userIO, recipeBookLibrary);
            recipe.Ingredients.AddIngredient(ingredientToAdd);
            UserInterface.SuccessfulChange(userIO, true, "ingredient", "added");
        }

        public static void DeleteExistingIngredient(IUserIO userIO, Recipe recipe)
        {
            userIO.DisplayData("Select the ingredient line you would like to delete:");

            List<Ingredient> allRecipeIngredients = recipe.Ingredients.AllIngredients;
            List<string> ingredientLineOptions = new List<string>();
            for (int i = 1; i <= allRecipeIngredients.Count; i++)
            {
                ingredientLineOptions.Add(i.ToString());
            }

            string userOption = GetUserInput.GetUserOption(userIO, ingredientLineOptions);
            int indexOfIngredientToDelete = int.Parse(userOption) - 1;

            GetUserInput.AreYouSure(userIO, "delete this ingredient", out bool isSure);

            if (isSure)
            {
                recipe.Ingredients.DeleteIngredient(recipe.Ingredients.AllIngredients[indexOfIngredientToDelete]);
                UserInterface.SuccessfulChange(userIO, true, "ingredient", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "ingredient", "deleted");
            }
        }

        public static void EditRecipeInstructions(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            userIO.DisplayDataLite(recipe.CookingInstructions.ProduceInstructionsText(true));
            userIO.DisplayData();

            List<string[]> menuOptions = new List<string[]>()
            {
                new string[] { "A", "Add New Instruction Block" },
                new string[] { "E", "Edit an Instruction Block"},
                new string[] { "D", "Delete an Instruction Block"},
                new string[] { "R", "Return to Previous Menu"},
            };
            List<string> options = new List<string>();

            if (recipe.CookingInstructions.InstructionBlocks.Count == 0)
            {
                menuOptions.RemoveAt(1);
                menuOptions.RemoveAt(1);
            }

            UserInterface.DisplayOptionsMenu(userIO, menuOptions, out options);
            userIO.DisplayData();
            userIO.DisplayDataLite("Select an editing option: ");
            string userOption = GetUserInput.GetUserOption(userIO, options);

            userIO.DisplayData();
            switch (userOption)
            {
                case "A":
                    AddNewInstructionBlock(userIO, recipe);
                    break;
                case "E":
                    EditExistingInstructionBlock(userIO, recipe);
                    break;
                case "D":
                    DeleteExistingInstructionBlock(userIO, recipe);
                    break;
                case "R":
                    return;
            }
        }

        public static void AddNewInstructionBlock(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            InstructionBlock newInstructionBlock = GetUserInput.GetInstructionBlockFromUser(userIO);

            GetUserInput.AreYouSure(userIO, "add this new instruction block", out bool isSure);

            if (isSure)
            {
                recipe.CookingInstructions.AddInstructionBlock(newInstructionBlock);
                UserInterface.SuccessfulChange(userIO, true, "new instruction block", "added");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "new instruction block", "added");
            }
        }

        public static void EditExistingInstructionBlock(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            userIO.DisplayDataLite(recipe.CookingInstructions.ProduceInstructionsText(true, true));
            InstructionBlock instructionBlockToEdit;
            int numberOfInstructionBlocks = recipe.CookingInstructions.InstructionBlocks.Count;
            List<string> instructionBlockOptions = new List<string>();
            for (int i = 1; i <= numberOfInstructionBlocks; i++)
            {
                instructionBlockOptions.Add(i.ToString());
            }

            if (numberOfInstructionBlocks > 1)
            {
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Enter the instruction block you would like to edit:"));

                string userBlockSelection = GetUserInput.GetUserOption(userIO, instructionBlockOptions);
                int instructionBlockIndex = int.Parse(userBlockSelection);

                instructionBlockToEdit = recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex - 1];
            }
            else if (numberOfInstructionBlocks == 1)
            {
                instructionBlockToEdit = recipe.CookingInstructions.InstructionBlocks[0];
            }
            else
            {
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("This recipe does not have any instruction blocks. Add a new instruction block in order to edit the recipe."));
                userIO.DisplayData();
                userIO.DisplayData("Press \"Enter\" to continue...");
                userIO.GetInput();

                return;
            }

            List<string[]> editBlockMenuOptions = new List<string[]>()
            {
                new string[] { "", "Add Instruction Line" },
                new string[] { "", "Edit Instruction Line" },
                new string[] { "", "Delete Instruction Line" },
                new string[] { "", "Add Block Heading" },
                new string[] { "", "Edit Block Heading" },
                new string[] { "", "Delete Block Heading" },
            };
            List<string> editBlockOptions = new List<string>();

            bool instructionLinesAreBlank = instructionBlockToEdit.InstructionLines.Count == 0;
            bool blockHeadingIsBlank = instructionBlockToEdit.BlockHeading == "";

            if (instructionLinesAreBlank)
            {
                editBlockMenuOptions.RemoveAt(1);
                editBlockMenuOptions.RemoveAt(1);
            }

            if (blockHeadingIsBlank)
            {
                editBlockMenuOptions.RemoveAt(editBlockMenuOptions.Count - 1);
                editBlockMenuOptions.RemoveAt(editBlockMenuOptions.Count - 1);
            }

            if (!blockHeadingIsBlank)
            {
                editBlockMenuOptions.RemoveAt(editBlockMenuOptions.Count - 3);
            }

            for (int i = 0; i < editBlockMenuOptions.Count; i++)
            {
                editBlockMenuOptions[i][0] = (i + 1).ToString();
            }

            userIO.DisplayData();
            UserInterface.DisplayOptionsMenu(userIO, editBlockMenuOptions, out editBlockOptions);
            userIO.DisplayData();
            userIO.DisplayDataLite("Enter an editing option from the menu: ");
            string editBlockOption = GetUserInput.GetUserOption(userIO, editBlockOptions);
            string menuSelection = editBlockMenuOptions[int.Parse(editBlockOption) - 1][1];

            userIO.DisplayData();
            switch (menuSelection)
            {
                case "Add Instruction Line":
                    AddInstructionLine(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Edit Instruction Line":
                    EditInstructionLine(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Delete Instruction Line":
                    DeleteInstructionLine(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Add Block Heading":
                    AddInstructionBlockHeading(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Edit Block Heading":
                    EditInstructionBlockHeading(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Delete Block Heading":
                    DeleteInstructionBlockHeading(userIO, instructionBlockToEdit, recipe);
                    break;
                default:
                    break;
            }
        }

        public static void AddInstructionLine(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            userIO.DisplayData();
            userIO.DisplayData("Enter the new instruction line to add:");
            string newInstructionLine = GetUserInput.GetUserInputString(userIO, false, 360);

            instructionBlock.AddInstructionLine(newInstructionLine);
            UserInterface.SuccessfulChange(userIO, true, "new instruction line", "added");
        }

        public static void EditInstructionLine(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            List<string> allInstructionLines = instructionBlock.InstructionLines;
            List<string> instructionLineOptions = new List<string>();

            for (int i = 1; i <= allInstructionLines.Count; i++)
            {
                instructionLineOptions.Add(i.ToString());
            }
            userIO.DisplayData();
            userIO.DisplayDataLite("Select the instruction line to edit: ");
            string instructionLineSelected = GetUserInput.GetUserOption(userIO, instructionLineOptions);

            userIO.DisplayData();
            userIO.DisplayData("Enter the new text for the instruction line:");
            string newInstructionLineText = GetUserInput.GetUserInputString(userIO, false, 360);

            instructionBlock.EditInstructionLine(int.Parse(instructionLineSelected) - 1, newInstructionLineText);
            UserInterface.SuccessfulChange(userIO, true, "instruction line", "edited");
        }

        public static void DeleteInstructionLine(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            List<string> allInstructionLines = instructionBlock.InstructionLines;
            List<string> instructionLineOptions = new List<string>();

            for (int i = 1; i <= allInstructionLines.Count; i++)
            {
                instructionLineOptions.Add(i.ToString());
            }
            userIO.DisplayData();
            userIO.DisplayDataLite("Select the instruction line to delete: ");
            string instructionLineSelected = GetUserInput.GetUserOption(userIO, instructionLineOptions);

            GetUserInput.AreYouSure(userIO, "delete this instruction line", out bool isSure);

            if (isSure)
            {
                instructionBlock.DeleteInstructionLine(int.Parse(instructionLineSelected) - 1);
                UserInterface.SuccessfulChange(userIO, true, "instruction line", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "instruction line", "deleted");
            }
        }

        public static void AddInstructionBlockHeading(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            userIO.DisplayData();
            userIO.DisplayData("Enter the new block heading to add:");
            string newBlockHeading = GetUserInput.GetUserInputString(userIO, false, 100);

            instructionBlock.BlockHeading = newBlockHeading;
            UserInterface.SuccessfulChange(userIO, true, "new block heading", "added");
        }

        public static void EditInstructionBlockHeading(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            userIO.DisplayData();
            userIO.DisplayData("Enter the new block heading:");
            string newBlockHeading = GetUserInput.GetUserInputString(userIO, false, 100);

            instructionBlock.BlockHeading = newBlockHeading;
            UserInterface.SuccessfulChange(userIO, true, "block heading", "edited");
        }

        public static void DeleteInstructionBlockHeading(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            GetUserInput.AreYouSure(userIO, "delete the block heading", out bool isSure);

            if (isSure)
            {
                instructionBlock.BlockHeading = "";
                UserInterface.SuccessfulChange(userIO, true, "block heading", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "block heading", "deleted");
            }
        }

        public static void DeleteExistingInstructionBlock(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            userIO.DisplayDataLite(recipe.CookingInstructions.ProduceInstructionsText(true, true));
            userIO.DisplayData();
            userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Enter the instruction block you would like to delete:"));

            int numberOfInstructionBlocks = recipe.CookingInstructions.InstructionBlocks.Count;
            List<string> instructionBlockOptions = new List<string>();
            for (int i = 1; i <= numberOfInstructionBlocks; i++)
            {
                instructionBlockOptions.Add(i.ToString());
            }

            string userBlockSelection = GetUserInput.GetUserOption(userIO, instructionBlockOptions);

            GetUserInput.AreYouSure(userIO, "delete this instruction block", out bool isSure);

            if (isSure)
            {
                recipe.CookingInstructions.DeleteInstructionBlock(int.Parse(userBlockSelection) - 1);
                UserInterface.SuccessfulChange(userIO, true, "instruction block", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "instruction block", "deleted");
            }
        }

        public static void AddExistingRecipeToShoppingList(IUserIO userIO, RecipeBook recipeBook, ShoppingList shoppingList)
        {
            string header = "---------- ADD RECIPE TO SHOPPING LIST ----------";
            UserInterface.DisplayMenuHeader(userIO, header);

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string> recipeOptions = new List<string>();

            for (int i = 0; i < recipeBook.Recipes.Length; i++)
            {
                recipesToDisplay.Add(new string[] { (i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title });
            }

            //Adds the "R" option to return to previous menu
            recipesToDisplay.Add(new string[] { "R", "Return to Previous Menu" });

            UserInterface.DisplayOptionsMenu(userIO, recipesToDisplay, out recipeOptions);
            userIO.DisplayData();
            userIO.DisplayDataLite("Enter the recipe you would like to add to the shopping list: ");
            string userOption = GetUserInput.GetUserOption(userIO, recipeOptions);

            if (userOption != "R")
            {
                int recipeOption = int.Parse(userOption);

                Recipe recipeToAdd = recipeBook.Recipes[recipeOption - 1];

                AddRecipeToShoppingList(userIO, shoppingList, recipeToAdd);
            }
        }

        public static void AddRecipeToShoppingList(IUserIO userIO, ShoppingList shoppingList, Recipe recipe)
        {
            string header = "-------- ADD RECIPE TO SHOPPING LIST --------";
            UserInterface.DisplayMenuHeader(userIO, header);

            List<Ingredient> recipeIngredients = recipe.Ingredients.AllIngredients;

            foreach (Ingredient element in recipeIngredients)
            {
                if (element.StoreLocation == "")
                {
                    GetStoreLocationForIngredient(userIO, shoppingList, element);
                }

                AddIngredientToStoreLocation(userIO, shoppingList, element);
            }

            UserInterface.SuccessfulChange(userIO, true, "recipe", "added to the shopping list");
        }

        public static void GetStoreLocationForIngredient(IUserIO userIO, ShoppingList shoppingList, Ingredient ingredient)
        {
            string header = "-------- SET STORE LOCATION FOR INGREDIENT --------";
            string message = UserInterface.MakeStringConsoleLengthLines($"INGREDIENT: {ingredient.Name}");
            UserInterface.DisplayMenuHeader(userIO, header, message);
            userIO.DisplayData("Which department is this ingredient generally found in at the store?");

            List<string[]> menuOptions = new List<string[]>();
            List<string> optionChoices = new List<string>();

            for (int i = 0; i < shoppingList.StoreLocations.Length; i++)
            {
                menuOptions.Add(new string[] { $"{i + 1}", shoppingList.StoreLocations[i] });
            }

            UserInterface.DisplayOptionsMenu(userIO, menuOptions, out optionChoices);
            userIO.DisplayData();
            userIO.DisplayData("Select the store location of this ingredient:");
            string userOption = GetUserInput.GetUserOption(userIO, optionChoices);
            userIO.DisplayData();

            string storeLocation = shoppingList.StoreLocations[int.Parse(userOption) - 1];

            ingredient.StoreLocation = storeLocation;
        }

        public static void AddIngredientToStoreLocation(IUserIO userIO, ShoppingList shoppingList, Ingredient ingredient)
        {
            string storeLocation = ingredient.StoreLocation;

            List<Ingredient> currentLocationIngredients = new List<Ingredient>();
            Ingredient combinedIngredientToAdd = new Ingredient(0, "", "");

            switch (storeLocation)
            {
                case "Produce":
                    currentLocationIngredients.AddRange(shoppingList.Produce);
                    break;
                case "Bakery/Deli":
                    currentLocationIngredients.AddRange(shoppingList.BakeryDeli);
                    break;
                case "Dry Goods":
                    currentLocationIngredients.AddRange(shoppingList.DryGoods);
                    break;
                case "Meat":
                    currentLocationIngredients.AddRange(shoppingList.Meat);
                    break;
                case "Refrigerated":
                    currentLocationIngredients.AddRange(shoppingList.Refrigerated);
                    break;
                case "Frozen":
                    currentLocationIngredients.AddRange(shoppingList.Frozen);
                    break;
                case "Non-Grocery":
                    currentLocationIngredients.AddRange(shoppingList.NonGrocery);
                    break;
                default:
                    break;
            }

            string currentIngredientName = "";
            bool currentIngredientHasVolumeMeasurementUnit;
            string currentIngredientMeasurementUnit;
            string newIngredientName = ingredient.Name;
            string newIngredientMeasurementUnit = ingredient.MeasurementUnit;
            bool newIngredientHasVolumeMeasurementUnit = MeasurementUnits.IngredientHasVolumeMeasurementUnit(ingredient);
            int indexOfMatchingIngredient = 0;
            bool ingredientsAreCombinable;
            bool ingredientsAreTheSame = false;

            //This loop looks for ingredients that have an exact name and measurement unit match
            for (int i = 0; i < currentLocationIngredients.Count; i++)
            {
                indexOfMatchingIngredient = i;
                currentIngredientName = currentLocationIngredients[i].Name;
                currentIngredientHasVolumeMeasurementUnit = MeasurementUnits.IngredientHasVolumeMeasurementUnit(currentLocationIngredients[i]);
                currentIngredientMeasurementUnit = currentLocationIngredients[i].MeasurementUnit;

                ingredientsAreCombinable = !(currentIngredientHasVolumeMeasurementUnit ^ newIngredientHasVolumeMeasurementUnit);

                if (newIngredientName == currentIngredientName && ingredientsAreCombinable && !(newIngredientMeasurementUnit == "" ^ newIngredientMeasurementUnit == ""))
                {
                    ingredientsAreTheSame = true;
                    break;
                }
            }

            //This loop looks for ingredients that may be similar if an exact match was not found on the shopping list
            if (!ingredientsAreTheSame)
            {
                for (int i = 0; i < currentLocationIngredients.Count; i++)
                {
                    indexOfMatchingIngredient = i;
                    currentIngredientName = currentLocationIngredients[i].Name;
                    currentIngredientHasVolumeMeasurementUnit = MeasurementUnits.IngredientHasVolumeMeasurementUnit(currentLocationIngredients[i]);

                    ingredientsAreCombinable = !(currentIngredientHasVolumeMeasurementUnit ^ newIngredientHasVolumeMeasurementUnit);

                    double percentSimilar = GetSimilarityPercentage(currentIngredientName, newIngredientName);

                    if (percentSimilar >= .3 && ingredientsAreCombinable)
                    {
                        ingredientsAreTheSame = AreIngredientsTheSame(userIO, currentIngredientName, newIngredientName);

                        if (ingredientsAreTheSame)
                        {
                            break;
                        }
                    }
                }
            }

            Ingredient ingredientToAdd;
            bool combineIngredients = false;

            //If the ingredient already matches an ingredient on the shopping list, remove the current
            //ingredient to the shopping list and add a new ingredient (which is the ingredient
            //that was already on the shopping list "added to" the new ingredient being added to the shopping list.)
            if (ingredientsAreTheSame)
            {
                Ingredient ingredientAlreadyOnShoppingList = currentLocationIngredients[indexOfMatchingIngredient];
                ingredientToAdd = ingredientAlreadyOnShoppingList.CombineIngredientsForShoppingList(ingredient);
                combineIngredients = true;
            }
            else
            {
                ingredientToAdd = ingredient;
            }

            //Add the ingredient to the store location on the shopping list
            switch (storeLocation)
            {
                case "Produce":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateProduce(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddProduce(ingredientToAdd);
                    }
                    break;
                case "Bakery/Deli":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateBakeryDeli(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddBakeryDeli(ingredientToAdd);
                    }
                    break;
                case "Dry Goods":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateDryGoods(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddDryGoods(ingredientToAdd);
                    }
                    break;
                case "Meat":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateMeat(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddMeat(ingredientToAdd);
                    }
                    break;
                case "Refrigerated":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateRefrigerated(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddRefrigerated(ingredientToAdd);
                    }
                    break;
                case "Frozen":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateFrozen(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddFrozen(ingredientToAdd);
                    }
                    break;
                case "Non-Grocery":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateNonGrocery(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddNonGrocery(ingredientToAdd);
                    }
                    break;
                default:
                    break;
            }
        }

        public static double GetSimilarityPercentage(string firstIngredientName, string secondIngredientName)
        {
            int countOfCharactersTheSame = 0;
            double percentSimilar = 0;

            string shorterPhrase;
            string longerPhrase;

            if (firstIngredientName.Length <= secondIngredientName.Length)
            {
                shorterPhrase = firstIngredientName.ToLower();
                longerPhrase = secondIngredientName.ToLower();
            }
            else
            {
                shorterPhrase = secondIngredientName.ToLower();
                longerPhrase = firstIngredientName.ToLower();
            }

            string[] shorterPhraseWords = shorterPhrase.Split(" ");
            string wordToCheck = "";
            string testString = "";
            string regexExpression = "";
            bool matchFound = false;

            for (int i = 0; i < shorterPhraseWords.Length; i++)
            {
                wordToCheck = shorterPhraseWords[i];
                
                for (int j = 0; j < wordToCheck.Length; j++)
                {
                    testString = wordToCheck.Substring(0, j + 1);
                    regexExpression = $"{testString}.*?";
                    matchFound = Regex.Match(longerPhrase, regexExpression).Success;

                    if (matchFound)
                    {
                        countOfCharactersTheSame++;
                    }
                }
            }

            percentSimilar = (double)countOfCharactersTheSame / longerPhrase.Length;

            return percentSimilar;
        }

        public static bool AreIngredientsTheSame(IUserIO userIO, string currentIngredientName, string newIngredientName)
        {
            bool ingredientsAreTheSame = false;
            string header = "-------- SIMILAR INGREDIENTS FOUND --------";
            UserInterface.DisplayMenuHeader(userIO, header);
            userIO.DisplayData();
            userIO.DisplayData("The following ingredients might match:");
            userIO.DisplayData();
            userIO.DisplayData($"<<Ingredient Already On Shopping List>>{Environment.NewLine}{currentIngredientName}");
            userIO.DisplayData();
            userIO.DisplayData($"<<New Ingredient>>{Environment.NewLine}{newIngredientName}");
            userIO.DisplayData();
            userIO.DisplayData("Are these ingredients the same? Enter \"Y\" for Yes or \"N\" for No:");
            List<string> userOptions = new List<string>() { "Y", "N" };
            string userOption = GetUserInput.GetUserOption(userIO, userOptions);

            if (userOption == "Y")
            {
                ingredientsAreTheSame = true;
            }

            return ingredientsAreTheSame;
        }
    }
}
