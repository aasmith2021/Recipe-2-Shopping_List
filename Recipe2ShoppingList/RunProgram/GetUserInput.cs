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
    }
}
