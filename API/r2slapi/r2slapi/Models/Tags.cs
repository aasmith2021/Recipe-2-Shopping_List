using System;
using System.Collections.Generic;
using System.Text;

namespace r2slapi.Models
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
    }
}
