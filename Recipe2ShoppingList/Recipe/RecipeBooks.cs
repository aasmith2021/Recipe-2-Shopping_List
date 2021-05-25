using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class RecipeBooks
    {
        private List<RecipeBook> allRecipeBooksCollection = new List<RecipeBook>();

        public RecipeBook[] AllRecipeBooks
        {
            get
            {
                RecipeBook[] allRecipeBooks = allRecipeBooksCollection.ToArray();
                return allRecipeBooks;
            }
        }

        public void AddRecipeBook(RecipeBook recipeBook)
        {
            allRecipeBooksCollection.Add(recipeBook);
        }
    }
}
