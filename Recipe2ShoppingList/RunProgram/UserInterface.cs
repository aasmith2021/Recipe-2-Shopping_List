using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class UserInterface
    {
        private static void DisplayOptionsMenu(SortedList<string, string> options)
        {
            foreach (KeyValuePair<string, string> element in options)
            {
                Console.WriteLine(MakeStringConsoleLengthLines($"[{element.Key}] {element.Value}"));
            }
        }

        private static string MakeStringConsoleLengthLines(string originalStatement)
        {
            string convertedStatement = "";
            string tempStatement = "";
            int lineLengthMax = 80;
            int breakIndex = 0;
            int segmentCounter = 0;
            int counter = 0;

            if (originalStatement.Length <= lineLengthMax)
            {
                return originalStatement;
            }
            else
            {
                while (counter < originalStatement.Length)
                {
                    tempStatement = "";
                    for (int i = 0; (i < lineLengthMax && (i + (segmentCounter * lineLengthMax)) < originalStatement.Length); i++)
                    {
                        tempStatement += Convert.ToString(originalStatement[i + (segmentCounter * lineLengthMax)]);
                    }

                    for (int j = tempStatement.Length - 1; j >= 0; j--)
                    {
                        if (tempStatement[j] == ' ')
                        {
                            breakIndex = j;
                            break;
                        }
                    }

                    if (tempStatement.Length < lineLengthMax)
                    {
                        convertedStatement += tempStatement;
                        for (int l = 0; l < tempStatement.Length; l++)
                        {
                            counter++;
                        }
                    }
                    else
                    {
                        for (int k = 0; k < tempStatement.Length; k++)
                        {
                            if (k == breakIndex)
                            {
                                convertedStatement += $"{Environment.NewLine}";
                                counter++;
                            }
                            else
                            {
                                convertedStatement += Convert.ToString(tempStatement[k]);
                                counter++;
                            }
                        }
                    }

                    segmentCounter++;
                }
            }

            return convertedStatement;
        }

        public static void DisplayMainMenu(RecipeBookLibrary recipeBookLibrary, out List<string> mainMenuOptions)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Recipe-2-Shopping List Program!".ToUpper());
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine();

            SortedList<string, string> recipeBooksToDisplay = new SortedList<string, string>();
            SortedList<string, string> mainMenuStandardOptions = new SortedList<string, string>();

            if (recipeBookLibrary.AllRecipeBooks.Length == 0)
            {
                Console.WriteLine("You currently don't have any recipe books saved.");
                Console.WriteLine();
                Console.WriteLine("Add a new recipe book using the options below:");
                Console.WriteLine();

                mainMenuStandardOptions.Add("A", "Add New Recipe Book");
                mainMenuStandardOptions.Add("X", "Exit Program");
            }
            else
            {
                Console.WriteLine(MakeStringConsoleLengthLines("Open a recipe book by selecting it from the list below, or select an editing option:"));
                Console.WriteLine();

                for (int i = 0; i < recipeBookLibrary.AllRecipeBooks.Length; i++)
                {
                    recipeBooksToDisplay.Add((i + 1).ToString(), recipeBookLibrary.AllRecipeBooks[i].Name);
                }

                DisplayOptionsMenu(recipeBooksToDisplay);
                Console.WriteLine();

                mainMenuStandardOptions.Add("A", "Add New Recipe Book");
                mainMenuStandardOptions.Add("D", "Delete Existing Recipe Book");
                mainMenuStandardOptions.Add("R", "Rename Existing Recipe Book");
                mainMenuStandardOptions.Add("X", "Exit Program");
            }

            DisplayOptionsMenu(mainMenuStandardOptions);
            Console.WriteLine();

            mainMenuOptions = new List<string>();
            
            foreach (KeyValuePair<string, string> element in recipeBooksToDisplay)
            {
                mainMenuOptions.Add(element.Key);
            }

            foreach (KeyValuePair<string, string> element in mainMenuStandardOptions)
            {
                mainMenuOptions.Add(element.Key);
            }
        }

        public static void OpenRecipeBook(RecipeBook recipeBook, out List<string> recipeBookOptions)
        {
            Console.Clear();
            Console.WriteLine(MakeStringConsoleLengthLines($"Recipe Book: {recipeBook.Name}"));
            Console.WriteLine("----------------------------------------------");

            SortedList<string, string> recipesToDisplay = new SortedList<string, string>();
            SortedList<string, string> recipBookStandardOptions = new SortedList<string, string>();

            if (recipeBook.Recipes.Length == 0)
            {
                Console.WriteLine("You currently don't have any recipes saved.");
                Console.WriteLine();
                Console.WriteLine("Add a new recipe using the options below:");
                Console.WriteLine();

                recipBookStandardOptions.Add("A", "Add New Recipe");
                recipBookStandardOptions.Add("X", "Exit Program");
            }
            else
            {
                Console.WriteLine(MakeStringConsoleLengthLines("Open a recipe by selecting it from the list below, or select an editing option:"));
                Console.WriteLine();

                for (int i = 0; i < recipeBook.Recipes.Length; i++)
                {
                    recipesToDisplay.Add((i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title);
                }

                DisplayOptionsMenu(recipesToDisplay);
                Console.WriteLine();

                recipBookStandardOptions.Add("A", "Add New Recipe");
                recipBookStandardOptions.Add("D", "Delete Existing Recipe");
                recipBookStandardOptions.Add("E", "Edit Existing Recipe");
                recipBookStandardOptions.Add("X", "Exit Program");
            }

            recipeBookOptions = new List<string>();

            foreach (KeyValuePair<string, string> element in recipesToDisplay)
            {
                recipeBookOptions.Add(element.Key);
            }

            foreach (KeyValuePair<string, string> element in recipBookStandardOptions)
            {
                recipeBookOptions.Add(element.Key);
            }
        }

    }
}
