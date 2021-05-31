using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recipe2ShoppingList
{
    public class RecipeBookLibrary : DataHelperMethods
    {
        private List<RecipeBook> allRecipeBooksList = new List<RecipeBook>();

        public RecipeBook[] AllRecipeBooks
        {
            get
            {
                RecipeBook[] allRecipeBooksArray = allRecipeBooksList.ToArray();
                return allRecipeBooksArray;
            }
        }

        public void AddRecipeBook(RecipeBook recipeBook)
        {
            allRecipeBooksList.Add(recipeBook);
        }

        public void WriteRecipeBookLibraryToFile(string alternateFilePath = "")
        {
            RecipeBook[] allRecipeBooks = this.AllRecipeBooks;
            StreamWriter sw;

            if (alternateFilePath == "")
            {
                sw = new StreamWriter(GetDatabaseFilePath());
            }
            else
            {
                sw = new StreamWriter(GetDatabaseFilePath(alternateFilePath));
            }

            foreach (RecipeBook recipeBook in allRecipeBooks)
            {
                sw.Write(recipeBook.ProduceRecipeBookText(false));
            }

            sw.Close();
        }
    }
}
