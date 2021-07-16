using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    public static class ManageIngredients
    {
        public const string editRecipeBanner = "---------- EDIT RECIPE ----------";
        public const string setStoreLocationBanner = "-------- SET STORE LOCATION FOR INGREDIENT --------";
        public const string similarIngredientsFoundBanner = "-------- SIMILAR INGREDIENTS FOUND --------";

        //Prompts user and captures input to add a new ingredient to a recipe
        public static void AddNewIngredient(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            Ingredient ingredientToAdd = GetUserInput.GetIngredientFromUser(userIO, recipeBookLibrary);
            recipe.IngredientList.AddIngredient(ingredientToAdd);
            UserInterface.SuccessfulChange(userIO, true, "ingredient", "added");
        }

        //Prompts user and captures input to edit an existing ingredient in a recipe
        public static void EditExistingIngredient(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            List<Ingredient> allRecipeIngredients = recipe.IngredientList.AllIngredients;
            List<string> ingredientLineOptions = new List<string>();
            for (int i = 0; i < allRecipeIngredients.Count; i++)
            {
                ingredientLineOptions.Add((i + 1).ToString());
            }

            Ingredient ingredientToEdit;

            if (allRecipeIngredients.Count > 1)
            {
                UserInterface.DisplayRegularPrompt(userIO, "Select the ingredient line you would like to edit", false);
                string userOption = GetUserInput.GetUserOption(userIO, ingredientLineOptions);
                UserInterface.InsertBlankLine(userIO);
                ingredientToEdit = allRecipeIngredients[int.Parse(userOption) - 1];
            }
            else
            {
                ingredientToEdit = allRecipeIngredients[0];
            }

            List<string[]> ingredientComponentsForMenu = new List<string[]>()
            {
                new string[] { "1", "Quantity" },
                new string[] { "2", "Measurement Unit" },
                new string[] { "3", "Ingredient Name" },
                new string[] { "4", "Preparation Note" }
            };
            List<string> ingredientComponentOptions = new List<string>();

            UserInterface.DisplayRegularPrompt(userIO, "Select the part of the ingredient you would like to edit", false);
            UserInterface.DisplayOptionsMenu(userIO, ingredientComponentsForMenu, out ingredientComponentOptions);
            string ingredientComponentToEdit = GetUserInput.GetUserOption(userIO, ingredientComponentOptions);

            UserInterface.InsertBlankLine(userIO);
            string measurementUnit = "";
            string newPrepNote;
            switch (ingredientComponentToEdit)
            {
                case "1":
                    UserInterface.DisplayLitePrompt(userIO, "Enter the new quantity of the ingredient (ex: 1.5)", false);
                    double newQuantity = GetUserInput.GetIngredientQuantityFromUser(userIO);

                    ingredientToEdit.Quantity = newQuantity;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient quantity", "updated");
                    break;
                case "2":
                    List<string[]> measurementUnits = MeasurementUnits.AllMeasurementUnitsForUserInput(recipeBookLibrary);
                    List<string> options = new List<string>();
                    UserInterface.DisplayRegularPrompt(userIO, "Select the new ingredient measurement unit from the list of options", false);
                    UserInterface.DisplayOptionsMenu(userIO, measurementUnits, out options);

                    int userOptionNumber = int.Parse(GetUserInput.GetUserOption(userIO, options));

                    if (userOptionNumber == options.Count)
                    {
                        GetUserInput.GetNewMeasurementUnitFromUser(userIO, out measurementUnit);
                        recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                        UserInterface.InsertBlankLine(userIO);
                        UserInterface.DisplayInformation(userIO, UserInterface.MakeStringConsoleLengthLines($"Success! New measurement unit, {measurementUnit}, was added and will be used for this ingredient."), false);
                    }
                    else if (measurementUnits[userOptionNumber - 1][1] != "None")
                    {
                        measurementUnit = measurementUnits[userOptionNumber - 1][1];
                    }
                    ingredientToEdit.MeasurementUnit = measurementUnit;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient meaurement unit", "updated");
                    break;
                case "3":
                    UserInterface.DisplayRegularPrompt(userIO, "Enter the new ingredient name", false);
                    string newName = GetUserInput.GetIngredientNameFromUser(userIO);
                    ingredientToEdit.Name = newName;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient name", "updated");
                    break;
                case "4":
                    UserInterface.DisplayRegularPrompt(userIO, "Enter the new ingredient preparation note (or press \"Enter\" to leave blank)", false);
                    newPrepNote = GetUserInput.GetIngredientPrepNoteFromUser(userIO);
                    ingredientToEdit.PreparationNote = newPrepNote;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient preparation note", "updated");
                    break;
                default:
                    break;
            }
        }

        //Prompts user and captures input to delete an existing ingredient in a recipe
        public static void DeleteExistingIngredient(IUserIO userIO, Recipe recipe)
        {
            UserInterface.DisplayRegularPrompt(userIO, "Select the ingredient line you would like to delete", false);

            List<Ingredient> allRecipeIngredients = recipe.IngredientList.AllIngredients;
            List<string> ingredientLineOptions = new List<string>();
            for (int i = 1; i <= allRecipeIngredients.Count; i++)
            {
                ingredientLineOptions.Add(i.ToString());
            }

            string userOption = GetUserInput.GetUserOption(userIO, ingredientLineOptions);
            int indexOfIngredientToDelete = int.Parse(userOption) - 1;

            GetUserInput.AreYouSure(userIO, "delete this ingredient", out bool isSure);

            if (isSure)
            {
                recipe.IngredientList.DeleteIngredient(recipe.IngredientList.AllIngredients[indexOfIngredientToDelete]);
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "ingredient", "deleted");
        }

        //Prompts user and captures input to set the store location of an ingredient (i.e., "Bakery")
        public static void GetStoreLocationForIngredient(IUserIO userIO, ShoppingList shoppingList, Ingredient ingredient)
        {
            UserInterface.DisplayMenuHeader(userIO, setStoreLocationBanner, UserInterface.MakeStringConsoleLengthLines($"INGREDIENT: {ingredient.Name}"));
            UserInterface.DisplayInformation(userIO, "Which department is this ingredient generally found in at the store?", false);

            List<string[]> menuOptions = new List<string[]>();
            List<string> optionChoices = new List<string>();

            for (int i = 0; i < shoppingList.StoreLocations.Length; i++)
            {
                menuOptions.Add(new string[] { $"{i + 1}", shoppingList.StoreLocations[i] });
            }

            UserInterface.DisplayOptionsMenu(userIO, menuOptions, out optionChoices);
            UserInterface.DisplayRegularPrompt(userIO, "Select the store location of this ingredient");
            string userOption = GetUserInput.GetUserOption(userIO, optionChoices);
            UserInterface.InsertBlankLine(userIO);

            string storeLocation = shoppingList.StoreLocations[int.Parse(userOption) - 1];

            ingredient.StoreLocation = storeLocation;
        }

        //Runs the logic of adding an ingredient to a specific store location in the Shopping List
        public static void AddIngredientToStoreLocation(IUserIO userIO, ShoppingList shoppingList, Ingredient ingredient)
        {
            string storeLocation = ingredient.StoreLocation;

            List<Ingredient> currentLocationIngredients = new List<Ingredient>();
            Ingredient combinedIngredientToAdd = new Ingredient(0, "", "");

            switch (storeLocation)
            {
                case "Produce":
                    currentLocationIngredients.AddRange(shoppingList.Produce);
                    break;
                case "Bakery/Deli":
                    currentLocationIngredients.AddRange(shoppingList.BakeryDeli);
                    break;
                case "Dry Goods":
                    currentLocationIngredients.AddRange(shoppingList.DryGoods);
                    break;
                case "Meat":
                    currentLocationIngredients.AddRange(shoppingList.Meat);
                    break;
                case "Refrigerated":
                    currentLocationIngredients.AddRange(shoppingList.Refrigerated);
                    break;
                case "Frozen":
                    currentLocationIngredients.AddRange(shoppingList.Frozen);
                    break;
                case "Non-Grocery":
                    currentLocationIngredients.AddRange(shoppingList.NonGrocery);
                    break;
                default:
                    break;
            }

            string currentIngredientName = "";
            bool currentIngredientHasVolumeMeasurementUnit;
            string currentIngredientMeasurementUnit;
            string newIngredientName = ingredient.Name;
            string newIngredientMeasurementUnit = ingredient.MeasurementUnit;
            bool newIngredientHasVolumeMeasurementUnit = MeasurementUnits.IngredientHasVolumeMeasurementUnit(ingredient);
            int indexOfMatchingIngredient = 0;
            bool ingredientsAreCombinable;
            bool ingredientsAreTheSame = false;

            //This loop looks for ingredients that have an exact name and measurement unit match
            for (int i = 0; i < currentLocationIngredients.Count; i++)
            {
                indexOfMatchingIngredient = i;
                currentIngredientName = currentLocationIngredients[i].Name;
                currentIngredientHasVolumeMeasurementUnit = MeasurementUnits.IngredientHasVolumeMeasurementUnit(currentLocationIngredients[i]);
                currentIngredientMeasurementUnit = currentLocationIngredients[i].MeasurementUnit;

                ingredientsAreCombinable = !(currentIngredientHasVolumeMeasurementUnit ^ newIngredientHasVolumeMeasurementUnit);

                if (newIngredientName == currentIngredientName && ingredientsAreCombinable && !(newIngredientMeasurementUnit == "" ^ newIngredientMeasurementUnit == ""))
                {
                    ingredientsAreTheSame = true;
                    break;
                }
            }

            //This loop looks for ingredients that may be similar if an exact match was not found on the shopping list
            if (!ingredientsAreTheSame)
            {
                for (int i = 0; i < currentLocationIngredients.Count; i++)
                {
                    indexOfMatchingIngredient = i;
                    currentIngredientName = currentLocationIngredients[i].Name;
                    currentIngredientHasVolumeMeasurementUnit = MeasurementUnits.IngredientHasVolumeMeasurementUnit(currentLocationIngredients[i]);

                    ingredientsAreCombinable = !(currentIngredientHasVolumeMeasurementUnit ^ newIngredientHasVolumeMeasurementUnit);

                    double percentSimilar = GetSimilarityPercentage(currentIngredientName, newIngredientName);

                    if (percentSimilar >= .3 && ingredientsAreCombinable)
                    {
                        ingredientsAreTheSame = AreIngredientsTheSame(userIO, currentIngredientName, newIngredientName);

                        if (ingredientsAreTheSame)
                        {
                            break;
                        }
                    }
                }
            }

            Ingredient ingredientToAdd;
            bool combineIngredients = false;

            //If the ingredient already matches an ingredient on the shopping list, remove the current
            //ingredient to the shopping list and add a new ingredient (which is the ingredient
            //that was already on the shopping list "added to" the new ingredient being added to the shopping list.)
            if (ingredientsAreTheSame)
            {
                Ingredient ingredientAlreadyOnShoppingList = currentLocationIngredients[indexOfMatchingIngredient];
                ingredientToAdd = ingredientAlreadyOnShoppingList.CombineIngredientsForShoppingList(ingredient);
                combineIngredients = true;
            }
            else
            {
                ingredientToAdd = ingredient;
            }

            //Add the ingredient to the store location on the shopping list
            switch (storeLocation)
            {
                case "Produce":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateProduce(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddProduce(ingredientToAdd);
                    }
                    break;
                case "Bakery/Deli":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateBakeryDeli(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddBakeryDeli(ingredientToAdd);
                    }
                    break;
                case "Dry Goods":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateDryGoods(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddDryGoods(ingredientToAdd);
                    }
                    break;
                case "Meat":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateMeat(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddMeat(ingredientToAdd);
                    }
                    break;
                case "Refrigerated":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateRefrigerated(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddRefrigerated(ingredientToAdd);
                    }
                    break;
                case "Frozen":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateFrozen(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddFrozen(ingredientToAdd);
                    }
                    break;
                case "Non-Grocery":
                    if (combineIngredients)
                    {
                        shoppingList.UpdateNonGrocery(ingredientToAdd, indexOfMatchingIngredient);
                    }
                    else
                    {
                        shoppingList.AddNonGrocery(ingredientToAdd);
                    }
                    break;
                default:
                    break;
            }
        }

        //Compares an ingredient being added to the shopping list with another ingredient already on the shopping list,
        //and uses the number of matching letters to determine what percent similar the two ingredients are
        public static double GetSimilarityPercentage(string firstIngredientName, string secondIngredientName)
        {
            //Ultimately, this method is trying to find out how many characters two ingredient names share,
            //and how similar they are by percentage value.
            //(i.e. how similar "raw chicken breast" and "boneless, skinless chicken breasts" might be to one another)
            int countOfCharactersTheSame = 0;
            double percentSimilar = 0;

            //This section sets a shorter phrase and a longer phrase so that the shorter phrase is compared to the
            //longer phrase to avoid any indexOutOfBounds errors
            string shorterPhrase;
            string longerPhrase;

            if (firstIngredientName.Length <= secondIngredientName.Length)
            {
                shorterPhrase = firstIngredientName.ToLower();
                longerPhrase = secondIngredientName.ToLower();
            }
            else
            {
                shorterPhrase = secondIngredientName.ToLower();
                longerPhrase = firstIngredientName.ToLower();
            }

            //The words in the shorter phrase are split apart into elements of an array
            string[] shorterPhraseWords = shorterPhrase.Split(" ");
            string wordToCheck = "";
            string testString = "";
            string regexExpression = "";
            bool matchFound = false;

            //This "for loop" loops through the shorter phrase word by work,
            //and then checks each word letter-by-letter to find a match with the longer
            //phrase. Each time a match is found, the countOfCharactersTheSame increments.

            //So, while "raw" doesn't match anything in "boneless, skinless chicken breasts", these loops
            //find that "chicken breast" in the first, shorter phrase matches "chicken breast" in the longer
            //phrase, and that they share 14 characters in common.
            for (int i = 0; i < shorterPhraseWords.Length; i++)
            {
                wordToCheck = shorterPhraseWords[i];

                for (int j = 0; j < wordToCheck.Length; j++)
                {
                    testString = wordToCheck.Substring(0, j + 1);
                    regexExpression = $"{testString}.*?";
                    matchFound = Regex.Match(longerPhrase, regexExpression).Success;

                    if (matchFound)
                    {
                        countOfCharactersTheSame++;
                    }
                }
            }


            //The percent similar is calculated by dividing the number of characters that are the same
            //by the length of the longer phrase.
            percentSimilar = (double)countOfCharactersTheSame / longerPhrase.Length;

            return percentSimilar;
        }

        //Displays a prompt asking the user if two ingrients that appear to be similar are the same so the two separate
        //ingredients can be combined on the shopping list.
        public static bool AreIngredientsTheSame(IUserIO userIO, string currentIngredientName, string newIngredientName)
        {
            bool ingredientsAreTheSame = false;

            UserInterface.DisplayMenuHeader(userIO, similarIngredientsFoundBanner);
            UserInterface.DisplayRegularPrompt(userIO, "The following ingredients might match");
            UserInterface.InsertBlankLine(userIO);
            UserInterface.DisplayInformation(userIO, $"<<Ingredient Already On Shopping List>>{Environment.NewLine}{currentIngredientName}");
            UserInterface.DisplayInformation(userIO, $"<<New Ingredient>>{Environment.NewLine}{newIngredientName}");
            UserInterface.DisplayRegularPrompt(userIO, "Are these ingredients the same? Enter \"Y\" for Yes or \"N\" for No", false);

            List<string> userOptions = new List<string>() { "Y", "N" };
            string userOption = GetUserInput.GetUserOption(userIO, userOptions);

            if (userOption == "Y")
            {
                ingredientsAreTheSame = true;
            }

            return ingredientsAreTheSame;
        }
    }
}
