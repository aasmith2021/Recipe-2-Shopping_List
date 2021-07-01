BEGIN TRANSACTION;

SELECT *
FROM recipe_book_library rbl
JOIN recipe_book rb ON rbl.recipe_book_library_id = rb.recipe_book_library_id
JOIN recipe r ON rb.recipe_book_id = r.recipe_book_id
JOIN metadata m ON r.metadata_id = m.metadata_id
JOIN tags tgs ON m.tags_id = tgs.tags_id
JOIN times tms ON m.times_id = tms.times_id
JOIN servings svgs ON m.servings_id = svgs.servings_id
JOIN cooking_instructions ci ON r.cooking_instructions_id = ci.cooking_instructions_id
JOIN instruction_block ib ON ci.cooking_instructions_id = ib.cooking_instructions_id
JOIN instruction_block_instruction ibi ON ib.instruction_block_id = ibi.instruction_block_id
JOIN instruction i ON ibi.instruction_id = i.instruction_id
JOIN ingredient_list il ON r.ingredient_list_id = il.ingredient_list_id
JOIN ingredient ing ON il.ingredient_list_id = ing.ingredient_list_id;

ROLLBACK;

DECLARE @recipe_book_library_id INT = 1;

SELECT rbl.recipe_book_library_id AS 'recipe_book_library_idx', rb.recipe_book_id AS 'recipe_book_id'
FROM recipe_book_library rbl
JOIN recipe_book rb ON rbl.recipe_book_library_id = rb.recipe_book_library_id
JOIN recipe r ON rb.recipe_book_id = r.recipe_book_id
WHERE recipe_book_library_idx = @recipe_book_library_id;