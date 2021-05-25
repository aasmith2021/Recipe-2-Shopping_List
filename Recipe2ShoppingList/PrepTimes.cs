using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class PrepTimes
    {
        public PrepTimes(int prepTime = 0, int cookTime = 0, int totalTime = 0)
        {
            this.PrepTime = prepTime;
            this.CookTime = cookTime;
        }

        public int PrepTime { get; set; }

        public int CookTime { get; set; }

        public int TotalTime { 
            get
            {
                return this.PrepTime + this.CookTime;
            }
        }

        public void PrintPrepTimes()
        {
            bool printPrepTime = this.PrepTime != 0;
            bool printCookTime = this.CookTime != 0;
            bool printTotalTime = this.TotalTime != 0;
            string convertedPrepTime = MetadataEditingMethods.ConvertTimeToHoursAndMinutes(this.PrepTime);
            string convertedCookTime = MetadataEditingMethods.ConvertTimeToHoursAndMinutes(this.CookTime);
            string convertedTotalTime = MetadataEditingMethods.ConvertTimeToHoursAndMinutes(this.TotalTime);

            string entireTimeStatement = "";

            if (printPrepTime)
            {
                entireTimeStatement += $"Prep: {convertedPrepTime}";
            }

            if ((printPrepTime && printCookTime) || (printPrepTime && printTotalTime))
            {
                entireTimeStatement += " | ";
            }

            if (printCookTime)
            {
                entireTimeStatement += $"Cook: {convertedCookTime}";
            }

            if(printCookTime && printTotalTime)
            {
                entireTimeStatement += " | ";
            }

            if (printTotalTime)
            {
                entireTimeStatement += $"Total: {convertedTotalTime}";
            }

            Console.WriteLine(entireTimeStatement);
        }
    }
}
