using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using r2slapi.Models;
using r2slapi.DAO;

namespace r2slapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private IRecipeDao recipe_dao = new RecipeDao();
        
        public RecipesController()
        {
            //Test recipe 1
            /*
            Times times1 = new Times(10, 15);
            Tags tags1 = new Tags("Dessert", "American");
            Servings servings1 = new Servings(4, 8);
            Metadata meta1 = new Metadata("Dirt Cups", times1, tags1, servings1, "Yummy summer treat!");

            IngredientList ingredientList1 = new IngredientList();
            Ingredient ingredient1 = new Ingredient(1d, "box", "chocolate pudding mix");
            Ingredient ingredient2 = new Ingredient(15d, "", "gummy worms");
            Ingredient ingredient3 = new Ingredient(.5, "Cup", "Oreo cookie crumbles");
            ingredientList1.AddIngredient(ingredient1);
            ingredientList1.AddIngredient(ingredient2);
            ingredientList1.AddIngredient(ingredient3);

            InstructionBlock instructionBlock1 = new InstructionBlock("Instructions");
            instructionBlock1.AddInstructionLine("Make chocolate pudding");
            instructionBlock1.AddInstructionLine("Top with cookie crumbles and gummy worms");
            instructionBlock1.AddInstructionLine("Enjoy!");
            CookingInstructions cooking1 = new CookingInstructions();
            cooking1.AddInstructionBlock(instructionBlock1); 

            Recipe testRecipe1 = new Recipe(meta1, cooking1, ingredientList1);


            RecipeBook testRecipeBook = new RecipeBook("Desserts");
            testRecipeBook.AddRecipe(testRecipe1);

            this.RecipeBookLibrary.AddRecipeBook(testRecipeBook);
            */
        }

        public RecipeBookLibrary RecipeBookLibrary { get; set; } = new RecipeBookLibrary();

        /*
        [HttpGet]
        public RecipeBookLibrary Get()
        {
            return this.RecipeBookLibrary;
        }
        */

        [HttpGet]
        public Recipe Get()
        {
            int id = 25;

            Recipe recipe = recipe_dao.GetRecipe(id);

            return recipe;
        }

        [HttpPost]
        public RecipeBookLibrary Post(RecipeBookLibrary newLibrary)
        {
            this.RecipeBookLibrary = newLibrary;

            return this.RecipeBookLibrary;
        }

        [HttpPut]
        public RecipeBookLibrary Put(RecipeBookLibrary updatedLibrary)
        {
            this.RecipeBookLibrary = updatedLibrary;

            return this.RecipeBookLibrary;
        }
    }
}
