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
            RecipeBookLibrary recipeBookLibrary = ReadFromFile.GetRecipeBookLibraryFromFile("test_database_alt");

            while (!exitProgram)
            {
                RunMainMenu(recipeBookLibrary, out exitProgram);
            }

            //Save <recipeBookLibrary> to database file before closing program
            recipeBookLibrary.WriteRecipeBookLibraryToFile("test_database_alt");
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

            if (Int32.TryParse(userOption, out int userOptionNumber))
            {
                bool exitRecipeSection = false;
                
                while (!exitRecipeSection)
                {
                    RunRecipe(recipeBookToOpen, userOptionNumber, out exitRecipeSection, out exitProgram);
                }
            }
            else
            {
                switch (userOption)
                {
                    case "A":
                        AddNewRecipe(recipeBookToOpen);
                        break;
                    case "S":
                        //Add a recipe to the shopping list
                        break;
                    case "E":
                        //Edit existing recipe
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

        private static void RunRecipe(RecipeBook recipeBook, int recipeOptionNumber, out bool exitRecipeSection, out bool exitProgram)
        {
            exitProgram = false;
            exitRecipeSection = false;

            Recipe recipeToOpen = recipeBook.Recipes[recipeOptionNumber - 1];
            UserInterface.DisplayOpenRecipe(recipeToOpen, recipeBook, out List<string> recipeEditOptions);
            string userOption = GetUserInput.GetUserOption(recipeEditOptions);

            switch (userOption)
            {
                case "S":
                    //Add this recipe to the shopping list
                    break;
                case "E":
                    //Edit this recipe
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
                recipeBookOptions.Add((i + 1).ToString());
            }

            UserInterface.DisplayOptionsMenu(recipeBooksToDisplay);
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
                recipeBookOptions.Add((i + 1).ToString());
            }

            UserInterface.DisplayOptionsMenu(recipeBooksToDisplay);
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

        private static void AddNewRecipe(RecipeBook recipeBook)
        {
            Metadata recipeMetadata = GetUserInput.GetMetadataFromUser();
            IngredientList recipeIngredientList = GetUserInput.GetIngredientsFomUser();
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
                recipeOptions.Add((i + 1).ToString());
            }

            UserInterface.DisplayOptionsMenu(recipesToDisplay);
            Console.WriteLine();
            Console.Write("Enter the recipe you would like to delete: ");
            int userOption = Int32.Parse(GetUserInput.GetUserOption(recipeOptions));

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
    }
}
