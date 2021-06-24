using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class GetUserInput
    {
        public static string GetUserOption(IUserIO userIO, List<string> options)
        {
            string userInput = userIO.GetInput().ToUpper();

            while (!options.Contains(userInput))
            {
                userIO.DisplayData();
                userIO.DisplayDataLite("Invalid entry. Please enter an option from the list: ");
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
                    userIO.DisplayData();
                    userIO.DisplayData("Invalid entry. Please try again:");
                }
                else if (inputIsEmptyString && !allowEmptyStringInput)
                {
                    userIO.DisplayData();
                    userIO.DisplayData("Invalid entry; entry cannot be blank. Please try again:");
                }
                else if (userInput.Length > maxCharacters)
                {
                    userIO.DisplayData();
                    userIO.DisplayData($"Invalid entry; input cannot exceed ${maxCharacters} characters. Please try again:");
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
                    userIO.DisplayData();

                    switch (inputOption)
                    {
                        case -2:
                            errorMessage = "Invalid entry. Please enter a negative integer: ";
                            break;
                        case -1:
                            errorMessage = "Invalid entry. Please enter an integer that is zero or negative: ";
                            break;
                        case 0:
                            errorMessage = "Invalid entry. Please enter an integer: ";
                            break;
                        case 1:
                            errorMessage = "Invalid entry. Please enter an integer that is zero or positive: ";
                            break;
                        case 2:
                            errorMessage = "Invalid entry. Please enter an integer that is greater than zero: ";
                            break;
                        default:
                            break;
                    }

                    userIO.DisplayDataLite(errorMessage);
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
                    userIO.DisplayData();

                    switch (inputOption)
                    {
                        case -2:
                            errorMessage = UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a negative number that has no more than 3 decimal places (ex: -1.125): ");
                            break;
                        case -1:
                            errorMessage = UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that is zero or negative and has no more than 3 decimal places (ex: -1.125): ");
                            break;
                        case 0:
                            errorMessage = UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that has no more than 3 decimal places (ex: 1.125): ");
                            break;
                        case 1:
                            errorMessage = UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that is zero or positive and has no more than 3 decimal places (ex: 1.125): ");
                            break;
                        case 2:
                            errorMessage = UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that is greater than zero and has no more than 3 decimal places (ex: 1.125): ");
                            break;
                        default:
                            break;
                    }
                    userIO.DisplayDataLite(errorMessage);
                    userInput = userIO.GetInput();
                }
            }

            return result;
        }

        public static void AreYouSure(IUserIO userIO, string changeMessage, out bool isSure)
        {
            isSure = false;

            userIO.DisplayData();
            userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines($"Are you sure you want to {changeMessage}?"));
            userIO.DisplayData();
            userIO.DisplayDataLite("Enter \"Y\" for Yes or \"N\" for No: ");

            List<string> options = new List<string>() { "Y", "N" };

            string userInput = GetUserOption(userIO, options);

            if (userInput == "Y")
            {
                isSure = true;
            }
        }

        public static Metadata GetMetadataFromUser(IUserIO userIO)
        {
            string header = "---------- ADD NEW RECIPE ----------";
            string additionalMessage = "<<< RECIPE BASIC INFO >>>";
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);
            
            userIO.DisplayDataLite("Enter the title of the new recipe: ");
            string title = GetUserInputString(userIO, false, 200);

            userIO.DisplayData();
            userIO.DisplayData("Enter notes about the new recipe (or press \"Enter\" to leave blank):");
            string userNotes = GetUserInputString(userIO, true, 1200);

            userIO.DisplayData();
            userIO.DisplayData("Enter the food type of the new recipe (or press \"Enter\" to leave blank): ");
            string foodType = GetUserInputString(userIO, true, 100);

            userIO.DisplayData();
            userIO.DisplayData("Enter the food genre of the new recipe (or press \"Enter\" to leave blank): ");
            string foodGenre = GetUserInputString(userIO, true, 100);

            userIO.DisplayData();
            userIO.DisplayDataLite("Enter the prep time of recipe in minutes: ");
            int prepTime = GetUserInputInt(userIO, 1);

            while (prepTime > 2880)
            {
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("A recipe cannot have a prep time of more than 2,880 minutes. Please enter a valid prep time:"));
                prepTime = GetUserInputInt(userIO, 1);
            }

            userIO.DisplayData();
            userIO.DisplayDataLite("Enter the cook time of recipe in minutes: ");
            int cookTime = GetUserInputInt(userIO, 1);

            while (cookTime > 1440)
            {
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("A recipe cannot have a cook time of more than 1,440 minutes. Please enter a valid cook time:"));
                cookTime = GetUserInputInt(userIO, 1);
            }

            userIO.DisplayData();
            userIO.DisplayDataLite("Enter the low number of estimated servings: ");
            int lowServings = GetUserInputInt(userIO, 1);

            while (lowServings > 500)
            {
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("A recipe cannot have more than 500 servings. Please enter a valid number of servings:"));
                lowServings = GetUserInputInt(userIO, 1);
            }

            userIO.DisplayData();
            userIO.DisplayData("Enter the high number of estimated servings (or press \"Enter\" to leave blank): ");
            int highServings = GetUserInputInt(userIO, 2, true);

            while (highServings > 500)
            {
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("A recipe cannot have more than 500 servings. Please enter a valid number of servings:"));
                highServings = GetUserInputInt(userIO, 2, true);
            }

            userIO.DisplayData();
            userIO.DisplayData("Basic info complete! Now on to the ingredients!");
            userIO.DisplayData();
            userIO.DisplayData("Press \"Enter\" to continue...");
            userIO.GetInput();

            Tags recipeTags = new Tags(foodType, foodGenre);
            Times recipePrepTimes = new Times(prepTime, cookTime);
            Servings recipeServings = new Servings(lowServings, highServings);
            Metadata recipeMetadata = new Metadata(title, recipePrepTimes, recipeTags, recipeServings, userNotes);

            return recipeMetadata;
        }

        public static IngredientList GetIngredientsFromUser(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            string header = "---------- ADD NEW RECIPE ----------";
            string additionalMessage = "<<< RECIPE INGREDIENTS >>>";
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            IngredientList recipeIngredientList = new IngredientList();

            userIO.DisplayDataLite("Enter the total number of ingredients in the recipe: ");
            int numberOfIngredients = GetUserInputInt(userIO, 2);

            while (numberOfIngredients > 30)
            {
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("A recipe cannot have more than 30 ingredients. Please enter a valid number of ingredients:"));
                numberOfIngredients = GetUserInputInt(userIO, 2);
            }

            for (int i = 0; i < numberOfIngredients; i++)
            {
                header = "---------- ADD NEW RECIPE ----------";
                additionalMessage = $"<<< INGREDIENT {i + 1} >>>";
                UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);
                
                userIO.DisplayDataLite($"Enter the quantity of ingredient {i + 1} needed (ex: 1.5): ");
                double qty = GetUserInputDouble(userIO, 2);

                while (qty > 1000)
                {
                    userIO.DisplayData();
                    userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("An ingredient quantity cannot be more than 1000. Please enter a valid ingredient quantity:"));
                    qty = GetUserInputDouble(userIO, 2);
                }

                List<string[]> measurementUnits = MeasurementUnits.AllMeasurementUnitsForUserInput(recipeBookLibrary);
                List<string> options = new List<string>();
                userIO.DisplayData();
                UserInterface.DisplayOptionsMenu(userIO, measurementUnits, out options);
                
                userIO.DisplayData();
                userIO.DisplayDataLite("Select the ingredient measurement unit from the list of options: ");
                int userOptionNumber = int.Parse(GetUserOption(userIO, options));
                string measurementUnit = "";

                if (userOptionNumber == options.Count)
                {
                    GetNewMeasurementUnitFromUser(userIO, out measurementUnit);
                    recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                    userIO.DisplayData();
                    userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines($"Success! New measurement unit, {measurementUnit}, was added and will be used for this ingredient."));
                }
                else if (measurementUnits[userOptionNumber - 1][1] != "None")
                {
                    measurementUnit = measurementUnits[userOptionNumber - 1][1];
                }

                userIO.DisplayData();
                userIO.DisplayDataLite("Enter the ingredient name: ");
                string name = GetUserInputString(userIO, false, 100);

                userIO.DisplayData();
                userIO.DisplayData("Enter the ingredient preparation note (or press \"Enter\" to leave blank):");
                string preparationNote = GetUserInputString(userIO, true, 120);

                Ingredient ingredientToAdd = new Ingredient(qty, measurementUnit, name, preparationNote);
                recipeIngredientList.AddIngredient(ingredientToAdd);

                if (numberOfIngredients > 1 && i < (numberOfIngredients - 1))
                {
                    userIO.DisplayData();
                    userIO.DisplayData("Ingredient complete! On to the next ingredient.");
                    userIO.DisplayData();
                    userIO.DisplayData("Press \"Enter\" to continue...");
                    userIO.GetInput();
                }
                else
                {
                    userIO.DisplayData();
                    userIO.DisplayData("Ingredients complete!");
                    userIO.DisplayData();
                    userIO.DisplayData("Press \"Enter\" to continue...");
                    userIO.GetInput();
                }
            }

            return recipeIngredientList;
        }

        public static Ingredient GetIngredientFromUser(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            userIO.DisplayDataLite($"Enter the quantity of ingredient needed (ex: 1.5): ");
            double qty = GetUserInputDouble(userIO, 2);

            while (qty > 1000)
            {
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("An ingredient quantity cannot be more than 1000. Please enter a valid ingredient quantity:"));
                qty = GetUserInputDouble(userIO, 2);
            }

            List<string[]> measurementUnits = MeasurementUnits.AllMeasurementUnitsForUserInput(recipeBookLibrary);
            List<string> options = new List<string>();
            userIO.DisplayData();
            UserInterface.DisplayOptionsMenu(userIO, measurementUnits, out options);

            userIO.DisplayData();
            userIO.DisplayDataLite("Select the ingredient measurement unit from the list of options: ");
            int userOptionNumber = int.Parse(GetUserOption(userIO, options));
            string measurementUnit = "";

            if (userOptionNumber == options.Count)
            {
                GetNewMeasurementUnitFromUser(userIO, out measurementUnit);
                recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines($"Success! New measurement unit, {measurementUnit}, was added and will be used for this ingredient."));
            }
            else if (measurementUnits[userOptionNumber - 1][1] != "None")
            {
                measurementUnit = measurementUnits[userOptionNumber - 1][1];
            }

            userIO.DisplayData();
            userIO.DisplayDataLite("Enter the ingredient name: ");
            string name = GetUserInputString(userIO, false, 100);

            userIO.DisplayData();
            userIO.DisplayData("Enter the ingredient preparation note (or press \"Enter\" to leave blank):");
            string preparationNote = GetUserInputString(userIO, true, 120);

            Ingredient ingredientToAdd = new Ingredient(qty, measurementUnit, name, preparationNote);
            return ingredientToAdd;
        }

        public static CookingInstructions GetCookingInstructionsFromUser(IUserIO userIO)
        {
            userIO.ClearDisplay();
            userIO.DisplayData("---------- ADD NEW RECIPE ----------");
            userIO.DisplayData();
            userIO.DisplayData("<<< RECIPE INSTRUCTIONS >>>");

            userIO.DisplayData();
            userIO.DisplayDataLite("Enter the number of cooking instruction blocks (most recipes only have 1): ");
            int numberOfInstructionBlocks = GetUserInputInt(userIO, 2);

            while (numberOfInstructionBlocks > 5)
            {
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("A recipe cannot have more than 5 instruction blocks. Please enter a valid number of instruction blocks:"));
                numberOfInstructionBlocks = GetUserInputInt(userIO, 2);
            }

            CookingInstructions recipeCookingInstructions = new CookingInstructions();

            for (int i = 0; i < numberOfInstructionBlocks; i++)
            {
                userIO.ClearDisplay();
                userIO.DisplayData("---------- ADD NEW RECIPE ----------");
                userIO.DisplayData();
                userIO.DisplayData($"<<< INSTRUCTION BLOCK {i + 1} >>>");
                userIO.DisplayData();

                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines($"Enter the heading for instruction block {i + 1} (or press \"Enter\" to leave blank):"));
                string blockHeading = GetUserInputString(userIO, true, 100);

                InstructionBlock instructionBlockToAdd = new InstructionBlock(blockHeading);

                userIO.DisplayData();
                userIO.DisplayDataLite($"Enter the number of instruction lines for instruction block {i + 1}: ");
                int numberOfLines = GetUserInputInt(userIO, 2);

                while (numberOfLines > 20)
                {
                    userIO.DisplayData();
                    userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("An instruction block cannot have more than 20 instruction lines. Please enter a valid number of instruction lines:"));
                    numberOfLines = GetUserInputInt(userIO, 2);
                }

                for (int j = 0; j < numberOfLines; j++)
                {
                    userIO.DisplayData();
                    userIO.DisplayData($"Enter the instructions for line #{j + 1}:");
                    string instructionLine = GetUserInputString(userIO, false, 400);

                    instructionBlockToAdd.AddInstructionLine(instructionLine);
                }

                recipeCookingInstructions.AddInstructionBlock(instructionBlockToAdd);

                if (numberOfInstructionBlocks > 1 && i < (numberOfInstructionBlocks - 1))
                {
                    userIO.DisplayData();
                    userIO.DisplayData("Instruction block complete! On to the next instruction block.");
                    userIO.DisplayData();
                    userIO.DisplayData("Press \"Enter\" to continue...");
                    userIO.GetInput();
                }
                else if (numberOfInstructionBlocks > 1 && i == (numberOfInstructionBlocks - 1))
                {
                    userIO.DisplayData();
                    userIO.DisplayData("Instruction blocks complete!");
                    userIO.DisplayData();
                    userIO.DisplayData("Press \"Enter\" to continue...");
                    userIO.GetInput();
                }
                else
                {
                    userIO.DisplayData();
                    userIO.DisplayData("Instruction block complete!");
                    userIO.DisplayData();
                    userIO.DisplayData("Press \"Enter\" to continue...");
                    userIO.GetInput();
                }
            }

            return recipeCookingInstructions;
        }

        public static InstructionBlock GetInstructionBlockFromUser(IUserIO userIO)
        {
            userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines($"Enter the heading for the new instruction block (or press \"Enter\" to leave blank):"));
            string blockHeading = GetUserInputString(userIO, true, 100);

            InstructionBlock newInstructionBlock = new InstructionBlock(blockHeading);

            userIO.DisplayData();
            userIO.DisplayDataLite($"Enter the number of instruction lines for the instruction block: ");
            int numberOfLines = GetUserInputInt(userIO, 2);

            while (numberOfLines > 20)
            {
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("An instruction block cannot have more than 20 instruction lines. Please enter a valid number of instruction lines:"));
                numberOfLines = GetUserInputInt(userIO, 2);
            }

            for (int j = 0; j < numberOfLines; j++)
            {
                userIO.DisplayData();
                userIO.DisplayData($"Enter the instructions for line #{j + 1}:");
                string instructionLine = GetUserInputString(userIO, false, 400);

                newInstructionBlock.AddInstructionLine(instructionLine);
            }

            return newInstructionBlock;
        }

        public static void GetNewMeasurementUnitFromUser(IUserIO userIO, out string measurementUnit)
        {
            userIO.DisplayData();
            userIO.DisplayData("Enter the name of the new measurement unit:");
            measurementUnit = GetUserInputString(userIO, false, 30);
        }

        public static string GetTheFieldToEditFromUser(IUserIO userIO, Recipe recipe, List<string[]> editOptions)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            List<string> userOptions = new List<string>();
            UserInterface.DisplayOptionsMenu(userIO, editOptions, out userOptions);

            userIO.DisplayData();
            userIO.DisplayDataLite("Select the option for the information you would like to edit: ");
            string userOption = GetUserOption(userIO, userOptions);

            return userOption;
        }

        public static string GetNewUserNotes(IUserIO userIO, Recipe recipe)
        {
            int currentLengthOfNotes = recipe.Metadata.Notes.Length;
            int maxLengthOfNotes = 1200;
            int maxLengthOfNewNote = maxLengthOfNotes - currentLengthOfNotes;
            
            userIO.DisplayData("Enter the notes you would like to add to this recipe:");
            string newNotes = GetUserInputString(userIO, false, maxLengthOfNewNote);

            return newNotes;
        }
    }
}
