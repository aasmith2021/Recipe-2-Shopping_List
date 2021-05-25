using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class Metadata
    {
        public Metadata(TitleNotes titleAndNotes, PrepTimes prepTimes, Tags tags, Servings servings)
        {
            this.TitleAndNotes = titleAndNotes;
            this.PrepTimes = prepTimes;
            this.Tags = tags;
            this.Servings = servings;
        }

        public TitleNotes TitleAndNotes { get; set; }

        public PrepTimes PrepTimes { get; set; }

        public Tags Tags { get; set; }

        public Servings Servings { get; set; }
    }
}
