using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recipe2ShoppingList
{
    public class WriteToFile : FileMethods
    {      
        public static void WriteRecipeBookLibraryToFile(RecipeBookLibrary recipeBookLibrary)
        {
            RecipeBook[] allRecipeBooks = recipeBookLibrary.AllRecipeBooks;

            StreamWriter sw = new StreamWriter(GetDatabaseFilePath());

            foreach (RecipeBook recipeBook in allRecipeBooks)
            {
                sw.WriteLine(recipeBook.ProduceRecipeBookText(false));
            }

            sw.Close();
        }

        public static void WriteRecipeBooksToAlternateFile(RecipeBookLibrary allData)
        {
            RecipeBook[] allRecipeBooks = allData.AllRecipeBooks;

            string filePath = Directory.GetCurrentDirectory();
            filePath += "\\Recipe_Database_Alternate.txt";

            StreamWriter sw = new StreamWriter(filePath);

            foreach (RecipeBook recipeBook in allRecipeBooks)
            {
                sw.WriteLine(recipeBook.ProduceRecipeBookText(false));
            }

            sw.Close();
        }
    }
}
