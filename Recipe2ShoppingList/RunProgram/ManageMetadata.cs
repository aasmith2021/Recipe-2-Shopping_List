using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public static class ManageMetadata
    {
        public static void EditRecipeTitle(IUserIO userIO, Recipe recipe)
        {
            string header = ManageRecipes.editRecipeBanner;
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayLitePrompt(userIO, "Enter the new title for this recipe", false);
            string newTitle = GetUserInput.GetUserInputString(userIO, false, 200);

            GetUserInput.AreYouSure(userIO, $"change the name of this recipe to {newTitle}", out bool isSure);

            if (isSure)
            {
                recipe.Metadata.Title = newTitle;
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "recipe", "renamed");
        }

        public static void EditRecipeNotes(IUserIO userIO, Recipe recipe)
        {
            string header = ManageRecipes.editRecipeBanner;
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            string userInput = "";
            if (recipe.Metadata.Notes != "")
            {
                UserInterface.DisplayInformation(userIO, "Do you want to add to the existing notes or delete the old notes and add a new note?", false);
                UserInterface.DisplayRegularPrompt(userIO, "Enter \"A\" to Add to the existing notes or \"D\" to Delete the old notes and add a new note");
                List<string> options = new List<string>() { "A", "D" };
                userInput = GetUserInput.GetUserOption(userIO, options);
            }

            UserInterface.InsertBlankLine(userIO);
            string newNotes = GetUserInput.GetNewUserNotes(userIO, recipe);

            GetUserInput.AreYouSure(userIO, "add these new notes to the recipe", out bool isSure);

            if (isSure)
            {
                if (userInput == "D")
                {
                    recipe.Metadata.Notes = "";
                }
                recipe.Metadata.Notes += " " + newNotes;
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "recipe notes", "updated", true);
        }

        public static void EditRecipePrepTimes(IUserIO userIO, Recipe recipe)
        {
            string header = ManageRecipes.editRecipeBanner;
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInformation(userIO, "Do you want to change the Prep Time, the Cook Time, or Both?", false);
            UserInterface.DisplayRegularPrompt(userIO, "Enter \"P\" to change the Prep Time, \"C\" to change the Cook Time, or \"B\" to change both");

            List<string> options = new List<string>() { "P", "C", "B" };
            string userInput = GetUserInput.GetUserOption(userIO, options);

            int changeStartIndex = 0;
            int changeEndIndex = 0;

            switch (userInput)
            {
                case "B":
                    changeStartIndex = 0;
                    changeEndIndex = 1;
                    break;
                case "P":
                    changeStartIndex = 0;
                    changeEndIndex = 0;
                    break;
                case "C":
                    changeStartIndex = 1;
                    changeEndIndex = 1;
                    break;
                default:
                    break;
            }

            int newPrepTime = 0;
            int newCookTime = 0;

            for (int i = changeStartIndex; i <= changeEndIndex; i++)
            {
                if (i == 0)
                {
                    UserInterface.DisplayLitePrompt(userIO, "Enter the new Prep Time in minutes");
                    newPrepTime = GetUserInput.GetUserInputInt(userIO, 1);
                    while (newPrepTime > 2880)
                    {
                        UserInterface.DisplayRegularPrompt(userIO, UserInterface.MakeStringConsoleLengthLines("A recipe cannot have a prep time of more than 2,880 minutes. Please enter a valid prep time"));
                        newPrepTime = GetUserInput.GetUserInputInt(userIO, 1);
                    }
                }
                else
                {
                    UserInterface.DisplayLitePrompt(userIO, "Enter the new Cook Time in minutes");
                    newCookTime = GetUserInput.GetUserInputInt(userIO, 1);
                    while (newCookTime > 1440)
                    {
                        UserInterface.DisplayRegularPrompt(userIO, UserInterface.MakeStringConsoleLengthLines("A recipe cannot have a cook time of more than 1,440 minutes. Please enter a valid cook time"));
                        newCookTime = GetUserInput.GetUserInputInt(userIO, 1);
                    }
                }
            }

            GetUserInput.AreYouSure(userIO, $"update the recipe preparation times", out bool isSure);

            if (isSure)
            {
                if (userInput == "B" || userInput == "P")
                {
                    recipe.Metadata.PrepTimes.PrepTime = newPrepTime;
                }

                if (userInput == "B" || userInput == "C")
                {
                    recipe.Metadata.PrepTimes.CookTime = newCookTime;
                }
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "recipe preparation times", "updated", true);
        }

        public static void EditRecipeEstimatedServings(IUserIO userIO, Recipe recipe)
        {
            string header = ManageRecipes.editRecipeBanner;
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInformation(userIO, "Do you want to change the Low # of Servings, High # of Servings, or Both?", false);
            UserInterface.DisplayRegularPrompt(userIO, UserInterface.MakeStringConsoleLengthLines("Enter \"L\" to change the Low # of Servings, \"H\" to change the High # of Servings, or \"B\" to change both"));
            List<string> options = new List<string>() { "L", "H", "B" };
            string userInput = GetUserInput.GetUserOption(userIO, options);

            int changeStartIndex = 0;
            int changeEndIndex = 0;

            switch (userInput)
            {
                case "B":
                    changeStartIndex = 0;
                    changeEndIndex = 1;
                    break;
                case "L":
                    changeStartIndex = 0;
                    changeEndIndex = 0;
                    break;
                case "H":
                    changeStartIndex = 1;
                    changeEndIndex = 1;
                    break;
                default:
                    break;
            }

            int newLowServings = 0;
            int newHighServings = 0;

            for (int i = changeStartIndex; i <= changeEndIndex; i++)
            {
                if (i == 0)
                {
                    UserInterface.DisplayLitePrompt(userIO, "Enter the new Low # of Servings");
                    newLowServings = GetUserInput.GetUserInputInt(userIO, 1);
                    while (newLowServings > 500)
                    {
                        UserInterface.DisplayRegularPrompt(userIO, UserInterface.MakeStringConsoleLengthLines("A recipe cannot have more than 500 servings. Please enter a valid number of servings"));
                        newLowServings = GetUserInput.GetUserInputInt(userIO, 1);
                    }
                }
                else
                {
                    UserInterface.DisplayLitePrompt(userIO, "Enter the new High # of Servings");
                    newHighServings = GetUserInput.GetUserInputInt(userIO, 1);
                    while (newHighServings > 500)
                    {
                        UserInterface.DisplayRegularPrompt(userIO, UserInterface.MakeStringConsoleLengthLines("A recipe cannot have more than 500 servings. Please enter a valid number of servings"));
                        newHighServings = GetUserInput.GetUserInputInt(userIO, 1);
                    }
                }
            }

            GetUserInput.AreYouSure(userIO, $"update the recipe estimated servings", out bool isSure);

            if (isSure)
            {
                if (userInput == "B" || userInput == "L")
                {
                    recipe.Metadata.Servings.LowNumberOfServings = newLowServings;
                }

                if (userInput == "B" || userInput == "H")
                {
                    recipe.Metadata.Servings.HighNumberOfServings = newHighServings;
                }
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "recipe estimated servings", "updated", true);
        }

        public static void EditRecipeFoodTypeGenre(IUserIO userIO, Recipe recipe)
        {
            string header = ManageRecipes.editRecipeBanner;
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInformation(userIO, "Do you want to change the Food Type, Food Genre, or Both?", false);
            UserInterface.DisplayRegularPrompt(userIO, UserInterface.MakeStringConsoleLengthLines("Enter \"T\" to change the Food Type, \"G\" to change the Food Genre, or \"B\" to change both"));
            List<string> options = new List<string>() { "T", "G", "B" };
            string userInput = GetUserInput.GetUserOption(userIO, options);

            int changeStartIndex = 0;
            int changeEndIndex = 0;

            switch (userInput)
            {
                case "B":
                    changeStartIndex = 0;
                    changeEndIndex = 1;
                    break;
                case "T":
                    changeStartIndex = 0;
                    changeEndIndex = 0;
                    break;
                case "G":
                    changeStartIndex = 1;
                    changeEndIndex = 1;
                    break;
                default:
                    break;
            }

            string newFoodType = "";
            string newFoodGenre = "";

            for (int i = changeStartIndex; i <= changeEndIndex; i++)
            {
                if (i == 0)
                {
                    UserInterface.DisplayLitePrompt(userIO, "Enter the new Food Type");
                    newFoodType = GetUserInput.GetUserInputString(userIO, true, 100);
                }
                else
                {
                    UserInterface.DisplayLitePrompt(userIO, "Enter the new Food Genre");
                    newFoodGenre = GetUserInput.GetUserInputString(userIO, true, 100);
                }
            }

            GetUserInput.AreYouSure(userIO, $"update the recipe food type/genre", out bool isSure);

            if (isSure)
            {
                if (userInput == "B" || userInput == "T")
                {
                    recipe.Metadata.Tags.FoodType = newFoodType;
                }

                if (userInput == "B" || userInput == "G")
                {
                    recipe.Metadata.Tags.FoodGenre = newFoodGenre;
                }

            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "recipe food type/genre", "updated");
        }
    }
}
