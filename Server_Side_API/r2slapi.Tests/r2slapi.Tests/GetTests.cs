using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using r2slapi.Tests.Models;

namespace r2slapi.Tests
{
    [TestClass]
    public class GetTests
    {
        private static readonly string API_URL = "https://localhost:44302/recipes";
        private static readonly IRestClient client = new RestClient();

        [TestMethod]
        public void GET_returns_entire_recipe_book_library()
        {
            //Arrange
            RestRequest request = new RestRequest(API_URL, DataFormat.Json);

            int? expectedIdOfRecipeBookLibrary = 1;
            int expectedIdOfRecipeBook_1 = 1;
            string expectedNameOfRecipeBook_1 = "Desserts";
            string expectedTitleOfRecipe_1 = "Dirt Cups";
            string expectedNoteOfRecipe_1 = "A fun treat during the summer!";
            int expectedPrepTimeOfRecipe_1 = 10;
            int expectedCookTimeOfRecipe_1 = 15;
            int expectedLowServingsOfRecipe_1 = 2;
            int expectedHighServingsOfRecipe_1 = 6;
            string expectedFoodTypeOfRecipe_1 = "Dessert";
            string expectedFoodGenreOfRecipe_1 = "American";
            int expectedNumberOfInstructionBlocksInRecipe_1 = 2;
            string expectedHeadingOfRecipe_1_InstructionBlock_1 = "How to make it";
            string expectedBlock1Line1Instruction = "Make pudding according to directions on the box";
            string expectedBlock1Line2Instruction = "Spoon pudding into cups and top with crushed cookies and gummy worms";
            string expectedHeadingOfRecipe_1_InstructionBlock_2 = "How to eat it";
            string expectedBlock2Line1Instruction = "Eat and enjoy!";
            int expectedNumberOfIngrdientsInRecipe_1 = 3;
            double expectedIngredient1Quantity = 1;
            string expectedIngredient1MeasurementUnit = "Box";
            string expectedIngredient1Name = "chocolate pudding mix";
            string expectedIngredient1PrepNote = "brand new";
            string expectedIngredient1StoreLocation = "Dry Goods";
            double expectedIngredient2Quantity = 15;
            string expectedIngredient2MeasurementUnit = "";
            string expectedIngredient2Name = "gummy worms";
            string expectedIngredient2PrepNote = "fun flavors";
            string expectedIngredient2StoreLocation = "Dry Goods";
            double expectedIngredient3Quantity = 0.5;
            string expectedIngredient3MeasurementUnit = "Cup";
            string expectedIngredient3Name = "chocolate cookie crumbles";
            string expectedIngredient3PrepNote = "";
            string expectedIngredient3StoreLocation = "Bakery/Deli";

            //Act
            IRestResponse<RecipeBookLibrary> response = client.Get<RecipeBookLibrary>(request);

            RecipeBookLibrary returnedRecipeBookLibrary = response.Data;

            int? actualIdOfRecipeBookLibrary = returnedRecipeBookLibrary.Id;
            int actualIdOfRecipeBook_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Id;
            string actualNameOfRecipeBook_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Name;
            string actualTitleOfRecipe_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].Metadata.Title;
            string actualNoteOfRecipe_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].Metadata.Notes;
            int actualPrepTimeOfRecipe_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].Metadata.PrepTimes.PrepTime;
            int actualCookTimeOfRecipe_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].Metadata.PrepTimes.CookTime;
            int actualLowServingsOfRecipe_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].Metadata.Servings.LowNumberOfServings;
            int actualHighServingsOfRecipe_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].Metadata.Servings.HighNumberOfServings;
            string actualFoodTypeOfRecipe_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].Metadata.Tags.FoodType;
            string actualFoodGenreOfRecipe_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].Metadata.Tags.FoodGenre;
            int actualNumberOfInstructionBlocksInRecipe_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].CookingInstructions.InstructionBlocks.Count;
            string actualHeadingOfRecipe_1_InstructionBlock_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].CookingInstructions.InstructionBlocks[0].BlockHeading;
            string actualBlock1Line1Instruction = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].CookingInstructions.InstructionBlocks[0].InstructionLines[0];
            string actualBlock1Line2Instruction = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].CookingInstructions.InstructionBlocks[0].InstructionLines[1];
            string actualHeadingOfRecipe_1_InstructionBlock_2 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].CookingInstructions.InstructionBlocks[1].BlockHeading;
            string actualBlock2Line1Instruction = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].CookingInstructions.InstructionBlocks[1].InstructionLines[0];
            int actualNumberOfIngredientsInRecipe_1 = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients.Count;
            double actualIngredient1Quantity = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[0].Quantity;
            string actualIngredient1MeasurementUnit = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[0].MeasurementUnit;
            string actualIngredient1Name = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[0].Name;
            string actualIngredient1PrepNote = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[0].PreparationNote;
            string actualIngredient1StoreLocation = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[0].StoreLocation;
            double actualIngredient2Quantity = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[1].Quantity;
            string actualIngredient2MeasurementUnit = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[1].MeasurementUnit;
            string actualIngredient2Name = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[1].Name;
            string actualIngredient2PrepNote = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[1].PreparationNote;
            string actualIngredient2StoreLocation = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[1].StoreLocation;
            double actualIngredient3Quantity = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[2].Quantity;
            string actualIngredient3MeasurementUnit = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[2].MeasurementUnit;
            string actualIngredient3Name = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[2].Name;
            string actualIngredient3PrepNote = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[2].PreparationNote;
            string actualIngredient3StoreLocation = returnedRecipeBookLibrary.AllRecipeBooks[0].Recipes[0].IngredientList.AllIngredients[2].StoreLocation;


            //Assert
            Assert.AreEqual(expectedIdOfRecipeBookLibrary, actualIdOfRecipeBookLibrary, "Recipe Book Library ID retrieved is incorrect");
            Assert.AreEqual(expectedIdOfRecipeBook_1, actualIdOfRecipeBook_1, "Recipe Book 1 ID retrieved is incorrect");
            Assert.AreEqual(expectedNameOfRecipeBook_1, actualNameOfRecipeBook_1, "Name of Recipe Book 1 retrieved is incorrect");
            Assert.AreEqual(expectedTitleOfRecipe_1, actualTitleOfRecipe_1, "Name of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedNoteOfRecipe_1, actualNoteOfRecipe_1, "Note of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedPrepTimeOfRecipe_1, actualPrepTimeOfRecipe_1, "Prep Time of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedCookTimeOfRecipe_1, actualCookTimeOfRecipe_1, "Cook Time of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedLowServingsOfRecipe_1, actualLowServingsOfRecipe_1, "Low Servings of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHighServingsOfRecipe_1, actualHighServingsOfRecipe_1, "High Servings of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedFoodTypeOfRecipe_1, actualFoodTypeOfRecipe_1, "Food Type of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedFoodGenreOfRecipe_1, actualFoodGenreOfRecipe_1, "Food Genre of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedNumberOfInstructionBlocksInRecipe_1, actualNumberOfInstructionBlocksInRecipe_1, "Number of Instruction Blocks in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHeadingOfRecipe_1_InstructionBlock_1, actualHeadingOfRecipe_1_InstructionBlock_1, "Heading of Instruction Block 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock1Line1Instruction, actualBlock1Line1Instruction, "Instruction line 1 in Block 1 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock1Line2Instruction, actualBlock1Line2Instruction, "Instruction line 2 in Block 1 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHeadingOfRecipe_1_InstructionBlock_2, actualHeadingOfRecipe_1_InstructionBlock_2, "Heading of Instruction Block 2 in Recipe 1 retrieved is incorrect");
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

        [TestMethod]
        public void GET_recipe_book_number_returns_entire_recipe_book()
        {
            //Arrange
            RestRequest request = new RestRequest(API_URL + "/1", DataFormat.Json);

            int expectedIdOfRecipeBook_1 = 1;
            string expectedNameOfRecipeBook_1 = "Desserts";
            string expectedTitleOfRecipe_1 = "Dirt Cups";
            string expectedNoteOfRecipe_1 = "A fun treat during the summer!";
            int expectedPrepTimeOfRecipe_1 = 10;
            int expectedCookTimeOfRecipe_1 = 15;
            int expectedLowServingsOfRecipe_1 = 2;
            int expectedHighServingsOfRecipe_1 = 6;
            string expectedFoodTypeOfRecipe_1 = "Dessert";
            string expectedFoodGenreOfRecipe_1 = "American";
            int expectedNumberOfInstructionBlocksInRecipe_1 = 2;
            string expectedHeadingOfRecipe_1_InstructionBlock_1 = "How to make it";
            string expectedBlock1Line1Instruction = "Make pudding according to directions on the box";
            string expectedBlock1Line2Instruction = "Spoon pudding into cups and top with crushed cookies and gummy worms";
            string expectedHeadingOfRecipe_1_InstructionBlock_2 = "How to eat it";
            string expectedBlock2Line1Instruction = "Eat and enjoy!";
            int expectedNumberOfIngrdientsInRecipe_1 = 3;
            double expectedIngredient1Quantity = 1;
            string expectedIngredient1MeasurementUnit = "Box";
            string expectedIngredient1Name = "chocolate pudding mix";
            string expectedIngredient1PrepNote = "brand new";
            string expectedIngredient1StoreLocation = "Dry Goods";
            double expectedIngredient2Quantity = 15;
            string expectedIngredient2MeasurementUnit = "";
            string expectedIngredient2Name = "gummy worms";
            string expectedIngredient2PrepNote = "fun flavors";
            string expectedIngredient2StoreLocation = "Dry Goods";
            double expectedIngredient3Quantity = 0.5;
            string expectedIngredient3MeasurementUnit = "Cup";
            string expectedIngredient3Name = "chocolate cookie crumbles";
            string expectedIngredient3PrepNote = "";
            string expectedIngredient3StoreLocation = "Bakery/Deli";

            //Act
            IRestResponse<RecipeBook> response = client.Get<RecipeBook>(request);

            RecipeBook returnedRecipeBook = response.Data;

            int actualIdOfRecipeBook_1 = returnedRecipeBook.Id;
            string actualNameOfRecipeBook_1 = returnedRecipeBook.Name;
            string actualTitleOfRecipe_1 = returnedRecipeBook.Recipes[0].Metadata.Title;
            string actualNoteOfRecipe_1 = returnedRecipeBook.Recipes[0].Metadata.Notes;
            int actualPrepTimeOfRecipe_1 = returnedRecipeBook.Recipes[0].Metadata.PrepTimes.PrepTime;
            int actualCookTimeOfRecipe_1 = returnedRecipeBook.Recipes[0].Metadata.PrepTimes.CookTime;
            int actualLowServingsOfRecipe_1 = returnedRecipeBook.Recipes[0].Metadata.Servings.LowNumberOfServings;
            int actualHighServingsOfRecipe_1 = returnedRecipeBook.Recipes[0].Metadata.Servings.HighNumberOfServings;
            string actualFoodTypeOfRecipe_1 = returnedRecipeBook.Recipes[0].Metadata.Tags.FoodType;
            string actualFoodGenreOfRecipe_1 = returnedRecipeBook.Recipes[0].Metadata.Tags.FoodGenre;
            int actualNumberOfInstructionBlocksInRecipe_1 = returnedRecipeBook.Recipes[0].CookingInstructions.InstructionBlocks.Count;
            string actualHeadingOfRecipe_1_InstructionBlock_1 = returnedRecipeBook.Recipes[0].CookingInstructions.InstructionBlocks[0].BlockHeading;
            string actualBlock1Line1Instruction = returnedRecipeBook.Recipes[0].CookingInstructions.InstructionBlocks[0].InstructionLines[0];
            string actualBlock1Line2Instruction = returnedRecipeBook.Recipes[0].CookingInstructions.InstructionBlocks[0].InstructionLines[1];
            string actualHeadingOfRecipe_1_InstructionBlock_2 = returnedRecipeBook.Recipes[0].CookingInstructions.InstructionBlocks[1].BlockHeading;
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
            Assert.AreEqual(expectedIdOfRecipeBook_1, actualIdOfRecipeBook_1, "Recipe Book 1 ID retrieved is incorrect");
            Assert.AreEqual(expectedNameOfRecipeBook_1, actualNameOfRecipeBook_1, "Name of Recipe Book 1 retrieved is incorrect");
            Assert.AreEqual(expectedTitleOfRecipe_1, actualTitleOfRecipe_1, "Name of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedNoteOfRecipe_1, actualNoteOfRecipe_1, "Note of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedPrepTimeOfRecipe_1, actualPrepTimeOfRecipe_1, "Prep Time of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedCookTimeOfRecipe_1, actualCookTimeOfRecipe_1, "Cook Time of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedLowServingsOfRecipe_1, actualLowServingsOfRecipe_1, "Low Servings of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHighServingsOfRecipe_1, actualHighServingsOfRecipe_1, "High Servings of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedFoodTypeOfRecipe_1, actualFoodTypeOfRecipe_1, "Food Type of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedFoodGenreOfRecipe_1, actualFoodGenreOfRecipe_1, "Food Genre of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedNumberOfInstructionBlocksInRecipe_1, actualNumberOfInstructionBlocksInRecipe_1, "Number of Instruction Blocks in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHeadingOfRecipe_1_InstructionBlock_1, actualHeadingOfRecipe_1_InstructionBlock_1, "Heading of Instruction Block 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock1Line1Instruction, actualBlock1Line1Instruction, "Instruction line 1 in Block 1 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock1Line2Instruction, actualBlock1Line2Instruction, "Instruction line 2 in Block 1 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHeadingOfRecipe_1_InstructionBlock_2, actualHeadingOfRecipe_1_InstructionBlock_2, "Heading of Instruction Block 2 in Recipe 1 retrieved is incorrect");
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

        [TestMethod]
        public void GET_recipe_number_returns_entire_recipe()
        {
            //Arrange
            RestRequest request = new RestRequest(API_URL + "/1/1", DataFormat.Json);

            string expectedTitleOfRecipe_1 = "Dirt Cups";
            string expectedNoteOfRecipe_1 = "A fun treat during the summer!";
            int expectedPrepTimeOfRecipe_1 = 10;
            int expectedCookTimeOfRecipe_1 = 15;
            int expectedLowServingsOfRecipe_1 = 2;
            int expectedHighServingsOfRecipe_1 = 6;
            string expectedFoodTypeOfRecipe_1 = "Dessert";
            string expectedFoodGenreOfRecipe_1 = "American";
            int expectedNumberOfInstructionBlocksInRecipe_1 = 2;
            string expectedHeadingOfRecipe_1_InstructionBlock_1 = "How to make it";
            string expectedBlock1Line1Instruction = "Make pudding according to directions on the box";
            string expectedBlock1Line2Instruction = "Spoon pudding into cups and top with crushed cookies and gummy worms";
            string expectedHeadingOfRecipe_1_InstructionBlock_2 = "How to eat it";
            string expectedBlock2Line1Instruction = "Eat and enjoy!";
            int expectedNumberOfIngrdientsInRecipe_1 = 3;
            double expectedIngredient1Quantity = 1;
            string expectedIngredient1MeasurementUnit = "Box";
            string expectedIngredient1Name = "chocolate pudding mix";
            string expectedIngredient1PrepNote = "brand new";
            string expectedIngredient1StoreLocation = "Dry Goods";
            double expectedIngredient2Quantity = 15;
            string expectedIngredient2MeasurementUnit = "";
            string expectedIngredient2Name = "gummy worms";
            string expectedIngredient2PrepNote = "fun flavors";
            string expectedIngredient2StoreLocation = "Dry Goods";
            double expectedIngredient3Quantity = 0.5;
            string expectedIngredient3MeasurementUnit = "Cup";
            string expectedIngredient3Name = "chocolate cookie crumbles";
            string expectedIngredient3PrepNote = "";
            string expectedIngredient3StoreLocation = "Bakery/Deli";

            //Act
            IRestResponse<Recipe> response = client.Get<Recipe>(request);

            Recipe returnedRecipe = response.Data;

            string actualTitleOfRecipe_1 = returnedRecipe.Metadata.Title;
            string actualNoteOfRecipe_1 = returnedRecipe.Metadata.Notes;
            int actualPrepTimeOfRecipe_1 = returnedRecipe.Metadata.PrepTimes.PrepTime;
            int actualCookTimeOfRecipe_1 = returnedRecipe.Metadata.PrepTimes.CookTime;
            int actualLowServingsOfRecipe_1 = returnedRecipe.Metadata.Servings.LowNumberOfServings;
            int actualHighServingsOfRecipe_1 = returnedRecipe.Metadata.Servings.HighNumberOfServings;
            string actualFoodTypeOfRecipe_1 = returnedRecipe.Metadata.Tags.FoodType;
            string actualFoodGenreOfRecipe_1 = returnedRecipe.Metadata.Tags.FoodGenre;
            int actualNumberOfInstructionBlocksInRecipe_1 = returnedRecipe.CookingInstructions.InstructionBlocks.Count;
            string actualHeadingOfRecipe_1_InstructionBlock_1 = returnedRecipe.CookingInstructions.InstructionBlocks[0].BlockHeading;
            string actualBlock1Line1Instruction = returnedRecipe.CookingInstructions.InstructionBlocks[0].InstructionLines[0];
            string actualBlock1Line2Instruction = returnedRecipe.CookingInstructions.InstructionBlocks[0].InstructionLines[1];
            string actualHeadingOfRecipe_1_InstructionBlock_2 = returnedRecipe.CookingInstructions.InstructionBlocks[1].BlockHeading;
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
            Assert.AreEqual(expectedTitleOfRecipe_1, actualTitleOfRecipe_1, "Name of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedNoteOfRecipe_1, actualNoteOfRecipe_1, "Note of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedPrepTimeOfRecipe_1, actualPrepTimeOfRecipe_1, "Prep Time of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedCookTimeOfRecipe_1, actualCookTimeOfRecipe_1, "Cook Time of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedLowServingsOfRecipe_1, actualLowServingsOfRecipe_1, "Low Servings of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHighServingsOfRecipe_1, actualHighServingsOfRecipe_1, "High Servings of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedFoodTypeOfRecipe_1, actualFoodTypeOfRecipe_1, "Food Type of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedFoodGenreOfRecipe_1, actualFoodGenreOfRecipe_1, "Food Genre of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedNumberOfInstructionBlocksInRecipe_1, actualNumberOfInstructionBlocksInRecipe_1, "Number of Instruction Blocks in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHeadingOfRecipe_1_InstructionBlock_1, actualHeadingOfRecipe_1_InstructionBlock_1, "Heading of Instruction Block 1 in Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock1Line1Instruction, actualBlock1Line1Instruction, "Instruction line 1 in Block 1 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedBlock1Line2Instruction, actualBlock1Line2Instruction, "Instruction line 2 in Block 1 of Recipe 1 retrieved is incorrect");
            Assert.AreEqual(expectedHeadingOfRecipe_1_InstructionBlock_2, actualHeadingOfRecipe_1_InstructionBlock_2, "Heading of Instruction Block 2 in Recipe 1 retrieved is incorrect");
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
    }
}
