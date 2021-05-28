using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class TitleNotes
    {
        public TitleNotes(string recipeTitle, string userNotes = "")
        {
            this.RecipeTitle = recipeTitle;
            this.UserNotes = userNotes;
        }

        public string RecipeTitle { get; set; }

        public string UserNotes { get; set; }

        public string ProduceTitleNotesText(bool printVersion)
        {
            string titleNotesText = "";

            if (printVersion)
            {
                titleNotesText += $"----- {this.RecipeTitle.ToUpper()} -----{Environment.NewLine}";
                titleNotesText += $"{Environment.NewLine}";
                if (this.UserNotes != "")
                {
                    titleNotesText += $"Notes: {this.UserNotes}{Environment.NewLine}";
                    titleNotesText += $"{Environment.NewLine}";
                }


            }
            else
            {
                titleNotesText += $"RECIPE_TITLE:{this.RecipeTitle}{Environment.NewLine}";
                titleNotesText += $"USER_NOTES:{this.UserNotes}{Environment.NewLine}";
            }

            return titleNotesText;
        }
    }
}
