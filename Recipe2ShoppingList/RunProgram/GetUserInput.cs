using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class GetUserInput
    {
        public static string GetUserOption(List<string> options)
        {
            string userInput = Console.ReadLine().ToUpper();

            while (!options.Contains(userInput))
            {
                Console.WriteLine();
                Console.Write("Invalid entry. Please enter an option from the list: ");
                userInput = Console.ReadLine().ToUpper();
            }

            return userInput;
        }

        public static string GetUserInputString(bool allowEmptyStringInput = true)
        {
            string userInput;
            bool inputIsNull;
            bool inputIsEmptyString;
            bool userInputIsValid = false;

            do
            {
                userInput = Console.ReadLine();

                inputIsNull = userInput == null;
                inputIsEmptyString = userInput == "";

                if (inputIsNull || (inputIsEmptyString && !allowEmptyStringInput))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid entry. Please try again:");
                }
                else
                {
                    userInputIsValid = true;
                }
            }
            while (!userInputIsValid);

            return userInput;
        }

        public static int GetUserInputInt(int inputOption = 0, bool allowEmpyStringInput = false)
        {
            //Option -2: Get negative integer
            //Option -1: Get negative or zero integer
            //Option 0: Get any integer
            //Option 1: Get positive or zero integer
            //Option 2: Get positive integer

            int result;
            string userInput = Console.ReadLine();

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
                    Console.WriteLine();

                    switch (inputOption)
                    {
                        case -2:
                            Console.Write("Invalid entry. Please enter a negative integer: ");
                            break;
                        case -1:
                            Console.Write("Invalid entry. Please enter an integer that is zero or negative: ");
                            break;
                        case 0:
                            Console.Write("Invalid entry. Please enter an integer: ");
                            break;
                        case 1:
                            Console.Write("Invalid entry. Please enter an integer that is zero or positive: ");
                            break;
                        case 2:
                            Console.Write("Invalid entry. Please enter an integer that is greater than zero: ");
                            break;
                        default:
                            break;
                    }

                    userInput = Console.ReadLine();
                }
            }

            return result;
        }

        public static double GetUserInputDouble(int inputOption = 0, bool allowEmpyStringInput = false)
        {
            //Option -2: Get negative double
            //Option -1: Get negative or zero double
            //Option 0: Get any double
            //Option 1: Get positive or zero double
            //Option 2: Get positive double

            double result;
            string userInput = Console.ReadLine();

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
                    Console.WriteLine();

                    switch (inputOption)
                    {
                        case -2:
                            Console.Write(UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a negative number that has no more than 3 decimal places (ex: -1.125): "));
                            break;
                        case -1:
                            Console.Write(UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that is zero or negative and has no more than 3 decimal places (ex: -1.125): "));
                            break;
                        case 0:
                            Console.Write(UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that has no more than 3 decimal places (ex: 1.125): "));
                            break;
                        case 1:
                            Console.Write(UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that is zero or positive and has no more than 3 decimal places (ex: 1.125): "));
                            break;
                        case 2:
                            Console.Write(UserInterface.MakeStringConsoleLengthLines("Invalid entry. Please enter a number that is greater than zero and has no more than 3 decimal places (ex: 1.125): "));
                            break;
                        default:
                            break;
                    }

                    userInput = Console.ReadLine();
                }
            }

            return result;
        }

        public static void AreYouSure(string changeMessage, out bool isSure)
        {
            isSure = false;

            Console.WriteLine();
            Console.WriteLine(UserInterface.MakeStringConsoleLengthLines($"Are you sure you want to {changeMessage}?"));
            Console.WriteLine();
            Console.Write("Enter \"Y\" for Yes or \"N\" for No: ");

            List<string> options = new List<string>() { "Y", "N" };

            string userInput = GetUserOption(options);

            if (userInput == "Y")
            {
                isSure = true;
            }
        }

        public static Metadata GetMetadataFromUser()
        {
            string header = "---------- ADD NEW RECIPE ----------";
            string additionalMessage = "<<< RECIPE BASIC INFO >>>";
            UserInterface.DisplayMenuHeader(header, additionalMessage);
            
            Console.Write("Enter the title of the new recipe: ");
            string title = GetUserInputString(false);

            Console.WriteLine();
            Console.WriteLine("Enter notes about the new recipe (or press \"Enter\" to leave blank):");
            string userNotes = GetUserInputString(true);

            Console.WriteLine();
            Console.WriteLine("Enter the food type of the new recipe (or press \"Enter\" to leave blank): ");
            string foodType = GetUserInputString(true);

            Console.WriteLine();
            Console.WriteLine("Enter the food genre of the new recipe (or press \"Enter\" to leave blank): ");
            string foodGenre = GetUserInputString(true);

            Console.WriteLine();
            Console.Write("Enter the prep time of recipe in minutes: ");
            int prepTime = GetUserInputInt(1);

            Console.WriteLine();
            Console.Write("Enter the cook time of recipe in minutes: ");
            int cookTime = GetUserInputInt(1);

            Console.WriteLine();
            Console.Write("Enter the low number of estimated servings: ");
            int lowServings = GetUserInputInt(2);

            Console.WriteLine();
            Console.WriteLine("Enter the high number of estimated servings (or press \"Enter\" to leave blank): ");
            int highServings = GetUserInputInt(2, true);

            Console.WriteLine();
            Console.WriteLine("Basic info complete! Now on to the ingredients!");
            Console.WriteLine();
            Console.WriteLine("Press \"Enter\" to continue...");
            Console.ReadLine();

            Tags recipeTags = new Tags(foodType, foodGenre);
            Times recipePrepTimes = new Times(prepTime, cookTime);
            Servings recipeServings = new Servings(lowServings, highServings);
            Metadata recipeMetadata = new Metadata(title, recipePrepTimes, recipeTags, recipeServings, userNotes);

            return recipeMetadata;
        }

        public static IngredientList GetIngredientsFomUser(RecipeBookLibrary recipeBookLibrary)
        {
            string header = "---------- ADD NEW RECIPE ----------";
            string additionalMessage = "<<< RECIPE INGREDIENTS >>>";
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            IngredientList recipeIngredientList = new IngredientList();

            Console.Write("Enter the total number of ingredients in the recipe: ");
            int numberOfIngredients = GetUserInput.GetUserInputInt(2);

            for (int i = 0; i < numberOfIngredients; i++)
            {
                header = "---------- ADD NEW RECIPE ----------";
                additionalMessage = $"<<< INGREDIENT {i + 1} >>>";
                UserInterface.DisplayMenuHeader(header, additionalMessage);
                
                Console.Write($"Enter the quantity of ingredient {i + 1} needed (ex: 1.5): ");
                double qty = GetUserInputDouble(2);

                List<string[]> measurementUnits = MeasurementUnits.AllMeasurementUnitsForUserInput(recipeBookLibrary);
                List<string> options = new List<string>();
                UserInterface.DisplayOptionsMenu(measurementUnits, out options);
                
                Console.WriteLine();
                Console.Write("Select the ingredient measurement unit from the list of options: ");
                int userOptionNumber = int.Parse(GetUserOption(options));
                string measurementUnit = "";

                if (userOptionNumber == options.Count)
                {
                    GetNewMeasurementUnitFromUser(out measurementUnit);
                    recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                    Console.WriteLine();
                    Console.WriteLine(UserInterface.MakeStringConsoleLengthLines($"Success! New measurement unit, {measurementUnit}, was added and will be used for this ingredient."));
                }
                else if (measurementUnits[userOptionNumber - 1][1] != "None")
                {
                    measurementUnit = measurementUnits[userOptionNumber - 1][1];
                }

                Console.WriteLine();
                Console.Write("Enter the ingredient name: ");
                string name = GetUserInputString(false);

                Console.WriteLine();
                Console.WriteLine("Enter the ingredient preparation note (or press \"Enter\" to leave blank):");
                string preparationNote = GetUserInputString(true);

                Ingredient ingredientToAdd = new Ingredient(qty, measurementUnit, name, preparationNote);
                recipeIngredientList.AddIngredient(ingredientToAdd);

                if (numberOfIngredients > 1 && i < (numberOfIngredients - 1))
                {
                    Console.WriteLine();
                    Console.WriteLine("Ingredient complete! On to the next ingredient.");
                    Console.WriteLine();
                    Console.WriteLine("Press \"Enter\" to continue...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Ingredients complete!");
                    Console.WriteLine();
                    Console.WriteLine("Press \"Enter\" to continue...");
                    Console.ReadLine();
                }
            }

            return recipeIngredientList;
        }

        public static CookingInstructions GetCookingInstructionsFromUser()
        {
            Console.Clear();
            Console.WriteLine("---------- ADD NEW RECIPE ----------");
            Console.WriteLine();
            Console.WriteLine("<<< RECIPE INSTRUCTIONS >>>");

            Console.WriteLine();
            Console.Write("Enter the number of cooking instruction blocks (most recipes only have 1): ");
            int numberOfInstructionBlocks = GetUserInput.GetUserInputInt(2);

            CookingInstructions recipeCookingInstructions = new CookingInstructions();

            for (int i = 0; i < numberOfInstructionBlocks; i++)
            {
                Console.Clear();
                Console.WriteLine("---------- ADD NEW RECIPE ----------");
                Console.WriteLine();
                Console.WriteLine($"<<< INSTRUCTION BLOCK {i + 1} >>>");
                Console.WriteLine();

                Console.WriteLine(UserInterface.MakeStringConsoleLengthLines($"Enter the heading for instruction block {i + 1} (or press \"Enter\" to leave blank): "));
                string blockHeading = GetUserInput.GetUserInputString(true);

                InstructionBlock instructionBlockToAdd = new InstructionBlock(blockHeading);

                Console.WriteLine();
                Console.Write($"Enter the number of instruction lines for instruction block {i + 1}: ");
                int numberOfLines = GetUserInput.GetUserInputInt(2);

                for (int j = 0; j < numberOfLines; j++)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Enter the instructions for line #{j + 1}:");
                    string instructionLine = GetUserInput.GetUserInputString(false);

                    instructionBlockToAdd.AddInstructionLine(instructionLine);
                }

                recipeCookingInstructions.AddInstructionBlock(instructionBlockToAdd);

                if (numberOfInstructionBlocks > 1 && i < (numberOfInstructionBlocks - 1))
                {
                    Console.WriteLine();
                    Console.WriteLine("Instruction block complete! On to the next instruction block.");
                    Console.WriteLine();
                    Console.WriteLine("Press \"Enter\" to continue...");
                    Console.ReadLine();
                }
                else if (numberOfInstructionBlocks > 1 && i == (numberOfInstructionBlocks - 1))
                {
                    Console.WriteLine();
                    Console.WriteLine("Instruction blocks complete!");
                    Console.WriteLine();
                    Console.WriteLine("Press \"Enter\" to continue...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Instruction block complete!");
                    Console.WriteLine();
                    Console.WriteLine("Press \"Enter\" to continue...");
                    Console.ReadLine();
                }
            }

            return recipeCookingInstructions;
        }

        public static void GetNewMeasurementUnitFromUser(out string measurementUnit)
        {
            Console.WriteLine();
            Console.WriteLine("Enter the name of the new measurement unit:");
            measurementUnit = GetUserInputString(false);
        }

        public static string GetTheFieldToEditFromUser(Recipe recipe, List<string[]> editOptions)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(header, additionalMessage);

            List<string> userOptions = new List<string>();
            UserInterface.DisplayOptionsMenu(editOptions, out userOptions);

            Console.WriteLine();
            Console.Write("Select the option for the information you would like to edit: ");
            string userOption = GetUserOption(userOptions);

            return userOption;
        }

        public static string GetNewUserNotes()
        {
            Console.WriteLine("Enter the notes you would like to add to this recipe:");
            string newNotes = GetUserInput.GetUserInputString(false);

            return newNotes;
        }
    }
}
