using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    public class Recipe
    {
        public Recipe()
        {

        }
        
        public Recipe(Metadata metadata, CookingInstructions cookingInstructions, IngredientList ingredientList)
        {
            this.Metadata = metadata;
            this.CookingInstructions = cookingInstructions;
            this.IngredientList = ingredientList;
        }

        public int Id { get; set; }

        public int RecipeNumber { get; set; } = 0;

        public Metadata Metadata { get; set; } = new Metadata();

        public CookingInstructions CookingInstructions { get; set; } = new CookingInstructions();

        public IngredientList IngredientList { get; set; } = new IngredientList();

        //Creates a string of an entire recipe's text. When printVersion is true, the output is meant
        //to be displayed directly to the user on the console. When it is false, the output is meant to be
        //written to the database file so it can be parsed and loaded back into the program later.
        public string ProduceRecipeText(bool printVersion)
        {
            string recipeText = "";

            if (!printVersion)
            {
                recipeText += $"RECIPE_#:{this.RecipeNumber}{Environment.NewLine}";
                recipeText += $"RECIPE_ID:{this.Id}{Environment.NewLine}";
            }
            recipeText += this.Metadata.ProduceMetadataText(printVersion);
            recipeText += this.IngredientList.ProduceIngredientsText(printVersion);
            recipeText += this.CookingInstructions.ProduceInstructionsText(printVersion);

            return recipeText;
        }

        //This method uses regular expressions patterns to parse the data from a saved text file
        //to populate a recipe's metadata into the program.
        public void AddMetadataFromFile(string recipeText)
        {
            Dictionary<string, string> metadataRegexDictionary = new Dictionary<string, string>();
            metadataRegexDictionary["recipeNumber"] = @"RECIPE_#:(.*?)RECIPE_ID:";
            metadataRegexDictionary["recipeId"] = @"RECIPE_ID:(.*?)METADATA_ID:";
            metadataRegexDictionary["metadataId"] = @"METADATA_ID:(.*?)RECIPE_TITLE:";
            metadataRegexDictionary["title"] = @"RECIPE_TITLE:(.*?)USER_NOTES:";
            metadataRegexDictionary["notes"] = @"USER_NOTES:(.*?)TAGS_ID:";
            metadataRegexDictionary["tagsId"] = @"TAGS_ID:(.*?)FOOD_TYPE:";
            metadataRegexDictionary["foodType"] = @"FOOD_TYPE:(.*?)FOOD_GENRE:";
            metadataRegexDictionary["foodGenre"] = @"FOOD_GENRE:(.*?)PREP_TIME_ID:";
            metadataRegexDictionary["prepTimeId"] = @"PREP_TIME_ID:(.*?)PREP_TIME:";
            metadataRegexDictionary["prepTime"] = @"PREP_TIME:(.*?)COOK_TIME:";
            metadataRegexDictionary["cookTime"] = @"COOK_TIME:(.*?)SERVINGS_ID:";
            metadataRegexDictionary["servingsId"] = @"SERVINGS_ID:(.*?)LOW_SERVINGS:";
            metadataRegexDictionary["lowServings"] = @"LOW_SERVINGS:(.*?)HIGH_SERVINGS:";
            metadataRegexDictionary["highServings"] = @"HIGH_SERVINGS:(.*?)-START_OF_INGREDIENTS-";

            string regexExpression;
            string regexResult;
            int recipeNumber = 0;
            int recipeId = 0;
            int metadataId = 0;
            string title = "";
            string notes = "";
            int tagsId = 0;
            string foodType = "";
            string foodGenre = "";
            int prepTimeId = 0;
            int prepTime = 0;
            int cookTime = 0;
            int servingsId = 0;
            int lowServings = 0;
            int highServings = 0;

            foreach (KeyValuePair<string, string> element in metadataRegexDictionary)
            {
                regexExpression = element.Value;
                regexResult = Regex.Match(recipeText, regexExpression).Groups[1].Value;

                switch (element.Key)
                {
                    case "recipeNumber":
                        recipeNumber = int.Parse(regexResult);
                        break;

                    case "recipeId":
                        recipeId = int.Parse(regexResult);
                        break;

                    case "metadataId":
                        metadataId = int.Parse(regexResult);
                        break;

                    case "title":
                        title = regexResult;
                        break;

                    case "notes":
                        notes = regexResult;
                        break;

                    case "tagsId":
                        tagsId = int.Parse(regexResult);
                        break;

                    case "foodType":
                        foodType = regexResult;
                        break;

                    case "foodGenre":
                        foodGenre = regexResult;
                        break;

                    case "prepTimeId":
                        prepTimeId = int.Parse(regexResult);
                        break;

                    case "prepTime":
                        prepTime = int.Parse(regexResult);
                        break;

                    case "cookTime":
                        cookTime = int.Parse(regexResult);
                        break;

                    case "servingsId":
                        servingsId = int.Parse(regexResult);
                        break;

                    case "lowServings":
                        lowServings = int.Parse(regexResult);
                        break;

                    case "highServings":
                        highServings = int.Parse(regexResult);
                        break;

                    default:
                        break;
                }
            }

            this.RecipeNumber = recipeNumber;
            this.Id = recipeId;
            this.Metadata.Id = metadataId;
            this.Metadata.Title = title;
            this.Metadata.Notes = notes;
            this.Metadata.Tags.Id = tagsId;
            this.Metadata.Tags.FoodType = foodType;
            this.Metadata.Tags.FoodGenre = foodGenre;
            this.Metadata.PrepTimes.Id = prepTimeId;
            this.Metadata.PrepTimes.PrepTime = prepTime;
            this.Metadata.PrepTimes.CookTime = cookTime;
            this.Metadata.Servings.Id = servingsId;
            this.Metadata.Servings.LowNumberOfServings = lowServings;
            this.Metadata.Servings.HighNumberOfServings = highServings;
        }

        //This method parses the data from a saved text file to populate a recipe's cooking instructions into the program.
        public void AddCookingInstructionsFromFile(string recipeText)
        {
            string regexExpression = @"COOKING_INSTRUCTIONS_ID:(.*?)-START_OF_INSTRUCTIONS-";
            int cookingInstructionsId = Convert.ToInt32(Regex.Match(recipeText, regexExpression).Groups[1].Value.ToString());
            this.CookingInstructions.Id = cookingInstructionsId;

            string cookingInstructionsMarker = "-START_OF_INSTRUCTIONS-";
            string endMarker = $"-END_OF_RECIPE-";

            string cookingInstructionsText = FileIO.GetDataFromStartAndEndMarkers(recipeText, cookingInstructionsMarker, endMarker);

            string[] instructionBlocksText = cookingInstructionsText.Split("-NEW_INSTRUCTION_BLOCK-", StringSplitOptions.RemoveEmptyEntries);

            foreach (string instructionBlockText in instructionBlocksText)
            {
                InstructionBlock instructionBlockToAdd = new InstructionBlock();

                instructionBlockToAdd.GetInstructionBlockFromText(instructionBlockText);

                this.CookingInstructions.AddInstructionBlock(instructionBlockToAdd);
            }
        }

        //This method parses the data from a saved text file to populate a recipe's ingredients into the program.
        public void AddIngredientsFromFile(string recipeText)
        {
            string recipeDataStartMarker = "-START_OF_INGREDIENTS-";
            string endMarker = "-START_OF_INSTRUCTIONS-";
            string ingredientsText = FileIO.GetDataFromStartAndEndMarkers(recipeText, recipeDataStartMarker, endMarker);

            IngredientList allRecipeIngredients = new IngredientList();
            allRecipeIngredients.Id = GetIngredientListIdFromText(ingredientsText);

            Ingredient[] ingredientsToAdd = GetIngredientsFromText(ingredientsText);

            foreach (Ingredient ingredient in ingredientsToAdd)
            {
                allRecipeIngredients.AddIngredient(ingredient);
            }

            this.IngredientList = allRecipeIngredients;
        }

        //This method parses the data from a saved text file to populate a recipe's ingredient list ID into the program.
        private int GetIngredientListIdFromText(string ingredientsText)
        {
            string idStartMarker = "INGREDIENT_LIST_ID:";
            string endMarker = "-START_OF_INGREDIENTS-";
            int ingredientListId = Convert.ToInt32(FileIO.GetDataFromStartAndEndMarkers(ingredientsText, idStartMarker, endMarker));

            return ingredientListId;
        }

        //This method parses the data from a saved text file to populate a recipe's ingredients into the program.
        private Ingredient[] GetIngredientsFromText(string ingredientsText)
        {            
            string[] splitMarkers = new string[] { "INGREDIENT_ID:", "INGREDIENT_NAME:", "QTY:", "UNIT:", "PREP_NOTE:", "STORE_LOC:" };
            string[] ingredientsAsComponents = ingredientsText.Split(splitMarkers, StringSplitOptions.RemoveEmptyEntries);

            List<Ingredient> allIngredients = new List<Ingredient>();

            for (int i = 0; i < ingredientsAsComponents.Length; i += 6)
            {
                int ingredientId = 0;
                if(int.TryParse(ingredientsAsComponents[i], out int intResult))
                {
                    ingredientId = intResult;
                }
                
                string ingredientName = ingredientsAsComponents[i + 1];

                double ingredientQty = 0;
                if (double.TryParse(ingredientsAsComponents[i + 2], out double doubleResult))
                {
                    ingredientQty = doubleResult;
                }

                string ingredientUnit = ingredientsAsComponents[i + 3] == "NONE" ? "" : ingredientsAsComponents[i + 2];

                string ingredientPrepNote = ingredientsAsComponents[i + 4] == "NONE" ? "" : ingredientsAsComponents[i + 3];

                string ingredientStoreLocation = ingredientsAsComponents[i + 5] == "NONE" ? "" : ingredientsAsComponents[i + 4];

                Ingredient newIngredientToAdd = new Ingredient(ingredientQty, ingredientUnit, ingredientName, ingredientPrepNote, ingredientStoreLocation);

                allIngredients.Add(newIngredientToAdd);
            }

            Ingredient[] ingredientsToReturn = allIngredients.ToArray();

            return ingredientsToReturn;
        }
    }
}
