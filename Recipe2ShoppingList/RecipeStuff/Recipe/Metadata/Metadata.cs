using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
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
        public string Title { get; set; }

        public string Notes { get; set; }

        public Times PrepTimes { get; set; } = new Times();

        public Tags Tags { get; set; } = new Tags();

        public Servings Servings { get; set; } = new Servings();

        public string ProduceMetadataText(bool printVersion)
        {
            string metadataText = "";

            if (printVersion)
            {
                metadataText += $"----- {this.Title.ToUpper()} -----{Environment.NewLine}";
                metadataText += $"{Environment.NewLine}";
                if (this.Notes != "")
                {
                    metadataText += $"Notes: {this.Notes}{Environment.NewLine}";
                    metadataText += $"{Environment.NewLine}";
                }
            }
            else
            {
                metadataText += $"RECIPE_TITLE:{this.Title}{Environment.NewLine}";
                metadataText += $"USER_NOTES:{this.Notes}{Environment.NewLine}";
            }

            //Adds the tags text for when data is writte to a file, but nothing
            //when the printVersion is created
            metadataText += printVersion ? "" : this.Tags.ProduceTagsText(printVersion);

            metadataText += this.PrepTimes.ProducePrepTimesText(printVersion);
            metadataText += this.Servings.ProduceServingsText(printVersion);

            return metadataText;
        }
    }
}
