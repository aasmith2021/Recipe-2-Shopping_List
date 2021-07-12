using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    public class ProgramExecution
    {
        public const string manageSavedMeasurementUnitsBanner = "---------- MANAGE SAVED MEASUREMENT UNITS ----------";
        public const string editRecipeBanner = "---------- EDIT RECIPE ----------";

        public static void RunProgram(IUserIO userIO, IDataIO dataIO, IDataIO backupIO, IDataIO shoppingListIO, out bool exitProgram)
        {
            exitProgram = false;
            RecipeBookLibrary recipeBookLibrary = new RecipeBookLibrary();
            RecipeBookLibrary databaseData = new RecipeBookLibrary();
            RecipeBookLibrary backupData = new RecipeBookLibrary();

            //Get <recipeBookLibrary> from the database and the local backup file
            try
            {
                backupData = backupIO.GetRecipeBookLibraryFromDataSource();
                databaseData = dataIO.GetRecipeBookLibraryFromDataSource();
            }
            catch (Exception ex)
            {
                UserInterface.DisplayErrorMessage(userIO, ex.Message, "open backup file from computer");
            }

            //Checks to see which version of data was saved most recently and sets the recipeBookLibraryToUseForProgram
            //to the most recent version of saved data
            if (databaseData.LastSaved > backupData.LastSaved)
            {
                recipeBookLibrary = databaseData;
            }
            else
            {
                recipeBookLibrary = backupData;
            }

            ShoppingList shoppingList = new ShoppingList();

            while (!exitProgram)
            {
                RunMainMenu(userIO, recipeBookLibrary, shoppingList, out exitProgram);
            }

            bool writeRecipeBookLibrarySuccessful = false;
            bool writeShoppingListSuccessful = false;
            bool writeToBackupFileSuccessful = false;

            try
            {
                //Save <recipeBookLibrary> to local file on computer as a backup before closing program
                writeToBackupFileSuccessful = backupIO.WriteRecipeBookLibraryToDataSource(userIO, recipeBookLibrary);

                //Save the Shopping List to file before closing the program
                writeShoppingListSuccessful = shoppingListIO.WriteShoppingListToDataSource(userIO, shoppingList);

                //Save <recipeBookLibrary> to database file before closing program
                writeRecipeBookLibrarySuccessful = dataIO.WriteRecipeBookLibraryToDataSource(userIO, recipeBookLibrary);
            }
            catch (Exception ex)
            {
                UserInterface.DisplayErrorMessage(userIO, ex.Message, "save data to a local file on your computer");
                writeRecipeBookLibrarySuccessful = false;
            }

            string exitMessage = "";

            if (writeRecipeBookLibrarySuccessful && writeShoppingListSuccessful)
            {
                exitMessage = "All data saved. Have a great day!";
            }
            else if (writeRecipeBookLibrarySuccessful == true)
            {
                exitMessage = "Recipe data saved, but shopping list was not able to be saved to your computer.";
            }
            else if (writeRecipeBookLibrarySuccessful == false)
            {
                if (writeToBackupFileSuccessful)
                {
                    exitMessage = "Data saved to local file on your computer, and will be loaded into the program the next time you open it.";
                }
            }
            else
            {
                exitMessage = "Unfortunately, data could not be saved to the remote database or your local computer. All changes made during this session will be lost.";
            }

            UserInterface.DisplayInformation(userIO, exitMessage);
            UserInterface.DisplayInformation(userIO, "Press \"Enter\" to exit program...", false);
            GetUserInput.GetEnterFromUser(userIO);
        }

        private static void RunMainMenu(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, ShoppingList shoppingList, out bool exitProgram)
        {
            exitProgram = false;

            UserInterface.DisplayMainMenu(userIO, recipeBookLibrary, out List<string> mainMenuOptions);
            string userOption = GetUserInput.GetUserOption(userIO, mainMenuOptions);

            if (int.TryParse(userOption, out int userOptionNumber))
            {
                bool exitRecipeBookSection = false;

                while (!exitRecipeBookSection)
                {
                    RunRecipeBook(userIO, recipeBookLibrary, shoppingList, userOptionNumber, out exitRecipeBookSection, out exitProgram);
                }
            }
            else
            {
                switch (userOption)
                {
                    case "A":
                        ManageRecipeBooks.AddNewRecipeBook(userIO, recipeBookLibrary);
                        break;
                    case "R":
                        ManageRecipeBooks.RenameRecipeBook(userIO, recipeBookLibrary);
                        break;
                    case "D":
                        ManageRecipeBooks.DeleteRecipeBook(userIO, recipeBookLibrary);
                        break;
                    case "V":
                        UserInterface.DisplayShoppingList(userIO, shoppingList);
                        break;
                    case "M":
                        RunManageSavedMeasurementUnits(userIO, recipeBookLibrary);
                        break;
                    case "X":
                        exitProgram = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void RunRecipeBook(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, ShoppingList shoppingList, int recipeBookOptionNumber, out bool exitRecipeBookSection, out bool exitProgram)
        {
            exitProgram = false;
            exitRecipeBookSection = false;

            RecipeBook recipeBook = recipeBookLibrary.AllRecipeBooks[recipeBookOptionNumber - 1];
            UserInterface.DisplayOpenRecipeBook(userIO, recipeBook, out List<string> recipeBookOptions);
            string userOption = GetUserInput.GetUserOption(userIO, recipeBookOptions);

            if (int.TryParse(userOption, out int userOptionNumber))
            {
                bool exitRecipeSection = false;

                while (!exitRecipeSection)
                {
                    RunRecipe(userIO, recipeBookLibrary, shoppingList, recipeBook, userOptionNumber, out exitRecipeSection, out exitRecipeBookSection, out exitProgram);
                }
            }
            else
            {
                switch (userOption)
                {
                    case "A":
                        ManageRecipes.AddNewRecipe(userIO, recipeBookLibrary, recipeBook);
                        break;
                    case "E":
                        ManageRecipes.EditExistingRecipe(userIO, recipeBookLibrary, recipeBook);
                        break;
                    case "D":
                        ManageRecipes.DeleteExistingRecipe(userIO, recipeBook);
                        break;
                    case "S":
                        ManageRecipes.AddExistingRecipeToShoppingList(userIO, recipeBook, shoppingList);
                        break;
                    case "V":
                        UserInterface.DisplayShoppingList(userIO, shoppingList);
                        break;
                    case "R":
                        exitRecipeBookSection = true;
                        break;
                    case "X":
                        exitRecipeBookSection = true;
                        exitProgram = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void RunRecipe(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, ShoppingList shoppingList, RecipeBook recipeBook, int recipeOptionNumber, out bool exitRecipeSection, out bool exitRecipeBookSection, out bool exitProgram)
        {
            exitProgram = false;
            exitRecipeSection = false;
            exitRecipeBookSection = false;

            Recipe recipe = recipeBook.Recipes[recipeOptionNumber - 1];
            UserInterface.DisplayOpenRecipe(userIO, recipe, recipeBook, out List<string> recipeEditOptions);
            string userOption = GetUserInput.GetUserOption(userIO, recipeEditOptions);

            switch (userOption)
            {
                case "S":
                    ManageRecipes.AddRecipeToShoppingList(userIO, shoppingList, recipe);
                    break;
                case "E":
                    RunEditRecipe(userIO, recipeBookLibrary, recipe);
                    break;
                case "D":
                    ManageRecipes.DeleteOpenRecipe(userIO, recipeBook, recipe);
                    exitRecipeSection = true;
                    break;
                case "V":
                    UserInterface.DisplayShoppingList(userIO, shoppingList);
                    break;
                case "R":
                    exitRecipeSection = true;
                    break;
                case "X":
                    exitRecipeSection = true;
                    exitRecipeBookSection = true;
                    exitProgram = true;
                    break;
                default:
                    break;
            }
        }

        private static void RunManageSavedMeasurementUnits(IUserIO userIO, RecipeBookLibrary recipeBookLibrary)
        {
            bool exitMeasurementUnits = false;

            do
            {
                UserInterface.DisplayMenuHeader(userIO, manageSavedMeasurementUnitsBanner);

                int allStandardMeasurementUnitsLength = MeasurementUnits.AllStandardMeasurementUnits().Count;
                List<string> allMeasurementUnits = recipeBookLibrary.AllMeasurementUnits;

                List<string> userAddedMeasurementUnits = new List<string>();
                userAddedMeasurementUnits.AddRange(allMeasurementUnits);
                userAddedMeasurementUnits.RemoveRange(0, allStandardMeasurementUnitsLength);

                List<string[]> editOptions = new List<string[]>()
                {
                    new string[] { "A", "Add New Measurement Unit"},
                    new string[] { "E", "Edit Measurement Unit"},
                    new string[] { "D", "Delete Measurement Unit"},
                    new string[] { "R", "Return to Main Menu"},
                };
                List<string> options = new List<string>();

                if (userAddedMeasurementUnits.Count == 0)
                {
                    editOptions.RemoveAt(1);
                    editOptions.RemoveAt(1);
                }

                UserInterface.DisplayCurrentMeasurementUnits(userIO, userAddedMeasurementUnits, true);
                UserInterface.DisplayOptionsMenu(userIO, editOptions, out options);
                UserInterface.DisplayLitePrompt(userIO, "Select an editing option");

                string userOption = GetUserInput.GetUserOption(userIO, options);

                UserInterface.InsertBlankLine(userIO);
                switch (userOption)
                {
                    case "A":
                        ManageMeasurementUnits.AddNewMeasurementUnit(userIO, recipeBookLibrary, userAddedMeasurementUnits);
                        break;
                    case "E":
                        ManageMeasurementUnits.EditExistingMeasurementUnit(userIO, recipeBookLibrary, userAddedMeasurementUnits);
                        break;
                    case "D":
                        ManageMeasurementUnits.DeleteExistingMeasurementUnit(userIO, recipeBookLibrary, userAddedMeasurementUnits);
                        break;
                    case "R":
                        exitMeasurementUnits = true;
                        break;
                    default:
                        break;
                }
            }
            while (!exitMeasurementUnits);
        }

        public static void RunEditRecipe(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            List<string[]> editRecipeOptions = new List<string[]>()
            {
                new string[] { "1", "Recipe Title" },
                new string[] { "2", "Notes" },
                new string[] { "3", "Preparation Times" },
                new string[] { "4", "Estimated Servings" },
                new string[] { "5", "Food Type & Genre"},
                new string[] { "6", "Ingredients" },
                new string[] { "7", "Instructions" },
                new string[] { "R", "Return to Previous Menu" }
            };

            string fieldToEdit = GetUserInput.GetTheFieldToEditFromUser(userIO, recipe, editRecipeOptions);

            switch (fieldToEdit)
            {
                case "1":
                    ManageMetadata.EditRecipeTitle(userIO, recipe);
                    break;
                case "2":
                    ManageMetadata.EditRecipeNotes(userIO, recipe);
                    break;
                case "3":
                    ManageMetadata.EditRecipePrepTimes(userIO, recipe);
                    break;
                case "4":
                    ManageMetadata.EditRecipeEstimatedServings(userIO, recipe);
                    break;
                case "5":
                    ManageMetadata.EditRecipeFoodTypeGenre(userIO, recipe);
                    break;
                case "6":
                    RunEditRecipeIngredients(userIO, recipeBookLibrary, recipe);
                    break;
                case "7":
                    RunEditRecipeInstructions(userIO, recipe);
                    break;
                case "R":
                    return;
            }
        }

        public static void RunEditRecipeIngredients(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));
            UserInterface.DisplayInformation(userIO, recipe.IngredientList.ProduceIngredientsText(true, true), false);

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

            UserInterface.InsertBlankLine(userIO);
            UserInterface.DisplayOptionsMenu(userIO, menuOptions, out options);
            UserInterface.DisplayLitePrompt(userIO, "Select an editing option");
            string userOption = GetUserInput.GetUserOption(userIO, options);

            UserInterface.InsertBlankLine(userIO);
            switch (userOption)
            {
                case "A":
                    ManageIngredients.AddNewIngredient(userIO, recipeBookLibrary, recipe);
                    break;
                case "E":
                    ManageIngredients.EditExistingIngredient(userIO, recipeBookLibrary, recipe);
                    break;
                case "D":
                    ManageIngredients.DeleteExistingIngredient(userIO, recipe);
                    break;
                case "R":
                    return;
            }
        }

        public static void RunEditRecipeInstructions(IUserIO userIO, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));
            UserInterface.DisplayInformation(userIO, recipe.CookingInstructions.ProduceInstructionsText(true));

            List<string[]> menuOptions = new List<string[]>()
            {
                new string[] { "A", "Add New Instruction Block" },
                new string[] { "E", "Edit an Instruction Block"},
                new string[] { "D", "Delete an Instruction Block"},
                new string[] { "R", "Return to Previous Menu"},
            };
            List<string> options = new List<string>();

            if (recipe.CookingInstructions.InstructionBlocks.Count == 0)
            {
                menuOptions.RemoveAt(1);
                menuOptions.RemoveAt(1);
            }

            UserInterface.DisplayOptionsMenu(userIO, menuOptions, out options);
            UserInterface.DisplayLitePrompt(userIO, "Select an editing option");
            string userOption = GetUserInput.GetUserOption(userIO, options);

            UserInterface.InsertBlankLine(userIO);
            switch (userOption)
            {
                case "A":
                    ManageCookingInstructions.AddNewInstructionBlock(userIO, recipe);
                    break;
                case "E":
                    RunEditExistingInstructionBlock(userIO, recipe);
                    break;
                case "D":
                    ManageCookingInstructions.DeleteExistingInstructionBlock(userIO, recipe);
                    break;
                case "R":
                    return;
            }
        }

        public static void RunEditExistingInstructionBlock(IUserIO userIO, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));

            UserInterface.DisplayInformation(userIO, recipe.CookingInstructions.ProduceInstructionsText(true, true), false);

            InstructionBlock instructionBlockToEdit;
            int numberOfInstructionBlocks = recipe.CookingInstructions.InstructionBlocks.Count;
            List<string> instructionBlockOptions = new List<string>();
            for (int i = 1; i <= numberOfInstructionBlocks; i++)
            {
                instructionBlockOptions.Add(i.ToString());
            }

            if (numberOfInstructionBlocks > 1)
            {
                UserInterface.DisplayRegularPrompt(userIO, "Enter the instruction block you would like to edit");

                string userBlockSelection = GetUserInput.GetUserOption(userIO, instructionBlockOptions);
                int instructionBlockIndex = int.Parse(userBlockSelection);

                instructionBlockToEdit = recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex - 1];
            }
            else if (numberOfInstructionBlocks == 1)
            {
                instructionBlockToEdit = recipe.CookingInstructions.InstructionBlocks[0];
            }
            else
            {
                UserInterface.DisplayInformation(userIO, UserInterface.MakeStringConsoleLengthLines("This recipe does not have any instruction blocks. Add a new instruction block in order to edit the recipe."));
                UserInterface.DisplayInformation(userIO, "Press \"Enter\" to continue...", false);
                GetUserInput.GetEnterFromUser(userIO);

                return;
            }

            List<string[]> editBlockMenuOptions = new List<string[]>()
            {
                new string[] { "", "Add Instruction Line" },
                new string[] { "", "Edit Instruction Line" },
                new string[] { "", "Delete Instruction Line" },
                new string[] { "", "Add Block Heading" },
                new string[] { "", "Edit Block Heading" },
                new string[] { "", "Delete Block Heading" },
            };
            List<string> editBlockOptions = new List<string>();

            bool instructionLinesAreBlank = instructionBlockToEdit.InstructionLines.Count == 0;
            bool blockHeadingIsBlank = instructionBlockToEdit.BlockHeading == "";

            if (instructionLinesAreBlank)
            {
                editBlockMenuOptions.RemoveAt(1);
                editBlockMenuOptions.RemoveAt(1);
            }

            if (blockHeadingIsBlank)
            {
                editBlockMenuOptions.RemoveAt(editBlockMenuOptions.Count - 1);
                editBlockMenuOptions.RemoveAt(editBlockMenuOptions.Count - 1);
            }

            if (!blockHeadingIsBlank)
            {
                editBlockMenuOptions.RemoveAt(editBlockMenuOptions.Count - 3);
            }

            for (int i = 0; i < editBlockMenuOptions.Count; i++)
            {
                editBlockMenuOptions[i][0] = (i + 1).ToString();
            }

            UserInterface.InsertBlankLine(userIO);
            UserInterface.DisplayOptionsMenu(userIO, editBlockMenuOptions, out editBlockOptions);
            UserInterface.DisplayLitePrompt(userIO, "Enter an editing option from the menu");
            string editBlockOption = GetUserInput.GetUserOption(userIO, editBlockOptions);
            string menuSelection = editBlockMenuOptions[int.Parse(editBlockOption) - 1][1];

            UserInterface.InsertBlankLine(userIO);
            switch (menuSelection)
            {
                case "Add Instruction Line":
                    ManageCookingInstructions.AddInstructionLine(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Edit Instruction Line":
                    ManageCookingInstructions.EditInstructionLine(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Delete Instruction Line":
                    ManageCookingInstructions.DeleteInstructionLine(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Add Block Heading":
                    ManageCookingInstructions.AddInstructionBlockHeading(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Edit Block Heading":
                    ManageCookingInstructions.EditInstructionBlockHeading(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Delete Block Heading":
                    ManageCookingInstructions.DeleteInstructionBlockHeading(userIO, instructionBlockToEdit, recipe);
                    break;
                default:
                    break;
            }
        }
    }
}
