using r2slapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace r2slapi.DAO
{
    public class RecipeDao : IRecipeDao
    {
        private readonly string connectionString = "Server=.\\SQLEXPRESS;Database=RecipeDB;Trusted_Connection=True;";
        
        public RecipeDao()
        {
            
        }
        
        /*
        public void GetLibrary(int libraryId)
        {
            RecipeBookLibrary recipeBookLibrary = new RecipeBookLibrary();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectAllRecipesForRecipeBook = "";
                    //SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipeBookLibrary, sqlConn);
                    //sqlCmd.Parameters.AddWithValue("@library_id", libraryId);

                    //SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
            }
        }

        public RecipeBookLibrary CreateNewLibrary()
        {
            throw new NotImplementedException();
        }

        public void UpdateLibrary(RecipeBookLibrary libraryToUpdate)
        {
            throw new NotImplementedException();
        }
        */

        public Recipe GetRecipe(int recipeId)
        {
            Metadata metadata = GetRecipeMetadata(recipeId);

            IngredientList ingredients = GetRecipeIngriedients(recipeId);

            CookingInstructions cookingInstructions = GetRecipeCookingInstructions(recipeId);

            Recipe recipe = new Recipe(metadata, cookingInstructions, ingredients);
            recipe.Id = recipeId;
            recipe.RecipeNumber = GetRecipeNumber(recipeId);

            return recipe;
        }

        public int GetRecipeNumber(int recipeId)
        {
            int recipeNumber = 0;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectRecipeMetadata = "SELECT recipe_number FROM recipe WHERE id = @recipe_id";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipeMetadata, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        recipeNumber = Convert.ToInt32(reader["recipe_number"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
            }

            return recipeNumber;
        }

        public Metadata GetRecipeMetadata(int recipeId)
        {
            Metadata metadata = new Metadata();
            metadata.PrepTimes = GetRecipeMetadataPrepTimes(recipeId);
            metadata.Tags = GetRecipeMetadataTags(recipeId);
            metadata.Servings = GetRecipeMetadataServings(recipeId);

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectRecipeMetadata =    "SELECT m.id AS 'metadata_id', m.title AS 'title', m.notes AS 'notes' " +
                                                        "FROM recipe r " +
                                                        "JOIN metadata m ON r.metadata_id = m.id " +
                                                        "WHERE r.id = @recipe_id";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipeMetadata, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        metadata.Id = Convert.ToInt32(reader["metadata_id"]);
                        metadata.Title = Convert.ToString(reader["title"]);
                        metadata.Notes = Convert.ToString(reader["notes"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
            }

            return metadata;
        }

        public Times GetRecipeMetadataPrepTimes(int recipeId)
        {
            Times prepTimes = new Times();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectRecipePrepTimes = "SELECT tms.id AS 'times_id', tms.prep_time AS 'prep_time', tms.cook_time AS 'cook_time'" +
                                                        "FROM recipe r " +
                                                        "JOIN metadata m ON r.metadata_id = m.id " +
                                                        "JOIN times tms ON m.times_id = tms.id " +
                                                        "WHERE r.id = @recipe_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipePrepTimes, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        prepTimes.Id = Convert.ToInt32(reader["times_id"]);
                        prepTimes.PrepTime = Convert.ToInt32(reader["prep_time"]);
                        prepTimes.CookTime = Convert.ToInt32(reader["cook_time"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
            }

            return prepTimes;
        }

        public Tags GetRecipeMetadataTags(int recipeId)
        {
            Tags tags = new Tags();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectRecipeTags =    "SELECT tgs.id AS 'tags_id', tgs.food_type AS 'food_type', tgs.food_genre AS 'food_genre'" +
                                                    "FROM recipe r " +
                                                    "JOIN metadata m ON r.metadata_id = m.id " +
                                                    "JOIN tags tgs ON m.tags_id = tgs.id " +
                                                    "WHERE r.id = @recipe_id";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipeTags, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        tags.Id = Convert.ToInt32(reader["tags_id"]);
                        tags.FoodType = Convert.ToString(reader["food_type"]);
                        tags.FoodGenre = Convert.ToString(reader["food_genre"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
            }

            return tags;
        }

        public Servings GetRecipeMetadataServings(int recipeId)
        {
            Servings servings = new Servings();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectRecipeServings = "SELECT svgs.id AS 'servings_id', svgs.low_servings AS 'low_servings', svgs.high_servings AS 'high_servings'" +
                                                    "FROM recipe r " +
                                                    "JOIN metadata m ON r.metadata_id = m.id " +
                                                    "JOIN servings svgs ON m.servings_id = svgs.id " +
                                                    "WHERE r.id = @recipe_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipeServings, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        servings.Id = Convert.ToInt32(reader["servings_id"]);
                        servings.LowNumberOfServings = Convert.ToInt32(reader["low_servings"]);
                        servings.HighNumberOfServings = Convert.ToInt32(reader["high_servings"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
            }

            return servings;
        }

        public IngredientList GetRecipeIngriedients(int recipeId)
        {
            IngredientList ingredients = new IngredientList();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectAllRecipeIngredients = "SELECT il.id AS 'ingredients_id', i.id AS 'ingredient_id', i.quantity AS 'quantity', i.measurement_unit AS 'measurement_unit', i.name AS 'name', i.prep_note AS 'prep_note', i.store_location AS 'store_location' " +
                                                                "FROM recipe r " +
                                                                "JOIN ingredient_list il ON r.ingredient_list_id = il.id " +
                                                                "JOIN ingredient i ON il.id = i.ingredient_list_id " +
                                                                "WHERE r.id = @recipe_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectAllRecipeIngredients, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ingredients.Id = Convert.ToInt32(reader["ingredients_id"]);

                        Ingredient ingredient = new Ingredient();
                        ingredient.Id = Convert.ToInt32(reader["ingredient_id"]);
                        ingredient.Quantity = Convert.ToDouble(reader["quantity"]);
                        ingredient.MeasurementUnit = Convert.ToString(reader["measurement_unit"]);
                        ingredient.Name = Convert.ToString(reader["name"]);
                        ingredient.PreparationNote = Convert.ToString(reader["prep_note"]);
                        ingredient.StoreLocation = Convert.ToString(reader["store_location"]);

                        ingredients.AddIngredient(ingredient);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
            }

            return ingredients;
        }

        public CookingInstructions GetRecipeCookingInstructions(int recipeId)
        {
            CookingInstructions cookingInstructions = new CookingInstructions();
            SortedList<int, string> instructionBlockIdsAndHeadings = new SortedList<int, string>();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectCookingInstructions = "SELECT ci.id AS 'cooking_instructions_id', ib.id AS 'instruction_block_id', ib.block_heading AS 'instruction_block_heading' " +
                                                            "FROM recipe r " +
                                                            "JOIN cooking_instructions ci ON r.cooking_instructions_id = ci.id " +
                                                            "JOIN instruction_block ib ON ci.id = ib.cooking_instructions_id " +
                                                            "WHERE r.id = @recipe_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectCookingInstructions, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cookingInstructions.Id = Convert.ToInt32(reader["cooking_instructions_id"]);
                        int instructionBlockId = Convert.ToInt32(reader["instruction_block_id"]);

                        if (!instructionBlockIdsAndHeadings.ContainsKey(instructionBlockId))
                        {
                            instructionBlockIdsAndHeadings[instructionBlockId] = Convert.ToString(reader["instruction_block_heading"]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
            }

            foreach (KeyValuePair<int, string> element in instructionBlockIdsAndHeadings)
            {
                InstructionBlock instructionBlock = GetRecipeInstructionBlock(element.Key, element.Value);
                cookingInstructions.AddInstructionBlock(instructionBlock);
            }

            return cookingInstructions;
        }

        public InstructionBlock GetRecipeInstructionBlock(int instructionBlockId, string instructionBlockHeading)
        {
            InstructionBlock instructionBlock = new InstructionBlock();
            instructionBlock.Id = instructionBlockId;
            instructionBlock.BlockHeading = instructionBlockHeading;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectInstructionBlocks = "SELECT text " +
                                                        "FROM instruction_block ib " +
                                                        "JOIN instruction_block_instruction ibi ON ib.id = ibi.block_id " +
                                                        "JOIN instruction i ON ibi.instruction_id = i.id " +
                                                        "WHERE ib.id = @instruction_block_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectInstructionBlocks, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@instruction_block_id", instructionBlockId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        instructionBlock.AddInstructionLine(Convert.ToString(reader["text"]));
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
            }

            return instructionBlock;
        }
    }
}
