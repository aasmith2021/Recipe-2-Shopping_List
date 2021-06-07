using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class ProgramExecution
    {
        public static void RunProgram(out bool exitProgram)
        {
            exitProgram = false;

            //Load <recipeBookLibrary> into the program from the database file
            RecipeBookLibrary recipeBookLibrary = ReadFromFile.GetRecipeBookLibraryFromFile();

            while (!exitProgram)
            {
                RunMainMenu(recipeBookLibrary, out exitProgram);
            }

            //Save <recipeBookLibrary> to the "write" database file before closing program
            recipeBookLibrary.WriteRecipeBookLibraryToFile();

            //Delete original database, then rename the "write" database file to become the new master database file
            DataHelperMethods.DeleteOldDatabaseFileAndRenameNewDatabase(DataHelperMethods.GetReadDatabaseFilePath(), DataHelperMethods.GetWriteDatabaseFilePath());
        }

        private static void RunMainMenu(RecipeBookLibrary recipeBookLibrary, out bool exitProgram)
        {
            exitProgram = false;
            
            UserInterface.DisplayMainMenu(recipeBookLibrary, out List<string> mainMenuOptions);
            string userOption = GetUserInput.GetUserOption(mainMenuOptions);

            if (Int32.TryParse(userOption, out int userOptionNumber))
            {
                bool exitRecipeBookSection = false;

                while (!exitRecipeBookSection)
                {
                    RunRecipeBook(recipeBookLibrary, userOptionNumber, out exitRecipeBookSection, out exitProgram);
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
                    case "X":
                        exitProgram = true;
                        break;
                    default:
                        break;
                }
            }
        }
        
        private static void RunRecipeBook(RecipeBookLibrary recipeBookLibrary, int recipeBookOptionNumber, out bool exitRecipeBookSection, out bool exitProgram)
        {
            exitProgram = false;
            exitRecipeBookSection = false;
            
            RecipeBook recipeBookToOpen = recipeBookLibrary.AllRecipeBooks[recipeBookOptionNumber - 1];
            UserInterface.DisplayOpenRecipeBook(recipeBookToOpen, out List<string> recipeBookOptions);
            string userOption = GetUserInput.GetUserOption(recipeBookOptions);

            if (int.TryParse(userOption, out int userOptionNumber))
            {
                bool exitRecipeSection = false;
                
                while (!exitRecipeSection)
                {
                    RunRecipe(recipeBookLibrary, recipeBookToOpen, userOptionNumber, out exitRecipeSection, out exitRecipeBookSection, out exitProgram);
                }
            }
            else
            {
                switch (userOption)
                {
                    case "A":
                        AddNewRecipe(recipeBookLibrary, recipeBookToOpen);
                        break;
                    case "S":
                        //Add a recipe to the shopping list
                        break;
                    case "D":
                        DeleteExistingRecipe(recipeBookToOpen);
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

        private static void RunRecipe(RecipeBookLibrary recipeBookLibrary, RecipeBook recipeBook, int recipeOptionNumber, out bool exitRecipeSection, out bool exitRecipeBookSection, out bool exitProgram)
        {
            exitProgram = false;
            exitRecipeSection = false;
            exitRecipeBookSection = false;

            Recipe recipeToOpen = recipeBook.Recipes[recipeOptionNumber - 1];
            UserInterface.DisplayOpenRecipe(recipeToOpen, recipeBook, out List<string> recipeEditOptions);
            string userOption = GetUserInput.GetUserOption(recipeEditOptions);

            switch (userOption)
            {
                case "S":
                    //Add this recipe to the shopping list
                    break;
                case "E":
                    EditRecipe(recipeBookLibrary, recipeToOpen);
                    break;
                case "D":
                    DeleteOpenRecipe(recipeBook, recipeToOpen);
                    exitRecipeSection = true;
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
                new string[] { "7", "Instructions" }
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
                default:
                    break;
            }
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
            Console.Clear();
            Console.WriteLine("---------- DELETE RECIPE ----------");
            Console.WriteLine();

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

            switch(userInput)
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
            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Enter \"T\" to change the Low # of Servings, \"G\" to change the High # of Servings, or \"B\" to change both:"));
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
            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Enter \"A\" to Add a new ingredient, \"E\" to edit an existing ingredient, or \"D\" to delete an ingredient:"));
            List<string> options = new List<string>() { "A", "E", "D" };
            string userOption = GetUserInput.GetUserOption(options);

            Console.WriteLine();
            switch(userOption)
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
                default:
                    break;
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
            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines("Enter \"A\" to Add a new instruction block, \"E\" to edit an existing instruction block, or \"D\" to delete an instruction block:"));
            List<string> options = new List<string>() { "A", "E", "D" };
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
                default:
                    break;
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
            else
            {
                instructionBlockToEdit = recipe.CookingInstructions.InstructionBlocks[0];
            }
            
            List<string[]> editBlockMenuOptions = new List<string[]>()
            {
                new string[] { "1", "Add Instruction Line" },
                new string[] { "2", "Edit Instruction Line" },
                new string[] { "3", "Delete Instruction Line" },
                new string[] { "4", "Add Block Heading" },
                new string[] { "5", "Edit Block Heading" },
                new string[] { "6", "Delete Block Heading" },
            };

            if (instructionBlockToEdit.BlockHeading == "")
            {              
                editBlockMenuOptions.RemoveAt(5);
                editBlockMenuOptions.RemoveAt(4);
            }

            List<string> editBlockOptions = new List<string>();
            Console.WriteLine();
            UserInterface.DisplayOptionsMenu(editBlockMenuOptions, out editBlockOptions);
            Console.WriteLine();
            Console.Write("Enter an editing option from the menu: ");
            string editBlockOption = GetUserInput.GetUserOption(editBlockOptions);

            Console.WriteLine();
            switch (editBlockOption)
            {
                case "1":
                    AddInstructionLine(instructionBlockToEdit, recipe);
                    break;
                case "2":
                    EditInstructionLine(instructionBlockToEdit, recipe);
                    break;
                case "3":
                    DeleteInstructionLine(instructionBlockToEdit, recipe);
                    break;
                case "4":
                    AddInstructionBlockHeading(instructionBlockToEdit, recipe);
                    break;
                case "5":
                    EditInstructionBlockHeading(instructionBlockToEdit, recipe);
                    break;
                case "6":
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

            instructionBlock.instructionLines[int.Parse(instructionLineSelected) - 1] = newInstructionLineText;
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
    }
}
