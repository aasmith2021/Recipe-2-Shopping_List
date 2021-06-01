using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class MeasurementUnits
    {
        public static List<string[]> AllMeasurementUnits()
        {
            List<string[]> allMeasurementUnits = new List<string[]>();
            allMeasurementUnits.Add(new string[] { "", "None" });
            allMeasurementUnits.Add(new string[] { "", "pinch" });
            allMeasurementUnits.Add(new string[] { "", "tsp" });
            allMeasurementUnits.Add(new string[] { "", "Tbsp" });
            allMeasurementUnits.Add(new string[] { "", "cup" });
            allMeasurementUnits.Add(new string[] { "", "lb." });
            allMeasurementUnits.Add(new string[] { "", "oz." });
            allMeasurementUnits.Add(new string[] { "", "fl. oz." });

            for (int i = 0; i < allMeasurementUnits.Count; i++)
            {
                allMeasurementUnits[i][0] = (i + 1).ToString();
            }

            return allMeasurementUnits;
        }
    }
}
