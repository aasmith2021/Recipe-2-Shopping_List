using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using r2slapi.Models;

namespace r2slapi.DAO
{
    interface IRecipeDao
    {
        public Recipe GetRecipe(int recipeId);
        /*
        public RecipeBookLibrary GetLibrary();

        public RecipeBookLibrary CreateNewLibrary();

        public void UpdateLibrary(RecipeBookLibrary libraryToUpdate);
        */
    }
}
