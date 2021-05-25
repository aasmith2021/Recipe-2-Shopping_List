using System;
using System.IO;

namespace Recipe2ShoppingList
{
    class Program
    {
        static void Main(string[] args)
        {
            TitleNotes tn1 = new TitleNotes("Mac & Cheese");
            PrepTimes prepTimes1 = new PrepTimes(10, 55);
            Tags tags1 = new Tags("Pasta", "Italian");
            Servings servings1 = new Servings(8, 12);
            Metadata metadata1 = new Metadata(tn1, prepTimes1, tags1, servings1);

            string step1 = "Cook the pasta";
            string step2 = "Drain pasta";
            string step3 = "Add cheese";
            string step4 = "Serve and enjoy!";

            InstructionBlock firstBlock = new InstructionBlock();
            firstBlock.AddInstructionLine(step1);
            firstBlock.AddInstructionLine(step2);

            InstructionBlock secondBlock = new InstructionBlock();
            secondBlock.AddInstructionLine(step3);
            secondBlock.AddInstructionLine(step4);

            CookingInstructions ckinst1 = new CookingInstructions();
            ckinst1.AddInstructionBlock(firstBlock);
            ckinst1.AddInstructionBlock(secondBlock);

            Ingredient pasta = new Ingredient("1", "8 oz", "Box of macaroni pasta");
            Ingredient cheese = new Ingredient("3", "Cups", "Shredded cheddar cheese");
            
            Ingredients macAndCheeseIngredients = new Ingredients();
            macAndCheeseIngredients.AddIngredient(pasta);
            macAndCheeseIngredients.AddIngredient(cheese);
            
            Recipe recipe1 = new Recipe(metadata1, ckinst1, macAndCheeseIngredients);

            RecipeBook myCookBook = new RecipeBook();
            myCookBook.AddRecipe(recipe1);

            foreach (Recipe recipe in myCookBook.Recipes)
            {
                recipe.PrintRecipe();
            }

            
            
            //string currentDirectory = Directory.GetCurrentDirectory();
            //string line;

            ////Read from file
            //StreamReader sr = new StreamReader($"{currentDirectory}\\RecipesDatabase.txt");
            //line = sr.ReadLine();

            //while (line != null)
            //{
            //    Console.WriteLine(line);
            //    line = sr.ReadLine();
            //}

            //sr.Close();


            ////Write to file
            //StreamWriter sw = new StreamWriter($"{currentDirectory}\\RecipesDatabase.txt", true);
            //sw.WriteLine("I'm adding new text to this file!");
            //sw.WriteLine();
            //sw.Close();

        }
    }
}
