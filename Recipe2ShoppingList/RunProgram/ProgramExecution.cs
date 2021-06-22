using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    public class ProgramExecution
    {
        public static void RunProgram(out bool exitProgram)
        {
            exitProgram = false;

            //Load <recipeBookLibrary> into the program from the database file
            RecipeBookLibrary recipeBookLibrary = ReadFromFile.GetRecipeBookLibraryFromFile();

            ShoppingList shoppingList = new ShoppingList();

            while (!exitProgram)
            {
                RunMainMenu(recipeBookLibrary, shoppingList, out exitProgram);
            }

            //Save <recipeBookLibrary> to the "write" database file before closing program
            recipeBookLibrary.WriteRecipeBookLibraryToFile();

            //Delete original database, then rename the "write" database file to become the new master database file
            DataHelperMethods.DeleteOldFileAndRenameNewFile(DataHelperMethods.GetReadDatabaseFilePath(), DataHelperMethods.GetWriteDatabaseFilePath());

            //Save the Shopping List to the Shopping List file before closing the program
            shoppingList.WriteShoppingListToFile();

            //Delete original Shopping List file, then rename the "write" Shopping List file to become the new master Shopping List
            DataHelperMethods.DeleteOldFileAndRenameNewFile(DataHelperMethods.GetReadShoppingListFilePath(), DataHelperMethods.GetWriteShoppingListFilePath());
        }

        private static void RunMainMenu(RecipeBookLibrary recipeBookLibrary, ShoppingList shoppingList, out bool exitProgram)
        {
            exitProgram = false;

            UserInterface.DisplayMainMenu(recipeBookLibrary, out List<string> mainMenuOptions);
            string userOption = GetUserInput.GetUserOption(mainMenuOptions);

            if (Int32.TryParse(userOption, out int userOptionNumber))
            {
                bool exitRecipeBookSection = false;

                while (!exitRecipeBookSection)
                {
                    RunRecipeBook(recipeBookLibrary, shoppingList, userOptionNumber, out exitRecipeBookSection, out exitProgram);
                }
            }
            else
            {
                switch (userOption)
                {
                    case "A":
                        AddNewRecipeBook(recipeBookLibrary);
                        break;
                    case "R":
                        RenameRecipeBook(recipeBookLibrary);
                        break;
                    case "D":
                        DeleteRecipeBook(recipeBookLibrary);
                        break;
                    case "V":
                        UserInterface.DisplayShoppingList(shoppingList);
                        break;
                    case "M":
                        ManageSavedMeasurementUnits(recipeBookLibrary);
                        break;
                    case "X":
                        exitProgram = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public static void ManageSavedMeasurementUnits(RecipeBookLibrary recipeBookLibrary)
        {
            bool exitMeasurementUnits = false;

            do
            {
                string header = "---------- MANAGE SAVED MEASUREMENT UNITS ----------";
                UserInterface.DisplayMenuHeader(header);

                int allStandardMeasurementUnitsLength = MeasurementUnits.AllStandardMeasurementUnits().Length;
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
                    Console.WriteLine("There are currently no user-added measurement units saved.");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("<<< Current Measurement Units >>>");
                    for (int i = 0; i < userAddedMeasurementUnits.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {userAddedMeasurementUnits[i]}");
                    }
                    Console.WriteLine();
                }

                UserInterface.DisplayOptionsMenu(editOptions, out options);
                Console.WriteLine();
                Console.Write("Select an editing option: ");
                string userOption = GetUserInput.GetUserOption(options);

                Console.WriteLine();
                switch (userOption)
                {
                    case "A":
                        AddNewMeasurementUnit(recipeBookLibrary, userAddedMeasurementUnits);
                        break;
                    case "E":
                        EditExistingMeasurementUnit(recipeBookLibrary, userAddedMeasurementUnits);
                        break;
                    case "D":
                        DeleteExistingMeasurementUnit(recipeBookLibrary, userAddedMeasurementUnits);
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

        public static void AddNewMeasurementUnit(RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            string header = "---------- MANAGE SAVED MEASUREMENT UNITS ----------";
            UserInterface.DisplayMenuHeader(header);

            if (userAddedMeasurementUnits.Count != 0)
            {
                Console.WriteLine("<<< Current Measurement Units >>>");
                for (int i = 0; i < userAddedMeasurementUnits.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {userAddedMeasurementUnits[i]}");
                }
            }

            string measurementUnit = "";
            GetUserInput.GetNewMeasurementUnitFromUser(out measurementUnit);

            GetUserInput.AreYouSure("add this measurement unit", out bool isSure);

            if (isSure)
            {
                recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                UserInterface.SuccessfulChange(true, "measurement unit", "added");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "measurement unit", "added");
            }
        }

        public static void EditExistingMeasurementUnit(RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            string header = "---------- MANAGE SAVED MEASUREMENT UNITS ----------";
            UserInterface.DisplayMenuHeader(header);

            List<string> userOptions = new List<string>();

            Console.WriteLine("<<< Current Measurement Units >>>");
            for (int i = 0; i < userAddedMeasurementUnits.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userAddedMeasurementUnits[i]}");
                userOptions.Add((i + 1).ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Select the measurement unit to edit:");
            string userOption = GetUserInput.GetUserOption(userOptions);

            Console.WriteLine();
            Console.WriteLine("Enter the new name for the measurement unit:");
            string newName = GetUserInput.GetUserInputString(false);

            recipeBookLibrary.EditMeasurementUnit(userAddedMeasurementUnits[int.Parse(userOption) - 1], newName);
            UserInterface.SuccessfulChange(true, "measurement unit name", "updated");
        }

        public static void DeleteExistingMeasurementUnit(RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            string header = "---------- MANAGE SAVED MEASUREMENT UNITS ----------";
            UserInterface.DisplayMenuHeader(header);

            List<string> userOptions = new List<string>();

            Console.WriteLine("<<< Current Measurement Units >>>");
            for (int i = 0; i < userAddedMeasurementUnits.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userAddedMeasurementUnits[i]}");
                userOptions.Add((i + 1).ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Select the measurement unit to delete:");
            string userOption = GetUserInput.GetUserOption(userOptions);

            GetUserInput.AreYouSure("delete this measurement unit", out bool isSure);

            if (isSure)
            {
                recipeBookLibrary.DeleteMeasurementUnit(userAddedMeasurementUnits[int.Parse(userOption) - 1]);
                UserInterface.SuccessfulChange(true, "measurement unit", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "measurement unit", "deleted");
            }
        }

        private static void RunRecipeBook(RecipeBookLibrary recipeBookLibrary, ShoppingList shoppingList, int recipeBookOptionNumber, out bool exitRecipeBookSection, out bool exitProgram)
        {
            exitProgram = false;
            exitRecipeBookSection = false;

            RecipeBook recipeBook = recipeBookLibrary.AllRecipeBooks[recipeBookOptionNumber - 1];
            UserInterface.DisplayOpenRecipeBook(recipeBook, out List<string> recipeBookOptions);
            string userOption = GetUserInput.GetUserOption(recipeBookOptions);

            if (int.TryParse(userOption, out int userOptionNumber))
            {
                bool exitRecipeSection = false;

                while (!exitRecipeSection)
                {
                    RunRecipe(recipeBookLibrary, shoppingList, recipeBook, userOptionNumber, out exitRecipeSection, out exitRecipeBookSection, out exitProgram);
                }
            }
            else
            {
                switch (userOption)
                {
                    case "A":
                        AddNewRecipe(recipeBookLibrary, recipeBook);
                        break;
                    case "E":
                        EditExistingRecipe(recipeBookLibrary, recipeBook);
                        break;
                    case "D":
                        DeleteExistingRecipe(recipeBook);
                        break;
                    case "S":
                        AddExistingRecipeToShoppingList(recipeBook, shoppingList);
                        break;
                    case "V":
                        UserInterface.DisplayShoppingList(shoppingList);
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

        private static void RunRecipe(RecipeBookLibrary recipeBookLibrary, ShoppingList shoppingList, RecipeBook recipeBook, int recipeOptionNumber, out bool exitRecipeSection, out bool exitRecipeBookSection, out bool exitProgram)
        {
            exitProgram = false;
            exitRecipeSection = false;
            exitRecipeBookSection = false;

            Recipe recipe = recipeBook.Recipes[recipeOptionNumber - 1];
            UserInterface.DisplayOpenRecipe(recipe, recipeBook, out List<string> recipeEditOptions);
            string userOption = GetUserInput.GetUserOption(recipeEditOptions);

            switch (userOption)
            {
                case "S":
                    AddRecipeToShoppingList(shoppingList, recipe);
                    break;
                case "E":
                    EditRecipe(recipeBookLibrary, recipe);
                    break;
                case "D":
                    DeleteOpenRecipe(recipeBook, recipe);
                    exitRecipeSection = true;
                    break;
                case "V":
                    UserInterface.DisplayShoppingList(shoppingList);
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

        private static void AddNewRecipeBook(RecipeBookLibrary recipeBookLibrary)
        {
            Console.Clear();
            Console.WriteLine("---------- ADD NEW RECIPE BOOK ----------");
            Console.WriteLine();
            Console.Write("Enter a name for the new recipe book: ");
            string bookName = GetUserInput.GetUserInputString(false);

            GetUserInput.AreYouSure($"create a new recipe book named {bookName}", out bool isSure);

            if (isSure)
            {
                RecipeBook newRecipeBook = new RecipeBook(bookName);
                recipeBookLibrary.AddRecipeBook(newRecipeBook);
                UserInterface.SuccessfulChange(true, $"new recipe book, {bookName},", "created");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "new recipe book", "created");
            }
        }

        private static void RenameRecipeBook(RecipeBookLibrary recipeBookLibrary)
        {
            Console.Clear();
            Console.WriteLine("---------- RENAME RECIPE BOOK ----------");
            Console.WriteLine();

            List<string[]> recipeBooksToDisplay = new List<string[]>();
            List<string> recipeBookOptions = new List<string>();

            for (int i = 0; i < recipeBookLibrary.AllRecipeBooks.Length; i++)
            {
                recipeBooksToDisplay.Add(new string[] { (i + 1).ToString(), recipeBookLibrary.AllRecipeBooks[i].Name });
            }

            UserInterface.DisplayOptionsMenu(recipeBooksToDisplay, out recipeBookOptions);
            Console.WriteLine();
            Console.Write("Enter the recipe book you would like to rename: ");
            int userOption = Int32.Parse(GetUserInput.GetUserOption(recipeBookOptions));

            RecipeBook recipeBookToRename = recipeBookLibrary.AllRecipeBooks[userOption - 1];
            string oldName = recipeBookToRename.Name;

            Console.WriteLine("");
            Console.Write($"Enter the new name for the {oldName} recipe book: ");
            string newName = GetUserInput.GetUserInputString(false);

            GetUserInput.AreYouSure($"rename the {oldName} recipe book to \"{newName}\"", out bool isSure);

            if (isSure)
            {
                recipeBookToRename.Name = newName;
                UserInterface.SuccessfulChange(true, $"{oldName} recipe book", $"renamed {newName}");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "recipe book", "renamed");
            }
        }

        private static void DeleteRecipeBook(RecipeBookLibrary recipeBookLibrary)
        {
            Console.Clear();
            Console.WriteLine("---------- DELETE RECIPE BOOK ----------");
            Console.WriteLine();

            List<string[]> recipeBooksToDisplay = new List<string[]>();
            List<string> recipeBookOptions = new List<string>();

            for (int i = 0; i < recipeBookLibrary.AllRecipeBooks.Length; i++)
            {
                recipeBooksToDisplay.Add(new string[] { (i + 1).ToString(), recipeBookLibrary.AllRecipeBooks[i].Name });
            }

            UserInterface.DisplayOptionsMenu(recipeBooksToDisplay, out recipeBookOptions);
            Console.WriteLine();
            Console.Write("Enter the recipe book you would like to delete: ");
            int userOption = Int32.Parse(GetUserInput.GetUserOption(recipeBookOptions));

            RecipeBook recipeBookToDelete = recipeBookLibrary.AllRecipeBooks[userOption - 1];

            GetUserInput.AreYouSure($"delete the {recipeBookToDelete.Name} recipe book", out bool isSure);

            if (isSure)
            {
                recipeBookLibrary.DeleteRecipeBook(recipeBookToDelete);
                UserInterface.SuccessfulChange(true, $"{recipeBookToDelete.Name} recipe book", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "recipe book", "deleted");
            }
        }

        private static void AddNewRecipe(RecipeBookLibrary recipeBookLibrary, RecipeBook recipeBook)
        {
            Metadata recipeMetadata = GetUserInput.GetMetadataFromUser();
            IngredientList recipeIngredientList = GetUserInput.GetIngredientsFromUser(recipeBookLibrary);
            CookingInstructions recipeCookingInstructions = GetUserInput.GetCookingInstructionsFromUser();

            Recipe newRecipe = new Recipe(recipeMetadata, recipeCookingInstructions, recipeIngredientList);

            GetUserInput.AreYouSure($"add this new recipe", out bool isSure);

            if (isSure)
            {
                recipeBook.AddRecipe(newRecipe);
                UserInterface.SuccessfulChange(true, $"new recipe", "added to the recipe book");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "new recipe", "added");
            }
        }

        private static void EditRecipe(RecipeBookLibrary recipeBookLibrary, Recipe recipe)
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

            string fieldToEdit = GetUserInput.GetTheFieldToEditFromUser(recipe, editRecipeOptions);

            switch (fieldToEdit)
            {
                case "1":
                    EditRecipeTitle(recipe);
                    break;
                case "2":
                    EditRecipeNotes(recipe);
                    break;
                case "3":
                    EditRecipePrepTimes(recipe);
                    break;
                case "4":
                    EditRecipeEstimatedServings(recipe);
                    break;
                case "5":
                    EditRecipeFoodTypeGenre(recipe);
                    break;
                case "6":
                    EditRecipeIngredients(recipeBookLibrary, recipe);
                    break;
                case "7":
                    EditRecipeInstructions(recipe);
                    break;
                case "R":
                    return;
            }
        }

        private static void EditExistingRecipe(RecipeBookLibrary recipeBookLibrary, RecipeBook recipeBook)
        {
            string header = "---------- EDIT RECIPE ----------";
            UserInterface.DisplayMenuHeader(header);

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string> recipeOptions = new List<string>();

            for (int i = 0; i < recipeBook.Recipes.Length; i++)
            {
                recipesToDisplay.Add(new string[] { (i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title });
            }

            UserInterface.DisplayOptionsMenu(recipesToDisplay, out recipeOptions);
            Console.WriteLine();
            Console.Write("Enter the recipe you would like to edit: ");
            int userOption = int.Parse(GetUserInput.GetUserOption(recipeOptions));

            Recipe recipeToEdit = recipeBook.Recipes[userOption - 1];

            EditRecipe(recipeBookLibrary, recipeToEdit);
        }

        private static void DeleteOpenRecipe(RecipeBook recipeBook, Recipe recipeToDelete)
        {
            Console.Clear();
            Console.WriteLine("---------- DELETE RECIPE ----------");
            Console.WriteLine();
            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines($"Recipe Title: {recipeToDelete.Metadata.Title}"));

            GetUserInput.AreYouSure($"delete this recipe", out bool isSure);

            if (isSure)
            {
                recipeBook.DeleteRecipe(recipeToDelete);
                UserInterface.SuccessfulChange(true, $"recipe", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "recipe", "deleted");
            }
        }

        public static void DeleteExistingRecipe(RecipeBook recipeBook)
        {
            string header = "---------- DELETE RECIPE ----------";
            UserInterface.DisplayMenuHeader(header);

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string> recipeOptions = new List<string>();

            for (int i = 0; i < recipeBook.Recipes.Length; i++)
            {
                recipesToDisplay.Add(new string[] { (i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title });
            }

            UserInterface.DisplayOptionsMenu(recipesToDisplay, out recipeOptions);
            Console.WriteLine();
            Console.Write("Enter the recipe you would like to delete: ");
            int userOption = int.Parse(GetUserInput.GetUserOption(recipeOptions));

            Recipe recipeToDelete = recipeBook.Recipes[userOption - 1];
            string recipeTitle = recipeToDelete.Metadata.Title;

            GetUserInput.AreYouSure($"delete the {recipeTitle} recipe", out bool isSure);

            if (isSure)
            {
                recipeBook.DeleteRecipe(recipeToDelete);
                UserInterface.SuccessfulChange(true, $"recipe", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "recipe", "deleted");
            }
        }

        public static void EditRecipeTitle(Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            Console.Write("Enter the new title for this recipe: ");
            string newTitle = GetUserInput.GetUserInputString(false);

            GetUserInput.AreYouSure($"change the name of this recipe to {newTitle}", out bool isSure);

            if (isSure)
            {
                recipe.Metadata.Title = newTitle;
                UserInterface.SuccessfulChange(true, "recipe", "renamed");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "recipe", "renamed");
            }
        }

        public static void EditRecipeNotes(Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            string userInput = "";
            if (recipe.Metadata.Notes != "")
            {
                Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Do you want to add to the existing notes or delete the old notes and add a new note?"));
                Console.WriteLine();
                Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Enter \"A\" to Add to the existing notes or \"D\" to Delete the old notes and add a new note:"));
                List<string> options = new List<string>() { "A", "D" };
                userInput = GetUserInput.GetUserOption(options);
            }

            Console.WriteLine();
            string newNotes = GetUserInput.GetNewUserNotes();

            GetUserInput.AreYouSure("add these new notes to the recipe", out bool isSure);

            if (isSure)
            {
                if (userInput == "D")
                {
                    recipe.Metadata.Notes = "";
                }
                recipe.Metadata.Notes += " " + newNotes;
                UserInterface.SuccessfulChange(true, "recipe notes", "updated", true);
            }
            else
            {
                UserInterface.SuccessfulChange(false, "recipe notes", "updated", true);
            }
        }

        public static void EditRecipePrepTimes(Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Do you want to change the Prep Time, the Cook Time, or Both?"));
            Console.WriteLine();
            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Enter \"P\" to change the Prep Time, \"C\" to change the Cook Time, or \"B\" to change both:"));
            List<string> options = new List<string>() { "P", "C", "B" };
            string userInput = GetUserInput.GetUserOption(options);

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
                    Console.WriteLine();
                    Console.Write("Enter the new Prep Time: ");
                    newPrepTime = GetUserInput.GetUserInputInt(1);
                }
                else
                {
                    Console.WriteLine();
                    Console.Write("Enter the new Cook Time: ");
                    newCookTime = GetUserInput.GetUserInputInt(1);
                }
            }

            GetUserInput.AreYouSure($"update the recipe preparation times", out bool isSure);

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

                UserInterface.SuccessfulChange(true, "recipe preparation times", "updated", true);
            }
            else
            {
                UserInterface.SuccessfulChange(false, "recipe preparation times", "updated", true);
            }
        }

        public static void EditRecipeEstimatedServings(Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Do you want to change the Low # of Servings, High # of Servings, or Both?"));
            Console.WriteLine();
            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Enter \"L\" to change the Low # of Servings, \"H\" to change the High # of Servings, or \"B\" to change both:"));
            List<string> options = new List<string>() { "L", "H", "B" };
            string userInput = GetUserInput.GetUserOption(options);

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
                    Console.WriteLine();
                    Console.Write("Enter the new Low # of Servings: ");
                    newLowServings = GetUserInput.GetUserInputInt(1);
                }
                else
                {
                    Console.WriteLine();
                    Console.Write("Enter the new High # of Servings: ");
                    newHighServings = GetUserInput.GetUserInputInt(1);
                }
            }

            GetUserInput.AreYouSure($"update the recipe estimated servings", out bool isSure);

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

                UserInterface.SuccessfulChange(true, "recipe estimated servings", "updated", true);
            }
            else
            {
                UserInterface.SuccessfulChange(false, "recipe estimated servings", "updated", true);
            }
        }

        public static void EditRecipeFoodTypeGenre(Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Do you want to change the Food Type, Food Genre, or Both?"));
            Console.WriteLine();
            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Enter \"T\" to change the Food Type, \"G\" to change the Food Genre, or \"B\" to change both:"));
            List<string> options = new List<string>() { "T", "G", "B" };
            string userInput = GetUserInput.GetUserOption(options);

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
                    Console.WriteLine();
                    Console.Write("Enter the new Food Type: ");
                    newFoodType = GetUserInput.GetUserInputString(true);
                }
                else
                {
                    Console.WriteLine();
                    Console.Write("Enter the new Food Genre: ");
                    newFoodGenre = GetUserInput.GetUserInputString(true);
                }
            }

            GetUserInput.AreYouSure($"update the recipe food type/genre", out bool isSure);

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

                UserInterface.SuccessfulChange(true, "recipe food type/genre", "updated", false);
            }
            else
            {
                UserInterface.SuccessfulChange(false, "recipe food type/genre", "updated", false);
            }
        }

        public static void EditRecipeIngredients(RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);
            Console.Write(recipe.Ingredients.ProduceIngredientsText(true, true));

            List<string[]> menuOptions = new List<string[]>()
            {
                new string[] { "A", "Add a New Igredient" },
                new string[] { "E", "Edit an Ingredient"},
                new string[] { "D", "Delete an Ingredient"},
                new string[] { "R", "Return to Previous Menu"},
            };
            List<string> options = new List<string>();

            if (recipe.Ingredients.AllIngredients.Length == 0)
            {
                menuOptions.RemoveAt(1);
                menuOptions.RemoveAt(1);
            }

            Console.WriteLine();
            UserInterface.DisplayOptionsMenu(menuOptions, out options);
            Console.WriteLine();
            Console.Write("Select an editing option: ");
            string userOption = GetUserInput.GetUserOption(options);

            Console.WriteLine();
            switch (userOption)
            {
                case "A":
                    AddNewIngredient(recipeBookLibrary, recipe);
                    break;
                case "E":
                    EditExistingIngredient(recipeBookLibrary, recipe);
                    break;
                case "D":
                    DeleteExistingIngredient(recipe);
                    break;
                case "R":
                    return;
            }
        }

        public static void EditExistingIngredient(RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            Ingredient[] allRecipeIngredients = recipe.Ingredients.AllIngredients;
            List<string> ingredientLineOptions = new List<string>();
            for (int i = 0; i < allRecipeIngredients.Length; i++)
            {
                ingredientLineOptions.Add((i + 1).ToString());
            }

            Ingredient ingredientToEdit;

            if (allRecipeIngredients.Length > 1)
            {
                Console.WriteLine("Select the ingredient line you would like to edit:");
                string userOption = GetUserInput.GetUserOption(ingredientLineOptions);
                Console.WriteLine();
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

            Console.WriteLine("Select the part of the ingredient you would like to edit:");
            UserInterface.DisplayOptionsMenu(ingredientComponentsForMenu, out ingredientComponentOptions);
            string ingredientComponentToEdit = GetUserInput.GetUserOption(ingredientComponentOptions);

            Console.WriteLine();
            string measurementUnit = "";
            string newPrepNote;
            switch (ingredientComponentToEdit)
            {
                case "1":
                    Console.Write("Enter the new quantity of the ingredient (ex: 1.5): ");
                    double newQuantity = GetUserInput.GetUserInputDouble(2);
                    ingredientToEdit.Quantity = newQuantity;
                    UserInterface.SuccessfulChange(true, "ingredient quantity", "updated");
                    break;
                case "2":
                    List<string[]> measurementUnits = MeasurementUnits.AllMeasurementUnitsForUserInput(recipeBookLibrary);
                    List<string> options = new List<string>();
                    Console.WriteLine("Select the new ingredient measurement unit from the list of options:");
                    UserInterface.DisplayOptionsMenu(measurementUnits, out options);

                    int userOptionNumber = int.Parse(GetUserInput.GetUserOption(options));

                    if (userOptionNumber == options.Count)
                    {
                        GetUserInput.GetNewMeasurementUnitFromUser(out measurementUnit);
                        recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                        Console.WriteLine();
                        Console.WriteLine(UserInterface.MakeStringConsoleLengthLines($"Success! New measurement unit, {measurementUnit}, was added and will be used for this ingredient."));
                    }
                    else if (measurementUnits[userOptionNumber - 1][1] != "None")
                    {
                        measurementUnit = measurementUnits[userOptionNumber - 1][1];
                    }
                    ingredientToEdit.MeasurementUnit = measurementUnit;
                    UserInterface.SuccessfulChange(true, "ingredient meaurement unit", "updated");
                    break;
                case "3":
                    Console.WriteLine("Enter the new ingredient name:");
                    string newName = GetUserInput.GetUserInputString(false);
                    ingredientToEdit.Name = newName;
                    UserInterface.SuccessfulChange(true, "ingredient name", "updated");
                    break;
                case "4":
                    Console.WriteLine("Enter the new ingredient preparation note (or press \"Enter\" to leave blank):");
                    newPrepNote = GetUserInput.GetUserInputString(true);
                    ingredientToEdit.PreparationNote = newPrepNote;
                    UserInterface.SuccessfulChange(true, "ingredient preparation note", "updated");
                    break;
                default:
                    break;
            }
        }

        public static void AddNewIngredient(RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            Ingredient ingredientToAdd = GetUserInput.GetIngredientFromUser(recipeBookLibrary);
            recipe.Ingredients.AddIngredient(ingredientToAdd);
            UserInterface.SuccessfulChange(true, "ingredient", "added");
        }

        public static void DeleteExistingIngredient(Recipe recipe)
        {
            Console.WriteLine("Select the ingredient line you would like to delete:");

            Ingredient[] allRecipeIngredients = recipe.Ingredients.AllIngredients;
            List<string> ingredientLineOptions = new List<string>();
            for (int i = 1; i <= allRecipeIngredients.Length; i++)
            {
                ingredientLineOptions.Add(i.ToString());
            }

            string userOption = GetUserInput.GetUserOption(ingredientLineOptions);
            int indexOfIngredientToDelete = int.Parse(userOption) - 1;

            GetUserInput.AreYouSure("delete this ingredient", out bool isSure);

            if (isSure)
            {
                recipe.Ingredients.DeleteIngredient(recipe.Ingredients.AllIngredients[indexOfIngredientToDelete]);
                UserInterface.SuccessfulChange(true, "ingredient", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "ingredient", "deleted");
            }
        }

        public static void EditRecipeInstructions(Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            Console.Write(recipe.CookingInstructions.ProduceInstructionsText(true));
            Console.WriteLine();

            List<string[]> menuOptions = new List<string[]>()
            {
                new string[] { "A", "Add New Instruction Block" },
                new string[] { "E", "Edit an Instruction Block"},
                new string[] { "D", "Delete an Instruction Block"},
                new string[] { "R", "Return to Previous Menu"},
            };
            List<string> options = new List<string>();

            if (recipe.CookingInstructions.InstructionBlocks.Length == 0)
            {
                menuOptions.RemoveAt(1);
                menuOptions.RemoveAt(1);
            }

            UserInterface.DisplayOptionsMenu(menuOptions, out options);
            Console.WriteLine();
            Console.Write("Select an editing option: ");
            string userOption = GetUserInput.GetUserOption(options);

            Console.WriteLine();
            switch (userOption)
            {
                case "A":
                    AddNewInstructionBlock(recipe);
                    break;
                case "E":
                    EditExistingInstructionBlock(recipe);
                    break;
                case "D":
                    DeleteExistingInstructionBlock(recipe);
                    break;
                case "R":
                    return;
            }
        }

        public static void AddNewInstructionBlock(Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            InstructionBlock newInstructionBlock = GetUserInput.GetInstructionBlockFromUser();

            GetUserInput.AreYouSure("add this new instruction block", out bool isSure);

            if (isSure)
            {
                recipe.CookingInstructions.AddInstructionBlock(newInstructionBlock);
                UserInterface.SuccessfulChange(true, "new instruction block", "added");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "new instruction block", "added");
            }
        }

        public static void EditExistingInstructionBlock(Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            Console.Write(recipe.CookingInstructions.ProduceInstructionsText(true, true));
            InstructionBlock instructionBlockToEdit;
            int numberOfInstructionBlocks = recipe.CookingInstructions.InstructionBlocks.Length;
            List<string> instructionBlockOptions = new List<string>();
            for (int i = 1; i <= numberOfInstructionBlocks; i++)
            {
                instructionBlockOptions.Add(i.ToString());
            }

            if (numberOfInstructionBlocks > 1)
            {
                Console.WriteLine();
                Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Enter the instruction block you would like to edit:"));

                string userBlockSelection = GetUserInput.GetUserOption(instructionBlockOptions);
                int instructionBlockIndex = int.Parse(userBlockSelection);

                instructionBlockToEdit = recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex - 1];
            }
            else if (numberOfInstructionBlocks == 1)
            {
                instructionBlockToEdit = recipe.CookingInstructions.InstructionBlocks[0];
            }
            else
            {
                Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("This recipe does not have any instruction blocks. Add a new instruction block in order to edit the recipe."));
                Console.WriteLine();
                Console.WriteLine("Press \"Enter\" to continue...");
                Console.ReadLine();

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

            bool instructionLinesAreBlank = instructionBlockToEdit.InstructionLines.Length == 0;
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

            Console.WriteLine();
            UserInterface.DisplayOptionsMenu(editBlockMenuOptions, out editBlockOptions);
            Console.WriteLine();
            Console.Write("Enter an editing option from the menu: ");
            string editBlockOption = GetUserInput.GetUserOption(editBlockOptions);
            string menuSelection = editBlockMenuOptions[int.Parse(editBlockOption) - 1][1];

            Console.WriteLine();
            switch (menuSelection)
            {
                case "Add Instruction Line":
                    AddInstructionLine(instructionBlockToEdit, recipe);
                    break;
                case "Edit Instruction Line":
                    EditInstructionLine(instructionBlockToEdit, recipe);
                    break;
                case "Delete Instruction Line":
                    DeleteInstructionLine(instructionBlockToEdit, recipe);
                    break;
                case "Add Block Heading":
                    AddInstructionBlockHeading(instructionBlockToEdit, recipe);
                    break;
                case "Edit Block Heading":
                    EditInstructionBlockHeading(instructionBlockToEdit, recipe);
                    break;
                case "Delete Block Heading":
                    DeleteInstructionBlockHeading(instructionBlockToEdit, recipe);
                    break;
                default:
                    break;
            }
        }

        public static void AddInstructionLine(InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            UserInterface.DisplayInstructionBlock(instructionBlock);

            Console.WriteLine();
            Console.WriteLine("Enter the new instruction line to add:");
            string newInstructionLine = GetUserInput.GetUserInputString(false);

            instructionBlock.AddInstructionLine(newInstructionLine);
            UserInterface.SuccessfulChange(true, "new instruction line", "added");
        }

        public static void EditInstructionLine(InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            UserInterface.DisplayInstructionBlock(instructionBlock);

            string[] allInstructionLines = instructionBlock.InstructionLines;
            List<string> instructionLineOptions = new List<string>();

            for (int i = 1; i <= allInstructionLines.Length; i++)
            {
                instructionLineOptions.Add(i.ToString());
            }
            Console.WriteLine();
            Console.Write("Select the instruction line to edit: ");
            string instructionLineSelected = GetUserInput.GetUserOption(instructionLineOptions);

            Console.WriteLine();
            Console.WriteLine("Enter the new text for the instruction line:");
            string newInstructionLineText = GetUserInput.GetUserInputString(false);

            instructionBlock.EditInstructionLine(int.Parse(instructionLineSelected) - 1, newInstructionLineText);
            UserInterface.SuccessfulChange(true, "instruction line", "edited");
        }

        public static void DeleteInstructionLine(InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            UserInterface.DisplayInstructionBlock(instructionBlock);

            string[] allInstructionLines = instructionBlock.InstructionLines;
            List<string> instructionLineOptions = new List<string>();

            for (int i = 1; i <= allInstructionLines.Length; i++)
            {
                instructionLineOptions.Add(i.ToString());
            }
            Console.WriteLine();
            Console.Write("Select the instruction line to delete: ");
            string instructionLineSelected = GetUserInput.GetUserOption(instructionLineOptions);

            GetUserInput.AreYouSure("delete this instruction line", out bool isSure);

            if (isSure)
            {
                instructionBlock.DeleteInstructionLine(int.Parse(instructionLineSelected) - 1);
                UserInterface.SuccessfulChange(true, "instruction line", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "instruction line", "deleted");
            }
        }

        public static void AddInstructionBlockHeading(InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            UserInterface.DisplayInstructionBlock(instructionBlock);

            Console.WriteLine();
            Console.WriteLine("Enter the new block heading to add:");
            string newBlockHeading = GetUserInput.GetUserInputString(false);

            instructionBlock.BlockHeading = newBlockHeading;
            UserInterface.SuccessfulChange(true, "new block heading", "added");
        }

        public static void EditInstructionBlockHeading(InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            UserInterface.DisplayInstructionBlock(instructionBlock);

            Console.WriteLine();
            Console.WriteLine("Enter the new block heading:");
            string newBlockHeading = GetUserInput.GetUserInputString(false);

            instructionBlock.BlockHeading = newBlockHeading;
            UserInterface.SuccessfulChange(true, "block heading", "edited");
        }

        public static void DeleteInstructionBlockHeading(InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            UserInterface.DisplayInstructionBlock(instructionBlock);

            GetUserInput.AreYouSure("delete the block heading", out bool isSure);

            if (isSure)
            {
                instructionBlock.BlockHeading = "";
                UserInterface.SuccessfulChange(true, "block heading", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "block heading", "deleted");
            }
        }

        public static void DeleteExistingInstructionBlock(Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            Console.Write(recipe.CookingInstructions.ProduceInstructionsText(true, true));
            Console.WriteLine();
            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Enter the instruction block you would like to delete:"));

            int numberOfInstructionBlocks = recipe.CookingInstructions.InstructionBlocks.Length;
            List<string> instructionBlockOptions = new List<string>();
            for (int i = 1; i <= numberOfInstructionBlocks; i++)
            {
                instructionBlockOptions.Add(i.ToString());
            }

            string userBlockSelection = GetUserInput.GetUserOption(instructionBlockOptions);

            GetUserInput.AreYouSure("delete this instruction block", out bool isSure);

            if (isSure)
            {
                recipe.CookingInstructions.DeleteInstructionBlock(int.Parse(userBlockSelection) - 1);
                UserInterface.SuccessfulChange(true, "instruction block", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(false, "instruction block", "deleted");
            }
        }

        public static void AddExistingRecipeToShoppingList(RecipeBook recipeBook, ShoppingList shoppingList)
        {
            string header = "---------- ADD RECIPE TO SHOPPING LIST ----------";
            UserInterface.DisplayMenuHeader(header);

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string> recipeOptions = new List<string>();

            for (int i = 0; i < recipeBook.Recipes.Length; i++)
            {
                recipesToDisplay.Add(new string[] { (i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title });
            }

            //Adds the "R" option to return to previous menu
            recipesToDisplay.Add(new string[] { "R", "Return to Previous Menu" });

            UserInterface.DisplayOptionsMenu(recipesToDisplay, out recipeOptions);
            Console.WriteLine();
            Console.Write("Enter the recipe you would like to add to the shopping list: ");
            string userOption = GetUserInput.GetUserOption(recipeOptions);

            if (userOption != "R")
            {
                int recipeOption = int.Parse(userOption);

                Recipe recipeToAdd = recipeBook.Recipes[recipeOption - 1];

                AddRecipeToShoppingList(shoppingList, recipeToAdd);
            }
        }

        public static void AddRecipeToShoppingList(ShoppingList shoppingList, Recipe recipe)
        {
            string header = "-------- ADD RECIPE TO SHOPPING LIST --------";
            UserInterface.DisplayMenuHeader(header);

            Ingredient[] recipeIngredients = recipe.Ingredients.AllIngredients;

            foreach (Ingredient element in recipeIngredients)
            {
                if (element.StoreLocation == "")
                {
                    GetStoreLocationForIngredient(shoppingList, element);
                }

                AddIngredientToStoreLocation(shoppingList, element);
            }

            UserInterface.SuccessfulChange(true, "recipe", "added to the shopping list");
        }

        public static void GetStoreLocationForIngredient(ShoppingList shoppingList, Ingredient ingredient)
        {
            string header = "-------- SET STORE LOCATION FOR INGREDIENT --------";
            string message = UserInterface.MakeStringConsoleLengthLines($"INGREDIENT: {ingredient.Name}");
            UserInterface.DisplayMenuHeader(header, message);
            Console.WriteLine("Which department is this ingredient generally found in at the store?");

            List<string[]> menuOptions = new List<string[]>();
            List<string> optionChoices = new List<string>();

            for (int i = 0; i < shoppingList.StoreLocations.Length; i++)
            {
                menuOptions.Add(new string[] { $"{i + 1}", shoppingList.StoreLocations[i] });
            }

            UserInterface.DisplayOptionsMenu(menuOptions, out optionChoices);
            Console.WriteLine();
            Console.WriteLine("Select the store location of this ingredient:");
            string userOption = GetUserInput.GetUserOption(optionChoices);
            Console.WriteLine();

            string storeLocation = shoppingList.StoreLocations[int.Parse(userOption) - 1];

            ingredient.StoreLocation = storeLocation;
        }

        public static void AddIngredientToStoreLocation(ShoppingList shoppingList, Ingredient ingredient)
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
                        ingredientsAreTheSame = AreIngredientsTheSame(currentIngredientName, newIngredientName);

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

        public static bool AreIngredientsTheSame(string currentIngredientName, string newIngredientName)
        {
            bool ingredientsAreTheSame = false;
            string header = "-------- SIMILAR INGREDIENTS FOUND --------";
            UserInterface.DisplayMenuHeader(header);
            Console.WriteLine();
            Console.WriteLine("The following ingredients might match:");
            Console.WriteLine();
            Console.WriteLine($"<<Ingredient Already On Shopping List>>{Environment.NewLine}{currentIngredientName}");
            Console.WriteLine();
            Console.WriteLine($"<<New Ingredient>>{Environment.NewLine}{newIngredientName}");
            Console.WriteLine();
            Console.WriteLine("Are these ingredients the same? Enter \"Y\" for Yes or \"N\" for No:");
            List<string> userOptions = new List<string>() { "Y", "N" };
            string userOption = GetUserInput.GetUserOption(userOptions);

            if (userOption == "Y")
            {
                ingredientsAreTheSame = true;
            }

            return ingredientsAreTheSame;
        }
    }
}
