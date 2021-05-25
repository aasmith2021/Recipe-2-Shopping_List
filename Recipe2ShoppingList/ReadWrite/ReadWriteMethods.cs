using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recipe2ShoppingList
{
    class ReadWriteMethods
    {      
        public static string GetAllDatabaseText()
        {
            //TODO When the program starts, get all the recipe book file         
            StreamReader sr = new StreamReader(GetDatabaseFilePath());
            string databaseText = "";

            string currentLineOfText = sr.ReadLine();

            while (currentLineOfText != null)
            {              
                databaseText += currentLineOfText;
                currentLineOfText = sr.ReadLine();
            }
            sr.Close();

            return databaseText;
        }

        public static void WriteRecipeBooksToFile(RecipeBooks allData)
        {
            RecipeBook[] allRecipeBooks = allData.AllRecipeBooks;
            StreamWriter sw = new StreamWriter(GetDatabaseFilePath());

            foreach (RecipeBook recipeBook in allRecipeBooks)
            {
                sw.WriteLine(recipeBook.ProduceRecipeBookText());
            }

            sw.Close();
        }

        private static string GetDatabaseFilePath()
        {
            string filePath = Directory.GetCurrentDirectory();
            filePath += "\\Recipe_Database.txt";

            return filePath;
        }
    }
}
