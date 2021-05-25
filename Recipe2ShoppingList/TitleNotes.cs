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

        public void PrintTitleNotes()
        {
            Console.WriteLine($"----- {this.RecipeTitle.ToUpper()} -----");
            if (this.UserNotes != "")
            {
                Console.WriteLine();
                Console.WriteLine($"Notes: {this.UserNotes}");
            }
        }
    }
}
