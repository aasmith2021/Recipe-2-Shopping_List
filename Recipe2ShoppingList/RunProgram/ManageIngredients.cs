using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    public static class ManageIngredients
    {
        public static void AddNewIngredient(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            Ingredient ingredientToAdd = GetUserInput.GetIngredientFromUser(userIO, recipeBookLibrary);
            recipe.IngredientList.AddIngredient(ingredientToAdd);
            UserInterface.SuccessfulChange(userIO, true, "ingredient", "added");
        }

        public static void EditRecipeIngredients(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);
            userIO.DisplayDataLite(recipe.IngredientList.ProduceIngredientsText(true, true));

            List<string[]> menuOptions = new List<string[]>()
            {
                new string[] { "A", "Add a New Igredient" },
                new string[] { "E", "Edit an Ingredient"},
                new string[] { "D", "Delete an Ingredient"},
                new string[] { "R", "Return to Previous Menu"},
            };
            List<string> options = new List<string>();

            if (recipe.IngredientList.AllIngredients.Count == 0)
            {
                menuOptions.RemoveAt(1);
                menuOptions.RemoveAt(1);
            }

            userIO.DisplayData();
            UserInterface.DisplayOptionsMenu(userIO, menuOptions, out options);
            userIO.DisplayData();
            userIO.DisplayDataLite("Select an editing option: ");
            string userOption = GetUserInput.GetUserOption(userIO, options);

            userIO.DisplayData();
            switch (userOption)
            {
                case "A":
                    AddNewIngredient(userIO, recipeBookLibrary, recipe);
                    break;
                case "E":
                    EditExistingIngredient(userIO, recipeBookLibrary, recipe);
                    break;
                case "D":
                    DeleteExistingIngredient(userIO, recipe);
                    break;
                case "R":
                    return;
            }
        }

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
                userIO.DisplayData("Select the ingredient line you would like to edit:");
                string userOption = GetUserInput.GetUserOption(userIO, ingredientLineOptions);
                userIO.DisplayData();
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

            userIO.DisplayData("Select the part of the ingredient you would like to edit:");
            UserInterface.DisplayOptionsMenu(userIO, ingredientComponentsForMenu, out ingredientComponentOptions);
            string ingredientComponentToEdit = GetUserInput.GetUserOption(userIO, ingredientComponentOptions);

            userIO.DisplayData();
            string measurementUnit = "";
            string newPrepNote;
            switch (ingredientComponentToEdit)
            {
                case "1":
                    userIO.DisplayDataLite("Enter the new quantity of the ingredient (ex: 1.5): ");
                    double newQuantity = GetUserInput.GetUserInputDouble(userIO, 2);
                    while (newQuantity > 1000)
                    {
                        userIO.DisplayData();
                        userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("An ingredient quantity cannot be more than 1000. Please enter a valid ingredient quantity:"));
                        newQuantity = GetUserInput.GetUserInputDouble(userIO, 2);
                    }
                    ingredientToEdit.Quantity = newQuantity;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient quantity", "updated");
                    break;
                case "2":
                    List<string[]> measurementUnits = MeasurementUnits.AllMeasurementUnitsForUserInput(recipeBookLibrary);
                    List<string> options = new List<string>();
                    userIO.DisplayData("Select the new ingredient measurement unit from the list of options:");
                    UserInterface.DisplayOptionsMenu(userIO, measurementUnits, out options);

                    int userOptionNumber = int.Parse(GetUserInput.GetUserOption(userIO, options));

                    if (userOptionNumber == options.Count)
                    {
                        GetUserInput.GetNewMeasurementUnitFromUser(userIO, out measurementUnit);
                        recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                        userIO.DisplayData();
                        userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines($"Success! New measurement unit, {measurementUnit}, was added and will be used for this ingredient."));
                    }
                    else if (measurementUnits[userOptionNumber - 1][1] != "None")
                    {
                        measurementUnit = measurementUnits[userOptionNumber - 1][1];
                    }
                    ingredientToEdit.MeasurementUnit = measurementUnit;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient meaurement unit", "updated");
                    break;
                case "3":
                    userIO.DisplayData("Enter the new ingredient name:");
                    string newName = GetUserInput.GetUserInputString(userIO, false, 100);
                    ingredientToEdit.Name = newName;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient name", "updated");
                    break;
                case "4":
                    userIO.DisplayData("Enter the new ingredient preparation note (or press \"Enter\" to leave blank):");
                    newPrepNote = GetUserInput.GetUserInputString(userIO, true, 120);
                    ingredientToEdit.PreparationNote = newPrepNote;
                    UserInterface.SuccessfulChange(userIO, true, "ingredient preparation note", "updated");
                    break;
                default:
                    break;
            }
        }

        public static void DeleteExistingIngredient(IUserIO userIO, Recipe recipe)
        {
            userIO.DisplayData("Select the ingredient line you would like to delete:");

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
                UserInterface.SuccessfulChange(userIO, true, "ingredient", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "ingredient", "deleted");
            }
        }

        public static void GetStoreLocationForIngredient(IUserIO userIO, ShoppingList shoppingList, Ingredient ingredient)
        {
            string header = "-------- SET STORE LOCATION FOR INGREDIENT --------";
            string message = UserInterface.MakeStringConsoleLengthLines($"INGREDIENT: {ingredient.Name}");
            UserInterface.DisplayMenuHeader(userIO, header, message);
            userIO.DisplayData("Which department is this ingredient generally found in at the store?");

            List<string[]> menuOptions = new List<string[]>();
            List<string> optionChoices = new List<string>();

            for (int i = 0; i < shoppingList.StoreLocations.Length; i++)
            {
                menuOptions.Add(new string[] { $"{i + 1}", shoppingList.StoreLocations[i] });
            }

            UserInterface.DisplayOptionsMenu(userIO, menuOptions, out optionChoices);
            userIO.DisplayData();
            userIO.DisplayData("Select the store location of this ingredient:");
            string userOption = GetUserInput.GetUserOption(userIO, optionChoices);
            userIO.DisplayData();

            string storeLocation = shoppingList.StoreLocations[int.Parse(userOption) - 1];

            ingredient.StoreLocation = storeLocation;
        }

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

        public static double GetSimilarityPercentage(string firstIngredientName, string secondIngredientName)
        {
            int countOfCharactersTheSame = 0;
            double percentSimilar = 0;

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

            string[] shorterPhraseWords = shorterPhrase.Split(" ");
            string wordToCheck = "";
            string testString = "";
            string regexExpression = "";
            bool matchFound = false;

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

            percentSimilar = (double)countOfCharactersTheSame / longerPhrase.Length;

            return percentSimilar;
        }

        public static bool AreIngredientsTheSame(IUserIO userIO, string currentIngredientName, string newIngredientName)
        {
            bool ingredientsAreTheSame = false;
            string header = "-------- SIMILAR INGREDIENTS FOUND --------";
            UserInterface.DisplayMenuHeader(userIO, header);
            userIO.DisplayData();
            userIO.DisplayData("The following ingredients might match:");
            userIO.DisplayData();
            userIO.DisplayData($"<<Ingredient Already On Shopping List>>{Environment.NewLine}{currentIngredientName}");
            userIO.DisplayData();
            userIO.DisplayData($"<<New Ingredient>>{Environment.NewLine}{newIngredientName}");
            userIO.DisplayData();
            userIO.DisplayData("Are these ingredients the same? Enter \"Y\" for Yes or \"N\" for No:");
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
