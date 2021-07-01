using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace r2slapi.Tests.Models
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

        [StringLength(100, MinimumLength = 0, ErrorMessage = "Food Type length cannot exceed 100 characters.")]
        public string FoodType { get; set; }

        [StringLength(100, MinimumLength = 0, ErrorMessage = "Food Type length cannot exceed 100 characters.")]
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
