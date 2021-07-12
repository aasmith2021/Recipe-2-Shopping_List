using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class GetUserInput
    {
        public const string addNewRecipeBanner = "---------- ADD NEW RECIPE ----------";
        public const string editRecipeBanner = "---------- EDIT RECIPE ----------";

        public static string GetUserOption(IUserIO userIO, List<string> options)
        {
            string userInput = userIO.GetInput().ToUpper();

            while (!options.Contains(userInput))
            {
                UserInterface.DisplayLitePrompt(userIO, "Invalid entry. Please enter an option from the list");
                userInput = userIO.GetInput().ToUpper();
            }

            return userInput;
        }

        public static string GetUserInputString(IUserIO userIO, bool allowEmptyStringInput = true, int maxCharacters = 1200)
        {
            string userInput;
            bool inputIsNull;
            bool inputIsEmptyString;
            bool userInputIsValid = false;

            do
            {
                userInput = userIO.GetInput();

                inputIsNull = userInput == null;
                inputIsEmptyString = userInput == "";

                if (inputIsNull)
                {
                    UserInterface.DisplayRegularPrompt(userIO, "Invalid entry. Please try again");
                }
                else if (inputIsEmptyString && !allowEmptyStringInput)
                {
                    UserInterface.DisplayRegularPrompt(userIO, "Invalid entry; entry cannot be blank. Please try again");
                }
                else if (userInput.Length > maxCharacters)
                {
                    UserInterface.DisplayRegularPrompt(userIO, $"Invalid entry; input cannot exceed ${maxCharacters} characters. Please try again");
                }
                else
                {
                    userInputIsValid = true;
                }
            }
            while (!userInputIsValid);

            return userInput;
        }

        public static int GetUserInputInt(IUserIO userIO, int inputOption = 0, bool allowEmpyStringInput = false)
        {
            //Option -2: Get negative integer
            //Option -1: Get negative or zero integer
            //Option 0: Get any integer
            //Option 1: Get positive or zero integer
            //Option 2: Get positive integer

            int result;
            string userInput = userIO.GetInput();
            string errorMessage;

            if (allowEmpyStringInput && userInput == "")
            {
                result = 0;
            }
            else
            {
                while (!int.TryParse(userInput, out result) || userInput == "" ||
                            (inputOption == -2 && result >= 0) ||
                            (inputOption == -1 && result > 0) ||
                            (inputOption == 1 && result < 0) ||
                            (inputOption == 2 && result <= 0))
                {
                    errorMessage = "";
                    UserInterface.InsertBlankLine(userIO);

                    switch (inputOption)
                    {
                        case -2:
                            errorMessage = "Invalid entry. Please enter a negative integer";
                            break;
                        case -1:
                            errorMessage = "Invalid entry. Please enter an integer that is zero or negative";
                            break;
                        case 0:
                            errorMessage = "Invalid entry. Please enter an integer";
                            break;
                        case 1:
                            errorMessage = "Invalid entry. Please enter an integer that is zero or positive";
                            break;
                        case 2:
                            errorMessage = "Invalid entry. Please enter an integer that is greater than zero";
                            break;
                        default:
                            break;
                    }

                    UserInterface.DisplayLitePrompt(userIO, errorMessage, false);
                    userInput = userIO.GetInput();
                }
            }

            return result;
        }

        public static double GetUserInputDouble(IUserIO userIO, int inputOption = 0, bool allowEmpyStringInput = false)
        {
            //Option -2: Get negative double
            //Option -1: Get negative or zero double
            //Option 0: Get any double
            //Option 1: Get positive or zero double
            //Option 2: Get positive double

            double result;
            string errorMessage;
            string userInput = userIO.GetInput();

            if (allowEmpyStringInput && userInput == "")
            {
                result = 0;
            }
            else
            {
                while (!double.TryParse(userInput, out result) || ((result * 1000) - Math.Floor(result * 1000)) != 0 || userInput == "" ||
                            (inputOption == -2 && result >= 0) || (inputOption == -1 && result > 0) || (inputOption == 1 && result < 0) ||
                            (inputOption == 2 && result <= 0))
                {
                    errorMessage = "";
                    UserInterface.InsertBlankLine(userIO);

                    switch (inputOption)
                    {
                        case -2:
                            errorMessage = UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a negative number that has no more than 3 decimal places (ex: -1.125)");
                            break;
                        case -1:
                            errorMessage = UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that is zero or negative and has no more than 3 decimal places (ex: -1.125)");
                            break;
                        case 0:
                            errorMessage = UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that has no more than 3 decimal places (ex: 1.125)");
                            break;
                        case 1:
                            errorMessage = UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that is zero or positive and has no more than 3 decimal places (ex: 1.125)");
                            break;
                        case 2:
                            errorMessage = UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that is greater than zero and has no more than 3 decimal places (ex: 1.125)");
                            break;
                        default:
                            break;
                    }
                    UserInterface.DisplayLitePrompt(userIO, errorMessage, false);
                    userInput = userIO.GetInput();
                }
            }

            return result;
        }

        public static void AreYouSure(IUserIO userIO, string changeMessage, out bool isSure)
        {
            isSure = false;

            UserInterface.InsertBlankLine(userIO);
            UserInterface.DisplayInformation(userIO, UserInterface.MakeStringConsoleLengthLines($"Are you sure you want to {changeMessage}?"), false);
            UserInterface.DisplayLitePrompt(userIO, "Enter \"Y\" for Yes or \"N\" for No");

            List<string> options = new List<string>() { "Y", "N" };

            string userInput = GetUserOption(userIO, options);

            if (userInput == "Y")
            {
                isSure = true;
            }
        }

        public static string GetRecipeBookNameFromUser(IUserIO userIO)
        {
            string valueToReturn = GetUserInput.GetUserInputString(userIO, false, DataMaxValues.RECIPE_BOOK_NAME_LENGTH);
            return valueToReturn;
        }

        public static Metadata GetMetadataFromUser(IUserIO userIO)
        {
            UserInterface.DisplayMenuHeader(userIO, addNewRecipeBanner, "<<< RECIPE BASIC INFO >>>");

            UserInterface.DisplayLitePrompt(userIO, "Enter the title of the new recipe", false);
            string title = GetRecipeTitleFromUser(userIO);

            UserInterface.DisplayRegularPrompt(userIO, "Enter notes about the new recipe (or press \"Enter\" to leave blank)");
            string userNotes = GetRecipeUserNotesFromUser(userIO);

            UserInterface.DisplayRegularPrompt(userIO, "Enter the food type of the new recipe (or press \"Enter\" to leave blank)");
            string foodType = GetRecipeFoodTypeFromUser(userIO);

            UserInterface.DisplayRegularPrompt(userIO, "Enter the food genre of the new recipe (or press \"Enter\" to leave blank)");
            string foodGenre = GetRecipeFoodGenreFromUser(userIO);

            UserInterface.DisplayLitePrompt(userIO, "Enter the prep time of recipe in minutes");
            int prepTime = GetRecipePrepTimeFromUser(userIO);

            UserInterface.DisplayLitePrompt(userIO, "Enter the cook time of recipe in minutes");
            int cookTime = GetRecipeCookTimeFromUser(userIO);

            UserInterface.DisplayLitePrompt(userIO, "Enter the low number of estimated servings");
            int lowServings = GetRecipeLowServingsFromUser(userIO);

            UserInterface.DisplayRegularPrompt(userIO, "Enter the high number of estimated servings (or press \"Enter\" to leave blank)");
            int highServings = GetRecipeHighServingsFromUser(userIO);

            UserInterface.InsertBlankLine(userIO);
            UserInterface.DisplayInformation(userIO, "Basic info complete! Now on to the ingredients!");
            UserInterface.DisplayInformation(userIO, "Press \"Enter\" to continue...", false);
            GetUserInput.GetEnterFromUser(userIO);

            Tags recipeTags = new Tags(foodType, foodGenre);
            Times recipePrepTimes = new Times(prepTime, cookTime);
            Servings recipeServings = new Servings(lowServings, highServings);
            Metadata recipeMetadata = new Metadata(title, recipePrepTimes, recipeTags, recipeServings, userNotes);

            return recipeMetadata;
        }

        public static string GetRecipeTitleFromUser(IUserIO userIO)
        {
            string valueToReturn = GetUserInputString(userIO, false, DataMaxValues.RECIPE_TITLE_LENGTH);
            return valueToReturn;
        }

        public static string GetRecipeUserNotesFromUser(IUserIO userIO)
        {
            string valueToReturn = GetUserInputString(userIO, true, DataMaxValues.USER_NOTES_LENGTH);
            return valueToReturn;
        }

        public static string GetRecipeFoodTypeFromUser(IUserIO userIO)
        {
            string valueToReturn = GetUserInputString(userIO, true, DataMaxValues.FOOD_TYPE_LENGTH);
            return valueToReturn;
        }

        public static string GetRecipeFoodGenreFromUser(IUserIO userIO)
        {
            string valueToReturn = GetUserInputString(userIO, true, DataMaxValues.FOOD_GENRE_LENGTH);
            return valueToReturn;
        }

        public static int GetRecipePrepTimeFromUser(IUserIO userIO)
        {
            int valueToReturn = GetIntQuantityFromUser(userIO, DataMaxValues.MAX_PREP_TIME, 1, $"A recipe cannot have a prep time of more than {DataMaxValues.MAX_PREP_TIME} minutes. Please enter a valid prep time");
            return valueToReturn;
        }

        public static int GetRecipeCookTimeFromUser(IUserIO userIO)
        {
            int valueToReturn = GetIntQuantityFromUser(userIO, DataMaxValues.MAX_COOK_TIME, 1, $"A recipe cannot have a cook time of more than {DataMaxValues.MAX_COOK_TIME} minutes. Please enter a valid cook time");
            return valueToReturn;
        }

        public static int GetRecipeLowServingsFromUser(IUserIO userIO)
        {
            int valueToReturn = GetIntQuantityFromUser(userIO, DataMaxValues.MAX_LOW_SERVINGS, 1, $"A recipe cannot have more than {DataMaxValues.MAX_LOW_SERVINGS} servings. Please enter a valid number of servings");
            return valueToReturn;
        }

        public static int GetRecipeHighServingsFromUser(IUserIO userIO)
        {
            int valueToReturn = GetIntQuantityFromUser(userIO, DataMaxValues.MAX_HIGH_SERVINGS, 2, $"A recipe cannot have more than {DataMaxValues.MAX_HIGH_SERVINGS} servings. Please enter a valid number of servings", true);
            return valueToReturn;
        }

        public static int GetNumberOfIngredientsFromUser(IUserIO userIO)
        {
            int valueToReturn = GetIntQuantityFromUser(userIO, DataMaxValues.NUMBER_OF_INGREDIENTS, 2, $"A recipe cannot have more than {DataMaxValues.NUMBER_OF_INGREDIENTS} ingredients. Please enter a valid number of ingredients");
            return valueToReturn;
        }

        public static double GetIngredientQuantityFromUser(IUserIO userIO)
        {
            double valueToReturn = GetDoubleQuantityFromUser(userIO, DataMaxValues.INGREDIENT_QUANTITY, 2, $"An ingredient quantity cannot be more than {DataMaxValues.INGREDIENT_QUANTITY}. Please enter a valid ingredient quantity");
            return valueToReturn;
        }

        public static string GetIngredientNameFromUser(IUserIO userIO)
        {
            string valueToReturn = GetUserInputString(userIO, false, DataMaxValues.INGREDIENT_NAME_LENGTH);
            return valueToReturn;
        }

        public static string GetIngredientPrepNoteFromUser(IUserIO userIO)
        {
            string valueToReturn = GetUserInputString(userIO, true, DataMaxValues.INGREDIENT_PREP_NOTE_LENGTH);
            return valueToReturn;
        }

        public static int GetNumberOfInstructionBlocksFromUser(IUserIO userIO)
        {
            int valueToReturn = GetIntQuantityFromUser(userIO, DataMaxValues.NUMBER_OF_INSTRUCTION_BLOCKS, 2, $"A recipe cannot have more than {DataMaxValues.NUMBER_OF_INSTRUCTION_BLOCKS} instruction blocks. Please enter a valid number of instruction blocks");
            return valueToReturn;
        }

        public static string GetBlockHeadingFromUser(IUserIO userIO)
        {
            string valueToReturn = GetUserInputString(userIO, true, DataMaxValues.BLOCK_HEADING_LENGTH);
            return valueToReturn;
        }

        public static int GetNumberOfInstructionLinesFromUser(IUserIO userIO)
        {
            int valueToReturn = GetIntQuantityFromUser(userIO, DataMaxValues.NUMBER_OF_INSTRUCTION_LINES, 2, $"An instruction block cannot have more than {DataMaxValues.NUMBER_OF_INSTRUCTION_LINES} instruction lines. Please enter a valid number of instruction lines");
            return valueToReturn;
        }

        public static string GetInstructionLineFromUser(IUserIO userIO)
        {
            string valueToReturn = GetUserInputString(userIO, false, DataMaxValues.INSTRUCTION_LINE_LENGTH);
            return valueToReturn;
        }

        public static IngredientList GetIngredientsFromUser(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            UserInterface.DisplayMenuHeader(userIO, addNewRecipeBanner, "<<< RECIPE INGREDIENTS >>>");

            IngredientList recipeIngredientList = new IngredientList();

            UserInterface.DisplayLitePrompt(userIO, "Enter the total number of ingredients in the recipe", false);
            int numberOfIngredients = GetNumberOfIngredientsFromUser(userIO);

            for (int i = 0; i < numberOfIngredients; i++)
            {
                UserInterface.DisplayMenuHeader(userIO, addNewRecipeBanner, $"<<< INGREDIENT {i + 1} >>>");

                UserInterface.DisplayLitePrompt(userIO, $"Enter the quantity of ingredient {i + 1} needed (ex: 1.5)", false);
                double qty = GetIngredientQuantityFromUser(userIO);

                List<string[]> measurementUnits = MeasurementUnits.AllMeasurementUnitsForUserInput(recipeBookLibrary);
                List<string> options = new List<string>();
                UserInterface.InsertBlankLine(userIO);
                UserInterface.DisplayOptionsMenu(userIO, measurementUnits, out options);
                
                UserInterface.DisplayLitePrompt(userIO, "Select the ingredient measurement unit from the list of options");
                int userOptionNumber = int.Parse(GetUserOption(userIO, options));
                string measurementUnit = "";

                if (userOptionNumber == options.Count)
                {
                    GetNewMeasurementUnitFromUser(userIO, out measurementUnit);
                    recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                    UserInterface.InsertBlankLine(userIO);
                    UserInterface.DisplayInformation(userIO, $"Success! New measurement unit, {measurementUnit}, was added and will be used for this ingredient.", false);
                }
                else if (measurementUnits[userOptionNumber - 1][1] != "None")
                {
                    measurementUnit = measurementUnits[userOptionNumber - 1][1];
                }

                UserInterface.DisplayLitePrompt(userIO, "Enter the ingredient name");
                string name = GetIngredientNameFromUser(userIO);

                UserInterface.DisplayRegularPrompt(userIO, "Enter the ingredient preparation note (or press \"Enter\" to leave blank)");
                string preparationNote = GetIngredientPrepNoteFromUser(userIO);

                Ingredient ingredientToAdd = new Ingredient(qty, measurementUnit, name, preparationNote);
                recipeIngredientList.AddIngredient(ingredientToAdd);

                if (numberOfIngredients > 1 && i < (numberOfIngredients - 1))
                {
                    UserInterface.InsertBlankLine(userIO);
                    UserInterface.DisplayInformation(userIO, "Ingredient complete! On to the next ingredient.");
                    UserInterface.DisplayInformation(userIO, "Press \"Enter\" to continue...", false);
                    GetUserInput.GetEnterFromUser(userIO);
                }
                else
                {
                    UserInterface.InsertBlankLine(userIO);
                    UserInterface.DisplayInformation(userIO, "Ingredients complete!");
                    UserInterface.DisplayInformation(userIO, "Press \"Enter\" to continue...", false);
                    GetUserInput.GetEnterFromUser(userIO);
                }
            }

            return recipeIngredientList;
        }

        public static Ingredient GetIngredientFromUser(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            UserInterface.DisplayLitePrompt(userIO, $"Enter the quantity of ingredient needed (ex: 1.5)", false);
            double qty = GetIngredientQuantityFromUser(userIO);

            List<string[]> measurementUnits = MeasurementUnits.AllMeasurementUnitsForUserInput(recipeBookLibrary);
            List<string> options = new List<string>();
            UserInterface.InsertBlankLine(userIO);
            UserInterface.DisplayOptionsMenu(userIO, measurementUnits, out options);

            UserInterface.DisplayLitePrompt(userIO, "Select the ingredient measurement unit from the list of options");
            int userOptionNumber = int.Parse(GetUserOption(userIO, options));
            string measurementUnit = "";

            if (userOptionNumber == options.Count)
            {
                GetNewMeasurementUnitFromUser(userIO, out measurementUnit);
                recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                UserInterface.InsertBlankLine(userIO);
                UserInterface.DisplayInformation(userIO, "Success! New measurement unit, " + measurementUnit + " was added and will be used for this ingredient.", false);
            }
            else if (measurementUnits[userOptionNumber - 1][1] != "None")
            {
                measurementUnit = measurementUnits[userOptionNumber - 1][1];
            }

            UserInterface.DisplayLitePrompt(userIO, "Enter the ingredient name");
            string name = GetIngredientNameFromUser(userIO);

            UserInterface.DisplayRegularPrompt(userIO, "Enter the ingredient preparation note (or press \"Enter\" to leave blank)");
            string preparationNote = GetIngredientPrepNoteFromUser(userIO);

            Ingredient ingredientToAdd = new Ingredient(qty, measurementUnit, name, preparationNote);
            return ingredientToAdd;
        }

        public static CookingInstructions GetCookingInstructionsFromUser(IUserIO userIO)
        {
            UserInterface.DisplayMenuHeader(userIO, addNewRecipeBanner, "<<< RECIPE INSTRUCTIONS >>>");
            UserInterface.DisplayLitePrompt(userIO, "Enter the number of cooking instruction blocks (most recipes only have 1)", false);
            int numberOfInstructionBlocks = GetNumberOfInstructionBlocksFromUser(userIO);

            CookingInstructions recipeCookingInstructions = new CookingInstructions();

            for (int i = 0; i < numberOfInstructionBlocks; i++)
            {
                UserInterface.DisplayMenuHeader(userIO, addNewRecipeBanner, $"<<< INSTRUCTION BLOCK {i + 1} >>>");

                UserInterface.DisplayRegularPrompt(userIO, UserInterface.MakeStringConsoleLengthLines($"Enter the heading for instruction block {i + 1} (or press \"Enter\" to leave blank)"), false);
                string blockHeading = GetBlockHeadingFromUser(userIO);

                InstructionBlock instructionBlockToAdd = new InstructionBlock(blockHeading);

                UserInterface.DisplayLitePrompt(userIO, $"Enter the number of instruction lines for instruction block {i + 1}");
                int numberOfLines = GetNumberOfInstructionLinesFromUser(userIO);

                for (int j = 0; j < numberOfLines; j++)
                {
                    UserInterface.DisplayRegularPrompt(userIO, $"Enter the instructions for line #{j + 1}");
                    string instructionLine = GetInstructionLineFromUser(userIO);

                    instructionBlockToAdd.AddInstructionLine(instructionLine);
                }

                recipeCookingInstructions.AddInstructionBlock(instructionBlockToAdd);

                if (numberOfInstructionBlocks > 1 && i < (numberOfInstructionBlocks - 1))
                {
                    UserInterface.InsertBlankLine(userIO);
                    UserInterface.DisplayInformation(userIO, "Instruction block complete! On to the next instruction block.");
                    UserInterface.DisplayInformation(userIO, "Press \"Enter\" to continue...", false);
                }
                else if (numberOfInstructionBlocks > 1 && i == (numberOfInstructionBlocks - 1))
                {
                    UserInterface.InsertBlankLine(userIO);
                    UserInterface.DisplayInformation(userIO, "Instruction blocks complete!");
                    UserInterface.DisplayInformation(userIO, "Press \"Enter\" to continue...", false);
                }
                else
                {
                    UserInterface.InsertBlankLine(userIO);
                    UserInterface.DisplayInformation(userIO, "Instruction block complete!");
                    UserInterface.DisplayInformation(userIO, "Press \"Enter\" to continue...", false);
                }
                GetUserInput.GetEnterFromUser(userIO);
            }

            return recipeCookingInstructions;
        }

        public static InstructionBlock GetInstructionBlockFromUser(IUserIO userIO)
        {
            UserInterface.DisplayRegularPrompt(userIO, UserInterface.MakeStringConsoleLengthLines($"Enter the heading for the new instruction block (or press \"Enter\" to leave blank)"), false);
            string blockHeading = GetBlockHeadingFromUser(userIO);

            InstructionBlock newInstructionBlock = new InstructionBlock(blockHeading);

            UserInterface.DisplayLitePrompt(userIO, $"Enter the number of instruction lines for the instruction block");
            int numberOfLines = GetNumberOfInstructionLinesFromUser(userIO);

            for (int j = 0; j < numberOfLines; j++)
            {
                UserInterface.DisplayRegularPrompt(userIO, $"Enter the instructions for line #{j + 1}");
                string instructionLine = GetInstructionLineFromUser(userIO);

                newInstructionBlock.AddInstructionLine(instructionLine);
            }

            return newInstructionBlock;
        }

        public static void GetNewMeasurementUnitFromUser(IUserIO userIO, out string measurementUnit)
        {
            UserInterface.DisplayRegularPrompt(userIO, "Enter the name of the new measurement unit");
            measurementUnit = GetNewMeasurementUnitName(userIO);
        }

        public static string GetTheFieldToEditFromUser(IUserIO userIO, Recipe recipe, List<string[]> editOptions)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));

            List<string> userOptions = new List<string>();
            UserInterface.DisplayOptionsMenu(userIO, editOptions, out userOptions);

            UserInterface.DisplayLitePrompt(userIO, "Select the option for the information you would like to edit");
            string userOption = GetUserOption(userIO, userOptions);

            return userOption;
        }

        public static string GetNewUserNotes(IUserIO userIO, Recipe recipe)
        {
            int currentLengthOfNotes = recipe.Metadata.Notes.Length;
            int maxLengthOfNotes = DataMaxValues.USER_NOTES_LENGTH;
            int maxLengthOfNewNote = maxLengthOfNotes - currentLengthOfNotes;
            
            UserInterface.DisplayRegularPrompt(userIO, "Enter the notes you would like to add to this recipe", false);
            string newNotes = GetUserInputString(userIO, false, maxLengthOfNewNote);

            return newNotes;
        }

        public static string GetNewMeasurementUnitName(IUserIO userIO)
        {
            return GetUserInputString(userIO, false, DataMaxValues.MEASUREMENT_UNIT_NAME_LENGTH);
        }

        public static int GetIntQuantityFromUser(IUserIO userIO, int maxQuantity, int option, string message, bool allowEmptyStringInput = false)
        {
            int valueToReturn = GetUserInputInt(userIO, option, allowEmptyStringInput);

            while (valueToReturn > maxQuantity)
            {
                UserInterface.DisplayRegularPrompt(userIO, UserInterface.MakeStringConsoleLengthLines(message));
                valueToReturn = GetUserInputInt(userIO, option, allowEmptyStringInput);
            }

            return valueToReturn;
        }

        public static double GetDoubleQuantityFromUser(IUserIO userIO, double maxQuantity, int option, string message, bool allowEmptyStringInput = false)
        {
            double valueToReturn = GetUserInputDouble(userIO, option, allowEmptyStringInput);

            while (valueToReturn > maxQuantity)
            {
                UserInterface.DisplayRegularPrompt(userIO, UserInterface.MakeStringConsoleLengthLines(message));
                valueToReturn = GetUserInputDouble(userIO, option, allowEmptyStringInput);
            }

            return valueToReturn;
        }

        public static void GetEnterFromUser(IUserIO userIO)
        {
            userIO.GetInput();
        }
    }
}
