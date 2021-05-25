using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class Metadata
    {
        public Metadata(TitleNotes titleAndNotes, PrepTimes prepTimes, Tags tags, Servings servings)
        {
            this.TitleNotes = titleAndNotes;
            this.PrepTimes = prepTimes;
            this.Tags = tags;
            this.Servings = servings;
        }

        public TitleNotes TitleNotes { get; set; }

        public PrepTimes PrepTimes { get; set; }

        public Tags Tags { get; set; }

        public Servings Servings { get; set; }

        public string ProduceMetadataText()
        {
            string metadataText = "";
            
            metadataText += this.TitleNotes.ProduceTitleNotesText();
            metadataText += $"{Environment.NewLine}";
            metadataText += this.PrepTimes.ProducePrepTimesText();
            metadataText += $"{Environment.NewLine}";
            metadataText += this.Servings.ProduceServingsText();
            metadataText += $"{Environment.NewLine}";

            return metadataText;
        }
    }
}
