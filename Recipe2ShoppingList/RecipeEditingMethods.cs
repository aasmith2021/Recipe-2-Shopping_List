using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class RecipeEditingMethods
    {
        public void UpdateTitle(Recipe recipe, string newTitle)
        {
            recipe.RecipeTitle = newTitle;
        }

        public void UpdateNumberOfServings(Recipe recipe, int newNumberOfServings, bool updateLowNumberOfServings)
        {
            if (updateLowNumberOfServings)
            {
                recipe.LowNumberOfServings = newNumberOfServings;
            }
            else
            {
                recipe.HighNumberOfServings = newNumberOfServings;
            }            
        }

        public void UpdatePrepOrCookTime(Recipe recipe, int newTime, bool updatePrepTime)
        {
            if (updatePrepTime)
            {
                recipe.PrepTime = newTime;
            }
            else
            {
                recipe.CookTime = newTime;
            }
        }

        public void UpdateSlowCookerRecipe(Recipe recipe, bool isSlowCookerMeal)
        {
            recipe.IsSlowCookerMeal = isSlowCookerMeal;
        }

        public void UpdateSpecialNotes(Recipe recipe, string newSpecialNotes)
        {
            recipe.SpecialNotes = newSpecialNotes;
        }
        
        public void UpdateFoodGenre(Recipe recipe, string newFoodGenre)
        {
            recipe.FoodGenre = newFoodGenre;
        }
    }
}
