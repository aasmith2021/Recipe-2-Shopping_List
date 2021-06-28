BEGIN TRANSACTION;
SELECT *
FROM recipe_book_library rbl
JOIN recipe_book rb ON rbl.id = rb.recipe_book_library_id
JOIN recipe r ON rb.id = r.recipe_book_id
JOIN metadata m ON r.metadata_id = m.id
JOIN tags tgs ON m.tags_id = tgs.id
JOIN times tms ON m.times_id = tms.id
JOIN servings svgs ON m.servings_id = svgs.id
JOIN cooking_instructions ci ON r.cooking_instructions_id = ci.id
JOIN instruction_block ib ON ci.id = ib.cooking_instructions_id
JOIN instruction_block_instruction ibi ON ib.id = ibi.block_id
JOIN instruction i ON ibi.instruction_id = i.id
JOIN ingredient_list il ON r.ingredient_list_id = il.id
JOIN ingredient ing ON il.id = ing.ingredient_list_id;

DECLARE @recipe_id INT = 2;
DECLARE @recipe_book_id INT = 1;
DECLARE @metadata_id INT = 2;
DECLARE @times_id INT = 2;
DECLARE @tags_id INT = 2;
DECLARE @servings_id INT = 2;
DECLARE @ingredient_list_id INT = 2;
DECLARE @cooking_instructions_id INT = 2;

DELETE FROM recipe WHERE id = @recipe_id AND recipe_book_id = @recipe_book_id;
DELETE FROM metadata WHERE id = @metadata_id;
DELETE FROM times WHERE id = @times_id;
DELETE FROM tags WHERE id = @tags_id;
DELETE FROM servings WHERE id = @servings_id;
DELETE FROM ingredient_list WHERE id = @ingredient_list_id;
DELETE FROM ingredient WHERE id IN (SELECT id FROM ingredient WHERE ingredient_list_id = @ingredient_list_id);;
DELETE FROM instruction_block_instruction WHERE block_id IN (SELECT id FROM instruction_block WHERE cooking_instructions_id = @cooking_instructions_id);
DELETE FROM instruction_block WHERE cooking_instructions_id = @cooking_instructions_id;
DELETE FROM cooking_instructions WHERE id = @cooking_instructions_id
DELETE FROM instruction WHERE id IN (SELECT instruction_id FROM instruction_block_instruction WHERE block_id IN (SELECT id FROM instruction_block WHERE cooking_instructions_id = @cooking_instructions_id));

SELECT *
FROM recipe_book_library rbl
JOIN recipe_book rb ON rbl.id = rb.recipe_book_library_id
JOIN recipe r ON rb.id = r.recipe_book_id
JOIN metadata m ON r.metadata_id = m.id
JOIN tags tgs ON m.tags_id = tgs.id
JOIN times tms ON m.times_id = tms.id
JOIN servings svgs ON m.servings_id = svgs.id
JOIN cooking_instructions ci ON r.cooking_instructions_id = ci.id
JOIN instruction_block ib ON ci.id = ib.cooking_instructions_id
JOIN instruction_block_instruction ibi ON ib.id = ibi.block_id
JOIN instruction i ON ibi.instruction_id = i.id
JOIN ingredient_list il ON r.ingredient_list_id = il.id
JOIN ingredient ing ON il.id = ing.ingredient_list_id;


ROLLBACK;