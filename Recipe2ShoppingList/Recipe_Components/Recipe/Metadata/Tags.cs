using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class Tags
    {
        public Tags()
        {

        }
        
        public Tags(string foodType, string foodGenre)
        {
            this.FoodType = foodType;
            this.FoodGenre = foodGenre;
        }

        public int Id { get; set; }

        public string FoodType { get; set; }

        public string FoodGenre { get; set; }

        public void UpdateFoodType(string newFoodType)
        {
            this.FoodType = newFoodType;
        }

        public void UpdateFoodGenre(string newFoodGenre)
        {
            this.FoodGenre = newFoodGenre;
        }

        //Creates a string of the recipe tags. When printVersion is true, the output is meant
        //to be displayed directly to the user on the console. When it is false, the output is meant to be
        //written to the database file so it can be parsed and loaded back into the program later.
        public string ProduceTagsText(bool printVersion)
        {
            string tagsText = "";

            if(printVersion)
            {
                //This is stubbed in for the future if there is ever a need to print out
                //the tags as a part of the recipe on the screen to the user.
                tagsText = "";
            }
            else
            {
                tagsText += $"TAGS_ID:{this.Id}{Environment.NewLine}";
                tagsText += UserInterface.MakeStringConsoleLengthLines($"FOOD_TYPE:{this.FoodType}{Environment.NewLine}");
                tagsText += UserInterface.MakeStringConsoleLengthLines($"FOOD_GENRE:{this.FoodGenre}{Environment.NewLine}");
            }

            return tagsText;
        }
    }
}
