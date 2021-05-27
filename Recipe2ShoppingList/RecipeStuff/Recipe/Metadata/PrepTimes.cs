﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class PrepTimes
    {
        public PrepTimes(int prepTime = 0, int cookTime = 0)
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

        public string ProducePrepTimesText(bool printVersion)
        {
            bool includePrepTime = this.PrepTime != 0;
            bool includeCookTime = this.CookTime != 0;
            bool includeTotalTime = this.TotalTime != 0;
            string convertedPrepTime = MetadataEditingMethods.ConvertTimeToHoursAndMinutes(this.PrepTime);
            string convertedCookTime = MetadataEditingMethods.ConvertTimeToHoursAndMinutes(this.CookTime);
            string convertedTotalTime = MetadataEditingMethods.ConvertTimeToHoursAndMinutes(this.TotalTime);

            string prepTimesText = "";

            if (printVersion)
            {
                if (includePrepTime)
                {
                    prepTimesText += $"Prep: {convertedPrepTime}";
                }

                if ((includePrepTime && includeCookTime) || (includePrepTime && includeTotalTime))
                {
                    prepTimesText += " | ";
                }

                if (includeCookTime)
                {
                    prepTimesText += $"Cook: {convertedCookTime}";
                }

                if (includeCookTime && includeTotalTime)
                {
                    prepTimesText += " | ";
                }

                if (includeTotalTime)
                {
                    prepTimesText += $"Total: {convertedTotalTime}";
                }

                prepTimesText += $"{Environment.NewLine}{Environment.NewLine}";
            }
            else
            {
                prepTimesText += $"PREP_TIME:{this.PrepTime}{Environment.NewLine}";
                prepTimesText += $"COOK_TIME:{this.CookTime}{Environment.NewLine}";
            }

            return prepTimesText;
        }
    }
}
