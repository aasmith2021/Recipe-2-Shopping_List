using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class PrepTimes
    {
        public PrepTimes(int prepTime, int cookTime)
        {
            this.PrepTime = prepTime;
            this.CookTime = cookTime;
        }

        public int PrepTime { get; set; } = 0;

        public int CookTime { get; set; } = 0;

        public int TotalTime
        {
            get
            {
                return this.PrepTime + this.CookTime;
            }
        }
    }
}
