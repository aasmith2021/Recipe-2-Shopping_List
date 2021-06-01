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
            string userInput = Console.ReadLine();

            bool inputIsNull = userInput == null;
            bool inputIsEmptyString = userInput == "";
            bool userInputIsValid = true;

            if (inputIsNull || (inputIsEmptyString && !allowEmptyStringInput))
            {
                userInputIsValid = false;
            }

            while (!userInputIsValid)
            {
                Console.WriteLine();
                Console.Write("Invalid entry. Please try again: ");
                userInput = Console.ReadLine();
            }

            return userInput;
        }

        public static int GetUserInputInt(int inputOption = 0)
        {
            //Option -2: Get negative integer
            //Option -1: Get negative or zero integer
            //Option 0: Get any integer
            //Option 1: Get positive or zero integer
            //Option 2: Get positive integer

            int result;
            string userInput = Console.ReadLine();

            while (!Int32.TryParse(userInput, out result))
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
            Console.Clear();
            Console.WriteLine("---------- ADD NEW RECIPE ----------");
            Console.WriteLine();
            Console.WriteLine("<<< RECIPE BASIC INFO >>>");
            Console.WriteLine();
            Console.Write("Enter the title of the new recipe: ");
            string title = GetUserInput.GetUserInputString(false);

            Console.WriteLine();
            Console.WriteLine("Enter notes about the new recipe (or press \"Enter\" to leave blank):");
            string userNotes = GetUserInput.GetUserInputString(true);

            Console.WriteLine();
            Console.WriteLine("Enter the food type of the new recipe (or press \"Enter\" to leave blank): ");
            string foodType = GetUserInput.GetUserInputString(true);

            Console.WriteLine();
            Console.WriteLine("Enter the food genre of the new recipe (or press \"Enter\" to leave blank): ");
            string foodGenre = GetUserInput.GetUserInputString(true);

            Console.WriteLine();
            Console.Write("Enter the prep time of recipe in minutes: ");
            int prepTime = GetUserInput.GetUserInputInt(1);

            Console.WriteLine();
            Console.Write("Enter the cook time of recipe in minutes: ");
            int cookTime = GetUserInput.GetUserInputInt(1);

            Console.WriteLine();
            Console.Write("Enter the low number of estimated servings: ");
            int lowServings = GetUserInput.GetUserInputInt(2);

            Console.WriteLine();
            Console.WriteLine("Enter the high number of estimated servings (or press \"Enter\" to leave blank): ");
            int highServings = GetUserInput.GetUserInputInt(2);

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

        public static IngredientList GetIngredientsFomUser()
        {
            Console.Clear();
            Console.WriteLine("---------- ADD NEW RECIPE ----------");
            Console.WriteLine();
            Console.WriteLine("<<< RECIPE INGREDIENTS >>>");
            Console.WriteLine();

            IngredientList recipeIngredientList = new IngredientList();

            Console.Write("Enter the total number of ingredients in the recipe: ");
            int numberOfIngredients = GetUserInput.GetUserInputInt(2);

            for (int i = 0; i < numberOfIngredients; i++)
            {
                Console.Clear();
                Console.WriteLine("---------- ADD NEW RECIPE ----------");
                Console.WriteLine();
                Console.WriteLine($"<<< INGREDIENT {i + 1} >>>");

                Console.WriteLine();
                Console.Write($"Enter the quantity of ingredient {i + 1} needed (ex: 1 1/2): ");
                string qty = GetUserInput.GetUserInputString(false);

                List<string[]> measurementUnits = MeasurementUnits.AllMeasurementUnits();
                List<string> options = new List<string>();
                foreach (string[] element in measurementUnits)
                {
                    options.Add(element[0]);
                }

                UserInterface.DisplayOptionsMenu(measurementUnits);
                
                Console.WriteLine();
                Console.Write("Select the ingredient measurement unit: ");
                int userOptionNumber = Int32.Parse(GetUserInput.GetUserOption(options));
                string measurementUnit = measurementUnits[userOptionNumber - 1][1];

                Console.WriteLine();
                Console.Write("Enter the ingredient name: ");
                string name = GetUserInput.GetUserInputString(false);

                Ingredient ingredientToAdd = new Ingredient(qty, measurementUnit, name);
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
    }
}
