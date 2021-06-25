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

        public RecipeBookLibrary GetRecipeBookLibrary(int recipeBookLibraryId)
        {
            RecipeBookLibrary recipeBookLibrary = new RecipeBookLibrary();
            recipeBookLibrary.Id = -1;

            List<int> recipeBookIds = new List<int>();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectRecipeBookLibrary = "SELECT rbl.id AS 'recipe_book_library_id', rb.id AS 'recipe_book_id' " +
                                                        "FROM recipe_book_library rbl " +
                                                        "JOIN recipe_book rb ON rbl.id = rb.recipe_book_library_id " +
                                                        "JOIN recipe r ON rb.id = r.recipe_book_id " +
                                                        "WHERE recipe_book_library_id = @recipe_book_library_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipeBookLibrary, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_library_id", recipeBookLibraryId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int recipeBookLibraryIdFromDatabase = Convert.ToInt32(reader["recipe_book_library_id"]);
                        recipeBookIds.Add(Convert.ToInt32(reader["recipe_book_id"]));

                        if (recipeBookLibraryIdFromDatabase != 0)
                        {
                            recipeBookLibrary.Id = recipeBookLibraryIdFromDatabase;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            for (int i = 0; i < recipeBookIds.Count; i++)
            {
                RecipeBook recipeBook = GetRecipeBook(recipeBookIds[i]);
                recipeBookLibrary.AddRecipeBook(recipeBook);
            }

            return recipeBookLibrary;
        }

        public RecipeBook GetRecipeBook(int recipeBookId)
        {
            RecipeBook recipeBook = new RecipeBook();
            recipeBook.Id = -1;

            List<int> recipeIds = new List<int>();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectRecipeBook = "SELECT rb.id AS 'recipe_book_id', rb.name AS 'recipe_book_name', r.recipe_number AS 'recipe_number' " +
                                                        "FROM recipe_book rb " +
                                                        "JOIN recipe r ON rb.id = r.recipe_book_id " +
                                                        "WHERE recipe_book_id = @recipe_book_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipeBook, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_id", recipeBookId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int recipeBookIdFromDatabase = Convert.ToInt32(reader["recipe_book_id"]);
                        recipeBook.Name = Convert.ToString(reader["recipe_book_name"]);
                        recipeIds.Add(Convert.ToInt32(reader["recipe_number"]));

                        if (recipeBookIdFromDatabase != 0)
                        {
                            recipeBook.Id = recipeBookIdFromDatabase;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            for (int i = 0; i < recipeIds.Count; i++)
            {
                Recipe recipe = GetRecipe(recipeIds[i]);
                recipeBook.AddRecipe(recipe);
            }

            return recipeBook;
        }

        public Recipe GetRecipe(int recipeId)
        {
            Metadata metadata = GetRecipeMetadata(recipeId);

            IngredientList ingredientList = GetRecipeIngriedients(recipeId);

            CookingInstructions cookingInstructions = GetRecipeCookingInstructions(recipeId);

            Recipe recipe = new Recipe(metadata, cookingInstructions, ingredientList);
            recipe.Id = recipeId;
            recipe.RecipeNumber = GetRecipeNumber(recipeId);

            if (recipe != null && metadata != null && ingredientList != null && cookingInstructions != null && recipe.RecipeNumber != null)
            {
                return recipe;
            }
            else
            {
                return null;
            }
        }

        private int? GetRecipeNumber(int recipeId)
        {
            int? recipeNumber = -1;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectRecipeNumber = "SELECT recipe_number FROM recipe WHERE id = @recipe_id";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipeNumber, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int recipeNumberFromDatabase = Convert.ToInt32(reader["recipe_number"]);

                        if (recipeNumberFromDatabase != 0)
                        {
                            recipeNumber = recipeNumberFromDatabase;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return recipeNumber;
        }

        private Metadata GetRecipeMetadata(int recipeId)
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
                return null;
            }

            return metadata;
        }

        private Times GetRecipeMetadataPrepTimes(int recipeId)
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
                return null;
            }

            return prepTimes;
        }

        private Tags GetRecipeMetadataTags(int recipeId)
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
                return null;
            }

            return tags;
        }

        private Servings GetRecipeMetadataServings(int recipeId)
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
                return null;
            }

            return servings;
        }

        private IngredientList GetRecipeIngriedients(int recipeId)
        {
            IngredientList ingredientList = new IngredientList();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectAllRecipeIngredients = "SELECT il.id AS 'ingredient_list_id', i.id AS 'ingredient_id', i.quantity AS 'quantity', i.measurement_unit AS 'measurement_unit', i.name AS 'name', i.prep_note AS 'prep_note', i.store_location AS 'store_location' " +
                                                                "FROM recipe r " +
                                                                "JOIN ingredient_list il ON r.ingredient_list_id = il.id " +
                                                                "JOIN ingredient i ON il.id = i.ingredient_list_id " +
                                                                "WHERE r.id = @recipe_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectAllRecipeIngredients, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ingredientList.Id = Convert.ToInt32(reader["ingredient_list_id"]);

                        Ingredient ingredient = new Ingredient();
                        ingredient.Id = Convert.ToInt32(reader["ingredient_id"]);
                        ingredient.Quantity = Convert.ToDouble(reader["quantity"]);
                        ingredient.MeasurementUnit = Convert.ToString(reader["measurement_unit"]);
                        ingredient.Name = Convert.ToString(reader["name"]);
                        ingredient.PreparationNote = Convert.ToString(reader["prep_note"]);
                        ingredient.StoreLocation = Convert.ToString(reader["store_location"]);

                        ingredientList.AddIngredient(ingredient);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return ingredientList;
        }

        private CookingInstructions GetRecipeCookingInstructions(int recipeId)
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
                return null;
            }

            foreach (KeyValuePair<int, string> element in instructionBlockIdsAndHeadings)
            {
                InstructionBlock instructionBlock = GetRecipeInstructionBlock(element.Key, element.Value);
                cookingInstructions.AddInstructionBlock(instructionBlock);
            }

            return cookingInstructions;
        }

        private InstructionBlock GetRecipeInstructionBlock(int instructionBlockId, string instructionBlockHeading)
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
                return null;
            }

            return instructionBlock;
        }

        public RecipeBook CreateRecipeBook(RecipeBook recipeBook)
        {
            return null;
        }
        
        public Recipe CreateRecipe(int recipeBookId, Recipe recipe)
        {
            int? metadataIdFromDatabase = CreateMetadata(recipe);
            int? ingredientListIdFromDatabase = CreateIngredientList(recipe);
            int? cookingInstructionsIdFromDatabase = CreateCookingInstructions(recipe);

            if (metadataIdFromDatabase == null || ingredientListIdFromDatabase == null || cookingInstructionsIdFromDatabase == null)
            {
                return null;
            }

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlInsertNewRecipe = "INSERT INTO recipe (recipe_book_id, recipe_number, metadata_id, ingredient_list_id, cooking_instructions_id) " +
                                                "OUTPUT INSERTED.ID " +
                                                "VALUES(@recipe_book_id, @recipe_number, @metadata_id, @ingredient_list_id, @cooking_instructions_id);";
                    SqlCommand sqlCmd = new SqlCommand(sqlInsertNewRecipe, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_id", recipeBookId);
                    sqlCmd.Parameters.AddWithValue("@recipe_number", recipe.RecipeNumber);
                    sqlCmd.Parameters.AddWithValue("@metadata_id", recipe.Metadata.Id);
                    sqlCmd.Parameters.AddWithValue("@ingredient_list_id", recipe.IngredientList.Id);
                    sqlCmd.Parameters.AddWithValue("@cooking_instructions_id", recipe.CookingInstructions.Id);

                    recipe.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return recipe;
        }

        private int? CreateMetadata(Recipe recipe)
        {           
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlInsertNewMetadata = "BEGIN TRANSACTION; " +
                                                    "INSERT INTO times (prep_time, cook_time) VALUES(@prep_time, @cook_time) DECLARE @times_id INT = (SELECT @@IDENTITY); " +
                                                    "INSERT INTO tags(food_type, food_genre) VALUES(@food_type, @food_genre) DECLARE @tags_id INT = (SELECT @@IDENTITY); " +
                                                    "INSERT INTO servings(low_servings, high_servings) VALUES(@low_servings, @high_servings) DECLARE @servings_id INT = (SELECT @@IDENTITY); " +
                                                    "INSERT INTO metadata(title, notes, times_id, tags_id, servings_id) OUTPUT INSERTED.ID VALUES(@title, @notes, @times_id, @tags_id, @servings_id); " +
                                                    "COMMIT;";
                    
                    SqlCommand sqlCmd = new SqlCommand(sqlInsertNewMetadata, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@prep_time", recipe.Metadata.PrepTimes.PrepTime);
                    sqlCmd.Parameters.AddWithValue("@cook_time", recipe.Metadata.PrepTimes.CookTime);
                    sqlCmd.Parameters.AddWithValue("@food_type", recipe.Metadata.Tags.FoodType);
                    sqlCmd.Parameters.AddWithValue("@food_genre", recipe.Metadata.Tags.FoodGenre);
                    sqlCmd.Parameters.AddWithValue("@low_servings", recipe.Metadata.Servings.LowNumberOfServings);
                    sqlCmd.Parameters.AddWithValue("@high_servings", recipe.Metadata.Servings.HighNumberOfServings);
                    sqlCmd.Parameters.AddWithValue("@title", recipe.Metadata.Title);
                    sqlCmd.Parameters.AddWithValue("@notes", recipe.Metadata.Notes);

                    recipe.Metadata.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return recipe.Metadata.Id;
        }

        private int? CreateIngredientList(Recipe recipe)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlInsertNewIngredientList = "INSERT INTO ingredient_list OUTPUT INSERTED.ID DEFAULT VALUES;";
                    SqlCommand sqlCmd = new SqlCommand(sqlInsertNewIngredientList, sqlConn);
                    recipe.IngredientList.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    string sqlInsertIngredient;

                    for (int i = 0; i < recipe.IngredientList.AllIngredients.Count; i++)
                    {
                        sqlInsertIngredient = "INSERT INTO ingredient (ingredient_list_id, quantity, measurement_unit, name, prep_note, store_location) " +
                                                "VALUES(@ingredient_list_id, @quantity, @measurment_unit, @name, @prep_note, @store_location);";
                        sqlCmd = new SqlCommand(sqlInsertIngredient, sqlConn);
                        sqlCmd.Parameters.AddWithValue("@ingredient_list_id", recipe.IngredientList.Id);
                        sqlCmd.Parameters.AddWithValue("@quantity", recipe.IngredientList.AllIngredients[i].Quantity);
                        sqlCmd.Parameters.AddWithValue("@measurment_unit", recipe.IngredientList.AllIngredients[i].MeasurementUnit);
                        sqlCmd.Parameters.AddWithValue("@name", recipe.IngredientList.AllIngredients[i].Name);
                        sqlCmd.Parameters.AddWithValue("@prep_note", recipe.IngredientList.AllIngredients[i].PreparationNote);
                        sqlCmd.Parameters.AddWithValue("@store_location", recipe.IngredientList.AllIngredients[i].StoreLocation);

                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return recipe.IngredientList.Id;
        }

        public int? CreateCookingInstructions(Recipe recipe)
        {           
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlInsertNewCookingInstructions = "INSERT INTO cooking_instructions OUTPUT INSERTED.ID DEFAULT VALUES;";
                    SqlCommand sqlCmd = new SqlCommand(sqlInsertNewCookingInstructions, sqlConn);
                    
                    recipe.CookingInstructions.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    for (int i = 0; i < recipe.CookingInstructions.InstructionBlocks.Count; i++)
                    {
                        CreateInstructionBlock(recipe, i);

                        for (int j = 0; j < recipe.CookingInstructions.InstructionBlocks[i].InstructionLines.Count; j++)
                        {
                            CreateInstructionLine(recipe, i, j);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return recipe.CookingInstructions.Id;
        }

        private int? CreateInstructionBlock(Recipe recipe, int instructionBlockIndex)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlInsertInstructionBlock = "INSERT INTO instruction_block (cooking_instructions_id, block_heading) OUTPUT INSERTED.ID VALUES(@cooking_instructions_id, @block_heading);";
                    
                    SqlCommand sqlCmd = new SqlCommand(sqlInsertInstructionBlock, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@cooking_instructions_id", recipe.CookingInstructions.Id);
                    sqlCmd.Parameters.AddWithValue("@block_heading", recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex].BlockHeading);

                    recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex].Id = Convert.ToInt32(sqlCmd.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex].Id;
        }

        private bool? CreateInstructionLine(Recipe recipe, int instructionBlockIndex, int instructionLineIndex)
        {
            bool isSuccessful = false;
            
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    //Create the new instruction in the Database
                    string sqlInsertInstructionLine = "INSERT INTO instruction (text) OUTPUT INSERTED.ID VALUES (@text);";
                    
                    SqlCommand sqlCmd = new SqlCommand(sqlInsertInstructionLine, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@text", recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex].InstructionLines[instructionLineIndex]);
                    
                    int instructionId = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    //Create the new entry in the instruction_block_instruction associative table to link the new instruction to it's instruction block
                    string sqlInsertIntoInstructionBlockInstruction = "INSERT INTO instruction_block_instruction (block_id, instruction_id) VALUES (@block_id, @instruction_id);";
                    
                    sqlCmd = new SqlCommand(sqlInsertIntoInstructionBlockInstruction, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@block_id", recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex].Id);
                    sqlCmd.Parameters.AddWithValue("@instruction_id", instructionId);

                    sqlCmd.ExecuteNonQuery();

                    isSuccessful = true;
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return isSuccessful;
        }
    }
}
