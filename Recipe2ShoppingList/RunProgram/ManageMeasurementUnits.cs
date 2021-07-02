using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public static class ManageMeasurementUnits
    {
        public static void AddNewMeasurementUnit(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            UserInterface.DisplayMenuHeader(userIO, "---------- MANAGE SAVED MEASUREMENT UNITS ----------");

            UserInterface.DisplayCurrentMeasurementUnits(userIO, userAddedMeasurementUnits);

            string measurementUnit = "";
            GetUserInput.GetNewMeasurementUnitFromUser(userIO, out measurementUnit);

            GetUserInput.AreYouSure(userIO, "add this measurement unit", out bool isSure);

            if (isSure)
            {
                recipeBookLibrary.AddMeasurementUnit(measurementUnit);
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, false, "measurement unit", "added");
        }

        public static void EditExistingMeasurementUnit(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            UserInterface.DisplayMenuHeader(userIO, "---------- MANAGE SAVED MEASUREMENT UNITS ----------");

            List<string> userOptions = new List<string>();

            UserInterface.DisplayCurrentMeasurementUnits(userIO, userAddedMeasurementUnits);

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

        public static void DeleteExistingMeasurementUnit(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, List<string> userAddedMeasurementUnits)
        {
            UserInterface.DisplayMenuHeader(userIO, "---------- MANAGE SAVED MEASUREMENT UNITS ----------");

            List<string> userOptions = new List<string>();

            UserInterface.DisplayCurrentMeasurementUnits(userIO, userAddedMeasurementUnits);

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
