using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class Times
    {
        public Times()
        {

        }
        
        public Times(int prepTime = 0, int cookTime = 0)
        {
            this.PrepTime = prepTime;
            this.CookTime = cookTime;
        }

        public int Id { get; set; }

        public int PrepTime { get; set; }

        public int CookTime { get; set; }

        public int TotalTime { 
            get
            {
                return this.PrepTime + this.CookTime;
            }
        }

        public void UpdatePrepTime(int newTime)
        {
            this.PrepTime = newTime;
        }

        public void UpdateCookTime(int newTime)
        {
            this.CookTime = newTime;
        }

        public static string ConvertTimeToHoursAndMinutes(int time)
        {
            int timeRemaining = time;
            int hoursCount = 0;
            string formattedTime = "";

            while (timeRemaining >= 60)
            {
                timeRemaining -= 60;
                hoursCount++;
            }

            if (hoursCount > 1)
            {
                formattedTime += $"{hoursCount} hrs ";
            }
            else if (hoursCount == 1)
            {
                formattedTime += $"{hoursCount} hr ";
            }

            if (timeRemaining < 60 && timeRemaining != 1)
            {
                formattedTime += $"{timeRemaining} mins";
            }
            else if (timeRemaining == 1)
            {
                formattedTime += $"{timeRemaining} min";
            }

            return formattedTime;
        }

        public string ProducePrepTimesText(bool printVersion)
        {
            bool includePrepTime = this.PrepTime != 0;
            bool includeCookTime = this.CookTime != 0;
            bool includeTotalTime = this.TotalTime != 0;
            string convertedPrepTime = Times.ConvertTimeToHoursAndMinutes(this.PrepTime);
            string convertedCookTime = Times.ConvertTimeToHoursAndMinutes(this.CookTime);
            string convertedTotalTime = Times.ConvertTimeToHoursAndMinutes(this.TotalTime);

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
