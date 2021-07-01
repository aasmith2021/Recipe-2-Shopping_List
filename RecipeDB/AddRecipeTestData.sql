-- Insert test data into RecipeDB --
BEGIN TRANSACTION;

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
	(@ingredient_list_id, 1, 'Box', 'chocolate pudding mix', 'brand new', 'Dry Goods'),
	(@ingredient_list_id, 15, '', 'gummy worms', 'fun flavors', 'Dry Goods'),
	(@ingredient_list_id, 0.5, 'Cup', 'chocolate cookie crumbles', '', 'Bakery/Deli');

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

INSERT INTO instruction_block_instruction (instruction_block_id, instruction_id)
VALUES
	(@block_id_1, @instruction_1_id),
	(@block_id_1, @instruction_2_id),
	(@block_id_2, @instruction_3_id);

INSERT INTO recipe (recipe_number, recipe_book_id, metadata_id, ingredient_list_id, cooking_instructions_id)
VALUES (1, @recipe_book_id, @metadata_id, @ingredient_list_id, @cooking_instructions_id)
DECLARE @recipe_id INT = (SELECT @@IDENTITY);

COMMIT;
--ROLLBACK;