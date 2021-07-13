using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public interface IDataIO
    {
        public RecipeBookLibrary GetRecipeBookLibraryFromDataSource(string alternatePath = "");

        public bool WriteRecipeBookLibraryToDataSource(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, string alternatePath = "");
    }
}
