using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class UserInterface
    {
        //Displays an options menu to the user based on the menuOptions passed into the method. The "out"
        //parameter outputs a list of option choices used by the program (usually the GetUserOption method) to validate
        //the user's selection so that they select one of the menu options listed on the screen.
        public static void DisplayOptionsMenu(IUserIO userIO, List<string[]> menuOptions, out List<string> optionChoices)
        {
            optionChoices = new List<string>();
            
            foreach (string[] element in menuOptions)
            {
                userIO.DisplayData(MakeStringConsoleLengthLines($"[{element[0]}] {element[1]}"));
                optionChoices.Add(element[0]);
            }
        }

        //Used to make long strings wrap at a natural space so that the lines being printed to the console screen are less
        //than 80 characters.
        public static string MakeStringConsoleLengthLines(string originalStatement)
        {
            string convertedStatement = "";
            string tempStatement = "";
            int lineLengthMax = 80;
            int breakIndex = 0;
            int segmentCounter = 0;
            int counter = 0;

            //If the original statement is shorter than the lineLengthMax, then it can be printed to the console without being shortened.
            if (originalStatement.Length <= lineLengthMax)
            {
                return originalStatement;
            }
            else
            {
                //This while loop runs while the number of characters processed (measured by the <counter> variable) is less than
                //the length of the original statement.
                while (counter < originalStatement.Length)
                {
                    
                    tempStatement = "";

                    //This for loop runs while i is less than the line length max (which is usually 80 characters). But, when processing the
                    //last portion of a long string that's been cut up into console length lines, it's possible for the final string that
                    //needs to be printed to the screen to be less than 80 characters. So, the second check of (i + (segmentCounter * lineLengthMax))
                    //ensures that the total number of characters being checked in this loop is less than the length of the original statement.

                    //Basically, this first for loop is setting the value of tempStatement to the next console-length-line of text to be processed.
                    for (int i = 0; (i < lineLengthMax && (i + (segmentCounter * lineLengthMax)) < originalStatement.Length); i++)
                    {
                        tempStatement += Convert.ToString(originalStatement[i + (segmentCounter * lineLengthMax)]);
                    }

                    //This "for loop" takes the tempStatement and finds the first space on the right end of the string to use as a "breaking point" for the
                    //text to wrap to the next line. The "breakIndex" of this breaking point is set to the index of that space character.
                    for (int j = tempStatement.Length - 1; j >= 0; j--)
                    {
                        if (tempStatement[j] == ' ')
                        {
                            breakIndex = j;
                            break;
                        }
                    }

                    //If the tempStatement's length is less than the line length maximum length (so, the line is shorter than 80 characters),
                    //the converted statement (where all the lines are console length) has the value of tempStatement appended to it, and the counter
                    //is updated to reflect all of the characters that were added to the convertedStatement.
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
                        //If tempStatement was the full line length maximum length (80 characters long), then
                        //it's characters are added to the convertedStatement one-by-one. If the index matches the index
                        //of where there should be a line break, the Enviornment.NewLine (line break) is added.
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

                    //The segment counter is incremented to measure how many 80 characters segments of the originalStatement have been processed
                    segmentCounter++;
                }
            }

            //The final converted statement, which is the originalStatement split into console-length lines with line breaks, is returned
            return convertedStatement;
        }

        //Displays the main menu to the user
        public static void DisplayMainMenu(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, out List<string> mainMenuOptions)
        {
            userIO.ClearDisplay();
            userIO.DisplayData("Welcome to the Recipe-2-Shopping List Program!".ToUpper());
            userIO.DisplayData("----------------------------------------------");
            userIO.DisplayData();

            List<string[]> recipeBooksToDisplay = new List<string[]>();
            List<string[]> mainMenuStandardOptions = new List<string[]>();
            mainMenuOptions = new List<string>();

            //If there are no recipe books in the recipe book library, the user is only given options to add a new recipe book,
            //manage saved measurement units, and save and exit the program. If there is at least one recipe book, then the user
            //is given options to select those recipe books and edit the recipe books.
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
            
            //This displays if there's at least one recipe book to open
            if (recipeBookLibrary.AllRecipeBooks.Count != 0)
            {
                userIO.DisplayData();
                userIO.DisplayData(MakeStringConsoleLengthLines("Open a recipe book by entering its number, or select an option from the menu:"));
            }
            mainMenuOptions.AddRange(standardOptionsToAdd);
        }

        //Displays a selected recipe book to the user
        public static void DisplayOpenRecipeBook(IUserIO userIO, RecipeBook recipeBook, out List<string> recipeBookOptions)
        {           
            userIO.ClearDisplay();
            userIO.DisplayData(MakeStringConsoleLengthLines($"Recipe Book: {recipeBook.Name}"));
            userIO.DisplayData("----------------------------------------------");
            userIO.DisplayData();

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string[]> recipeBookStandardOptions = new List<string[]>();
            recipeBookOptions = new List<string>();

            //If there are no recipes in the recipe book, the user is only given options to add a new recipe, view the shopping list,
            //return to the previous menu, and save and exit the program. If there is at least one recipe, then the user
            //is given options to select the recipes and edit the edit them.
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

            //This is only displayed if there are no recipes in a recipe book
            if (recipeBook.Recipes.Count != 0)
            {
                userIO.DisplayData();
                userIO.DisplayData(MakeStringConsoleLengthLines("Open a recipe by entering its number, or select an option from the menu:"));
            }
            recipeBookOptions.AddRange(standardOptionsToAdd);
        }

        //Displays a recipe to the user, and presents a menu of options for adding the recipe to the shopping list or editing the recipe
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

        //Displays text to indicate if a change was successfully made, or if a change was aborted based upon the user's choice to confirm a change or not
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

        //Used to run the logic of which successful change message should be displayed based upon a user's choice to confirm a change or not
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

        //Displays a menu header to the user based on the passed in parameters
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

        //Displays and instruction block of a recipe
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

        //Displays the shopping list, with items listed by store location, for the user to view
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

        //Displays the current custom (aka, user-created) measurement units for the user to view
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

        //Displays a prompt to the user with "regular formatting", meaning the user's input is collected on the next line after the prompt.
        public static void DisplayRegularPrompt(IUserIO userIO, string message, bool includeLeadingLineBreak = true)
        {
            if (includeLeadingLineBreak)
            {
                userIO.DisplayData();
            }

            userIO.DisplayData($"{message}:");
        }

        //Displays a prompt to the user with "lite formatting", meaning the user's input is collected on the same line, directly after the prompt.
        public static void DisplayLitePrompt(IUserIO userIO, string message, bool includeLeadingLineBreak = true)
        {
            if (includeLeadingLineBreak)
            {
                userIO.DisplayData();
            }

            userIO.DisplayDataLite($"{message}: ");
        }

        //Displays a line of information to the user
        public static void DisplayInformation(IUserIO userIO, string message, bool includeTrailingLineBreak = true)
        {
            userIO.DisplayData(MakeStringConsoleLengthLines(message));

            if (includeTrailingLineBreak)
            {
                userIO.DisplayData();
            }
        }

        //Used to insert a blank line on the display
        public static void InsertBlankLine(IUserIO userIO)
        {
            userIO.DisplayData();
        }

        //Used to display an error message to the user, requiring the user to press enter to continue.
        //(Mainly used to display errors that occur during loading or saving data)
        public static void DisplayErrorMessage(IUserIO userIO, string errorMessage, string action)
        {
            DisplayInformation(userIO, errorMessage);
            userIO.DisplayData($"Press \"Enter\" to {action}...");
            GetUserInput.GetEnterFromUser(userIO);
        }

        //Displays an exit message to the user as the exit the program indicating whether data was successfully saved or not
        public static void DisplayExitMessage(IUserIO userIO, bool writeRecipeBookLibrarySuccessful, bool writeShoppingListSuccessful, bool writeToBackupFileSuccessful)
        {
            string exitMessage = "";

            if (writeRecipeBookLibrarySuccessful && writeShoppingListSuccessful)
            {
                exitMessage = "All data saved. Have a great day!";
            }
            else if (writeRecipeBookLibrarySuccessful == true)
            {
                exitMessage = "Recipe data saved, but shopping list was not able to be saved to your computer.";
            }
            else if (writeRecipeBookLibrarySuccessful == false)
            {
                if (writeToBackupFileSuccessful)
                {
                    exitMessage = "Data saved to local file on your computer, and will be loaded into the program the next time you open it.";
                }
            }
            else
            {
                exitMessage = "Unfortunately, data could not be saved to the remote database or your local computer. All changes made during this session will be lost.";
            }

            DisplayInformation(userIO, exitMessage);
            DisplayInformation(userIO, "Press \"Enter\" to exit program...", false);
            GetUserInput.GetEnterFromUser(userIO);
        }
    }
}
