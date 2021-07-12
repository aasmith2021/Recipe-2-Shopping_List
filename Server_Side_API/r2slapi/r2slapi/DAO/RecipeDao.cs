using r2slapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace r2slapi.DAO
{
    public class RecipeDao : IRecipeDao
    {
        private readonly string connectionString;

        public RecipeDao()
        {
            connectionString = GetConnectionString();
        }

        private string GetConnectionString()
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            return configuration.GetConnectionString("RecipeDB");
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

                    //Check to see if recipe book library is in the database. If it is, get the Id and LastSaved values
                    string sqlSelectRecipeBookLibaray = "SELECT rbl_id, last_saved FROM recipe_book_library WHERE rbl_id = @recipe_book_library_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipeBookLibaray, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_library_id", recipeBookLibraryId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    int? recipeBookLibraryIdFromDatabase = 0;

                    while (reader.Read())
                    {
                        recipeBookLibraryIdFromDatabase = Convert.ToInt32(reader["rbl_id"]);
                        recipeBookLibrary.LastSaved = Convert.ToDateTime(reader["last_saved"]);
                    }

                    reader.Close();


                    //If the recipe book library exists in the database, get the custom measurement units and get all
                    //the recipe book ids for the recipe books in that recipe book library
                    if (recipeBookLibraryIdFromDatabase != null && recipeBookLibraryIdFromDatabase != 0)
                    {
                        recipeBookLibrary.Id = recipeBookLibraryIdFromDatabase;

                        GetCustomMeasurementUnits(recipeBookLibrary);

                        string sqlSelectRecipeBookLibraryRecipes = "SELECT rb_id AS 'recipe_book_id'" +
                                                                    "FROM recipe_book rbl " +
                                                                    "WHERE recipe_book_library_id = @recipe_book_library_id;";

                        sqlCmd = new SqlCommand(sqlSelectRecipeBookLibraryRecipes, sqlConn);
                        sqlCmd.Parameters.AddWithValue("@recipe_book_library_id", recipeBookLibraryId);

                        reader = sqlCmd.ExecuteReader();

                        //Select all recipe books for current recipe book library
                        while (reader.Read())
                        {
                            recipeBookIds.Add(Convert.ToInt32(reader["recipe_book_id"]));
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

        public bool? GetCustomMeasurementUnits(RecipeBookLibrary recipeBookLibrary)
        {
            bool? customMeasurementUnitsAdded = false;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectCustomMeasurementUnits = "SELECT measurement_unit_name FROM custom_measurement_units WHERE recipe_book_library_id = @recipe_book_library_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectCustomMeasurementUnits, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_library_id", recipeBookLibrary.Id);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string measurementUnit = Convert.ToString(reader["measurement_unit_name"]);

                        recipeBookLibrary.AddMeasurementUnit(measurementUnit);
                    }

                    customMeasurementUnitsAdded = true;
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return customMeasurementUnitsAdded;
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


                    //First, the code is getting the recipe book's id and name from the database
                    string sqlSelectRecipeBookInformation = "SELECT rb_id AS 'recipe_book_id', name AS 'recipe_book_name' FROM recipe_book WHERE rb_id = @recipe_book_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipeBookInformation, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_id", recipeBookId);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int recipeBookIdFromDatabase = Convert.ToInt32(reader["recipe_book_id"]);
                        recipeBook.Name = Convert.ToString(reader["recipe_book_name"]);

                        if (recipeBookIdFromDatabase != 0)
                        {
                            recipeBook.Id = recipeBookIdFromDatabase;
                        }
                    }

                    reader.Close();

                    //Second, the code is getting all of the recipe IDs for the recipe book from the database
                    string sqlSelectRecipeBookRecipes = "SELECT r.r_id AS 'recipe_id' " +
                                                        "FROM recipe_book rb " +
                                                        "JOIN recipe r ON rb.rb_id = r.recipe_book_id " +
                                                        "WHERE recipe_book_id = @recipe_book_id;";

                    sqlCmd = new SqlCommand(sqlSelectRecipeBookRecipes, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_id", recipeBookId);

                    reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        recipeIds.Add(Convert.ToInt32(reader["recipe_id"]));


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
            recipe.Id = -1;

            int? recipeIdFromDatabase = GetRecipeId(recipeId);
            int? recipeNumberFromDatabase = GetRecipeNumber(recipeId);

            if (recipeNumberFromDatabase > 0)
            {
                recipe.RecipeNumber = recipeNumberFromDatabase;
            }

            if (recipeIdFromDatabase > 0)
            {
                int result;
                int.TryParse(recipeIdFromDatabase.ToString(), out result);

                recipe.Id = result;
            }

            if (recipe != null && metadata != null && ingredientList != null && cookingInstructions != null)
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

                    string sqlSelectRecipeNumber = "SELECT recipe_number FROM recipe WHERE r_id = @recipe_id;";

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

        private int? GetRecipeId(int recipeId)
        {
            int? recipeIdToReturn = -1;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectRecipeNumber = "SELECT r_id FROM recipe WHERE r_id = @recipe_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectRecipeNumber, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);

                    int? recipeIdFromDatabase = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    if (recipeIdFromDatabase > 0)
                    {
                        recipeIdToReturn = recipeIdFromDatabase;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return recipeIdToReturn;
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

                    string sqlSelectRecipeMetadata = "SELECT m.m_id AS 'metadata_id', m.title AS 'title', m.notes AS 'notes' " +
                                                        "FROM recipe r " +
                                                        "JOIN metadata m ON r.metadata_id = m.m_id " +
                                                        "WHERE r.r_id = @recipe_id";

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

                    string sqlSelectRecipePrepTimes = "SELECT tms.tms_id AS 'times_id', tms.prep_time AS 'prep_time', tms.cook_time AS 'cook_time'" +
                                                        "FROM recipe r " +
                                                        "JOIN metadata m ON r.metadata_id = m.m_id " +
                                                        "JOIN times tms ON m.times_id = tms.tms_id " +
                                                        "WHERE r.r_id = @recipe_id;";

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

                    string sqlSelectRecipeTags = "SELECT tgs.tgs_id AS 'tags_id', tgs.food_type AS 'food_type', tgs.food_genre AS 'food_genre'" +
                                                    "FROM recipe r " +
                                                    "JOIN metadata m ON r.metadata_id = m.m_id " +
                                                    "JOIN tags tgs ON m.tags_id = tgs.tgs_id " +
                                                    "WHERE r.r_id = @recipe_id";

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

                    string sqlSelectRecipeServings = "SELECT svgs.svgs_id AS 'servings_id', svgs.low_servings AS 'low_servings', svgs.high_servings AS 'high_servings'" +
                                                    "FROM recipe r " +
                                                    "JOIN metadata m ON r.metadata_id = m.m_id " +
                                                    "JOIN servings svgs ON m.servings_id = svgs.svgs_id " +
                                                    "WHERE r.r_id = @recipe_id;";

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

                    string sqlSelectAllRecipeIngredients = "SELECT il.il_id AS 'ingredient_list_id', i.i_id AS 'ingredient_id', i.quantity AS 'quantity', i.measurement_unit AS 'measurement_unit', i.name AS 'name', i.prep_note AS 'prep_note', i.store_location AS 'store_location' " +
                                                                "FROM recipe r " +
                                                                "JOIN ingredient_list il ON r.ingredient_list_id = il.il_id " +
                                                                "JOIN ingredient i ON il.il_id = i.ingredient_list_id " +
                                                                "WHERE r.r_id = @recipe_id;";

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

                    string sqlSelectCookingInstructions = "SELECT ci.ci_id AS 'cooking_instructions_id', ib.ib_id AS 'instruction_block_id', ib.block_heading AS 'instruction_block_heading' " +
                                                            "FROM recipe r " +
                                                            "JOIN cooking_instructions ci ON r.cooking_instructions_id = ci.ci_id " +
                                                            "JOIN instruction_block ib ON ci.ci_id = ib.cooking_instructions_id " +
                                                            "WHERE r.r_id = @recipe_id;";

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
                                                        "JOIN instruction_block_instruction ibi ON ib.ib_id = ibi.instruction_block_id " +
                                                        "JOIN instruction i ON ibi.instruction_id = i.inst_id " +
                                                        "WHERE ib.ib_id = @instruction_block_id;";

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

        public RecipeBook CreateRecipeBook(int recipeBookLibraryId, RecipeBook recipeBook)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlInsertRecipeBook = "INSERT INTO recipe_book (name, recipe_book_library_id) OUTPUT INSERTED.RB_ID VALUES (@recipe_book_name, @recipe_book_library_id);";
                    SqlCommand sqlCmd = new SqlCommand(sqlInsertRecipeBook, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_name", recipeBook.Name);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_library_id", recipeBookLibraryId);

                    recipeBook.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    for (int i = 0; i < recipeBook.Recipes.Count; i++)
                    {
                        CreateRecipe(recipeBook.Id, recipeBook.Recipes[i]);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return recipeBook;
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
                                                "OUTPUT INSERTED.R_ID " +
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
                                                    "INSERT INTO metadata(title, notes, times_id, tags_id, servings_id) OUTPUT INSERTED.M_ID VALUES(@title, @notes, @times_id, @tags_id, @servings_id); " +
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

                    string sqlInsertNewIngredientList = "INSERT INTO ingredient_list OUTPUT INSERTED.IL_ID DEFAULT VALUES;";
                    SqlCommand sqlCmd = new SqlCommand(sqlInsertNewIngredientList, sqlConn);
                    recipe.IngredientList.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    for (int i = 0; i < recipe.IngredientList.AllIngredients.Count; i++)
                    {
                        CreateIngredient(recipe.IngredientList.Id, recipe.IngredientList.AllIngredients[i]);
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

        private bool? CreateIngredient(int ingredientListId, Ingredient ingredient)
        {
            bool? ingredientSucessfullyCreated = false;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlInsertIngredient = "INSERT INTO ingredient (ingredient_list_id, quantity, measurement_unit, name, prep_note, store_location) " +
                                                 "VALUES(@ingredient_list_id, @quantity, @measurment_unit, @name, @prep_note, @store_location);";
                    SqlCommand sqlCmd = new SqlCommand(sqlInsertIngredient, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@ingredient_list_id", ingredientListId);
                    sqlCmd.Parameters.AddWithValue("@quantity", ingredient.Quantity);
                    sqlCmd.Parameters.AddWithValue("@measurment_unit", ingredient.MeasurementUnit);
                    sqlCmd.Parameters.AddWithValue("@name", ingredient.Name);
                    sqlCmd.Parameters.AddWithValue("@prep_note", ingredient.PreparationNote);
                    sqlCmd.Parameters.AddWithValue("@store_location", ingredient.StoreLocation);

                    sqlCmd.ExecuteNonQuery();
                }

                ingredientSucessfullyCreated = true;
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return ingredientSucessfullyCreated;
        }

        private int? CreateCookingInstructions(Recipe recipe)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlInsertNewCookingInstructions = "INSERT INTO cooking_instructions OUTPUT INSERTED.CI_ID DEFAULT VALUES;";
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

                    string sqlInsertInstructionBlock = "INSERT INTO instruction_block (cooking_instructions_id, block_heading) OUTPUT INSERTED.IB_ID VALUES(@cooking_instructions_id, @block_heading);";

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
                    string sqlInsertInstructionLine = "INSERT INTO instruction (text) OUTPUT INSERTED.INST_ID VALUES (@text);";

                    SqlCommand sqlCmd = new SqlCommand(sqlInsertInstructionLine, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@text", recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex].InstructionLines[instructionLineIndex]);

                    int instructionId = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    //Create the new entry in the instruction_block_instruction associative table to link the new instruction to its instruction block
                    string sqlInsertIntoInstructionBlockInstruction = "INSERT INTO instruction_block_instruction (instruction_block_id, instruction_id) VALUES (@instruction_block_id, @instruction_id);";

                    sqlCmd = new SqlCommand(sqlInsertIntoInstructionBlockInstruction, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@instruction_block_id", recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex].Id);
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

        public bool? UpdateRecipeBookLibrary(int recipeBookLibraryId, RecipeBookLibrary recipeBookLibrary)
        {
            bool allRecipeBooksUpdated = false;
            int recipeBookUpdateCount = 0;
            int numberOfRecipeBooksToUpdate = recipeBookLibrary.AllRecipeBooks.Count;

            //Loops through all the recipe books in the recipe book library and updates each one
            for (int i = 0; i < numberOfRecipeBooksToUpdate; i++)
            {
                bool? recipeBookUpdated = false;

                RecipeBook recipeBook = recipeBookLibrary.AllRecipeBooks[i];

                RecipeBook recipeBookInDatabase = GetRecipeBook(recipeBook.Id);

                if (recipeBookInDatabase == null)
                {
                    return null;
                }
                else if (recipeBookInDatabase.Id == -1)
                {
                    RecipeBook createdRecipeBook = CreateRecipeBook(recipeBookLibraryId, recipeBook);

                    //If the recipe book to add was created without an error, set recipeBookUpdated to true
                    if (createdRecipeBook != null)
                    {
                        recipeBookUpdated = true;
                    }
                }
                else
                {
                    recipeBookUpdated = UpdateRecipeBook(recipeBook.Id, recipeBook);
                }

                if (recipeBookUpdated == null)
                {
                    return null;
                }
                else if (recipeBookUpdated == true)
                {
                    recipeBookUpdateCount++;
                }
            }

            //Validates that every recipe book in the recipe book library was updated
            if (numberOfRecipeBooksToUpdate == recipeBookUpdateCount)
            {
                allRecipeBooksUpdated = true;
            }

            //Update the recipe book library's custom measurement units
            bool customMeasurementUnitsUpdated = UpdateCustomMeasurementUnits(recipeBookLibrary);

            //Updates the last_saved time of the RecipeBookLibrary in the database
            bool lastSavedTimeUpdated = false;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlUpdateLastSaved = "UPDATE recipe_book_library SET last_saved = CURRENT_TIMESTAMP WHERE rbl_id = @recipe_book_library_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlUpdateLastSaved, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_library_id", recipeBookLibraryId);

                    sqlCmd.ExecuteNonQuery();

                    lastSavedTimeUpdated = true;
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return (allRecipeBooksUpdated && customMeasurementUnitsUpdated && lastSavedTimeUpdated);
        }

        public bool UpdateCustomMeasurementUnits(RecipeBookLibrary recipeBookLibrary)
        {
            bool customMeasurementUnitsUpdated = false;

            string[] allMeasurementUnits = recipeBookLibrary.AllMeasurementUnits.ToArray();
            string[] allStandardMeasurementUnits = MeasurementUnits.AllStandardMeasurementUnits().ToArray();
            string[] allCustomMeasurementUnits = new string[allMeasurementUnits.Length - allStandardMeasurementUnits.Length];

            int measurementUnitUpdateCounter = 0;
            int measurementUnitsToUpdate = allCustomMeasurementUnits.Length;

            //Gets all the custom measurement units from the recipeBookLibrary and put them in the allCustomMeasurementUnits array
            for (int i = 0; i < allCustomMeasurementUnits.Length; i++)
            {
                allCustomMeasurementUnits[i] = allMeasurementUnits[allStandardMeasurementUnits.Length + i];
            }

            //Removes all custom measurement units currently in the database
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    //Deletes all measurement units in the database and replaces 
                    string sqlSelectCustomMeasurementUnit = "DELETE FROM custom_measurement_units WHERE recipe_book_library_id = @recipe_book_library_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectCustomMeasurementUnit, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_library_id", recipeBookLibrary.Id);

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                customMeasurementUnitsUpdated = false;
                return customMeasurementUnitsUpdated;
            }

            //Update each measurementUnit in allCustomMeasurementUnits
            foreach (string measurementUnit in allCustomMeasurementUnits)
            {
                bool measurementUnitUpdated = UpdateCustomMeasurementUnit(recipeBookLibrary.Id, measurementUnit);

                if (measurementUnitUpdated)
                {
                    measurementUnitUpdateCounter++;
                }
            }

            //Verify that all of the measurement units were updated
            if (measurementUnitUpdateCounter == measurementUnitsToUpdate)
            {
                customMeasurementUnitsUpdated = true;
            }

            return customMeasurementUnitsUpdated;
        }

        public bool UpdateCustomMeasurementUnit(int? recipeBookLibraryId, string measurementUnitName)
        {
            bool customMeasurementUnitUpdated = false;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    //Inserts the custom measurement unit into the database

                    string sqlInsertCustomMeasurementUnit = "INSERT INTO custom_measurement_units (recipe_book_library_id, measurement_unit_name) VALUES (@recipe_book_library_id, @measurement_unit_name);";

                    SqlCommand sqlCmd = new SqlCommand(sqlInsertCustomMeasurementUnit, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_library_id", recipeBookLibraryId);
                    sqlCmd.Parameters.AddWithValue("@measurement_unit_name", measurementUnitName);

                    sqlCmd.ExecuteNonQuery();

                    customMeasurementUnitUpdated = true;
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                customMeasurementUnitUpdated = false;
            }

            return customMeasurementUnitUpdated;
        }

        public bool? UpdateRecipeBook(int recipeBookId, RecipeBook recipeBook)
        {
            bool recipeBookUpdated = false;
            bool allRecipesUpdated = false;
            int recipeUpdateCount = 0;
            int numberOfRecipesToUpdate = recipeBook.Recipes.Count;

            //Loops through all the recipes in the recipe book and, if a recipe doesn't exist, creates a new recipe.
            //If a recipe does exist, the recipe is updated
            for (int i = 0; i < numberOfRecipesToUpdate; i++)
            {
                bool? recipeUpdated = false;

                Recipe recipe = recipeBook.Recipes[i];

                Recipe recipeInDatabase = GetRecipe(recipe.Id);

                if (recipeInDatabase == null)
                {
                    return null;
                }
                else if (recipeInDatabase.Id == -1)
                {
                    Recipe createdRecipe = CreateRecipe(recipeBookId, recipe);

                    //If the recipe to add was created without an error, set recipeUpdated to true
                    if (createdRecipe != null)
                    {
                        recipeUpdated = true;
                    }
                }
                else
                {
                    recipeUpdated = UpdateRecipe(recipeBookId, recipe.Id, recipe);
                }

                if (recipeUpdated == true)
                {
                    recipeUpdateCount++;
                }
            }

            //Validates that every recipe in the recipe book was updated
            if (numberOfRecipesToUpdate == recipeUpdateCount)
            {
                allRecipesUpdated = true;
            }

            //Updates the name of the recipe book
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlUpdateRecipeBook = "UPDATE recipe_book SET name = @name WHERE rb_id = @recipe_book_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlUpdateRecipeBook, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@name", recipeBook.Name);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_id", recipeBookId);

                    sqlCmd.ExecuteNonQuery();

                    recipeBookUpdated = true;
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return (recipeBookUpdated && allRecipesUpdated);
        }

        public bool? UpdateRecipe(int recipeBookId, int recipeId, Recipe recipe)
        {
            bool recipeUpdated = false;

            bool? metadataUpdated = UpdateMetadata(recipe);
            bool? ingredientListUpdated = UpdateIngredientList(recipe);
            bool? cookingInstructionsUpdated = UpdateCookingInstructions(recipe);

            if (metadataUpdated == null || ingredientListUpdated == null || cookingInstructionsUpdated == null)
            {
                return null;
            }

            //Update the recipe book id and recipe number of the recipe
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlUpdateRecipe = "UPDATE recipe SET recipe_book_id = @recipe_book_id, recipe_number = @recipe_number WHERE r_id = @recipe_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlUpdateRecipe, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_id", recipeBookId);
                    sqlCmd.Parameters.AddWithValue("@recipe_number", recipe.RecipeNumber);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);

                    sqlCmd.ExecuteNonQuery();

                    recipeUpdated = true;
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return recipeUpdated;
        }

        private bool? UpdateMetadata(Recipe recipe)
        {
            bool metadataUpdated = false;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlUpdateMetadata = "UPDATE metadata SET title = @title, notes = @notes WHERE m_id = @metadata_id; " +
                                                "UPDATE times SET prep_time = @prep_time, cook_time = @cook_time WHERE tms_id = @times_id; " +
                                                "UPDATE tags SET food_type = @food_type, food_genre = @food_genre WHERE tgs_id = @tags_id; " +
                                                "UPDATE servings SET low_servings = @low_servings, high_servings = @high_servings WHERE svgs_id = @servings_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlUpdateMetadata, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@title", recipe.Metadata.Title);
                    sqlCmd.Parameters.AddWithValue("@notes", recipe.Metadata.Notes);
                    sqlCmd.Parameters.AddWithValue("@metadata_id", recipe.Metadata.Id);
                    sqlCmd.Parameters.AddWithValue("@prep_time", recipe.Metadata.PrepTimes.PrepTime);
                    sqlCmd.Parameters.AddWithValue("@cook_time", recipe.Metadata.PrepTimes.CookTime);
                    sqlCmd.Parameters.AddWithValue("@times_id", recipe.Metadata.PrepTimes.Id);
                    sqlCmd.Parameters.AddWithValue("@food_type", recipe.Metadata.Tags.FoodType);
                    sqlCmd.Parameters.AddWithValue("@food_genre", recipe.Metadata.Tags.FoodGenre);
                    sqlCmd.Parameters.AddWithValue("@tags_id", recipe.Metadata.Tags.Id);
                    sqlCmd.Parameters.AddWithValue("@low_servings", recipe.Metadata.Servings.LowNumberOfServings);
                    sqlCmd.Parameters.AddWithValue("@high_servings", recipe.Metadata.Servings.HighNumberOfServings);
                    sqlCmd.Parameters.AddWithValue("@servings_id", recipe.Metadata.Servings.Id);

                    sqlCmd.ExecuteNonQuery();

                    metadataUpdated = true;
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return metadataUpdated;
        }

        private bool? UpdateIngredientList(Recipe recipe)
        {
            bool ingredientListUpdated = false;
            int ingredientUpdateCount = 0;

            List<Ingredient> allIngredientsToUpdate = recipe.IngredientList.AllIngredients;

            for (int i = 0; i < allIngredientsToUpdate.Count; i++)
            {
                try
                {
                    bool? ingredientUpdated = UpdateIngredient(recipe.IngredientList.Id, allIngredientsToUpdate[i]);

                    if (ingredientUpdated == true)
                    {
                        ingredientUpdateCount++;
                    }

                }
                catch (SqlException ex)
                {
                    string errorMessage = ex.Message;
                    return null;
                }
            }

            if (ingredientUpdateCount == allIngredientsToUpdate.Count)
            {
                ingredientListUpdated = true;
            }

            return ingredientListUpdated;
        }

        private bool? UpdateIngredient(int ingredientListId, Ingredient ingredient)
        {
            bool? ingredientUpdated = false;

            //Check to see if an ingredient is in the database. If so, the ingredient is updated. If the ingredient is not in the database, it is created.
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectIngredient = "SELECT i_id FROM ingredient WHERE i_id = @ingredient_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlSelectIngredient, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@ingredient_id", ingredient.Id);

                    int? ingredientIdFromDatabase = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    //If the ingredientIdFromDatabase is null or 0, the ingredient is not in the database and needs to be created
                    if (ingredientIdFromDatabase == null || ingredientIdFromDatabase == 0)
                    {
                        ingredientUpdated = CreateIngredient(ingredientListId, ingredient);
                    }
                    else
                    {
                        //If the ingredient is in the database, the commands in this "else" statement update it

                        string sqlUpdateIngrient = "UPDATE ingredient SET quantity = @quantity, measurement_unit = @measurement_unit, name = @name, prep_note = @prep_note, store_location = @store_location WHERE ingredient_list_id = @ingredient_list_id AND i_id = @ingredient_id;";

                        sqlCmd = new SqlCommand(sqlUpdateIngrient, sqlConn);
                        sqlCmd.Parameters.AddWithValue("@quantity", ingredient.Quantity);
                        sqlCmd.Parameters.AddWithValue("@measurement_unit", ingredient.MeasurementUnit);
                        sqlCmd.Parameters.AddWithValue("@name", ingredient.Name);
                        sqlCmd.Parameters.AddWithValue("@prep_note", ingredient.PreparationNote);
                        sqlCmd.Parameters.AddWithValue("@store_location", ingredient.StoreLocation);
                        sqlCmd.Parameters.AddWithValue("@ingredient_list_id", ingredientListId);
                        sqlCmd.Parameters.AddWithValue("@ingredient_id", ingredient.Id);

                        sqlCmd.ExecuteNonQuery();

                        ingredientUpdated = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return ingredientUpdated;
        }

        private bool? UpdateCookingInstructions(Recipe recipe)
        {
            bool cookingInstructionsUpdated = false;
            int instructionBlockUpdateCount = 0;

            List<InstructionBlock> allInstructionBlocksToUpdate = recipe.CookingInstructions.InstructionBlocks;

            for (int i = 0; i < allInstructionBlocksToUpdate.Count; i++)
            {
                try
                {
                    int instructionBlockIndex = i;

                    bool? instructionBlockUpdated = UpdateInstructionBlock(recipe, instructionBlockIndex);

                    if (instructionBlockUpdated == true)
                    {
                        instructionBlockUpdateCount++;
                    }
                }
                catch (SqlException ex)
                {
                    string errorMessage = ex.Message;
                    return null;
                }
            }

            if (instructionBlockUpdateCount == allInstructionBlocksToUpdate.Count)
            {
                cookingInstructionsUpdated = true;
            }

            return cookingInstructionsUpdated;
        }

        private bool? UpdateInstructionBlock(Recipe recipe, int instructionBlockIndex)
        {
            bool? instructionBlockUpdated = false;
            InstructionBlock instructionBlockToUpdate = recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex];

            bool? instructionLineCreated = false;
            int instructionLinesCreatedCount = 0;

            //Check to see if the instruction block exists. If it doesn't exist, create it. If it does exist, update it.
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlSelectInstructionBlock = "SELECT ib_id FROM instruction_block WHERE ib_id = @instruction_block_id;";
                    SqlCommand sqlCmd = new SqlCommand(sqlSelectInstructionBlock, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@instruction_block_id", instructionBlockToUpdate.Id);

                    int? instructionBlockIdFromDatabase = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    //If the instructionBlockIdFromDatabase is null or 0, the InstructionBlock is not in the database and needs to be created
                    if (instructionBlockIdFromDatabase == null || instructionBlockIdFromDatabase == 0)
                    {
                        int? newInstructionBlockId = CreateInstructionBlock(recipe, instructionBlockIndex);

                        //Once the new instruction block is created, its instruction lines also need to be created
                        for (int i = 0; i < instructionBlockToUpdate.InstructionLines.Count; i++)
                        {
                            instructionLineCreated = false;

                            instructionLineCreated = CreateInstructionLine(recipe, instructionBlockIndex, i);

                            if (instructionLineCreated == true)
                            {
                                instructionLinesCreatedCount++;
                            }
                        }

                        if (newInstructionBlockId != null && newInstructionBlockId > 0 && (instructionLinesCreatedCount == instructionBlockToUpdate.InstructionLines.Count))
                        {
                            instructionBlockUpdated = true;
                        }
                    }
                    else
                    {
                        //If the instruction block is already in the database, the code below updates it

                        string sqlUpdateInstructionBlock = "UPDATE instruction_block SET block_heading = @block_heading WHERE ib_id = @instruction_block_id AND cooking_instructions_id = @cooking_instructions_id;";
                        sqlCmd = new SqlCommand(sqlUpdateInstructionBlock, sqlConn);
                        sqlCmd.Parameters.AddWithValue("@block_heading", instructionBlockToUpdate.BlockHeading);
                        sqlCmd.Parameters.AddWithValue("@instruction_block_id", instructionBlockToUpdate.Id);
                        sqlCmd.Parameters.AddWithValue("@cooking_instructions_id", recipe.CookingInstructions.Id);

                        sqlCmd.ExecuteNonQuery();

                        string sqlDeleteInstructionsFromBlock = "DELETE FROM instruction_block_instruction WHERE instruction_block_id = @instruction_block_id; " +
                                                                "DELETE FROM instruction WHERE inst_id IN (SELECT i.inst_id FROM instruction i JOIN instruction_block_instruction ibi ON i.inst_id = ibi.instruction_id JOIN instruction_block ib ON ibi.instruction_block_id = ib.ib_id WHERE ib.ib_id = @instruction_block_id);";

                        //This command deletes all of the instruction lines from the current instruction block in the database
                        //so that the new, updated instruction lines can be created in the "for" loop below.
                        //This simplifies updating instruction lines in the database.
                        sqlCmd = new SqlCommand(sqlDeleteInstructionsFromBlock, sqlConn);
                        sqlCmd.Parameters.AddWithValue("@instruction_block_id", instructionBlockToUpdate.Id);

                        sqlCmd.ExecuteNonQuery();

                        for (int j = 0; j < instructionBlockToUpdate.InstructionLines.Count; j++)
                        {
                            instructionLineCreated = false;

                            instructionLineCreated = CreateInstructionLine(recipe, instructionBlockIndex, j);

                            if (instructionLineCreated == true)
                            {
                                instructionLinesCreatedCount++;
                            }
                        }

                        if (instructionLinesCreatedCount == instructionBlockToUpdate.InstructionLines.Count)
                        {
                            instructionBlockUpdated = true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return instructionBlockUpdated;
        }

        public bool? DeleteRecipeBook(int recipeBookId)
        {
            RecipeBook recipeBookToDelete = GetRecipeBook(recipeBookId);

            bool allRecipesDeleted = false;
            bool recipeBookDeleted = false;
            int recipeDeleteCount = 0;
            int numberOfRecipesToDelete = recipeBookToDelete.Recipes.Count;

            //Delete all recipes from recipe book
            for (int i = 0; i < recipeBookToDelete.Recipes.Count; i++)
            {
                int recipeId = recipeBookToDelete.Recipes[i].Id;

                bool? recipeDeleted = DeleteRecipe(recipeBookId, recipeId);

                if (recipeDeleted != null && recipeDeleted != false)
                {
                    recipeDeleteCount++;
                }
            }

            if (numberOfRecipesToDelete == recipeDeleteCount)
            {
                allRecipesDeleted = true;
            }

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlDeleteRecipeBook = "DELETE FROM recipe_book WHERE rb_id = @recipe_book_id;";

                    SqlCommand sqlCmd = new SqlCommand(sqlDeleteRecipeBook, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_id", recipeBookId);

                    sqlCmd.ExecuteNonQuery();

                    recipeBookDeleted = true;
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return (recipeBookDeleted && allRecipesDeleted);
        }

        public bool? DeleteRecipe(int recipeBookId, int recipeId)
        {
            bool recipeDeleted = false;

            Recipe recipeToDelete = GetRecipe(recipeId);

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    string sqlDeleteRecipe = "BEGIN TRANSACTION; " +
                                             "DELETE FROM recipe WHERE r_id = @recipe_id AND recipe_book_id = @recipe_book_id; " +
                                             "DELETE FROM metadata WHERE m_id = @metadata_id; " +
                                             "DELETE FROM times WHERE tms_id = @times_id; " +
                                             "DELETE FROM tags WHERE tgs_id = @tags_id; " +
                                             "DELETE FROM servings WHERE svgs_id = @servings_id; " +
                                             "DELETE FROM ingredient_list WHERE il_id = @ingredient_list_id; " +
                                             "DELETE FROM ingredient WHERE i_id IN (SELECT i_id FROM ingredient WHERE ingredient_list_id = @ingredient_list_id); " +
                                             "DELETE FROM instruction_block_instruction WHERE instruction_block_id IN (SELECT ib_id FROM instruction_block WHERE cooking_instructions_id = @cooking_instructions_id); " +
                                             "DELETE FROM instruction_block WHERE cooking_instructions_id = @cooking_instructions_id; " +
                                             "DELETE FROM cooking_instructions WHERE ci_id = @cooking_instructions_id; " +
                                             "DELETE FROM instruction WHERE inst_id IN(SELECT instruction_id FROM instruction_block_instruction WHERE instruction_block_id IN (SELECT ib_id FROM instruction_block WHERE cooking_instructions_id = @cooking_instructions_id)) " +
                                             "COMMIT;";

                    SqlCommand sqlCmd = new SqlCommand(sqlDeleteRecipe, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@recipe_id", recipeId);
                    sqlCmd.Parameters.AddWithValue("@recipe_book_id", recipeBookId);
                    sqlCmd.Parameters.AddWithValue("@metadata_id", recipeToDelete.Metadata.Id);
                    sqlCmd.Parameters.AddWithValue("@times_id", recipeToDelete.Metadata.PrepTimes.Id);
                    sqlCmd.Parameters.AddWithValue("@tags_id", recipeToDelete.Metadata.Tags.Id);
                    sqlCmd.Parameters.AddWithValue("@servings_id", recipeToDelete.Metadata.Servings.Id);
                    sqlCmd.Parameters.AddWithValue("@ingredient_list_id", recipeToDelete.IngredientList.Id);
                    sqlCmd.Parameters.AddWithValue("@cooking_instructions_id", recipeToDelete.CookingInstructions.Id);

                    sqlCmd.ExecuteNonQuery();

                    recipeDeleted = true;
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = ex.Message;
                return null;
            }

            return recipeDeleted;
        }
    }
}
