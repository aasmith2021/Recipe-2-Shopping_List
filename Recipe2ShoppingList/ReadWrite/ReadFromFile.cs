using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    public class ReadFromFile : DataHelperMethods
    {
         public static RecipeBookLibrary GetRecipeBookLibraryFromFile(string alternateFilePath = "")
        {
            RecipeBookLibrary recipeBookLibrary = new RecipeBookLibrary();
            string allTextFromFile = GetAllDatabaseText(alternateFilePath);

            string[] separateRecipeBooks = allTextFromFile.Split("-NEW_RECIPE_BOOK-", StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < separateRecipeBooks.Length; i++)
            {
                RecipeBook newRecipeBookToAdd = new RecipeBook();
                string recipeBookText = separateRecipeBooks[i];

                newRecipeBookToAdd.Name = GetRecipeBookNameFromData(recipeBookText);

                newRecipeBookToAdd.AddAllRecipesToRecipeBook(recipeBookText);

                recipeBookLibrary.AddRecipeBook(newRecipeBookToAdd);
            }            
            
            return recipeBookLibrary;
        }

        private static string GetRecipeBookNameFromData(string recipeBookData)
        {
            string recipeBookNameStartMarker = "RECIPE_BOOK_NAME:";
            string endMarker = "-START_OF_RECIPE-";

            string recipeBookName = GetDataFromStartAndEndMarkers(recipeBookData, recipeBookNameStartMarker, endMarker);

            return recipeBookName;
        }
    }
}
