using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public static class ManageRecipeBooks
    {
        //Prompts user and captures input to add a new recipe book to the recipe book library
        public static void AddNewRecipeBook(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            UserInterface.DisplayMenuHeader(userIO, "---------- ADD NEW RECIPE BOOK ----------");

            UserInterface.DisplayLitePrompt(userIO, "Enter a name for the new recipe book", false);
            string bookName = GetUserInput.GetRecipeBookNameFromUser(userIO);

            GetUserInput.AreYouSure(userIO, $"create a new recipe book named {bookName}", out bool isSure);

            if (isSure)
            {
                RecipeBook newRecipeBook = new RecipeBook(bookName);
                recipeBookLibrary.AddRecipeBook(newRecipeBook);
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "new recipe book", "created");
        }

        //Prompts user and captures input to rename a recipe book
        public static void RenameRecipeBook(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            UserInterface.DisplayMenuHeader(userIO, "---------- RENAME RECIPE BOOK ----------");

            List<string[]> recipeBooksToDisplay = new List<string[]>();
            List<string> recipeBookOptions = new List<string>();

            for (int i = 0; i < recipeBookLibrary.AllRecipeBooks.Count; i++)
            {
                recipeBooksToDisplay.Add(new string[] { (i + 1).ToString(), recipeBookLibrary.AllRecipeBooks[i].Name });
            }

            UserInterface.DisplayOptionsMenu(userIO, recipeBooksToDisplay, out recipeBookOptions);
            UserInterface.DisplayLitePrompt(userIO, "Enter the recipe book you would like to rename");
            int userOption = int.Parse(GetUserInput.GetUserOption(userIO, recipeBookOptions));

            RecipeBook recipeBookToRename = recipeBookLibrary.AllRecipeBooks[userOption - 1];
            string oldName = recipeBookToRename.Name;

            UserInterface.DisplayLitePrompt(userIO, "Enter the new name for the recipe book");
            string newName = GetUserInput.GetRecipeBookNameFromUser(userIO);

            GetUserInput.AreYouSure(userIO, $"rename the {oldName} recipe book to \"{newName}\"", out bool isSure);

            if (isSure)
            {
                recipeBookToRename.Name = newName;
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "recipe book", "renamed");
        }

        //Prompts user and captures input to delete a recipe book
        public static void DeleteRecipeBook(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            UserInterface.DisplayMenuHeader(userIO, "---------- DELETE RECIPE BOOK ----------");

            List<string[]> recipeBooksToDisplay = new List<string[]>();
            List<string> recipeBookOptions = new List<string>();

            for (int i = 0; i < recipeBookLibrary.AllRecipeBooks.Count; i++)
            {
                recipeBooksToDisplay.Add(new string[] { (i + 1).ToString(), recipeBookLibrary.AllRecipeBooks[i].Name });
            }

            UserInterface.DisplayOptionsMenu(userIO, recipeBooksToDisplay, out recipeBookOptions);
            UserInterface.DisplayLitePrompt(userIO, "Enter the recipe book you would like to delete");
            int userOption = int.Parse(GetUserInput.GetUserOption(userIO, recipeBookOptions));

            RecipeBook recipeBookToDelete = recipeBookLibrary.AllRecipeBooks[userOption - 1];

            GetUserInput.AreYouSure(userIO, $"delete the {recipeBookToDelete.Name} recipe book", out bool isSure);

            if (isSure)
            {
                recipeBookLibrary.DeleteRecipeBook(recipeBookToDelete);
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "recipe book", "deleted");
        }
    }
}
