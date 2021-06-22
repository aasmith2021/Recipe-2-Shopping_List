using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class RecipeBook
    {
        public RecipeBook(string name = "")
        {
            this.Name = name;
        }
        
        public string Name { get; set; }

        private List<Recipe> recipes = new List<Recipe>();

        public Recipe[] Recipes
        {
            get
            {
                Recipe[] allRecipes = recipes.ToArray();
                return allRecipes;
            }
        }

        public void AddRecipe(Recipe newRecipe)
        {
            if (newRecipe.Metadata.RecipeId == 0)
            {
                newRecipe.Metadata.RecipeId = this.Recipes.Length + 1;
            }

            recipes.Add(newRecipe);
        }

        public void DeleteRecipe(Recipe recipeToDelete)
        {
            recipes.Remove(recipeToDelete);
        }

        public string ProduceRecipeBookText(bool printVersion)
        {
            string recipeBookText = "";

            if (printVersion)
            {
                recipeBookText += $"RECIPE BOOK NAME: {this.Name}{Environment.NewLine}{Environment.NewLine}";

                for (int i = 0; i < this.Recipes.Length; i++)
                {
                    recipeBookText += this.Recipes[i].ProduceRecipeText(printVersion);

                    if (i != this.Recipes.Length - 1)
                    {
                        recipeBookText += $"{Environment.NewLine}{Environment.NewLine}";
                    }
                }
            }
            else
            {
                recipeBookText += $"-NEW_RECIPE_BOOK-{Environment.NewLine}";
                recipeBookText += $"RECIPE_BOOK_NAME:{this.Name}{Environment.NewLine}";


                for (int i = 0; i < this.Recipes.Length; i++)
                {
                    recipeBookText += $"-START_OF_RECIPE-{Environment.NewLine}";
                    recipeBookText += this.Recipes[i].ProduceRecipeText(printVersion);
                    recipeBookText += $"-END_OF_RECIPE-{Environment.NewLine}";
                }
                recipeBookText += $"-END_OF_RECIPE_BOOK-{Environment.NewLine}";
            }

            return recipeBookText;
        }

        public void AddAllRecipesToRecipeBook(string recipeBookText)
        {
            string recipeStartMarker = "-START_OF_RECIPE-";

            if (recipeBookText.IndexOf(recipeStartMarker) >= 0)
            {
                string recipesDataForWholeBook = recipeBookText.Substring(recipeBookText.IndexOf(recipeStartMarker));

                string[] allRecipesTextSeparated = recipesDataForWholeBook.Split(recipeStartMarker, StringSplitOptions.RemoveEmptyEntries);

                foreach (string recipeText in allRecipesTextSeparated)
                {
                    this.AddOneRecipeToRecipeBook(recipeText);
                }
            }
        }

        private void AddOneRecipeToRecipeBook(string recipeText)
        {
            Recipe recipeToAdd = new Recipe();

            recipeToAdd.AddMetadataFromFile(recipeText);
            recipeToAdd.AddCookingInstructionsFromFile(recipeText);
            recipeToAdd.AddIngredientsFromFile(recipeText);

            this.AddRecipe(recipeToAdd);
        }
    }
}
