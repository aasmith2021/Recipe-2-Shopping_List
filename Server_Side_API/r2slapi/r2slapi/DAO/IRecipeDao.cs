using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using r2slapi.Models;

namespace r2slapi.DAO
{
    public interface IRecipeDao
    {
        public RecipeBookLibrary GetRecipeBookLibrary(int recipeBookLibraryId);

        public RecipeBook GetRecipeBook(int recipeBookId);

        public Recipe GetRecipe(int recipeId);

        public RecipeBook CreateRecipeBook(RecipeBook recipeBook);

        public Recipe CreateRecipe(int recipeBookId, Recipe recipe);
    }
}
