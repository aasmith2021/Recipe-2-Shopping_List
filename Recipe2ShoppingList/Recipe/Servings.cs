using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class Servings
    {
        public Servings(int lowNumberOfServings, int highNumberOfServings = 0)
        {
            this.LowNumberOfServings = lowNumberOfServings;
            this.HighNumberOfServings = highNumberOfServings;
        }

        public int LowNumberOfServings { get; set; }

        public int HighNumberOfServings { get; set; }

        public string ProduceServingsText()
        {
            string servingsText = $"Servings: {this.LowNumberOfServings}";
            bool includeHighNumberOfServings = this.HighNumberOfServings != 0;
            
            if (includeHighNumberOfServings)
            {
                servingsText += $" - {this.HighNumberOfServings}";
            }

            servingsText += $"{Environment.NewLine}";
            
            return servingsText;
        }
    }
}
