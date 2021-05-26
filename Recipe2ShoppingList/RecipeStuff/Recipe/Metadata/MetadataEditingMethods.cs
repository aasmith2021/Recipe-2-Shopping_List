using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class MetadataEditingMethods
    {
        public static void UpdateTitle(Recipe recipe, string newTitle)
        {
            recipe.Metadata.TitleNotes.RecipeTitle = newTitle;
        }

        public static void UpdateUserNotes(Recipe recipe, string newUserNotes, bool replaceCurrentNotes = false)
        {
            if (replaceCurrentNotes)
            {
                recipe.Metadata.TitleNotes.UserNotes = newUserNotes;
            }
            else
            {
                string currentUserNotes = recipe.Metadata.TitleNotes.UserNotes;
                recipe.Metadata.TitleNotes.UserNotes = currentUserNotes + $"{Environment.NewLine}" + newUserNotes;
            }
        }

        public static void UpdatePrepTime(Recipe recipe, int newTime)
        {
                recipe.Metadata.PrepTimes.PrepTime = newTime;
        }

        public static void UpdateCookTime(Recipe recipe, int newTime)
        {
            recipe.Metadata.PrepTimes.CookTime = newTime;
        }

        public static void UpdateFoodType(Recipe recipe, string newFoodType)
        {
            recipe.Metadata.Tags.FoodType = newFoodType;
        }

        public static void UpdateFoodGenre(Recipe recipe, string newFoodGenre)
        {
            recipe.Metadata.Tags.FoodGenre = newFoodGenre;
        }

        public static void UpdateNumberOfServings(Recipe recipe, int newNumberOfServings, bool updateLowNumberOfServings)
        {
            if (updateLowNumberOfServings)
            {
                recipe.Metadata.Servings.LowNumberOfServings = newNumberOfServings;
            }
            else
            {
                recipe.Metadata.Servings.HighNumberOfServings = newNumberOfServings;
            }
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
                formattedTime += $"{timeRemaining.ToString()} mins";
            }
            else if (timeRemaining == 1)
            {
                formattedTime += $"{timeRemaining.ToString()} min";
            }

            return formattedTime;
        }
    }
}
