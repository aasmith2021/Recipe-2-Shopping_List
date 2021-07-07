using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiService apiService = new ApiService();
            RecipeBookLibrary recipeBookLibrary = new RecipeBookLibrary();

            try
            {
                recipeBookLibrary = apiService.GetRecipeBookLibraryFromDataSource();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            for (int i = 0; i < recipeBookLibrary.AllRecipeBooks.Count; i++)
            {
                Console.WriteLine(recipeBookLibrary.AllRecipeBooks[i].Name);
            }
            
            
            IUserIO userIO = new ConsoleIO();
            IDataIO dataIO = new FileIO();
            
            bool exitProgram = false;

            while (!exitProgram)
            {
                ProgramExecution.RunProgram(userIO, dataIO, out exitProgram);
            }
        }
    }
}
