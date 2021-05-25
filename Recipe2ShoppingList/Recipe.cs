using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class Recipe
    {
        public Recipe(string recipeTitle, int lowNumberOfServings, int highNumberOfServings, int prepTime, int cookTime,
            bool isSlowCookerMeal, string foodGenre = "", string specialNotes = "")
        {
            this.RecipeTitle = recipeTitle;
            this.DateAdded = DateTime.Now;
            this.DateUpdated = DateTime.Now;
            this.LowNumberOfServings = lowNumberOfServings;
            this.HighNumberOfServings = highNumberOfServings;
            this.PrepTime = prepTime;
            this.CookTime = cookTime;
            this.IsSlowCookerMeal = isSlowCookerMeal;
            this.FoodGenre = foodGenre;
            this.SpecialNotes = specialNotes;
        }

        public string RecipeTitle { get; set; }

        public DateTime DateAdded { get; }

        public DateTime DateUpdated { get; private set; }

        public int LowNumberOfServings { get; set; } = 0;

        public int HighNumberOfServings { get; set; } = 0;

        public int PrepTime { get; set; } = 0;

        public int CookTime { get; set; } = 0;

        public int TotalTime
        {
            get
            {
                return this.PrepTime + this.CookTime;
            }
        }

        public bool IsSlowCookerMeal { get; set; }

        public string SpecialNotes { get; set; }

        public string FoodGenre { get; set; }

        public void ReplaceDateUpdated(DateTime newestUpdate)
        {
            this.DateUpdated = newestUpdate;
        }

    }
}
