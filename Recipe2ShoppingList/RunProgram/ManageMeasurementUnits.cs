using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public static class ManageMeasurementUnits
    {
        public const string manageSavedMeasurementUnitsBanner = "---------- MANAGE SAVED MEASUREMENT UNITS ----------";

        //Adds a new custom measurement unit to the recipe book library
        public static void AddNewMeasurementUnit(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            UserInterface.DisplayMenuHeader(userIO, manageSavedMeasurementUnitsBanner);

            UserInterface.DisplayCurrentMeasurementUnits(userIO, userAddedMeasurementUnits);

            string measurementUnit = "";
            GetUserInput.GetNewMeasurementUnitFromUser(userIO, out measurementUnit);

            GetUserInput.AreYouSure(userIO, "add this measurement unit", out bool isSure);

            if (isSure)
            {
                recipeBookLibrary.AddMeasurementUnit(measurementUnit);
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "measurement unit", "added");
        }

        //Prompts the user and captures input to edit an existing custom (aka, user-provided) measurement unit
        public static void EditExistingMeasurementUnit(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            UserInterface.DisplayMenuHeader(userIO, manageSavedMeasurementUnitsBanner);

            List<string> userOptions = new List<string>();

            UserInterface.DisplayCurrentMeasurementUnits(userIO, userAddedMeasurementUnits);

            //Populates userOptions with valid option choices based on the contents
            //of the userAddedMeasurementUnits
            for (int i = 0; i < userAddedMeasurementUnits.Count; i++)
            {
                userOptions.Add((i + 1).ToString());
            }

            UserInterface.DisplayRegularPrompt(userIO, "Select the measurement unit to edit");
            string userOption = GetUserInput.GetUserOption(userIO, userOptions);

            UserInterface.DisplayRegularPrompt(userIO, "Enter the new name for the measurement unit");
            string newName = GetUserInput.GetNewMeasurementUnitName(userIO);

            recipeBookLibrary.EditMeasurementUnit(userAddedMeasurementUnits[int.Parse(userOption) - 1], newName);
            UserInterface.SuccessfulChange(userIO, true, "measurement unit name", "updated");
        }

        //Prompts the user and captures input to delete an existing custom (aka, user-provided) measurement unit
        public static void DeleteExistingMeasurementUnit(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            UserInterface.DisplayMenuHeader(userIO, manageSavedMeasurementUnitsBanner);

            List<string> userOptions = new List<string>();

            UserInterface.DisplayCurrentMeasurementUnits(userIO, userAddedMeasurementUnits);

            //Populates userOptions with valid option choices based on the contents
            //of the userAddedMeasurementUnits
            for (int i = 0; i < userAddedMeasurementUnits.Count; i++)
            {
                userOptions.Add((i + 1).ToString());
            }

            UserInterface.DisplayRegularPrompt(userIO, "Select the measurement unit to delete");
            string userOption = GetUserInput.GetUserOption(userIO, userOptions);

            GetUserInput.AreYouSure(userIO, "delete this measurement unit", out bool isSure);

            if (isSure)
            {
                recipeBookLibrary.DeleteMeasurementUnit(userAddedMeasurementUnits[int.Parse(userOption) - 1]);
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "measurement unit", "deleted");
        }
    }
}
