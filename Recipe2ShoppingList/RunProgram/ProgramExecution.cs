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
            RecipeBookLibrary recipeBookLibrary = ReadFromFile.GetRecipeBookLibraryFromFile("Recipe_DatabaseNOTHERE");

            while (!exitProgram)
            {
                UserInterface.DisplayMainMenu(recipeBookLibrary, out List<string> mainMenuOptions);
                string mainMainOption = GetUserInput.GetUserOption(mainMenuOptions);

                if (Int32.TryParse(mainMainOption, out int recipeBookOptionNumber))
                {
                    RunRecipeBookSection(recipeBookLibrary, recipeBookOptionNumber, out exitProgram);
                }
                else
                {
                    switch (mainMainOption)
                    {
                        case "A":
                            //Add new recipe book
                            break;
                        case "D":
                            //Delete recipe book
                            break;
                        case "R":
                            //rename recipe book
                            break;
                        case "X":
                            exitProgram = true;
                            break;
                        default:
                            break;
                    }
                }
            }





            
            //The way to exit the program right now
            Console.WriteLine("Press ENTER to exit program");
            string input = Console.ReadLine();

            if (input == "")
            {
                exitProgram = true;
            }
        }

        private static void RunRecipeBookSection(RecipeBookLibrary recipeBookLibrary, int recipeBookOptionNumber, out bool exitProgram)
        {
            exitProgram = false;
            
            RecipeBook recipeBookToOpen = recipeBookLibrary.AllRecipeBooks[recipeBookOptionNumber - 1];
            UserInterface.OpenRecipeBook(recipeBookToOpen, out List<string> recipeBookOptions);
            string recipeBookOption = GetUserInput.GetUserOption(recipeBookOptions);

            if (Int32.TryParse(recipeBookOption, out int recipeOptionNumber))
            {

            }
            else
            {
                switch (recipeBookOption)
                {
                    case "A":
                        //Add new recipe
                        break;
                    case "D":
                        //Delete existing recipe
                        break;
                    case "E":
                        //Edit existing recipe
                        break;
                    case "X":
                        exitProgram = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
