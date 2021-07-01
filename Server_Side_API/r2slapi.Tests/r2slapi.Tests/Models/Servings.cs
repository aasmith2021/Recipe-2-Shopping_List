using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace r2slapi.Tests.Models
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

        [Range(0, 500, ErrorMessage = "Low Number of Servings must be between 0 - 500")]
        public int LowNumberOfServings { get; set; }

        [Range(0, 500, ErrorMessage = "High Number of Servings must be between 0 - 500")]
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
