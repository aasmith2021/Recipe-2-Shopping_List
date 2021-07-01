using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using r2slapi.Tests.Models;

namespace r2slapi.Tests
{
    [TestClass]
    public class PutTests
    {
        private static readonly string API_URL = "https://localhost:44302/recipes";
        private static readonly IRestClient client = new RestClient();

        private RecipeBook GetTestRecipeBook()
        {
            Times times = new Times(25, 10);
            Tags tags = new Tags("Better Salads", "Canadian");
            Servings servings = new Servings(5, 9);
            Metadata metadata = new Metadata("Salad de Cobb", times, tags, servings, "Great for a side dish");

            IngredientList ingredientList = new IngredientList();

            Ingredient ingredient1 = new Ingredient(5, "oz.", "dark and leafy greens", "cut into pieces", "Refrigerated");
            Ingredient ingredient2 = new Ingredient(6, "oz.", "white cheddar cheese", "cut up", "Bakery/Deli");
            Ingredient ingredient3 = new Ingredient(1.5, "oz.", "hard-boiled egg pieces", "cut into small squares", "Bakery/Deli");

            ingredientList.AddIngredient(ingredient1);
            ingredientList.AddIngredient(ingredient2);
            ingredientList.AddIngredient(ingredient3);

            CookingInstructions cookingInstructions = new CookingInstructions();

            InstructionBlock instructionBlock1 = new InstructionBlock("Start");
            InstructionBlock instructionBlock2 = new InstructionBlock("Finish");

            instructionBlock1.AddInstructionLine("Add salad to small salad plates");
            instructionBlock1.AddInstructionLine("Chill cheese");
            instructionBlock2.AddInstructionLine("Make the salads.");

            cookingInstructions.AddInstructionBlock(instructionBlock1);
            cookingInstructions.AddInstructionBlock(instructionBlock2);

            Recipe recipe = new Recipe(metadata, cookingInstructions, ingredientList);

            RecipeBook recipeBook = new RecipeBook("Even Better Salads");

            recipeBook.AddRecipe(recipe);

            return recipeBook;
        }

        private Recipe GetTestRecipe()
        {
            Times times = new Times(25, 10);
            Tags tags = new Tags("Better Salads", "Canadian");
            Servings servings = new Servings(5, 9);
            Metadata metadata = new Metadata("Salad de Cobb", times, tags, servings, "Great for a side dish");

            IngredientList ingredientList = new IngredientList();

            Ingredient ingredient1 = new Ingredient(5, "oz.", "dark and leafy greens", "cut into pieces", "Refrigerated");
            Ingredient ingredient2 = new Ingredient(6, "oz.", "white cheddar cheese", "cut up", "Bakery/Deli");
            Ingredient ingredient3 = new Ingredient(1.5, "oz.", "hard-boiled egg pieces", "cut into small squares", "Bakery/Deli");

            ingredientList.AddIngredient(ingredient1);
            ingredientList.AddIngredient(ingredient2);
            ingredientList.AddIngredient(ingredient3);

            CookingInstructions cookingInstructions = new CookingInstructions();

            InstructionBlock instructionBlock1 = new InstructionBlock("Start");
            InstructionBlock instructionBlock2 = new InstructionBlock("Finish");

            instructionBlock1.AddInstructionLine("Add salad to small salad plates");
            instructionBlock1.AddInstructionLine("Chill cheese");
            instructionBlock2.AddInstructionLine("Make the salads.");

            cookingInstructions.AddInstructionBlock(instructionBlock1);
            cookingInstructions.AddInstructionBlock(instructionBlock2);

            Recipe recipe = new Recipe(metadata, cookingInstructions, ingredientList);

            return recipe;
        }

