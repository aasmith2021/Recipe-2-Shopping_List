using System;
using System.Collections.Generic;
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

        public string Title { get; set; }

        public string Notes { get; set; }

        public Times PrepTimes { get; set; } = new Times();

        public Tags Tags { get; set; } = new Tags();

        public Servings Servings { get; set; } = new Servings();
    }
}
