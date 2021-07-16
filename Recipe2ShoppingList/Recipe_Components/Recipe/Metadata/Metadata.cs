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

        public int Id { get; set; }

        public string Title { get; set; }

        public string Notes { get; set; }

        public Times PrepTimes { get; set; } = new Times();

        public Tags Tags { get; set; } = new Tags();

        public Servings Servings { get; set; } = new Servings();

        //Creates a string of the recipe metadata. When printVersion is true, the output is meant
        //to be displayed directly to the user on the console. When it is false, the output is meant to be
        //written to the database file so it can be parsed and loaded back into the program later.
        public string ProduceMetadataText(bool printVersion)
        {
            string metadataText = "";

            if (printVersion)
            {
                metadataText += UserInterface.MakeStringConsoleLengthLines($"----- {this.Title.ToUpper()} -----{Environment.NewLine}");

                metadataText += $"{Environment.NewLine}";
                if (this.Notes != "")
                {
                    metadataText += UserInterface.MakeStringConsoleLengthLines($"Notes: {this.Notes}{Environment.NewLine}");
                    metadataText += $"{Environment.NewLine}";
                }
            }
            else
            {
                metadataText += $"METADATA_ID:{this.Id}{Environment.NewLine}";
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
