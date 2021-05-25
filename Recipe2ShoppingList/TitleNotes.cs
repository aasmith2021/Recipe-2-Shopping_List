using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class TitleNotes
    {
        public TitleNotes(string recipeTitle, string userNotes = "")
        {
            this.RecipeTitle = recipeTitle;
            this.UserNotes = userNotes;
        }

        public string RecipeTitle { get; set; }

        public string UserNotes { get; set; }
    }
}
