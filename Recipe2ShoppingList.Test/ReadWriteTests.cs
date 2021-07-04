using Microsoft.VisualStudio.TestTools.UnitTesting;
using Recipe2ShoppingList;

namespace Recipe2ShoppingList.Test
{
    [TestClass]
    public class ReadWriteTests
    {
        private Recipe GenerateTestRecipe1()
        {
            string recipeTitle = "Mac & Cheese";
            Times prepTimes1 = new Times(10, 55);
            Tags tags1 = new Tags("Pasta", "Italian");
            Servings servings1 = new Servings(8, 12);
            Metadata metadata1 = new Metadata(recipeTitle, prepTimes1, tags1, servings1);

            string step1 = "Cook the pasta";
            string step2 = "Drain pasta";
            string step3 = "Add cheese";
            string step4 = "Serve and enjoy!";

            InstructionBlock firstBlock = new InstructionBlock("Prep Pasta");
            firstBlock.AddInstructionLine(step1);
            firstBlock.AddInstructionLine(step2);

            InstructionBlock secondBlock = new InstructionBlock("Make Cheesey");
            secondBlock.AddInstructionLine(step3);
            secondBlock.AddInstructionLine(step4);

            CookingInstructions ckinst1 = new CookingInstructions();
            ckinst1.AddInstructionBlock(firstBlock);
            ckinst1.AddInstructionBlock(secondBlock);

            Ingredient pasta = new Ingredient(1d, "8 oz", "Box of macaroni pasta");
            Ingredient cheese = new Ingredient(3d, "Cups", "Shredded cheddar cheese");

            IngredientList macAndCheeseIngredients = new IngredientList();
            macAndCheeseIngredients.AddIngredient(pasta);
            macAndCheeseIngredients.AddIngredient(cheese);

            Recipe recipe1 = new Recipe(metadata1, ckinst1, macAndCheeseIngredients);

            return recipe1;
        }

        private Recipe GenerateTestRecipe2()
        {
            string recipeTitle = "Tuna Casserole";
            Times prepTimes1 = new Times(10, 45);
            Tags tags1 = new Tags("Pasta", "Italian");
            Servings servings1 = new Servings(4);
            Metadata metadata1 = new Metadata(recipeTitle, prepTimes1, tags1, servings1);

            string step1 = "Cook the pasta";
            string step2 = "Drain pasta";
            string step3 = "Add tuna and cream of mushroom soup";
            string step4 = "Put in 350 F degree oven for 20 minutes";
            string step5 = "Eat up!";

            InstructionBlock firstBlock = new InstructionBlock("Prep Pasta");
            firstBlock.AddInstructionLine(step1);
            firstBlock.AddInstructionLine(step2);

            InstructionBlock secondBlock = new InstructionBlock("Combine and Bake");
            secondBlock.AddInstructionLine(step3);
            secondBlock.AddInstructionLine(step4);
            secondBlock.AddInstructionLine(step5);

            CookingInstructions ckinst1 = new CookingInstructions();
            ckinst1.AddInstructionBlock(firstBlock);
            ckinst1.AddInstructionBlock(secondBlock);

            Ingredient pasta = new Ingredient(1d, "12 oz", "Box of penne pasta");
            Ingredient tuna = new Ingredient(1d, "can", "tuna");
            Ingredient soup = new Ingredient(1d, "10 oz can", "Cream of Mushroom soup");

            IngredientList recipeIngredients = new IngredientList();
            recipeIngredients.AddIngredient(pasta);
            recipeIngredients.AddIngredient(tuna);
            recipeIngredients.AddIngredient(soup);

            Recipe recipe1 = new Recipe(metadata1, ckinst1, recipeIngredients);

            return recipe1;
        }

        private RecipeBookLibrary GetTestRecipeBookLibrary()
        {
            RecipeBookLibrary testRecipeBookLibrary = new RecipeBookLibrary();
            RecipeBook myCookBook = new RecipeBook("My Cook Book");
            Recipe recipe1 = GenerateTestRecipe1();
            Recipe recipe2 = GenerateTestRecipe2();
            myCookBook.AddRecipe(recipe1);
            myCookBook.AddRecipe(recipe2);
            testRecipeBookLibrary.AddRecipeBook(myCookBook);
            return testRecipeBookLibrary;
        }

        [TestMethod]
        public void WriteToFile_ReadFromFile_WriteToFile_produces_original_RecipeBookLibrary()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO("X");
            RecipeBookLibrary test1RecipeBookLibrary = GetTestRecipeBookLibrary();
            test1RecipeBookLibrary.WriteRecipeBookLibraryToFile(userIO, "test_database");
            RecipeBookLibrary alternateRecipeBookLibraryReadFromFile = ReadFromFile.GetRecipeBookLibraryFromFile("test_database");
            alternateRecipeBookLibraryReadFromFile.WriteRecipeBookLibraryToFile(userIO, "alternate_test_database");

            //Act
            string allDatabaseText = FileDataHelperMethods.GetAllDatabaseText("test_database");
            string allAlternateText = FileDataHelperMethods.GetAllDatabaseText("alternate_test_database");

            //Assert
            Assert.AreEqual(allDatabaseText, allAlternateText, "Writing a RecipeBookLibrary to file, reading it back, and writing it to a new file did not produce the same data in both files.");
        }

        [TestMethod]
        public void WriteToFile_ReadFromFile_change_recipe_book_name_WriteToFile_produces_different_output()
        {
            //Arrange
            IUserIO userIO = new FakeUserIO("X");
            RecipeBookLibrary databaseRecipeBookLibraryToWrite = GetTestRecipeBookLibrary();
            databaseRecipeBookLibraryToWrite.WriteRecipeBookLibraryToFile(userIO, "test_database");
            RecipeBookLibrary alternateRecipeBookLibraryReadFromFile = ReadFromFile.GetRecipeBookLibraryFromFile("test_database");
            alternateRecipeBookLibraryReadFromFile.AllRecipeBooks[0].Name = "My Other Cook Book";
            alternateRecipeBookLibraryReadFromFile.WriteRecipeBookLibraryToFile(userIO, "alternate_test_database");

            //Act
            string allDatabaseText = FileDataHelperMethods.GetAllDatabaseText("test_database");
            string allAlternateText = FileDataHelperMethods.GetAllDatabaseText("alternate_test_database");

            //Assert
            Assert.AreNotEqual(allDatabaseText, allAlternateText, "Writing a RecipeBookLibrary to a file, reading it back, changing it and writing it to a new file incorrectly produced the same data in both files.");
        }
    }
}
