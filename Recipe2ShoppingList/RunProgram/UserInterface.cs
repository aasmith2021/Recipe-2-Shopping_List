using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class UserInterface
    {
        public static void DisplayOptionsMenu(IUserIO userIO, List<string[]> menuOptions, out List<string> optionChoices)
        {
            optionChoices = new List<string>();
            
            foreach (string[] element in menuOptions)
            {
                userIO.DisplayData(MakeStringConsoleLengthLines($"[{element[0]}] {element[1]}"));
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

        public static void DisplayMainMenu(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, out List<string> mainMenuOptions)
        {
            userIO.ClearDisplay();
            userIO.DisplayData("Welcome to the Recipe-2-Shopping List Program!".ToUpper());
            userIO.DisplayData("----------------------------------------------");
            userIO.DisplayData();

            List<string[]> recipeBooksToDisplay = new List<string[]>();
            List<string[]> mainMenuStandardOptions = new List<string[]>();
            mainMenuOptions = new List<string>();

            if (recipeBookLibrary.AllRecipeBooks.Count == 0)
            {
                userIO.DisplayData("You currently don't have any recipe books saved.");
                userIO.DisplayData();
                userIO.DisplayData("Add a new recipe book using the options below:");
                userIO.DisplayData();

                mainMenuStandardOptions.Add(new string[] { "A", "Add New Recipe Book"});
                mainMenuStandardOptions.Add(new string[] { "M", "Manage Saved Measurement Units"});
                mainMenuStandardOptions.Add(new string[] { "X", "Save and Exit Program" });
            }
            else
            {
                userIO.DisplayData("----- RECIPE BOOKS -----");

                for (int i = 0; i < recipeBookLibrary.AllRecipeBooks.Count; i++)
                {
                    recipeBooksToDisplay.Add(new string[] { (i + 1).ToString(), recipeBookLibrary.AllRecipeBooks[i].Name });
                }

                DisplayOptionsMenu(userIO, recipeBooksToDisplay, out mainMenuOptions);
                userIO.DisplayData();

                mainMenuStandardOptions.Add(new string[] { "A", "Add New Recipe Book"} );
                mainMenuStandardOptions.Add(new string[] { "R", "Rename Existing Recipe Book" });
                mainMenuStandardOptions.Add(new string[] { "D", "Delete Existing Recipe Book" });
                mainMenuStandardOptions.Add(new string[] { "V", "View Shopping List" });
                mainMenuStandardOptions.Add(new string[] { "M", "Manage Saved Measurement Units" });
                mainMenuStandardOptions.Add(new string[] { "X", "Save and Exit Program" });
            }

            userIO.DisplayData();
            userIO.DisplayData("----- OPTIONS -----");
            DisplayOptionsMenu(userIO, mainMenuStandardOptions, out List<string> standardOptionsToAdd);
            userIO.DisplayData();
            
            if (recipeBookLibrary.AllRecipeBooks.Count != 0)
            {
                userIO.DisplayData();
                userIO.DisplayData(MakeStringConsoleLengthLines("Open a recipe book by entering its number, or select an option from the menu:"));
            }
            mainMenuOptions.AddRange(standardOptionsToAdd);
        }

        public static void DisplayOpenRecipeBook(IUserIO userIO, RecipeBook recipeBook, out List<string> recipeBookOptions)
        {           
            userIO.ClearDisplay();
            userIO.DisplayData(MakeStringConsoleLengthLines($"Recipe Book: {recipeBook.Name}"));
            userIO.DisplayData("----------------------------------------------");
            userIO.DisplayData();

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string[]> recipeBookStandardOptions = new List<string[]>();
            recipeBookOptions = new List<string>();

            if (recipeBook.Recipes.Count == 0)
            {
                userIO.DisplayData("You currently don't have any recipes in this recipe book.");
                userIO.DisplayData();
                userIO.DisplayData("Add a new recipe using the options below:");
                userIO.DisplayData();

                recipeBookStandardOptions.Add(new string[] { "A", "Add New Recipe" });
                recipeBookStandardOptions.Add(new string[] { "V", "View Shopping List" });
                recipeBookStandardOptions.Add(new string[] { "R", "Return to Previous Menu" });
                recipeBookStandardOptions.Add(new string[] { "X", "Save and Exit Program" });
            }
            else
            {
                userIO.DisplayData("----- RECIPES -----");

                for (int i = 0; i < recipeBook.Recipes.Count; i++)
                {
                    recipesToDisplay.Add(new string[] { (i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title });
                }

                DisplayOptionsMenu(userIO, recipesToDisplay, out recipeBookOptions);
                userIO.DisplayData();

                recipeBookStandardOptions.Add(new string[] { "A", "Add New Recipe" });
                recipeBookStandardOptions.Add(new string[] { "E", "Edit Existing Recipe" });
                recipeBookStandardOptions.Add(new string[] { "D", "Delete Existing Recipe" });
                recipeBookStandardOptions.Add(new string[] { "S", "Add a Recipe to the Shopping List" });
                recipeBookStandardOptions.Add(new string[] { "V", "View Shopping List" });
                recipeBookStandardOptions.Add(new string[] { "R", "Return to Previous Menu" });
                recipeBookStandardOptions.Add(new string[] { "X", "Save and Exit Program" });
            }

            userIO.DisplayData();
            userIO.DisplayData("----- OPTIONS -----");
            DisplayOptionsMenu(userIO, recipeBookStandardOptions, out List<string> standardOptionsToAdd);
            userIO.DisplayData();

            if (recipeBook.Recipes.Count != 0)
            {
                userIO.DisplayData();
                userIO.DisplayData(MakeStringConsoleLengthLines("Open a recipe by entering its number, or select an option from the menu:"));
            }
            recipeBookOptions.AddRange(standardOptionsToAdd);
        }

        public static void DisplayOpenRecipe(IUserIO userIO, Recipe recipe, RecipeBook recipeBook, out List<string> recipeOptions)
        {
            userIO.ClearDisplay();
            userIO.DisplayData(MakeStringConsoleLengthLines($"Recipe Book: {recipeBook.Name}"));
            userIO.DisplayData("----------------------------------------------");
            userIO.DisplayData();
            userIO.DisplayData(recipe.ProduceRecipeText(true));
            userIO.DisplayData();
            userIO.DisplayData("----- OPTIONS -----");

            List<string[]> recipeStardardOptions = new List<string[]>();

                recipeStardardOptions.Add(new string[] { "S", "Add This Recipe to the Shopping List" });
                recipeStardardOptions.Add(new string[] { "E", "Edit This Recipe" });
                recipeStardardOptions.Add(new string[] { "D", "Delete This Recipe" });
                recipeStardardOptions.Add(new string[] { "V", "View Shopping List" });
                recipeStardardOptions.Add(new string[] { "R", "Return to Previous Menu" });
                recipeStardardOptions.Add(new string[] { "X", "Save and Exit Program" });

            DisplayOptionsMenu(userIO, recipeStardardOptions, out recipeOptions);
            userIO.DisplayData();
        }

        public static void SuccessfulChange(IUserIO userIO, bool changeConfirmed, string changeNoun, string changeVerb, bool isPluralNoun = false)
        {

            userIO.DisplayData();
            if (changeConfirmed && !isPluralNoun)
            {
                userIO.DisplayData($"Success! The {changeNoun} was {changeVerb}.");
            }
            else if (!changeConfirmed && !isPluralNoun)
            {
                userIO.DisplayData($"No worries! The {changeNoun} was not {changeVerb}.");
            }
            else if (changeConfirmed && isPluralNoun)
            {
                userIO.DisplayData($"Success! The {changeNoun} were {changeVerb}.");
            }
            else
            {
                userIO.DisplayData($"No worries! The {changeNoun} were not {changeVerb}.");
            }
            userIO.DisplayData();
            userIO.DisplayData("Press \"Enter\" to continue...");
            GetUserInput.GetEnterFromUser(userIO);
        }

        public static void DisplaySuccessfulChangeMessage(IUserIO userIO, bool isSure, string changeNoun, string changeVerb, bool isPluralNoun = false)
        {
            if (isSure)
            {
                SuccessfulChange(userIO, isSure, changeNoun, changeVerb, isPluralNoun);
            }
            else
            {
                SuccessfulChange(userIO, isSure, changeNoun, changeVerb, isPluralNoun);
            }
        }

        public static void DisplayMenuHeader(IUserIO userIO, string header, string additionalMessage = "", bool includeTrailingLineBreak = true)
        {
            userIO.ClearDisplay();
            userIO.DisplayData(header);
            userIO.DisplayData();

            if (additionalMessage != "")
            {
                userIO.DisplayData(additionalMessage);

                if (includeTrailingLineBreak)
                { 
                    userIO.DisplayData();
                }
            }
        }

        public static void DisplayInstructionBlock(IUserIO userIO, InstructionBlock instructionBlock)
        {
            int lineNumber = 1;
            string lineNumberString = $"{lineNumber}.";

            if (instructionBlock.BlockHeading != "")
            {
                userIO.DisplayData($"<{instructionBlock.BlockHeading}>");
            }

            foreach (string instructionLine in instructionBlock.InstructionLines)
            {
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines($"{lineNumberString,-2} {instructionLine}"));
                lineNumber++;
                lineNumberString = $"{lineNumber}.";
            }
        }

        public static void DisplayShoppingList(IUserIO userIO, ShoppingList shoppingList)
        {
            string header = "---------- SHOPPING LIST ----------";
            UserInterface.DisplayMenuHeader(userIO, header);

            string entireShoppingList = shoppingList.GetEntireShoppingList();

            if (entireShoppingList.Length == 0)
            {
                userIO.DisplayData("There are currently no items on the shopping list.");
            }
            else
            {
                userIO.DisplayData(entireShoppingList);
            }

            userIO.DisplayData();
            userIO.DisplayData("Press \"Enter\" to return to the main menu...");
            GetUserInput.GetEnterFromUser(userIO);
        }

        public static void DisplayCurrentMeasurementUnits(IUserIO userIO, List<string> userAddedMeasurementUnits, bool addExtraLineBreak = false)
        {
            if (userAddedMeasurementUnits.Count == 0)
            {
                userIO.DisplayData("There are currently no user-added measurement units saved.");
            }
            else
            {
                userIO.DisplayData("<<< Current Measurement Units >>>");
                for (int i = 0; i < userAddedMeasurementUnits.Count; i++)
                {
                    userIO.DisplayData($"{i + 1}. {userAddedMeasurementUnits[i]}");
                }
            }

            if (addExtraLineBreak)
            {
                userIO.DisplayData();
            }
        }

        public static void DisplayRegularPrompt(IUserIO userIO, string message, bool includeLeadingLineBreak = true)
        {
            if (includeLeadingLineBreak)
            {
                userIO.DisplayData();
            }

            userIO.DisplayData($"{message}:");
        }

        public static void DisplayLitePrompt(IUserIO userIO, string message, bool includeLeadingLineBreak = true)
        {
            if (includeLeadingLineBreak)
            {
                userIO.DisplayData();
            }

            userIO.DisplayDataLite($"{message}: ");
        }

        public static void DisplayInformation(IUserIO userIO, string message, bool includeTrailingLineBreak = true)
        {
            userIO.DisplayData(MakeStringConsoleLengthLines(message));

            if (includeTrailingLineBreak)
            {
                userIO.DisplayData();
            }
        }

        public static void InsertBlankLine(IUserIO userIO)
        {
            userIO.DisplayData();
        }

        public static void DisplayErrorMessage(IUserIO userIO, string errorMessage, string action)
        {
            DisplayInformation(userIO, errorMessage);
            userIO.DisplayData($"Press \"Enter\" to {action}...");
            GetUserInput.GetEnterFromUser(userIO);
        }
    }
}
