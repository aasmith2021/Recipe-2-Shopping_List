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
        private readonly IRecipeDao dao;
        private const int RECIPE_BOOK_LIBRARY_ID = 1;

        public RecipesController(IRecipeDao _dao)
        {
            dao = _dao;
        }


        [HttpGet()]
        public ActionResult<RecipeBookLibrary> Get()
        {
            RecipeBookLibrary recipeBookLibrary = dao.GetRecipeBookLibrary(RECIPE_BOOK_LIBRARY_ID);

            if (recipeBookLibrary == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else if (recipeBookLibrary.Id == -1)
            {
                return NotFound();
            }
            else
            {
                return Ok(recipeBookLibrary);
            }
        }

        [HttpGet("{recipeBookId}")]
        public ActionResult<RecipeBook> GetRecipeBook(int recipeBookId)
        {
            RecipeBook recipeBook = dao.GetRecipeBook(recipeBookId);

            if (recipeBook == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else if (recipeBook.Id == -1)
            {
                return NotFound();
            }
            else
            {
                return Ok(recipeBook);
            }
        }

        [HttpGet("{recipeBookId}/{recipeId}")]
        public ActionResult<Recipe> GetRecipe(int recipeId)
        {
            Recipe recipe = dao.GetRecipe(recipeId);

            if (recipe == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else if (recipe.RecipeNumber == -1)
            {
                return NotFound();
            }
            else
            {
                return Ok(recipe);
            }
        }

        [HttpPost]
        public ActionResult<RecipeBook> CreateRecipeBook(RecipeBook recipeBook, int recipeBookLibraryId = RECIPE_BOOK_LIBRARY_ID)
        {           
            RecipeBook newRecipeBook = dao.CreateRecipeBook(recipeBookLibraryId, recipeBook);

            if (newRecipeBook == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else
            {
                return Created($"/recipes/{newRecipeBook.Id}", newRecipeBook);
            }
        }

        [HttpPost("{recipeBookId}")]
        public ActionResult<Recipe> CreateRecipe(int recipeBookId, Recipe recipe)
        {
            Recipe newRecipe = dao.CreateRecipe(recipeBookId, recipe);

            if (newRecipe == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else
            {
                return Created($"/recipes/{recipeBookId}/{newRecipe.Id}", newRecipe);
            }
        }

        [HttpPut("{recipeBookId}/{recipeId}")]
        public ActionResult<Recipe> Put(int recipeBookId, int recipeId, Recipe recipe)
        {
            Recipe recipeToUpdate = dao.GetRecipe(recipeId);

            if (recipe == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else if (recipe.RecipeNumber == -1)
            {
                return NotFound();
            }
            else
            {
                Recipe updatedRecipe = dao.UpdateRecipe(recipeBookId, recipeId, recipe);

                if (updatedRecipe == null)
                {
                    return StatusCode(500, "Error: Unable to complete request. Please try again later.");
                }
                else
                {
                    return Ok(updatedRecipe);
                }
            }
        }
    }
}
