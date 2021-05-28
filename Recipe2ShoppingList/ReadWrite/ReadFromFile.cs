using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recipe2ShoppingList
{
    public class ReadFromFile : FileMethods
    {
        public static RecipeBookLibrary GetRecipeBookLibraryFromFile()
        {
            RecipeBookLibrary recipeBookLibrary = new RecipeBookLibrary();
            string allDataFromFile = GetAllDatabaseText();

            string[] separateRecipeBooks = allDataFromFile.Split("-NEW_RECIPE_BOOK-", StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < separateRecipeBooks.Length; i++)
            {
                RecipeBook newRecipeBookToAdd = new RecipeBook();
                newRecipeBookToAdd.Name = GetRecipeBookNameFromData(separateRecipeBooks[i]);

                AddAllRecipesToRecipeBook(newRecipeBookToAdd, separateRecipeBooks[i]);

                recipeBookLibrary.AddRecipeBook(newRecipeBookToAdd);
            }            
            
            return recipeBookLibrary;
        }

        public static string GetAllDatabaseText()
        {
            string databaseText = "";
            StreamReader sr = new StreamReader(WriteToFile.GetDatabaseFilePath());
            string currentLineOfText = sr.ReadLine();

            while (currentLineOfText != null)
            {
                databaseText += currentLineOfText;
                currentLineOfText = sr.ReadLine();
            }
            sr.Close();

            return databaseText;
        }

        private static string GetRecipeBookNameFromData(string recipeBookData)
        {
            string recipeBookNameStartMarker = "RECIPE_BOOK_NAME:";
            string endMarker = "-START_OF_RECIPE-";

            string recipeBookName = GetDataFromStartAndEndMarkers(recipeBookData, recipeBookNameStartMarker, endMarker);

            return recipeBookName;
        }

        public static string GetDataFromStartAndEndMarkers(string data, string startMarker, string endMarker)
        {
            int startIndexOfReturnData = data.IndexOf(startMarker) + startMarker.Length;
            int startIndexOfEndMarker = data.IndexOf(endMarker);
            int lengthOfReturnData = startIndexOfEndMarker - startIndexOfReturnData;

            string returnData = data.Substring(startIndexOfReturnData, lengthOfReturnData);

            return returnData;
        }

        private static void AddAllRecipesToRecipeBook(RecipeBook recipeBook, string recipeBookData)
        {
            string recipeStartMarker = "-START_OF_RECIPE-";
            string recipesDataForWholeBook = recipeBookData.Substring(recipeBookData.IndexOf(recipeStartMarker));

            string[] allRecipesDataSeparated = recipesDataForWholeBook.Split("-START_OF_RECIPE-", StringSplitOptions.RemoveEmptyEntries);

            foreach (string recipeData in allRecipesDataSeparated)
            {
                AddOneRecipeToRecipeBook(recipeBook, recipeData);
            }
        }

        private static void AddOneRecipeToRecipeBook(RecipeBook recipeBook, string recipeData)
        {
            Metadata metadataToAdd = GetMetadataForRecipe(recipeData);
            CookingInstructions cookingInstructionsToAdd = GetCookingInstructionsForRecipe(recipeData);
            Ingredients ingredientsToAdd = GetIngredientsForRecipe(recipeData);

            Recipe recipeToAdd = new Recipe(metadataToAdd, cookingInstructionsToAdd, ingredientsToAdd);

            recipeBook.AddRecipe(recipeToAdd);
        }

        private static Metadata GetMetadataForRecipe(string recipeData)
        {
            TitleNotes titleNotes = GetTitleNotesForRecipe(recipeData);
            PrepTimes prepTimes = GetPrepTimesForRecipe(recipeData);
            Tags tags = GetTagsForRecipe(recipeData);
            Servings servings = GetServingsForRecipe(recipeData);

            Metadata metadataToReturn = new Metadata(titleNotes, prepTimes, tags, servings);

            return metadataToReturn;
        }

        private static TitleNotes GetTitleNotesForRecipe(string recipeData)
        {
            string recipeTitleMarker = "RECIPE_TITLE:";
            string userNotesMarker = "USER_NOTES:";
            string endMarker = "FOOD_TYPE:";

            string recipeTitle = GetDataFromStartAndEndMarkers(recipeData, recipeTitleMarker, userNotesMarker);
            string userNotes = GetDataFromStartAndEndMarkers(recipeData, userNotesMarker, endMarker);

            TitleNotes titleNotesToReturn = new TitleNotes(recipeTitle, userNotes);

            return titleNotesToReturn;
        }

        private static PrepTimes GetPrepTimesForRecipe(string recipeData)
        {
            string prepTimeMarker = "PREP_TIME:";
            string cookTimeMarker = "COOK_TIME:";
            string endMarker = "LOW_SERVINGS:";

            int prepTime = Int32.Parse(GetDataFromStartAndEndMarkers(recipeData, prepTimeMarker, cookTimeMarker));
            int cookTime = Int32.Parse(GetDataFromStartAndEndMarkers(recipeData, cookTimeMarker, endMarker));

            PrepTimes prepTimesToReturn = new PrepTimes(prepTime, cookTime);

            return prepTimesToReturn;
        }

        private static Tags GetTagsForRecipe(string recipeData)
        {
            string foodTypeMarker = "FOOD_TYPE:";
            string foodGenreMarker = "FOOD_GENRE:";
            string endMarker = "PREP_TIME:";

            string foodType = GetDataFromStartAndEndMarkers(recipeData, foodTypeMarker, foodGenreMarker);
            string foodGenre = GetDataFromStartAndEndMarkers(recipeData, foodGenreMarker, endMarker);

            Tags tagsToReturn = new Tags(foodType, foodGenre);

            return tagsToReturn;
        }

        private static Servings GetServingsForRecipe(string recipeData)
        {
            string lowServingsMarker = "LOW_SERVINGS:";
            string highServingsMarker = "HIGH_SERVIGS:";
            string endMarker = "-START_OF_INGREDIENTS-";

            int lowServingsAmount = Int32.Parse(GetDataFromStartAndEndMarkers(recipeData, lowServingsMarker, highServingsMarker));
            int highServingsAmount = Int32.Parse(GetDataFromStartAndEndMarkers(recipeData, highServingsMarker, endMarker));

            Servings servingsToReturn = new Servings(lowServingsAmount, highServingsAmount);

            return servingsToReturn;
        }

        private static CookingInstructions GetCookingInstructionsForRecipe(string recipeData)
        {
            CookingInstructions cookingInstructionsToReturn = new CookingInstructions();
            
            string cookingInstructionsMarker = "-START_OF_INSTRUCTIONS-";
            string endMarker = $"-END_OF_RECIPE-";

            string cookingInstructionsData = GetDataFromStartAndEndMarkers(recipeData, cookingInstructionsMarker, endMarker);

            string[] instructionBlocksText = cookingInstructionsData.Split("-NEW_INSTRUCTION_BLOCK-", StringSplitOptions.RemoveEmptyEntries);

            foreach (string instructionBlockText in instructionBlocksText)
            {
                InstructionBlock instructionBlockFromText = GetInstructionBlockFromText(instructionBlockText);
                cookingInstructionsToReturn.AddInstructionBlock(instructionBlockFromText);
            }

            return cookingInstructionsToReturn;
        }

        private static InstructionBlock GetInstructionBlockFromText(string instructionBlockText)
        {
            InstructionBlock instructionBlockToReturn = new InstructionBlock();

            string headingStartMarker = "BLOCK_HEADING:";
            string endMarkder = "LINE:";

            instructionBlockToReturn.BlockHeading = GetDataFromStartAndEndMarkers(instructionBlockText, headingStartMarker, endMarkder);

            string[] instrctionBlockLines = GetLinesForInstructionBlock(instructionBlockText);

            foreach (string line in instrctionBlockLines)
            {
                instructionBlockToReturn.AddInstructionLine(line);
            }
            
            return instructionBlockToReturn;
        }

        private static string[] GetLinesForInstructionBlock(string instructionBlockText)
        {
            string lineStartMarker = "LINE:";
            string endMarker = "-END_OF_INSTRUCTION_BLOCK-";
            string linesText = GetDataFromStartAndEndMarkers(instructionBlockText, lineStartMarker, endMarker);
            
            string[] instructionLines = linesText.Split(lineStartMarker, StringSplitOptions.RemoveEmptyEntries);

            return instructionLines;
        }

        private static Ingredients GetIngredientsForRecipe(string recipeData)
        {
            string recipeDataStartMarker = "-START_OF_INGREDIENTS-";
            string endMarker = "-START_OF_INSTRUCTIONS-";
            string ingredientsText = GetDataFromStartAndEndMarkers(recipeData, recipeDataStartMarker, endMarker);

            Ingredient[] ingredientsToAdd = GetIngredientsFromText(ingredientsText);

            Ingredients allIngredientsToReturn = new Ingredients();
            
            foreach (Ingredient ingredient in ingredientsToAdd)
            {
                allIngredientsToReturn.AddIngredient(ingredient);
            }

            return allIngredientsToReturn;
        }

        private static Ingredient[] GetIngredientsFromText(string ingredientsText)
        {
            string[] splitMarkers = new string[] { "INGREDIENT_NAME:", "QTY:", "UNIT:" };
            string[] ingredientsAsComponents = ingredientsText.Split(splitMarkers, StringSplitOptions.RemoveEmptyEntries);

            List<Ingredient> allIngredients = new List<Ingredient>();

            for (int i = 0; i < ingredientsAsComponents.Length; i += 3)
            {
                string ingredientName = ingredientsAsComponents[i];
                string ingredientQty = ingredientsAsComponents[i + 1];
                string ingredientUnit = ingredientsAsComponents[i + 2];

                Ingredient newIngredientToAdd = new Ingredient(ingredientQty, ingredientUnit, ingredientName);

                allIngredients.Add(newIngredientToAdd);
            }

            Ingredient[] ingredientsToReturn = allIngredients.ToArray();

            return ingredientsToReturn;
        }

    }
}
