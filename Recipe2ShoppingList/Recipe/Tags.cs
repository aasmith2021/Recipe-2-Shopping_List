using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class Tags
    {
        public Tags(string foodType, string foodGenre)
        {
            this.FoodType = foodType;
            this.FoodGenre = foodGenre;
        }
        
        public string FoodType { get; set; }

        public string FoodGenre { get; set; }
    }
}
