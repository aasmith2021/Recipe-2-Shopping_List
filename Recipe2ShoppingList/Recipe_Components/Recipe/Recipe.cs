﻿using System;
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
        
        public Recipe(Metadata metadata, CookingInstructions cookingInstructions, IngredientList ingredients)
        {
            this.Metadata = metadata;
            this.CookingInstructions = cookingInstructions;
            this.Ingredients = ingredients;
        }

        public int Id { get; set; }

        public int RecipeNumber { get; set; } = 0;

        public Metadata Metadata { get; set; } = new Metadata();

        public CookingInstructions CookingInstructions { get; set; } = new CookingInstructions();

        public IngredientList Ingredients { get; set; } = new IngredientList();

        public string ProduceRecipeText(bool printVersion)
        {
            string recipeText = "";

            if (!printVersion)
            {
                recipeText += $"RECIPE_#:{this.RecipeNumber}{Environment.NewLine}";
            }
            recipeText += this.Metadata.ProduceMetadataText(printVersion);
            recipeText += this.Ingredients.ProduceIngredientsText(printVersion);
            recipeText += this.CookingInstructions.ProduceInstructionsText(printVersion);

            return recipeText;
        }

        public void AddMetadataFromFile(string recipeText)
        {
            Dictionary<string, string> metadataRegexDictionary = new Dictionary<string, string>();
            metadataRegexDictionary["recipeNumber"] = @"RECIPE_#:(.*?)RECIPE_TITLE:";
            metadataRegexDictionary["title"] = @"RECIPE_TITLE:(.*?)USER_NOTES:";
            metadataRegexDictionary["notes"] = @"USER_NOTES:(.*?)FOOD_TYPE:";
            metadataRegexDictionary["foodType"] = @"FOOD_TYPE:(.*?)FOOD_GENRE:";
            metadataRegexDictionary["foodGenre"] = @"FOOD_GENRE:(.*?)PREP_TIME:";
            metadataRegexDictionary["prepTime"] = @"PREP_TIME:(.*?)COOK_TIME:";
            metadataRegexDictionary["cookTime"] = @"COOK_TIME:(.*?)LOW_SERVINGS:";
            metadataRegexDictionary["lowServings"] = @"LOW_SERVINGS:(.*?)HIGH_SERVINGS:";
            metadataRegexDictionary["highServings"] = @"HIGH_SERVINGS:(.*?)-START_OF_INGREDIENTS-";

            string regexExpression;
            string regexResult;
            int recipeNumber = 0;
            string title = "";
            string notes = "";
            string foodType = "";
            string foodGenre = "";
            int prepTime = 0;
            int cookTime = 0;
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
                    
                    case "title":
                        title = regexResult;
                        break;

                    case "notes":
                        notes = regexResult;
                        break;

                    case "foodType":
                        foodType = regexResult;
                        break;

                    case "foodGenre":
                        foodGenre = regexResult;
                        break;

                    case "prepTime":
                        prepTime = int.Parse(regexResult);
                        break;

                    case "cookTime":
                        cookTime = int.Parse(regexResult);
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
            this.Metadata.Title = title;
            this.Metadata.Notes = notes;
            this.Metadata.Tags.FoodType = foodType;
            this.Metadata.Tags.FoodGenre = foodGenre;
            this.Metadata.PrepTimes.PrepTime = prepTime;
            this.Metadata.PrepTimes.CookTime = cookTime;
            this.Metadata.Servings.LowNumberOfServings = lowServings;
            this.Metadata.Servings.HighNumberOfServings = highServings;
        }

        public void AddCookingInstructionsFromFile(string recipeText)
        {
            string cookingInstructionsMarker = "-START_OF_INSTRUCTIONS-";
            string endMarker = $"-END_OF_RECIPE-";

            string cookingInstructionsText = DataHelperMethods.GetDataFromStartAndEndMarkers(recipeText, cookingInstructionsMarker, endMarker);

            string[] instructionBlocksText = cookingInstructionsText.Split("-NEW_INSTRUCTION_BLOCK-", StringSplitOptions.RemoveEmptyEntries);

            foreach (string instructionBlockText in instructionBlocksText)
            {
                InstructionBlock instructionBlockToAdd = new InstructionBlock();

                instructionBlockToAdd.GetInstructionBlockFromText(instructionBlockText);

                this.CookingInstructions.AddInstructionBlock(instructionBlockToAdd);
            }
        }

        public void AddIngredientsFromFile(string recipeText)
        {
            string recipeDataStartMarker = "-START_OF_INGREDIENTS-";
            string endMarker = "-START_OF_INSTRUCTIONS-";
            string ingredientsText = DataHelperMethods.GetDataFromStartAndEndMarkers(recipeText, recipeDataStartMarker, endMarker);

            IngredientList allRecipeIngredients = new IngredientList();

            Ingredient[] ingredientsToAdd = GetIngredientsFromText(ingredientsText);

            foreach (Ingredient ingredient in ingredientsToAdd)
            {
                allRecipeIngredients.AddIngredient(ingredient);
            }

            this.Ingredients = allRecipeIngredients;
        }

        private Ingredient[] GetIngredientsFromText(string ingredientsText)
        {
            string[] splitMarkers = new string[] { "INGREDIENT_NAME:", "QTY:", "UNIT:", "PREP_NOTE:", "STORE_LOC:" };
            string[] ingredientsAsComponents = ingredientsText.Split(splitMarkers, StringSplitOptions.RemoveEmptyEntries);

            List<Ingredient> allIngredients = new List<Ingredient>();

            for (int i = 0; i < ingredientsAsComponents.Length; i += 5)
            {
                string ingredientName = ingredientsAsComponents[i];

                double ingredientQty = 0;
                if (double.TryParse(ingredientsAsComponents[i + 1], out double result))
                {
                    ingredientQty = result;
                }

                string ingredientUnit = ingredientsAsComponents[i + 2] == "NONE" ? "" : ingredientsAsComponents[i + 2];

                string ingredientPrepNote = ingredientsAsComponents[i + 3] == "NONE" ? "" : ingredientsAsComponents[i + 3];

                string ingredientStoreLocation = ingredientsAsComponents[i + 4] == "NONE" ? "" : ingredientsAsComponents[i + 4];

                Ingredient newIngredientToAdd = new Ingredient(ingredientQty, ingredientUnit, ingredientName, ingredientPrepNote, ingredientStoreLocation);

                allIngredients.Add(newIngredientToAdd);
            }

            Ingredient[] ingredientsToReturn = allIngredients.ToArray();

            return ingredientsToReturn;
        }
    }
}