using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class RecipeBook
    {
        public RecipeBook()
        {

        }
        
        public RecipeBook(string name = "")
        {
            this.Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();

        public void AddRecipe(Recipe newRecipe)
        {
            if (newRecipe.RecipeNumber == 0)
            {
                newRecipe.RecipeNumber = this.Recipes.Count + 1;
            }

            Recipes.Add(newRecipe);
        }

        public void DeleteRecipe(Recipe recipeToDelete)
        {
            Recipes.Remove(recipeToDelete);
        }

        public string ProduceRecipeBookText(bool printVersion)
        {
            string recipeBookText = "";

            if (printVersion)
            {
                recipeBookText += $"RECIPE BOOK NAME: {this.Name}{Environment.NewLine}{Environment.NewLine}";

                for (int i = 0; i < this.Recipes.Count; i++)
                {
                    recipeBookText += this.Recipes[i].ProduceRecipeText(printVersion);

                    if (i != this.Recipes.Count - 1)
                    {
                        recipeBookText += $"{Environment.NewLine}{Environment.NewLine}";
                    }
                }
            }
            else
            {
                recipeBookText += $"-NEW_RECIPE_BOOK-{Environment.NewLine}";
                recipeBookText += $"RECIPE_BOOK_NAME:{this.Name}{Environment.NewLine}";


                for (int i = 0; i < this.Recipes.Count; i++)
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
