using System;
using System.Collections.Generic;
using System.Text;

namespace r2slapi.Models
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
    }
}
