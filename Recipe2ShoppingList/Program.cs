﻿using System;
using System.IO;

namespace Recipe2ShoppingList
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter a URL:");
            //string url = Console.ReadLine();
            //string output = DataHelperMethods.GetWebsiteDataFromURL(url);
            ////string[] splitSeparators = { "\"", ",", ":" };
            //string splitOutput = output.Substring(output.IndexOf("prep"), 1200);//.Split(splitSeparators, StringSplitOptions.RemoveEmptyEntries);

            //Console.WriteLine(splitOutput);

            //foreach (string myString in splitOutput)
            //{
            //    Console.WriteLine(myString);
            //}

            //string test = "Love.love love/love:joy\"peace\":patience:";
            //string[] splitSeparators = { "\"", ",", ":", ".", "/" };

            //string[] splitTest = test.Split(splitSeparators, StringSplitOptions.RemoveEmptyEntries);

            //foreach (string element in splitTest)
            //{
            //    Console.WriteLine(element);
            //}

            //RecipeBookLibrary databaseRecipeBookLibraryToWrite = GetTestRecipeBookLibrary();
            //databaseRecipeBookLibraryToWrite.WriteRecipeBookLibraryToFile();
            //RecipeBookLibrary alternateRecipeBookLibraryReadFromFile = ReadFromFile.GetRecipeBookLibraryFromFile();
            //alternateRecipeBookLibraryReadFromFile.WriteRecipeBookLibraryToFile("alternate");

            //string allDatabaseText = ReadFromFile.GetAllDatabaseText();
            //string allAlternateText = ReadFromFile.GetAllDatabaseText("alternate");

            //if (allDatabaseText == allAlternateText)
            //{
            //    Console.WriteLine("Databases are equal!");
            //}

            //if (allDatabaseText.Length == allAlternateText.Length)
            //{
            //    Console.WriteLine("Databases are the same length!");
            //}

            DataHelperMethods.ReadWriteTest();

        }

        private static Recipe GenerateTestRecipe1()
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

            Ingredient pasta = new Ingredient("1", "8 oz", "Box of macaroni pasta");
            Ingredient cheese = new Ingredient("3", "Cups", "Shredded cheddar cheese");

            IngredientsList macAndCheeseIngredients = new IngredientsList();
            macAndCheeseIngredients.AddIngredient(pasta);
            macAndCheeseIngredients.AddIngredient(cheese);

            Recipe recipe1 = new Recipe(metadata1, ckinst1, macAndCheeseIngredients);

            return recipe1;
        }

        private static Recipe GenerateTestRecipe2()
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

            Ingredient pasta = new Ingredient("1", "12 oz", "Box of penne pasta");
            Ingredient tuna = new Ingredient("1", "can", "tuna");
            Ingredient soup = new Ingredient("1", "10 oz can", "Cream of Mushroom soup");

            IngredientsList recipeIngredients = new IngredientsList();
            recipeIngredients.AddIngredient(pasta);
            recipeIngredients.AddIngredient(tuna);
            recipeIngredients.AddIngredient(soup);

            Recipe recipe1 = new Recipe(metadata1, ckinst1, recipeIngredients);

            return recipe1;
        }

        private static RecipeBookLibrary GetTestRecipeBookLibrary()
        {
            RecipeBookLibrary testRecipeBookLibrary = new RecipeBookLibrary();
            RecipeBook myCookBook = new RecipeBook("My Cook Book");
            testRecipeBookLibrary.AddRecipeBook(myCookBook);
            Recipe recipe1 = GenerateTestRecipe1();
            Recipe recipe2 = GenerateTestRecipe2();
            myCookBook.AddRecipe(recipe1);
            myCookBook.AddRecipe(recipe2);

            return testRecipeBookLibrary;
        }
    }
}
