using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class UserInterface
    {
        public static void DisplayOptionsMenu(List<string[]> menuOptions, out List<string> optionChoices)
        {
            optionChoices = new List<string>();
            
            foreach (string[] element in menuOptions)
            {
                Console.WriteLine(MakeStringConsoleLengthLines($"[{element[0]}] {element[1]}"));
                optionChoices.Add(element[0]);
            }
        }

        public static string MakeStringConsoleLengthLines(string originalStatement)
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

            List<string[]> recipeBooksToDisplay = new List<string[]>();
            List<string[]> mainMenuStandardOptions = new List<string[]>();
            mainMenuOptions = new List<string>();

            if (recipeBookLibrary.AllRecipeBooks.Length == 0)
            {
                Console.WriteLine("You currently don't have any recipe books saved.");
                Console.WriteLine();
                Console.WriteLine("Add a new recipe book using the options below:");
                Console.WriteLine();

                mainMenuStandardOptions.Add(new string[] { "A", "Add New Recipe Book"});
                mainMenuStandardOptions.Add(new string[] { "M", "Manage Saved Measurement Units"});
                mainMenuStandardOptions.Add(new string[] { "X", "Save and Exit Program" });
            }
            else
            {
                Console.WriteLine("----- RECIPE BOOKS -----");

                for (int i = 0; i < recipeBookLibrary.AllRecipeBooks.Length; i++)
                {
                    recipeBooksToDisplay.Add(new string[] { (i + 1).ToString(), recipeBookLibrary.AllRecipeBooks[i].Name });
                }

                DisplayOptionsMenu(recipeBooksToDisplay, out mainMenuOptions);
                Console.WriteLine();

                mainMenuStandardOptions.Add(new string[] { "A", "Add New Recipe Book"} );
                mainMenuStandardOptions.Add(new string[] { "R", "Rename Existing Recipe Book" });
                mainMenuStandardOptions.Add(new string[] { "D", "Delete Existing Recipe Book" });
                mainMenuStandardOptions.Add(new string[] { "V", "View Shopping List" });
                mainMenuStandardOptions.Add(new string[] { "M", "Manage Saved Measurement Units" });
                mainMenuStandardOptions.Add(new string[] { "X", "Save and Exit Program" });
            }

            Console.WriteLine();
            Console.WriteLine("----- OPTIONS -----");
            DisplayOptionsMenu(mainMenuStandardOptions, out List<string> standardOptionsToAdd);
            Console.WriteLine();
            
            if (recipeBookLibrary.AllRecipeBooks.Length != 0)
            {
                Console.WriteLine();
                Console.WriteLine(MakeStringConsoleLengthLines("Open a recipe book by entering its number, or select an option from the menu:"));
            }
            mainMenuOptions.AddRange(standardOptionsToAdd);
        }

        public static void DisplayOpenRecipeBook(RecipeBook recipeBook, out List<string> recipeBookOptions)
        {
            Console.Clear();
            Console.WriteLine(MakeStringConsoleLengthLines($"Recipe Book: {recipeBook.Name}"));
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine();

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string[]> recipeBookStandardOptions = new List<string[]>();
            recipeBookOptions = new List<string>();

            if (recipeBook.Recipes.Length == 0)
            {
                Console.WriteLine("You currently don't have any recipes in this recipe book.");
                Console.WriteLine();
                Console.WriteLine("Add a new recipe using the options below:");
                Console.WriteLine();

                recipeBookStandardOptions.Add(new string[] { "A", "Add New Recipe" });
                recipeBookStandardOptions.Add(new string[] { "V", "View Shopping List" });
                recipeBookStandardOptions.Add(new string[] { "R", "Return to Previous Menu" });
                recipeBookStandardOptions.Add(new string[] { "X", "Save and Exit Program" });
            }
            else
            {
                Console.WriteLine("----- RECIPES -----");

                for (int i = 0; i < recipeBook.Recipes.Length; i++)
                {
                    recipesToDisplay.Add(new string[] { (i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title });
                }

                DisplayOptionsMenu(recipesToDisplay, out recipeBookOptions);
                Console.WriteLine();

                recipeBookStandardOptions.Add(new string[] { "A", "Add New Recipe" });
                recipeBookStandardOptions.Add(new string[] { "E", "Edit Existing Recipe" });
                recipeBookStandardOptions.Add(new string[] { "D", "Delete Existing Recipe" });
                recipeBookStandardOptions.Add(new string[] { "S", "Add a Recipe to the Shopping List" });
                recipeBookStandardOptions.Add(new string[] { "V", "View Shopping List" });
                recipeBookStandardOptions.Add(new string[] { "R", "Return to Previous Menu" });
                recipeBookStandardOptions.Add(new string[] { "X", "Save and Exit Program" });
            }

            Console.WriteLine();
            Console.WriteLine("----- OPTIONS -----");
            DisplayOptionsMenu(recipeBookStandardOptions, out List<string> standardOptionsToAdd);
            Console.WriteLine();

            if (recipeBook.Recipes.Length != 0)
            {
                Console.WriteLine();
                Console.WriteLine(MakeStringConsoleLengthLines("Open a recipe by entering its number, or select an option from the menu:"));
            }
            recipeBookOptions.AddRange(standardOptionsToAdd);
        }

        public static void DisplayOpenRecipe(Recipe recipe, RecipeBook recipeBook, out List<string> recipeOptions)
        {
            Console.Clear();
            Console.WriteLine(MakeStringConsoleLengthLines($"Recipe Book: {recipeBook.Name}"));
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine();
            Console.WriteLine(recipe.ProduceRecipeText(true));
            Console.WriteLine();
            Console.WriteLine("----- OPTIONS -----");

            List<string[]> recipeStardardOptions = new List<string[]>();

                recipeStardardOptions.Add(new string[] { "S", "Add This Recipe to the Shopping List" });
                recipeStardardOptions.Add(new string[] { "E", "Edit This Recipe" });
                recipeStardardOptions.Add(new string[] { "D", "Delete This Recipe" });
                recipeStardardOptions.Add(new string[] { "V", "View Shopping List" });
                recipeStardardOptions.Add(new string[] { "R", "Return to Previous Menu" });
                recipeStardardOptions.Add(new string[] { "X", "Save and Exit Program" });

            DisplayOptionsMenu(recipeStardardOptions, out recipeOptions);
            Console.WriteLine();
        }

        public static void SuccessfulChange(bool changeConfirmed, string changeNoun, string changeVerb, bool isPluralNoun = false)
        {

            Console.WriteLine();
            if (changeConfirmed && !isPluralNoun)
            {
                Console.WriteLine($"Success! The {changeNoun} was {changeVerb}.");
            }
            else if (!changeConfirmed && !isPluralNoun)
            {
                Console.WriteLine($"No worries! The {changeNoun} was not {changeVerb}.");
            }
            else if (changeConfirmed && isPluralNoun)
            {
                Console.WriteLine($"Success! The {changeNoun} were {changeVerb}.");
            }
            else
            {
                Console.WriteLine($"No worries! The {changeNoun} were not {changeVerb}.");
            }
            Console.WriteLine();
            Console.WriteLine("Press \"Enter\" to continue...");
            Console.ReadLine();
        }

        public static void DisplayMenuHeader(string header, string additionalMessage = "")
        {
            Console.Clear();
            Console.WriteLine(header);
            Console.WriteLine();

            if (additionalMessage != "")
            {
                Console.WriteLine(additionalMessage);
                Console.WriteLine();
            }
        }

        public static void DisplayInstructionBlock(InstructionBlock instructionBlock)
        {
            int lineNumber = 1;
            string lineNumberString = $"{lineNumber}.";

            if (instructionBlock.BlockHeading != "")
            {
                Console.WriteLine($"<{instructionBlock.BlockHeading}>");
            }

            foreach (string instructionLine in instructionBlock.InstructionLines)
            {
                Console.WriteLine(UserInterface.MakeStringConsoleLengthLines($"{lineNumberString,-2} {instructionLine}"));
                lineNumber++;
                lineNumberString = $"{lineNumber}.";
            }
        }

        public static void DisplayShoppingList(ShoppingList shoppingList)
        {
            string header = "---------- SHOPPING LIST ----------";
            UserInterface.DisplayMenuHeader(header);

            string entireShoppingList = shoppingList.GetEntireShoppingList();

            if (entireShoppingList.Length == 0)
            {
                Console.WriteLine("There are currently no items on the shopping list.");
            }
            else
            {
                Console.WriteLine(entireShoppingList);
            }

            Console.WriteLine();
            Console.WriteLine("Press \"Enter\" to return to the main menu...");
            Console.ReadLine();
        }
    }
}
