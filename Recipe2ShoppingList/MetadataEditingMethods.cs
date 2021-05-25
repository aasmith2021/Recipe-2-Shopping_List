using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class MetadataEditingMethods
    {
        public void UpdateTitle(Recipe recipe, string newTitle)
        {
            recipe.RecipeMetadata.TitleAndNotes.RecipeTitle = newTitle;
        }

        public void UpdateUserNotes(Recipe recipe, string newUserNotes, bool replaceCurrentNotes = false)
        {
            if (replaceCurrentNotes)
            {
                recipe.RecipeMetadata.TitleAndNotes.UserNotes = newUserNotes;
            }
            else
            {
                string currentUserNotes = recipe.RecipeMetadata.TitleAndNotes.UserNotes;
                recipe.RecipeMetadata.TitleAndNotes.UserNotes = currentUserNotes + "\n" + newUserNotes;
            }
        }

        public void UpdatePrepOrCookTime(Recipe recipe, int newTime, bool updatePrepTime)
        {
            if (updatePrepTime)
            {
                recipe.RecipeMetadata.PrepTimes.PrepTime = newTime;
            }
            else
            {
                recipe.RecipeMetadata.PrepTimes.CookTime = newTime;
            }
        }

        public void UpdateFoodType(Recipe recipe, string newFoodType)
        {
            recipe.RecipeMetadata.Tags.FoodType = newFoodType;
        }

        public void UpdateFoodGenre(Recipe recipe, string newFoodGenre)
        {
            recipe.RecipeMetadata.Tags.FoodGenre = newFoodGenre;
        }

        public void UpdateNumberOfServings(Recipe recipe, int newNumberOfServings, bool updateLowNumberOfServings)
        {
            if (updateLowNumberOfServings)
            {
                recipe.RecipeMetadata.Servings.LowNumberOfServings = newNumberOfServings;
            }
            else
            {
                recipe.RecipeMetadata.Servings.HighNumberOfServings = newNumberOfServings;
            }
        }
    }
}
