using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class Metadata
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

        public string ProduceMetadataText(bool printVersion)
        {
            string metadataText = "";

            metadataText += this.TitleNotes.ProduceTitleNotesText(printVersion);
            
            //Adds the tags text for when data is writte to a file, but nothing
            //when the printVersion is created
            metadataText += printVersion ? "" : Tags.ProduceTagsText(printVersion);

            metadataText += this.PrepTimes.ProducePrepTimesText(printVersion);
            metadataText += this.Servings.ProduceServingsText(printVersion);

            return metadataText;
        }
    }
}
