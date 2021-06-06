using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class MeasurementUnits
    {
        public static string[] AllStandardMeasurementUnits()
        {
            string[] allStandardUnits = new string[]
            { "None", "tsp", "Tbsp", "Cup", "lb.", "oz.", "fl. oz." };

            return allStandardUnits;
        }
        
        public static List<string[]> AllMeasurementUnitsForUserInput(RecipeBookLibrary recipeBookLibrary)
        {
            List<string[]> allMeasurementUnitsForUserInput = new List<string[]>();

            for (int i = 0; i < recipeBookLibrary.AllMeasurementUnits.Length; i++)
            {
                string[] measurementUnitToAdd = new string[] { (i + 1).ToString(), recipeBookLibrary.AllMeasurementUnits[i] };
                allMeasurementUnitsForUserInput.Add(measurementUnitToAdd);
            }

            //Add a "Add New Measurement Unit" option to the list
            string optionNumber = (recipeBookLibrary.AllMeasurementUnits.Length + 1).ToString();
            string[] newOption = new string[] { optionNumber, "Add New Measurement Unit" };
            allMeasurementUnitsForUserInput.Add(newOption);

            return allMeasurementUnitsForUserInput;
        }

        public static string ProduceMeasurementUnitsText(RecipeBookLibrary recipeBookLibrary)
        {
            string measurementUnitsText = "";

            measurementUnitsText += $"-START_OF_MEASUREMENT_UNITS-{Environment.NewLine}";
            
            for (int i = AllStandardMeasurementUnits().Length; i < recipeBookLibrary.AllMeasurementUnits.Length; i++)
            {
                
                measurementUnitsText += $"MU:{recipeBookLibrary.AllMeasurementUnits[i]}{Environment.NewLine}";
            }

            measurementUnitsText += $"-END_OF_MEASUREMENT_UNITS-{Environment.NewLine}";

            return measurementUnitsText;
        }
    }
}
