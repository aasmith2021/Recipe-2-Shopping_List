using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recipe2ShoppingList
{
    class WriteToFile : FileMethods
    {      
        public static void WriteRecipeBooksToFile(RecipeBooks allData)
        {
            RecipeBook[] allRecipeBooks = allData.AllRecipeBooks;
            StreamWriter sw = new StreamWriter(GetDatabaseFilePath());

            foreach (RecipeBook recipeBook in allRecipeBooks)
            {
                sw.WriteLine(recipeBook.ProduceRecipeBookText(false));
            }

            sw.Close();
        }
    }
}
