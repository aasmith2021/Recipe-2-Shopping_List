-- Insert test data into RecipeDB --
BEGIN TRANSACTION;
/*
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
*/

INSERT INTO recipe_book_library DEFAULT VALUES;
DECLARE @recipe_book_library_id INT = (SELECT @@IDENTITY);

INSERT INTO recipe_book (name, recipe_book_library_id)
VALUES ('Desserts', @recipe_book_library_id)
DECLARE @recipe_book_id INT = (SELECT @@IDENTITY);

INSERT INTO times (prep_time, cook_time)
VALUES (10, 15)
DECLARE @times_id INT = (SELECT @@IDENTITY);

INSERT INTO tags (food_type, food_genre)
VALUES ('Dessert', 'American')
DECLARE @tags_id INT = (SELECT @@IDENTITY);

INSERT INTO servings (low_servings, high_servings)
VALUES (2, 6)
DECLARE @servings_id INT = (SELECT @@IDENTITY);

INSERT INTO metadata (title, notes, times_id, tags_id, servings_id)
VALUES ('Dirt Cups', 'A fun treat during the summer!', @times_id, @tags_id, @servings_id)
DECLARE @metadata_id INT = (SELECT @@IDENTITY);

INSERT INTO ingredient_list DEFAULT VALUES
DECLARE @ingredient_list_id INT = (SELECT @@IDENTITY);

INSERT INTO ingredient (ingredient_list_id, quantity, measurement_unit, name, prep_note, store_location)
VALUES
	(@ingredient_list_id, 1, 'Box', 'chocolate pudding mix', '', ''),
	(@ingredient_list_id, 15, '', 'gummy worms', '', ''),
	(@ingredient_list_id, 0.5, 'Cup', 'chocolate cookie crumbles', '', '');

INSERT INTO cooking_instructions DEFAULT VALUES
DECLARE @cooking_instructions_id INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block (cooking_instructions_id, block_heading)
VALUES (@cooking_instructions_id, 'How to make it')
DECLARE @block_id_1 INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block (cooking_instructions_id, block_heading)
VALUES (@cooking_instructions_id, 'How to eat it')
DECLARE @block_id_2 INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Make pudding according to directions on the box')
DECLARE @instruction_1_id INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Spoon pudding into cups and top with crushed cookies and gummy worms')
DECLARE @instruction_2_id INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Eat and enjoy!')
DECLARE @instruction_3_id INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block_instruction (block_id, instruction_id)
VALUES
	(@block_id_1, @instruction_1_id),
	(@block_id_1, @instruction_2_id),
	(@block_id_2, @instruction_3_id);

INSERT INTO recipe (recipe_number, recipe_book_id, metadata_id, ingredient_list_id, cooking_instructions_id)
VALUES (1, @recipe_book_id, @metadata_id, @ingredient_list_id, @cooking_instructions_id)
DECLARE @recipe_id INT = (SELECT @@IDENTITY);

--All data
/*
SELECT rbl.id AS 'recipe_book_library_id', rb.id AS 'recipe_book_id', r.id AS 'recipe_id', m.id AS 'metadata_id', m.title, m.notes, tgs.id AS 'tags_id', tgs.food_type, tgs.food_genre, tms.id AS 'times_id', tms.cook_time, tms.prep_time, svgs.id AS 'servings_id', svgs.low_servings, svgs.high_servings, ing.id AS 'ingredient_id', ing.quantity, ing.measurement_unit, ing.name, ing.prep_note, ci.id AS 'cooking_instructions_id', ib.id AS 'instruction_block_id', i.id AS 'instruction_id', i.text
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
*/

--Just cooking instructions
/*
SELECT rbl.id AS 'recipe_book_library_id', rb.id AS 'recipe_book_id', r.id AS 'recipe_id', m.id AS 'metadata_id', m.title, m.notes, tgs.id AS 'tags_id', tgs.food_type, tgs.food_genre,  tms.id AS 'times_id', tms.cook_time, tms.prep_time, svgs.id AS 'servings_id', svgs.low_servings, svgs.high_servings, ci.id AS 'cooking_instructions_id', ib.id AS 'instruction_block_id', i.id AS 'instruction_id', i.text
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
JOIN ingredient ing ON il.id = ing.ingredient_list_id
GROUP BY rbl.id, rb.id, r.id, m.id, m.title, m.notes, tms.id, tms.cook_time, tms.prep_time, svgs.id, svgs.low_servings, svgs.high_servings, ci.id, ib.id, i.id, i.text;
*/

--Just ingredients
/*
SELECT rbl.id AS 'recipe_book_library_id', rb.id AS 'recipe_book_id', r.id AS 'recipe_id', m.id AS 'metadata_id', m.title, m.notes, tgs.id AS 'tags_id', tgs.food_type, tgs.food_genre,  tms.id AS 'times_id', tms.cook_time, tms.prep_time, svgs.id AS 'servings_id', svgs.low_servings, svgs.high_servings, ing.id AS 'ingredient_id', ing.quantity, ing.measurement_unit, ing.name, ing.prep_note
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
JOIN ingredient ing ON il.id = ing.ingredient_list_id
GROUP BY rbl.id, rb.id, r.id, m.id, m.title, m.notes, tms.id, tms.cook_time, tms.prep_time, svgs.id, svgs.low_servings, svgs.high_servings, ing.id, ing.quantity, ing.measurement_unit, ing.name, ing.prep_note;
*/

COMMIT;
--ROLLBACK;