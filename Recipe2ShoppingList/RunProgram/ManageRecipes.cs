using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public static class ManageRecipes
    {
        public const string editRecipeBanner = "---------- EDIT RECIPE ----------";
        private const string deleteRecipeBanner = "---------- DELETE RECIPE ----------";
        private const string addRecipeToShoppingListBanner = "-------- ADD RECIPE TO SHOPPING LIST --------";

        public static void AddNewRecipe(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, RecipeBook recipeBook)
        {
            Metadata recipeMetadata = GetUserInput.GetMetadataFromUser(userIO);
            IngredientList recipeIngredientList = GetUserInput.GetIngredientsFromUser(userIO, recipeBookLibrary);
            CookingInstructions recipeCookingInstructions = GetUserInput.GetCookingInstructionsFromUser(userIO);

            Recipe newRecipe = new Recipe(recipeMetadata, recipeCookingInstructions, recipeIngredientList);

            GetUserInput.AreYouSure(userIO, $"add this new recipe", out bool isSure);

            if (isSure)
            {
                recipeBook.AddRecipe(newRecipe);
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "new recipe", "added to the recipe book");
        }

        public static void EditExistingRecipe(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, RecipeBook recipeBook)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner);

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string> recipeOptions = new List<string>();

            for (int i = 0; i < recipeBook.Recipes.Count; i++)
            {
                recipesToDisplay.Add(new string[] { (i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title });
            }

            UserInterface.DisplayOptionsMenu(userIO, recipesToDisplay, out recipeOptions);
            UserInterface.DisplayLitePrompt(userIO, "Enter the recipe you would like to edit");
            int userOption = int.Parse(GetUserInput.GetUserOption(userIO, recipeOptions));

            Recipe recipeToEdit = recipeBook.Recipes[userOption - 1];

            ProgramExecution.RunEditRecipe(userIO, recipeBookLibrary, recipeToEdit);
        }

        public static void DeleteOpenRecipe(IUserIO userIO, RecipeBook recipeBook, Recipe recipeToDelete)
        {
            UserInterface.DisplayMenuHeader(userIO, deleteRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe Title: {recipeToDelete.Metadata.Title}"), false);

            GetUserInput.AreYouSure(userIO, $"delete this recipe", out bool isSure);

            if (isSure)
            {
                recipeBook.DeleteRecipe(recipeToDelete);
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "recipe", "deleted");
        }

        public static void DeleteExistingRecipe(IUserIO userIO, RecipeBook recipeBook)
        {
            UserInterface.DisplayMenuHeader(userIO, deleteRecipeBanner);

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string> recipeOptions = new List<string>();

            for (int i = 0; i < recipeBook.Recipes.Count; i++)
            {
                recipesToDisplay.Add(new string[] { (i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title });
            }

            UserInterface.DisplayOptionsMenu(userIO, recipesToDisplay, out recipeOptions);
            userIO.DisplayData();
            userIO.DisplayDataLite("Enter the recipe you would like to delete: ");
            int userOption = int.Parse(GetUserInput.GetUserOption(userIO, recipeOptions));

            Recipe recipeToDelete = recipeBook.Recipes[userOption - 1];
            string recipeTitle = recipeToDelete.Metadata.Title;

            GetUserInput.AreYouSure(userIO, $"delete the {recipeTitle} recipe", out bool isSure);

            if (isSure)
            {
                recipeBook.DeleteRecipe(recipeToDelete);
                UserInterface.SuccessfulChange(userIO, true, $"recipe", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "recipe", "deleted");
            }
        }

        public static void AddRecipeToShoppingList(IUserIO userIO, ShoppingList shoppingList, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, addRecipeToShoppingListBanner);

            List<Ingredient> recipeIngredientList = recipe.IngredientList.AllIngredients;

            foreach (Ingredient element in recipeIngredientList)
            {
                if (element.StoreLocation == "")
                {
                    ManageIngredients.GetStoreLocationForIngredient(userIO, shoppingList, element);
                }

                ManageIngredients.AddIngredientToStoreLocation(userIO, shoppingList, element);
            }

            UserInterface.SuccessfulChange(userIO, true, "recipe", "added to the shopping list");
        }

        public static void AddExistingRecipeToShoppingList(IUserIO userIO, RecipeBook recipeBook, ShoppingList shoppingList)
        {
            UserInterface.DisplayMenuHeader(userIO, addRecipeToShoppingListBanner);

            List<string[]> recipesToDisplay = new List<string[]>();
            List<string> recipeOptions = new List<string>();

            for (int i = 0; i < recipeBook.Recipes.Count; i++)
            {
                recipesToDisplay.Add(new string[] { (i + 1).ToString(), recipeBook.Recipes[i].Metadata.Title });
            }

            //Adds the "R" option to return to previous menu
            recipesToDisplay.Add(new string[] { "R", "Return to Previous Menu" });

            UserInterface.DisplayOptionsMenu(userIO, recipesToDisplay, out recipeOptions);
            UserInterface.DisplayLitePrompt(userIO, "Enter the recipe you would like to add to the shopping list");
            string userOption = GetUserInput.GetUserOption(userIO, recipeOptions);

            if (userOption != "R")
            {
                int recipeOption = int.Parse(userOption);

                Recipe recipeToAdd = recipeBook.Recipes[recipeOption - 1];

                AddRecipeToShoppingList(userIO, shoppingList, recipeToAdd);
            }
        }
    }
}
