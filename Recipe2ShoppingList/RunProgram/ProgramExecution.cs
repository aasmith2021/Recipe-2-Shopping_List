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
                        //Rename recipe book
                        break;
                    case "D":
                        //Delete recipe book
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
                        //Add a new recipe
                        break;
                    case "S":
                        //Add a recipe to the shopping list
                        break;
                    case "E":
                        //Edit existing recipe
                        break;
                    case "D":
                        //Delete existing recipe
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
                    //Delete this recipe
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
                GetUserInput.SuccessfulChange(true, $"new recipe book, {bookName},", "created");
            }
            else
            {
                GetUserInput.SuccessfulChange(false, "new recipe book", "created");
            }
        }

        private static void DeleteRecipeBook(RecipeBookLibrary recipeBookLibrary)
        {
            //TODO: Add delete recipe book functionality
            //Console.Clear();
            //Console.WriteLine("---------- DELETE RECIPE BOOK ----------");
            //Console.WriteLine();
            //Console.Write("Enter a name for the new recipe book: ");
            //string bookName = GetUserInput.GetUserInputString(false);

            //GetUserInput.AreYouSure($"create a new recipe book named {bookName}", out bool isSure);

            //if (isSure)
            //{
            //    RecipeBook newRecipeBook = new RecipeBook(bookName);
            //    recipeBookLibrary.AddRecipeBook(newRecipeBook);
            //    GetUserInput.SuccessfulChange(true, $"new recipe book, {bookName},", "created");
            //}
            //else
            //{
            //    GetUserInput.SuccessfulChange(false, "new recipe book", "created");
            //}
        }
    }
}
