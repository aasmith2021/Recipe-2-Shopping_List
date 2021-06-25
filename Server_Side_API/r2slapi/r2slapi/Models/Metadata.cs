using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace r2slapi.Models
{
    public class Metadata
    {
        public Metadata()
        {

        }
        
        public Metadata(string title, Times prepTimes, Tags tags, Servings servings, string userNotes = "")
        {
            this.Title = title;
            this.Notes = userNotes;
            this.PrepTimes = prepTimes;
            this.Tags = tags;
            this.Servings = servings;
        }

        public int Id { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "Recipe Name cannot be blank, and cannot exceed 200 characters.")]
        public string Title { get; set; }

        [StringLength(1200, MinimumLength = 0, ErrorMessage = "Recipe Notes cannot be null, and cannot exceed 1,200 characters.")]
        public string Notes { get; set; }

        public Times PrepTimes { get; set; } = new Times();

        public Tags Tags { get; set; } = new Tags();

        public Servings Servings { get; set; } = new Servings();
    }
}
