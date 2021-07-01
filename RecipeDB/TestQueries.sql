--Select All Data
/*
SELECT *
FROM recipe_book_library rbl
JOIN recipe_book rb ON rbl.rbl_id = rb.recipe_book_library_id
JOIN recipe r ON rb.rb_id = r.recipe_book_id
JOIN metadata m ON r.metadata_id = m.m_id
JOIN tags tgs ON m.tags_id = tgs.tgs_id
JOIN times tms ON m.times_id = tms.tms_id
JOIN servings svgs ON m.servings_id = svgs.svgs_id
JOIN cooking_instructions ci ON r.cooking_instructions_id = ci.ci_id
JOIN instruction_block ib ON ci.ci_id = ib.cooking_instructions_id
JOIN instruction_block_instruction ibi ON ib.ib_id = ibi.instruction_block_id
JOIN instruction i ON ibi.instruction_id = i.inst_id
JOIN ingredient_list il ON r.ingredient_list_id = il.il_id
JOIN ingredient ing ON il.il_id = ing.ingredient_list_id;
*/

SELECT *
FROM recipe_book_library rbl
JOIN recipe_book rb ON rbl.rbl_id = rb.recipe_book_library_id
JOIN recipe r ON rb.rb_id = r.recipe_book_id
JOIN metadata m ON r.metadata_id = m.m_id
JOIN tags tgs ON m.tags_id = tgs.tgs_id
JOIN times tms ON m.times_id = tms.tms_id
JOIN servings svgs ON m.servings_id = svgs.svgs_id
JOIN cooking_instructions ci ON r.cooking_instructions_id = ci.ci_id
JOIN instruction_block ib ON ci.ci_id = ib.cooking_instructions_id
JOIN instruction_block_instruction ibi ON ib.ib_id = ibi.instruction_block_id
JOIN instruction i ON ibi.instruction_id = i.inst_id
JOIN ingredient_list il ON r.ingredient_list_id = il.il_id
JOIN ingredient ing ON il.il_id = ing.ingredient_list_id
WHERE rb.rb_id = 2;

SELECT * FROM recipe;

BEGIN TRANSACTION;
SELECT *
FROM recipe;

UPDATE recipe SET recipe_book_id = 1, recipe_number = 1 WHERE r_id = 1;

/*
DELETE FROM instruction_block_instruction WHERE instruction_block_id = 1;
DELETE FROM instruction WHERE inst_id IN (SELECT i.inst_id FROM instruction i JOIN instruction_block_instruction ibi ON i.inst_id = ibi.instruction_id JOIN instruction_block ib ON ibi.instruction_block_id = ib.ib_id WHERE ib.ib_id = 1);

DELETE FROM instruction_block_instruction WHERE instruction_block_id = 1;
DELETE FROM instruction WHERE inst_id IN (SELECT i.inst_id FROM instruction i JOIN instruction_block_instruction ibi ON i.inst_id = ibi.instruction_id JOIN instruction_block ib ON ibi.instruction_block_id = ib.ib_id WHERE ib.ib_id = 1);
*/

SELECT *
FROM recipe;
ROLLBACK;