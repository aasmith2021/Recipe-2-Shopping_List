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

        //GETS THE ENTIRE RECIPE BOOK LIBRARY
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

        //GETS A RECIPE BOOK
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

        //GETS A RECIPE FROM A SPECIFIED RECIPE BOOK
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

        //CREATES A NEW RECIPE BOOK IN THE MASTER RECIPE BOOK LIBRARY
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

        //CREATES A NEW RECIPE IN AN EXISTING RECIPE BOOK
        [HttpPost("{recipeBookId}")]
        public ActionResult<Recipe> CreateRecipe(int recipeBookId, Recipe recipe)
        {
            //Checks to see if the recipe book to add the new recipe to exists
            RecipeBook recipeBook = dao.GetRecipeBook(recipeBookId);

            if (recipeBook == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else if (recipeBook.Id == -1)
            {
                return NotFound();
            }

            //Creates the new recipe in the database in the provided recipe book
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

        //UPDATES ENTIRE RECIPE BOOK LIBRARY
        [HttpPut]
        public ActionResult UpdateRecipeBookLibrary(RecipeBookLibrary updatedRecipeBookLibrary)
        {
            //Checks to see if the recipe book library to update exists in the database. If so, the recipe book library is updated
            RecipeBookLibrary currentRecipeBookLibrary = dao.GetRecipeBookLibrary(RECIPE_BOOK_LIBRARY_ID);

            if (currentRecipeBookLibrary == null)
            {
                return StatusCode(500, "Error: Unable to complete request. Please try again later.");
            }
            else if (currentRecipeBookLibrary.Id == -1)
            {
                return NotFound();
            }
            else
            {
                bool? recipeBookLibraryUpdated = dao.UpdateRecipeBookLibrary(RECIPE_BOOK_LIBRARY_ID, updatedRecipeBookLibrary);

                if (recipeBookLibraryUpdated == null)
                {
                    return StatusCode(500, "Error: Unable to complete request. Please try again later.");
                }
                else if (recipeBookLibraryUpdated == false)
                {
                    return StatusCode(500, "Error: An error occurred while processing your request, and all of the Recipe Books were not updated. Please try again later.");
                }
                else
                {
                    return NoContent();
                }
            }
        }

        //UPDATES RECIPE BOOK
        [HttpPut("{recipeBookId}")]
        public ActionResult UpdateRecipeBook(int recipeBookId, RecipeBook recipeBook)
        {
            //Checks to see if the recipe book to update exists in the database. If so, the recipe book is updated
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
                else if (recipeBookUpdated == false)
                {
                    return StatusCode(500, "Error: Unable to update all recipes in this recipe book. Please try again later.");
                }
                else
                {
                    return NoContent();
                }
            }
        }
        
        //UPDATES RECIPE IN SPECIFIED RECIPE BOOK
        [HttpPut("{recipeBookId}/{recipeId}")]
        public ActionResult UpdateRecipe(int recipeBookId, int recipeId, Recipe recipe)
        {
            //Checks to see if the recipe to update exists in the database. If so, the recipe is updated
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

        //DELETES RECIPE BOOK
        [HttpDelete("{recipeBookId}")]
        public ActionResult DeleteRecipeBook(int recipeBookId)
        {
            //Checks to see if the recipe book to delete exists in the database. If so, the recipe book is deleted
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

        //DELETES A RECIPE IN A SPECIFIED RECIPE BOOK
        [HttpDelete("{recipeBookId}/{recipeId}")]
        public ActionResult DeleteRecipe(int recipeBookId, int recipeId)
        {
            //Checks to see if the recipe to delete exists in the database. If so, the recipe is deleted
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