        [TestMethod]
        public void PUT_new_recipe_book_updates_recipe_book()
        {
            //Arrange
            RecipeBook recipeBookToUpdate = GetTestRecipeBook();
            RestRequest updateRequest = new RestRequest(API_URL + "/1", DataFormat.Json);
            RestRequest getRequest = new RestRequest(API_URL + "/1", DataFormat.Json);
            updateRequest.AddJsonBody(recipeBookToUpdate);

            string expectedNameOfRecipeBook = "Even Better Salads";
            int expectedNumberOfRecipesInBook = 1;
            string expectedTitleOfNewRecipe = "Salad de Cobb";
            string expectedNoteOfNewRecipe = "Great for a side dish";
            int expectedPrepTimeOfNewRecipe = 25;
            int expectedCookTimeOfNewRecipe = 10;
            int expectedLowServingsOfNewRecipe = 5;
            int expectedHighServingsOfNewRecipe = 9;
            string expectedFoodTypeOfNewRecipe = "Better Salads";
            string expectedFoodGenreOfNewRecipe = "Canadian";
            int expectedNumberOfInstructionBlocksInRecipe_1 = 2;
            string expectedHeadingOfNewRecipe_InstructionBlock_1 = "Start";
            string expectedBlock1Line1Instruction = "Add salad mix to small salad plates";
            string expectedBlock1Line2Instruction = "Shred cheddar cheese and chill";
            string expectedHeadingOfNewRecipe_InstructionBlock_2 = "Finish";
            string expectedBlock2Line1Instruction = "Make the salads by adding ingredients together on top of salad mix.";
            int expectedNumberOfIngrdientsInRecipe_1 = 3;
            double expectedIngredient1Quantity = 5;
            string expectedIngredient1MeasurementUnit = "oz.";
            string expectedIngredient1Name = "dark and leafy greens";
            string expectedIngredient1PrepNote = "cut into pieces";
            string expectedIngredient1StoreLocation = "Refrigerated";
            double expectedIngredient2Quantity = 6;
            string expectedIngredient2MeasurementUnit = "oz.";
            string expectedIngredient2Name = "white cheddar cheese";
            string expectedIngredient2PrepNote = "cut up";
            string expectedIngredient2StoreLocation = "Bakery/Deli";
            double expectedIngredient3Quantity = 1.5;
            string expectedIngredient3MeasurementUnit = "oz.";
            string expectedIngredient3Name = "hard-boiled egg pieces";
            string expectedIngredient3PrepNote = "cut into small squares";
            string expectedIngredient3StoreLocation = "Bakery/Deli";

            //Act
            IRestResponse updateResponse = client.Put(updateRequest);
            IRestResponse<RecipeBook> getResponse = client.Get<RecipeBook>(getRequest);

            RecipeBook returnedRecipeBook = getResponse.Data;

            string actualNameOfRecipeBook = returnedRecipeBook.Name;
            int actualNumberOfRecipes = returnedRecipeBook.Recipes.Count;
            string actualTitleOfNewRecipe = returnedRecipeBook.Recipes[0].Metadata.Title;
            string actualNoteOfNewRecipe = returnedRecipeBook.Recipes[0].Metadata.Notes;
            int actualPrepTimeOfNewRecipe = returnedRecipeBook.Recipes[0].Metadata.PrepTimes.PrepTime;
            int actualCookTimeOfNewRecipe = returnedRecipeBook.Recipes[0].Metadata.PrepTimes.CookTime;
            int actualLowServingsOfNewRecipe = returnedRecipeBook.Recipes[0].Metadata.Servings.LowNumberOfServings;
            int actualHighServingsOfNewRecipe = returnedRecipeBook.Recipes[0].Metadata.Servings.HighNumberOfServings;
            string actualFoodTypeOfNewRecipe = returnedRecipeBook.Recipes[0].Metadata.Tags.FoodType;
            string actualFoodGenreOfNewRecipe = returnedRecipeBook.Recipes[0].Metadata.Tags.FoodGenre;
            int actualNumberOfInstructionBlocksInRecipe_1 = returnedRecipeBook.Recipes[0].CookingInstructions.InstructionBlocks.Count;
            string actualHeadingOfNewRecipe_InstructionBlock_1 = returnedRecipeBook.Recipes[0].CookingInstructions.InstructionBlocks[0].BlockHeading;
            string actualBlock1Line1Instruction = returnedRecipeBook.Recipes[0].CookingInstructions.InstructionBlocks[0].InstructionLines[0];
            string actualBlock1Line2Instruction = returnedRecipeBook.Recipes[0].CookingInstructions.InstructionBlocks[0].InstructionLines[1];
            string actualHeadingOfNewRecipe_InstructionBlock_2 = returnedRecipeBook.Recipes[0].CookingInstructions.InstructionBlocks[1].BlockHeading;
            string actualBlock2Line1Instruction = returnedRecipeBook.Recipes[0].CookingInstructions.InstructionBlocks[1].InstructionLines[0];
            int actualNumberOfIngredientsInRecipe_1 = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients.Count;
            double actualIngredient1Quantity = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[0].Quantity;
            string actualIngredient1MeasurementUnit = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[0].MeasurementUnit;
            string actualIngredient1Name = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[0].Name;
            string actualIngredient1PrepNote = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[0].PreparationNote;
            string actualIngredient1StoreLocation = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[0].StoreLocation;
            double actualIngredient2Quantity = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[1].Quantity;
            string actualIngredient2MeasurementUnit = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[1].MeasurementUnit;
            string actualIngredient2Name = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[1].Name;
            string actualIngredient2PrepNote = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[1].PreparationNote;
            string actualIngredient2StoreLocation = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[1].StoreLocation;
            double actualIngredient3Quantity = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[2].Quantity;
            string actualIngredient3MeasurementUnit = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[2].MeasurementUnit;
            string actualIngredient3Name = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[2].Name;
            string actualIngredient3PrepNote = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[2].PreparationNote;
            string actualIngredient3StoreLocation = returnedRecipeBook.Recipes[0].IngredientList.AllIngredients[2].StoreLocation;


            //Assert
            Assert.AreEqual((int)updateResponse.StatusCode, 204, "PUT request did not generate correct 204 status code");
            Assert.AreEqual(expectedNameOfRecipeBook, actualNameOfRecipeBook, "Name of Recipe Book retrieved is incorrect");
            Assert.AreEqual(expectedNumberOfRecipesInBook, actualNumberOfRecipes, "Number of recipes in Recipe Book is incorrect");
            Assert.AreEqual(expectedTitleOfNewRecipe, actualTitleOfNewRecipe, "Name of Recipe retrieved is incorrect");
            Assert.AreEqual(expectedNoteOfNewRecipe, actualNoteOfNewRecipe, "Note of Recipe retrieved is incorrect");
            Assert.AreEqual(expectedPrepTimeOfNewRecipe, actualPrepTimeOfNewRecipe, "Prep Time of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedCookTimeOfNewRecipe, actualCookTimeOfNewRecipe, "Cook Time of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedLowServingsOfNewRecipe, actualLowServingsOfNewRecipe, "Low Servings of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHighServingsOfNewRecipe, actualHighServingsOfNewRecipe, "High Servings of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedFoodTypeOfNewRecipe, actualFoodTypeOfNewRecipe, "Food Type of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedFoodGenreOfNewRecipe, actualFoodGenreOfNewRecipe, "Food Genre of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedNumberOfInstructionBlocksInRecipe_1, actualNumberOfInstructionBlocksInRecipe_1, "Number of Instruction Blocks in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHeadingOfNewRecipe_InstructionBlock_1, actualHeadingOfNewRecipe_InstructionBlock_1, "Heading of Instruction Block 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock1Line1Instruction, actualBlock1Line1Instruction, "Instruction line 1 in Block 1 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock1Line2Instruction, actualBlock1Line2Instruction, "Instruction line 2 in Block 1 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHeadingOfNewRecipe_InstructionBlock_2, actualHeadingOfNewRecipe_InstructionBlock_2, "Heading of Instruction Block 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock2Line1Instruction, actualBlock2Line1Instruction, "Instruction line 1 in Block 2 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedNumberOfIngrdientsInRecipe_1, actualNumberOfIngredientsInRecipe_1, "Number of ingredients in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient1Quantity, actualIngredient1Quantity, "Quantity of Ingredient 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient1MeasurementUnit, actualIngredient1MeasurementUnit, "Measurement Unit of Ingredient 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient1Name, actualIngredient1Name, "Name of Ingredient 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient1PrepNote, actualIngredient1PrepNote, "Prep Note of Ingredient 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient1StoreLocation, actualIngredient1StoreLocation, "Store Location of Ingredient 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient2Quantity, actualIngredient2Quantity, "Quantity of Ingredient 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient2MeasurementUnit, actualIngredient2MeasurementUnit, "Measurement Unit of Ingredient 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient2Name, actualIngredient2Name, "Name of Ingredient 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient2PrepNote, actualIngredient2PrepNote, "Prep Note of Ingredient 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient2StoreLocation, actualIngredient2StoreLocation, "Store Location of Ingredient 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient3Quantity, actualIngredient3Quantity, "Quantity of Ingredient 3 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient3MeasurementUnit, actualIngredient3MeasurementUnit, "Measurement Unit of Ingredient 3 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient3Name, actualIngredient3Name, "Name of Ingredient 3 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient3PrepNote, actualIngredient3PrepNote, "Prep Note of Ingredient 3 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient3StoreLocation, actualIngredient3StoreLocation, "Store Location of Ingredient 3 in Recipe 1 retrieved is incorrect");
        }

        /*
        [TestMethod]
        public void POST_recipe_returns_entire_recipe()
        {
            //Arrange
            Recipe recipeToCreate = GetTestRecipe();
            RestRequest request = new RestRequest(API_URL + "/1", DataFormat.Json);
            request.AddJsonBody(recipeToCreate);

            string expectedTitleOfNewRecipe = "Cobb Salad";
            string expectedNoteOfNewRecipe = "Excellent side-salad option";
            int expectedPrepTimeOfNewRecipe = 25;
            int expectedCookTimeOfNewRecipe = 10;
            int expectedLowServingsOfNewRecipe = 4;
            int expectedHighServingsOfNewRecipe = 8;
            string expectedFoodTypeOfNewRecipe = "Salads";
            string expectedFoodGenreOfNewRecipe = "European";
            int expectedNumberOfInstructionBlocksInRecipe_1 = 2;
            string expectedHeadingOfNewRecipe_InstructionBlock_1 = "Prep Work";
            string expectedBlock1Line1Instruction = "Add salad mix to small salad plates";
            string expectedBlock1Line2Instruction = "Shred cheddar cheese and chill";
            string expectedHeadingOfNewRecipe_InstructionBlock_2 = "Assembly";
            string expectedBlock2Line1Instruction = "Make the salads by adding ingredients together on top of salad mix.";
            int expectedNumberOfIngrdientsInRecipe_1 = 3;
            double expectedIngredient1Quantity = 4;
            string expectedIngredient1MeasurementUnit = "Cup";
            string expectedIngredient1Name = "dark leafy greens";
            string expectedIngredient1PrepNote = "cut into bite-sized pieces";
            string expectedIngredient1StoreLocation = "Produce";
            double expectedIngredient2Quantity = 2;
            string expectedIngredient2MeasurementUnit = "Cup";
            string expectedIngredient2Name = "cheddar cheese";
            string expectedIngredient2PrepNote = "shredded";
            string expectedIngredient2StoreLocation = "Refrigerated";
            double expectedIngredient3Quantity = 0.5;
            string expectedIngredient3MeasurementUnit = "Cup";
            string expectedIngredient3Name = "hard-boiled egg pieces";
            string expectedIngredient3PrepNote = "cut into small squares";
            string expectedIngredient3StoreLocation = "Refrigerated";

            //Act
            IRestResponse<Recipe> response = client.Post<Recipe>(request);

            Recipe returnedRecipe = response.Data;

            string actualTitleOfNewRecipe = returnedRecipe.Metadata.Title;
            string actualNoteOfNewRecipe = returnedRecipe.Metadata.Notes;
            int actualPrepTimeOfNewRecipe = returnedRecipe.Metadata.PrepTimes.PrepTime;
            int actualCookTimeOfNewRecipe = returnedRecipe.Metadata.PrepTimes.CookTime;
            int actualLowServingsOfNewRecipe = returnedRecipe.Metadata.Servings.LowNumberOfServings;
            int actualHighServingsOfNewRecipe = returnedRecipe.Metadata.Servings.HighNumberOfServings;
            string actualFoodTypeOfNewRecipe = returnedRecipe.Metadata.Tags.FoodType;
            string actualFoodGenreOfNewRecipe = returnedRecipe.Metadata.Tags.FoodGenre;
            int actualNumberOfInstructionBlocksInRecipe_1 = returnedRecipe.CookingInstructions.InstructionBlocks.Count;
            string actualHeadingOfNewRecipe_InstructionBlock_1 = returnedRecipe.CookingInstructions.InstructionBlocks[0].BlockHeading;
            string actualBlock1Line1Instruction = returnedRecipe.CookingInstructions.InstructionBlocks[0].InstructionLines[0];
            string actualBlock1Line2Instruction = returnedRecipe.CookingInstructions.InstructionBlocks[0].InstructionLines[1];
            string actualHeadingOfNewRecipe_InstructionBlock_2 = returnedRecipe.CookingInstructions.InstructionBlocks[1].BlockHeading;
            string actualBlock2Line1Instruction = returnedRecipe.CookingInstructions.InstructionBlocks[1].InstructionLines[0];
            int actualNumberOfIngredientsInRecipe_1 = returnedRecipe.IngredientList.AllIngredients.Count;
            double actualIngredient1Quantity = returnedRecipe.IngredientList.AllIngredients[0].Quantity;
            string actualIngredient1MeasurementUnit = returnedRecipe.IngredientList.AllIngredients[0].MeasurementUnit;
            string actualIngredient1Name = returnedRecipe.IngredientList.AllIngredients[0].Name;
            string actualIngredient1PrepNote = returnedRecipe.IngredientList.AllIngredients[0].PreparationNote;
            string actualIngredient1StoreLocation = returnedRecipe.IngredientList.AllIngredients[0].StoreLocation;
            double actualIngredient2Quantity = returnedRecipe.IngredientList.AllIngredients[1].Quantity;
            string actualIngredient2MeasurementUnit = returnedRecipe.IngredientList.AllIngredients[1].MeasurementUnit;
            string actualIngredient2Name = returnedRecipe.IngredientList.AllIngredients[1].Name;
            string actualIngredient2PrepNote = returnedRecipe.IngredientList.AllIngredients[1].PreparationNote;
            string actualIngredient2StoreLocation = returnedRecipe.IngredientList.AllIngredients[1].StoreLocation;
            double actualIngredient3Quantity = returnedRecipe.IngredientList.AllIngredients[2].Quantity;
            string actualIngredient3MeasurementUnit = returnedRecipe.IngredientList.AllIngredients[2].MeasurementUnit;
            string actualIngredient3Name = returnedRecipe.IngredientList.AllIngredients[2].Name;
            string actualIngredient3PrepNote = returnedRecipe.IngredientList.AllIngredients[2].PreparationNote;
            string actualIngredient3StoreLocation = returnedRecipe.IngredientList.AllIngredients[2].StoreLocation;


            //Assert
            Assert.AreEqual(expectedTitleOfNewRecipe, actualTitleOfNewRecipe, "Name of Recipe retrieved is incorrect");
            Assert.AreEqual(expectedNoteOfNewRecipe, actualNoteOfNewRecipe, "Note of Recipe retrieved is incorrect");
            Assert.AreEqual(expectedPrepTimeOfNewRecipe, actualPrepTimeOfNewRecipe, "Prep Time of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedCookTimeOfNewRecipe, actualCookTimeOfNewRecipe, "Cook Time of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedLowServingsOfNewRecipe, actualLowServingsOfNewRecipe, "Low Servings of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHighServingsOfNewRecipe, actualHighServingsOfNewRecipe, "High Servings of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedFoodTypeOfNewRecipe, actualFoodTypeOfNewRecipe, "Food Type of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedFoodGenreOfNewRecipe, actualFoodGenreOfNewRecipe, "Food Genre of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedNumberOfInstructionBlocksInRecipe_1, actualNumberOfInstructionBlocksInRecipe_1, "Number of Instruction Blocks in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHeadingOfNewRecipe_InstructionBlock_1, actualHeadingOfNewRecipe_InstructionBlock_1, "Heading of Instruction Block 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock1Line1Instruction, actualBlock1Line1Instruction, "Instruction line 1 in Block 1 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock1Line2Instruction, actualBlock1Line2Instruction, "Instruction line 2 in Block 1 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHeadingOfNewRecipe_InstructionBlock_2, actualHeadingOfNewRecipe_InstructionBlock_2, "Heading of Instruction Block 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock2Line1Instruction, actualBlock2Line1Instruction, "Instruction line 1 in Block 2 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedNumberOfIngrdientsInRecipe_1, actualNumberOfIngredientsInRecipe_1, "Number of ingredients in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient1Quantity, actualIngredient1Quantity, "Quantity of Ingredient 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient1MeasurementUnit, actualIngredient1MeasurementUnit, "Measurement Unit of Ingredient 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient1Name, actualIngredient1Name, "Name of Ingredient 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient1PrepNote, actualIngredient1PrepNote, "Prep Note of Ingredient 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient1StoreLocation, actualIngredient1StoreLocation, "Store Location of Ingredient 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient2Quantity, actualIngredient2Quantity, "Quantity of Ingredient 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient2MeasurementUnit, actualIngredient2MeasurementUnit, "Measurement Unit of Ingredient 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient2Name, actualIngredient2Name, "Name of Ingredient 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient2PrepNote, actualIngredient2PrepNote, "Prep Note of Ingredient 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient2StoreLocation, actualIngredient2StoreLocation, "Store Location of Ingredient 2 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient3Quantity, actualIngredient3Quantity, "Quantity of Ingredient 3 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient3MeasurementUnit, actualIngredient3MeasurementUnit, "Measurement Unit of Ingredient 3 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient3Name, actualIngredient3Name, "Name of Ingredient 3 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient3PrepNote, actualIngredient3PrepNote, "Prep Note of Ingredient 3 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedIngredient3StoreLocation, actualIngredient3StoreLocation, "Store Location of Ingredient 3 in Recipe 1 retrieved is incorrect");
        }
        */
    }
}
