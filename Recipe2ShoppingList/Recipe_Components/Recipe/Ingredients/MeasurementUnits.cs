using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public static class MeasurementUnits
    {
        private const string none = "None";
        private const string tsp = "tsp";
        private const string tbsp = "Tbsp";
        private const string cup = "Cup";
        private const string lb = "lb.";
        private const string oz = "oz.";
        private const string flOz = "fl. oz";
        
        public static List<string> AllStandardMeasurementUnits()
        {
            List<string> allStandardMeasurementUnits = new List<string>() { none, tsp, tbsp, cup, lb, oz, flOz };

            return allStandardMeasurementUnits;
        }

        public static string[] AllVolumeMeasurementUnits()
        {
            string[] allVolumeMeasurementUnits = new string[] { "", tsp, tbsp, flOz, cup };

            return allVolumeMeasurementUnits;
        }

        public static string[] AllWeightMeasurementUnits()
        {
            string[] allWeightMeasurementUnits = new string[] { oz, lb };

            return allWeightMeasurementUnits;
        }

        public static bool IngredientHasVolumeMeasurementUnit(Ingredient ingredient)
        {
            bool ingredientHasVolumeMeasurement = ingredient.MeasurementUnit == "" || ingredient.MeasurementUnit == tsp || ingredient.MeasurementUnit == tbsp || ingredient.MeasurementUnit == cup || ingredient.MeasurementUnit == flOz;
            return ingredientHasVolumeMeasurement;
        }
        
        public static Tuple<double, string> CombineMeasurementUnits(double quantity1, string measurementUnit1, bool mu1IsVolumeMeasure, double quantity2, string measurementUnit2, bool mu2IsVolumeMeasure)
        {
            List<string> allStandardMeasurementUnits = AllStandardMeasurementUnits();
            double combinedQuantity;
            string combinedMeasurementUnit;
            double[] quantities = new double[] { quantity1, quantity2 };
            string[] measurementUnits = new string[] { measurementUnit1, measurementUnit2 };
            bool bothMeasurementUnitsAreBlankOrCustom = (measurementUnit1 == "" && measurementUnit2 == "") || (!allStandardMeasurementUnits.Contains(measurementUnit1) && !allStandardMeasurementUnits.Contains(measurementUnit2));

            if (bothMeasurementUnitsAreBlankOrCustom)
            {
                Tuple<double, string> combinedBlankMeasurementUnit = CombineBlankOrCustomMeasurementUnit(quantities, measurementUnits);
                combinedQuantity = combinedBlankMeasurementUnit.Item1;
                combinedMeasurementUnit = combinedBlankMeasurementUnit.Item2;
            }            
            else if(mu1IsVolumeMeasure && mu2IsVolumeMeasure && !bothMeasurementUnitsAreBlankOrCustom)
            {               
                Tuple<double, string> combinedVolumeUnit = CombineToCommonVolumeUnit(quantities, measurementUnits);
                combinedQuantity = combinedVolumeUnit.Item1;
                combinedMeasurementUnit = combinedVolumeUnit.Item2;
            }
            else
            {
                Tuple<double, string> combinedWeightUnit = CombineToCommonWeightUnit(quantities, measurementUnits);
                combinedQuantity = combinedWeightUnit.Item1;
                combinedMeasurementUnit = combinedWeightUnit.Item2;
            }

            Tuple<double, string> result = new Tuple<double, string>(combinedQuantity, combinedMeasurementUnit);

            return result;
        }

        private static Tuple<double, string> CombineBlankOrCustomMeasurementUnit(double[] quantities, string[] measurementUnits)
        {
            double combinedQuantity = 0;
            string combinedMeasurementUnit = measurementUnits[0];

            for (int i = 0; i < quantities.Length; i++)
            {
                combinedQuantity += quantities[i];
            }

            Tuple<double, string> combinedBlankOrCustomMeasurementUnit = new Tuple<double, string>(combinedQuantity, combinedMeasurementUnit);

            return combinedBlankOrCustomMeasurementUnit;
        }

        private static Tuple<double, string> CombineToCommonVolumeUnit(double[] quantities, string[] measurementUnits)
        {
            string[] allVolumeMeasurementUnits = AllVolumeMeasurementUnits();
            double totalTeaspoons = 0;
            int largerMeasurementUnitIndex = GetLargerVolumeMeasurementUnit(measurementUnits);
            int conversionMultiplier;
            int conversionDivisor;
            string combinedMeasurementUnit;
            double combinedQuantity;

            for (int i = 0; i < quantities.Length; i++)
            {
                conversionMultiplier = GetVolumeConversionMultiplier(measurementUnits[i]);
                totalTeaspoons += quantities[i] * conversionMultiplier;
            }

            //Anything equal to 1/4 cup or more is converted to cups
            if ((totalTeaspoons / 16) >= 1)
            {
                combinedMeasurementUnit = allVolumeMeasurementUnits[4];
            }//Anything equal to 3 teaspoons ore more is converted to Tablespoons
            else if ((totalTeaspoons / 3) >= 1)
            {
                combinedMeasurementUnit = allVolumeMeasurementUnits[2];
            }
            else
            {
                combinedMeasurementUnit = measurementUnits[largerMeasurementUnitIndex];
            }

            conversionDivisor = GetVolumeConversionMultiplier(combinedMeasurementUnit);

            combinedQuantity = totalTeaspoons / conversionDivisor;

            Tuple<double, string> combinedVolumeUnit = new Tuple<double, string>(combinedQuantity, combinedMeasurementUnit);

            return combinedVolumeUnit;
        }

        private static Tuple<double, string> CombineToCommonWeightUnit(double[] quantities, string[] measurementUnits)
        {
            string[] allWeightMeasurementUnits = AllWeightMeasurementUnits();
            double totalOunces = 0;
            int largerMeasurementUnitIndex = GetLargerWeightMeasurementUnit(measurementUnits);
            int conversionMultiplier;
            int conversionDivisor;
            string combinedMeasurementUnit;
            double combinedQuantity;

            for (int i = 0; i < quantities.Length; i++)
            {
                conversionMultiplier = GetWeightConversionMultiplier(measurementUnits[i]);
                totalOunces += quantities[i] * conversionMultiplier;
            }

            if (totalOunces / 16 >= 1)
            {
                combinedMeasurementUnit = allWeightMeasurementUnits[1];
            }
            else
            {
                combinedMeasurementUnit = measurementUnits[largerMeasurementUnitIndex];
            }

            conversionDivisor = GetWeightConversionMultiplier(combinedMeasurementUnit);

            combinedQuantity = totalOunces / conversionDivisor;

            Tuple<double, string> combinedWeightUnit = new Tuple<double, string>(combinedQuantity, combinedMeasurementUnit);

            return combinedWeightUnit;
        }

        private static int GetLargerVolumeMeasurementUnit(string[] measurementUnits)
        {
            int cupPriority = 3;
            int flOzPriority = 2;
            int tablespoonPriority = 1;
            int teaspoonPriority = 0;
            int mu1Priority = -1;
            int mu2Priority = -1;
            int[] muPriorities = new int[] { mu1Priority, mu2Priority };
            int largerMeasurementUnitIndex;

            for (int i = 0; i < measurementUnits.Length; i++)
            {
                switch(measurementUnits[i])
                {
                    case cup:
                        muPriorities[i] = cupPriority;
                        break;
                    case flOz:
                        muPriorities[i] = flOzPriority;
                        break;
                    case tbsp:
                        muPriorities[i] = tablespoonPriority;
                        break;
                    case tsp:
                        muPriorities[i] = teaspoonPriority;
                        break;
                    default:
                        break;
                }
            }

            if (mu1Priority >= mu2Priority)
            {
                largerMeasurementUnitIndex = 0;
            }
            else
            {
                largerMeasurementUnitIndex = 1;
            }

            return largerMeasurementUnitIndex;
        }

        private static int GetLargerWeightMeasurementUnit(string[] measurementUnits)
        {
            int lbPriority = 1;
            int ozPriority = 0;
            int mu1Priority = -1;
            int mu2Priority = -1;
            int[] muPriorities = new int[] { mu1Priority, mu2Priority };
            int largerMeasurementUnitIndex;

            for (int i = 0; i < measurementUnits.Length; i++)
            {
                switch (measurementUnits[i])
                {
                    case lb:
                        muPriorities[i] = lbPriority;
                        break;
                    case oz:
                        muPriorities[i] = ozPriority;
                        break;
                    default:
                        break;
                }
            }

            if (mu1Priority >= mu2Priority)
            {
                largerMeasurementUnitIndex = 0;
            }
            else
            {
                largerMeasurementUnitIndex = 1;
            }

            return largerMeasurementUnitIndex;
        }

        private static int GetVolumeConversionMultiplier(string measurementUnit)
        {
            int conversionMultiplier = 0;

            switch (measurementUnit)
            {
                case cup:
                    conversionMultiplier = 48;
                    break;
                case tbsp:
                    conversionMultiplier = 3;
                    break;
                case flOz:
                    conversionMultiplier = 6;
                    break;
                case tsp:
                    conversionMultiplier = 1;
                    break;
                default:
                    break;
            }

            return conversionMultiplier;
        }

        private static int GetWeightConversionMultiplier(string measurementUnit)
        {
            int conversionMultiplier = 0;

            switch (measurementUnit)
            {
                case lb:
                    conversionMultiplier = 16;
                    break;
                case oz:
                    conversionMultiplier = 1;
                    break;
                default:
                    break;
            }

            return conversionMultiplier;
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
            
            for (int i = AllStandardMeasurementUnits().Count; i < recipeBookLibrary.AllMeasurementUnits.Length; i++)
            {
                
                measurementUnitsText += $"MU:{recipeBookLibrary.AllMeasurementUnits[i]}{Environment.NewLine}";
            }

            measurementUnitsText += $"-END_OF_MEASUREMENT_UNITS-{Environment.NewLine}";

            return measurementUnitsText;
        }
    }
}
