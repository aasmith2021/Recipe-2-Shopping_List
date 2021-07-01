using System;
using System.Collections.Generic;
using System.Text;

namespace r2slapi.Tests.Models
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
    }
}
