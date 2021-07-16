using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class Servings
    {
        public Servings()
        {

        }
        
        public Servings(int lowNumberOfServings, int highNumberOfServings = 0)
        {
            this.LowNumberOfServings = lowNumberOfServings;
            this.HighNumberOfServings = highNumberOfServings;
        }

        public int Id { get; set; }

        public int LowNumberOfServings { get; set; }

        public int HighNumberOfServings { get; set; }

        public void UpdateNumberOfServings(int newNumberOfServings, bool updateLowNumberOfServings)
        {
            if (updateLowNumberOfServings)
            {
                this.LowNumberOfServings = newNumberOfServings;
            }
            else
            {
                this.HighNumberOfServings = newNumberOfServings;
            }
        }

        //Creates a string of the recipe estimated serving values. When printVersion is true, the output is meant
        //to be displayed directly to the user on the console. When it is false, the output is meant to be
        //written to the database file so it can be parsed and loaded back into the program later.
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
                servingsText += $"SERVINGS_ID:{this.Id}{Environment.NewLine}";
                servingsText += $"LOW_SERVINGS:{this.LowNumberOfServings}{Environment.NewLine}";
                servingsText += $"HIGH_SERVINGS:{this.HighNumberOfServings}{Environment.NewLine}";
            }
            
            return servingsText;
        }
    }
}
