using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class Servings
    {
        public Servings(int lowNumberOfServings, int highNumberOfServings = 0)
        {
            this.LowNumberOfServings = lowNumberOfServings;
            this.HighNumberOfServings = highNumberOfServings;
        }

        public int LowNumberOfServings { get; set; }

        public int HighNumberOfServings { get; set; }

        public string ProduceServingsText(bool printVersion)
        {
            string servingsText = "";
            bool includeHighNumberOfServings = this.HighNumberOfServings != 0;

            if (printVersion)
            {
                servingsText += $"Servings: {this.LowNumberOfServings}";

                if (includeHighNumberOfServings)
                {
                    servingsText += $" - {this.HighNumberOfServings}";
                }

                servingsText += $"{Environment.NewLine}{Environment.NewLine}";
            }
            else
            {
                servingsText += $"LOW_SERVINGS:{this.LowNumberOfServings}{Environment.NewLine}";
                servingsText += $"HIGH_SERVIGS:{this.HighNumberOfServings}{Environment.NewLine}";
            }
            
            return servingsText;
        }
    }
}
