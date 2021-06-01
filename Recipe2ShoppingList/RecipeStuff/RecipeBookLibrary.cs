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

        public void DeleteRecipeBook(RecipeBook recipeBook)
        {
            allRecipeBooksList.Remove(recipeBook);
        }

        public void WriteRecipeBookLibraryToFile(string alternateFilePath = "")
        {
            RecipeBook[] allRecipeBooks = this.AllRecipeBooks;

            try
            {
                using (StreamWriter sw = new StreamWriter(GetWriteDatabaseFilePath(alternateFilePath)))
                {
                    foreach (RecipeBook recipeBook in allRecipeBooks)
                    {
                        sw.Write(recipeBook.ProduceRecipeBookText(false));
                    }
                }
            }
            catch (IOException exception)
            {
                Console.WriteLine("Cannot open file to save data.");
                Console.WriteLine("Press \"Enter\" to continue...");
                Console.ReadLine();
            }
        }
    }
}
