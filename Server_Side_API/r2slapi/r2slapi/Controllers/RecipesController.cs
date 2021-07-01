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

        [HttpGet]
        public ActionResult<RecipeBookLibrary> GetRecipeBookLibrary()
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

        [HttpPut("{recipeBookId}")]
        public ActionResult UpdateRecipeBook(int recipeBookId, RecipeBook recipeBook)
        {
            RecipeBook recipeBookToUpdate = dao.GetRecipeBook(recipeBookId);

            if (recipeBookToUpdate == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else if (recipeBookToUpdate.Id == -1)
            {
                return NotFound();
            }
            else
            {
                bool? recipeBookUpdated = dao.UpdateRecipeBook(recipeBookId, recipeBook);

                if (recipeBookUpdated == null)
                {
                    return StatusCode(500, "Error: Unable to complete request. Please try again later.");
                }
                else
                {
                    return NoContent();
                }
            }
        }

        [HttpPut("{recipeBookId}/{recipeId}")]
        public ActionResult UpdateRecipe(int recipeBookId, int recipeId, Recipe recipe)
        {
            Recipe recipeToUpdate = dao.GetRecipe(recipeId);

            if (recipeToUpdate == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else if (recipeToUpdate.RecipeNumber == -1)
            {
                return NotFound();
            }
            else
            {
                bool? recipeUpdated = dao.UpdateRecipe(recipeBookId, recipeId, recipe);

                if (recipeUpdated == null)
                {
                    return StatusCode(500, "Error: Unable to complete request. Please try again later.");
                }
                else
                {
                    return NoContent();
                }
            }
        }

        [HttpDelete("{recipeBookId}")]
        public ActionResult DeleteRecipeBook(int recipeBookId)
        {
            RecipeBook recipeBookToDelete = dao.GetRecipeBook(recipeBookId);

            if (recipeBookToDelete == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else if (recipeBookToDelete.Id == -1)
            {
                return NotFound();
            }
            else
            {
                bool? recipeBookDeleted = dao.DeleteRecipeBook(recipeBookId);

                if (recipeBookDeleted == null)
                {
                    return StatusCode(500, "Error: Unable to complete request. Please try again later.");
                }
                else
                {
                    return NoContent();
                }
            }
        }

        [HttpDelete("{recipeBookId}/{recipeId}")]
        public ActionResult DeleteRecipe(int recipeBookId, int recipeId)
        {
            Recipe recipeToDelete = dao.GetRecipe(recipeId);

            if (recipeToDelete == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else if (recipeToDelete.RecipeNumber == -1)
            {
                return NotFound();
            }
            else
            {
                bool? recipeDeleted = dao.DeleteRecipe(recipeBookId, recipeId);

                if (recipeDeleted == null)
                {
                    return StatusCode(500, "Error: Unable to complete request. Please try again later.");
                }
                else
                {
                    return NoContent();
                }
            }
        }
    }
}
