using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class Tags
    {
        public Tags(string foodType, string foodGenre)
        {
            this.FoodType = foodType;
            this.FoodGenre = foodGenre;
        }
        
        public string FoodType { get; set; }

        public string FoodGenre { get; set; }

        public string ProduceTagsText(bool printVersion)
        {
            string tagsText = "";

            if(printVersion)
            {
                //This is stubbed in for the future if we want to print out
                //the tags as a part of the recipe.
                tagsText = "";
            }
            else
            {
                tagsText += $"FOOD_TYPE:{this.FoodType}{Environment.NewLine}";
                tagsText += $"FOOD_GENRE:{this.FoodGenre}{Environment.NewLine}";
            }

            return tagsText;
        }
    }
}
