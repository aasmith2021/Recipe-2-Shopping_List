/* Deployment script for RecipeDB */
USE master;
GO

IF DB_ID('RecipeDB') IS NOT NULL
BEGIN
	ALTER DATABASE RecipeDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE RecipeDB;
END
GO

CREATE DATABASE RecipeDB;
GO

USE RecipeDB
GO

CREATE TABLE recipe_book_library
(
	rbl_id		INT					IDENTITY(1,1),
	CONSTRAINT PK_recipe_book_library PRIMARY KEY (rbl_id)
);

CREATE TABLE recipe_book
(
	rb_id					INT					IDENTITY(1,1),
	recipe_book_library_id	INT					NOT NULL,
	name					VARCHAR (120)		NOT NULL,
	CONSTRAINT PK_recipe_book PRIMARY KEY (rb_id),
	CONSTRAINT FK_recipe_book_recipe_book_library FOREIGN KEY (recipe_book_library_id) REFERENCES recipe_book_library(rbl_id)
);

CREATE TABLE times
(
	tms_id			 INT		IDENTITY(1,1),
	prep_time	SMALLINT		NOT NULL,
	cook_time	SMALLINT		NOT NULL,
	CONSTRAINT PK_times PRIMARY KEY (tms_id)
);

CREATE TABLE tags
(
	tgs_id			INT				IDENTITY(1,1),
	food_type	VARCHAR (100)	NOT NULL,
	food_genre	VARCHAR (100)	NOT NULL,
	CONSTRAINT PK_tags PRIMARY KEY (tgs_id)
);

CREATE TABLE servings
(
	svgs_id		INT				IDENTITY(1,1),
	low_servings	SMALLINT		NOT NULL,
	high_servings	SMALLINT		NOT NULL,
	CONSTRAINT PK_servings PRIMARY KEY (svgs_id)
);

CREATE TABLE metadata
(
	m_id			INT				IDENTITY(1,1),
	title			VARCHAR (200)	NOT NULL,
	notes			VARCHAR (1200)	NOT NULL,
	times_id		INT				NOT NULL,
	tags_id			INT				NOT NULL,
	servings_id		INT				NOT NULL,
	CONSTRAINT PK_metadata PRIMARY KEY (m_id),
	CONSTRAINT FK_metadata_times FOREIGN KEY (times_id) REFERENCES times(tms_id),
	CONSTRAINT FK_metadata_tags FOREIGN KEY (tags_id) REFERENCES tags(tgs_id),
	CONSTRAINT FK_metadata_servings FOREIGN KEY (servings_id) REFERENCES servings(svgs_id),
);

CREATE TABLE ingredient_list
(
	il_id			INT			IDENTITY(1,1),
	CONSTRAINT PK_ingredient_list PRIMARY KEY (il_id)
);

CREATE TABLE ingredient
(
	i_id					INT				IDENTITY(1,1),
	ingredient_list_id		INT				NOT NULL,
	quantity				DECIMAL (7,3)	NOT NULL,
	measurement_unit		VARCHAR (30)	NOT NULL,
	name					VARCHAR (100)	NOT NULL,
	prep_note				VARCHAR (120)	NOT NULL,
	store_location			VARCHAR (30)	NOT NULL,
	CONSTRAINT PK_ingredient PRIMARY KEY (i_id),
);

CREATE TABLE cooking_instructions
(
	ci_id				INT				IDENTITY(1,1),
	CONSTRAINT PK_cooking_instructions PRIMARY KEY (ci_id)
);

CREATE TABLE instruction_block
(
	ib_id					INT				IDENTITY(1,1),
	cooking_instructions_id	INT				NOT NULL,
	block_heading			VARCHAR (100)	NOT NULL,
	CONSTRAINT PK_instruction_block PRIMARY KEY (ib_id),
	CONSTRAINT FK_instruction_block FOREIGN KEY (cooking_instructions_id) REFERENCES cooking_instructions(ci_id)
);

CREATE TABLE instruction
(
	inst_id				INT				IDENTITY(1,1),
	text			VARCHAR (360)	NOT NULL,
	CONSTRAINT PK_instruction PRIMARY KEY (inst_id)
);

CREATE TABLE instruction_block_instruction
(
	instruction_block_id			INT,
	instruction_id					INT,
	CONSTRAINT PK_instruction_block_instruction PRIMARY KEY (instruction_block_id, instruction_id),
	CONSTRAINT FK_instruction_block_instruction_block_id FOREIGN KEY (instruction_block_id) REFERENCES instruction_block(ib_id),
	CONSTRAINT FK_instruction_block_instruction_instruction_id FOREIGN KEY (instruction_id) REFERENCES instruction(inst_id),
);

CREATE TABLE recipe
(
	r_id					INT		IDENTITY(1,1),
	recipe_number			INT		NOT NULL,
	recipe_book_id			INT		NOT NULL,
	metadata_id				INT		NOT NULL,
	ingredient_list_id		INT		NOT NULL,
	cooking_instructions_id	INT		NOT NULL,
	CONSTRAINT PK_recipe PRIMARY KEY (r_id),
	CONSTRAINT FK_recipe_recipe_book FOREIGN KEY (recipe_book_id) REFERENCES recipe_book(rb_id),
	CONSTRAINT FK_recipe_metadata FOREIGN KEY (metadata_id) REFERENCES metadata(m_id),
	CONSTRAINT FK_recipe_ingredient_list FOREIGN KEY (ingredient_list_id) REFERENCES ingredient_list(il_id),
	CONSTRAINT FK_recipe_cooking_instructions FOREIGN KEY (cooking_instructions_id) REFERENCES cooking_instructions(ci_id)
);

BEGIN TRANSACTION;

-- <<<< ADD INITIAL RECIPE BOOK LIBRARY >>>>
INSERT INTO recipe_book_library DEFAULT VALUES;
DECLARE @recipe_book_library_id INT = (SELECT @@IDENTITY);

-- <<<< ADD FIRST TEST RECIPE BOOK FOR GET TESTS >>>>
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

-- <<<< ADD SECOND TEST RECIPE BOOK FOR PUT TESTS >>>>
INSERT INTO recipe_book (name, recipe_book_library_id)
VALUES ('Desserts', @recipe_book_library_id)
DECLARE @recipe_book_id2 INT = (SELECT @@IDENTITY);

INSERT INTO times (prep_time, cook_time)
VALUES (10, 15)
DECLARE @times_id2 INT = (SELECT @@IDENTITY);

INSERT INTO tags (food_type, food_genre)
VALUES ('Dessert', 'American')
DECLARE @tags_id2 INT = (SELECT @@IDENTITY);

INSERT INTO servings (low_servings, high_servings)
VALUES (2, 6)
DECLARE @servings_id2 INT = (SELECT @@IDENTITY);

INSERT INTO metadata (title, notes, times_id, tags_id, servings_id)
VALUES ('Dirt Cups', 'A fun treat during the summer!', @times_id2, @tags_id2, @servings_id2)
DECLARE @metadata_id2 INT = (SELECT @@IDENTITY);

INSERT INTO ingredient_list DEFAULT VALUES
DECLARE @ingredient_list_id2 INT = (SELECT @@IDENTITY);

INSERT INTO ingredient (ingredient_list_id, quantity, measurement_unit, name, prep_note, store_location)
VALUES
	(@ingredient_list_id2, 1, 'Box', 'chocolate pudding mix', 'brand new', 'Dry Goods'),
	(@ingredient_list_id2, 15, '', 'gummy worms', 'fun flavors', 'Dry Goods'),
	(@ingredient_list_id2, 0.5, 'Cup', 'chocolate cookie crumbles', '', 'Bakery/Deli');

INSERT INTO cooking_instructions DEFAULT VALUES
DECLARE @cooking_instructions_id2 INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block (cooking_instructions_id, block_heading)
VALUES (@cooking_instructions_id2, 'How to make it')
DECLARE @block_id_12 INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block (cooking_instructions_id, block_heading)
VALUES (@cooking_instructions_id2, 'How to eat it')
DECLARE @block_id_22 INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Make pudding according to directions on the box')
DECLARE @instruction_1_id2 INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Spoon pudding into cups and top with crushed cookies and gummy worms')
DECLARE @instruction_2_id2 INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Eat and enjoy!')
DECLARE @instruction_3_id2 INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block_instruction (instruction_block_id, instruction_id)
VALUES
	(@block_id_12, @instruction_1_id2),
	(@block_id_12, @instruction_2_id2),
	(@block_id_22, @instruction_3_id2);

INSERT INTO recipe (recipe_number, recipe_book_id, metadata_id, ingredient_list_id, cooking_instructions_id)
VALUES (1, @recipe_book_id2, @metadata_id2, @ingredient_list_id2, @cooking_instructions_id2)
DECLARE @recipe_id2 INT = (SELECT @@IDENTITY);

-- <<<< ADD THIRD TEST RECIPE BOOK FOR DELETE TESTS >>>>
INSERT INTO recipe_book (name, recipe_book_library_id)
VALUES ('Desserts', @recipe_book_library_id)
DECLARE @recipe_book_id3 INT = (SELECT @@IDENTITY);

INSERT INTO times (prep_time, cook_time)
VALUES (10, 15)
DECLARE @times_id3 INT = (SELECT @@IDENTITY);

INSERT INTO tags (food_type, food_genre)
VALUES ('Dessert', 'American')
DECLARE @tags_id3 INT = (SELECT @@IDENTITY);

INSERT INTO servings (low_servings, high_servings)
VALUES (2, 6)
DECLARE @servings_id3 INT = (SELECT @@IDENTITY);

INSERT INTO metadata (title, notes, times_id, tags_id, servings_id)
VALUES ('Dirt Cups', 'A fun treat during the summer!', @times_id3, @tags_id3, @servings_id3)
DECLARE @metadata_id3 INT = (SELECT @@IDENTITY);

INSERT INTO ingredient_list DEFAULT VALUES
DECLARE @ingredient_list_id3 INT = (SELECT @@IDENTITY);

INSERT INTO ingredient (ingredient_list_id, quantity, measurement_unit, name, prep_note, store_location)
VALUES
	(@ingredient_list_id3, 1, 'Box', 'chocolate pudding mix', 'brand new', 'Dry Goods'),
	(@ingredient_list_id3, 15, '', 'gummy worms', 'fun flavors', 'Dry Goods'),
	(@ingredient_list_id3, 0.5, 'Cup', 'chocolate cookie crumbles', '', 'Bakery/Deli');

INSERT INTO cooking_instructions DEFAULT VALUES
DECLARE @cooking_instructions_id3 INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block (cooking_instructions_id, block_heading)
VALUES (@cooking_instructions_id3, 'How to make it')
DECLARE @block_id_13 INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block (cooking_instructions_id, block_heading)
VALUES (@cooking_instructions_id3, 'How to eat it')
DECLARE @block_id_23 INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Make pudding according to directions on the box')
DECLARE @instruction_1_id3 INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Spoon pudding into cups and top with crushed cookies and gummy worms')
DECLARE @instruction_2_id3 INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Eat and enjoy!')
DECLARE @instruction_3_id3 INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block_instruction (instruction_block_id, instruction_id)
VALUES
	(@block_id_13, @instruction_1_id3),
	(@block_id_13, @instruction_2_id3),
	(@block_id_23, @instruction_3_id3);

INSERT INTO recipe (recipe_number, recipe_book_id, metadata_id, ingredient_list_id, cooking_instructions_id)
VALUES (1, @recipe_book_id3, @metadata_id3, @ingredient_list_id3, @cooking_instructions_id3)
DECLARE @recipe_id3 INT = (SELECT @@IDENTITY);

-- <<<< ADD FOURTH TEST RECIPE BOOK FOR DELETE TESTS >>>>
INSERT INTO recipe_book (name, recipe_book_library_id)
VALUES ('Desserts', @recipe_book_library_id)
DECLARE @recipe_book_id4 INT = (SELECT @@IDENTITY);

INSERT INTO times (prep_time, cook_time)
VALUES (10, 15)
DECLARE @times_id4 INT = (SELECT @@IDENTITY);

INSERT INTO tags (food_type, food_genre)
VALUES ('Dessert', 'American')
DECLARE @tags_id4 INT = (SELECT @@IDENTITY);

INSERT INTO servings (low_servings, high_servings)
VALUES (2, 6)
DECLARE @servings_id4 INT = (SELECT @@IDENTITY);

INSERT INTO metadata (title, notes, times_id, tags_id, servings_id)
VALUES ('Dirt Cups', 'A fun treat during the summer!', @times_id4, @tags_id4, @servings_id4)
DECLARE @metadata_id4 INT = (SELECT @@IDENTITY);

INSERT INTO ingredient_list DEFAULT VALUES
DECLARE @ingredient_list_id4 INT = (SELECT @@IDENTITY);

INSERT INTO ingredient (ingredient_list_id, quantity, measurement_unit, name, prep_note, store_location)
VALUES
	(@ingredient_list_id4, 1, 'Box', 'chocolate pudding mix', 'brand new', 'Dry Goods'),
	(@ingredient_list_id4, 15, '', 'gummy worms', 'fun flavors', 'Dry Goods'),
	(@ingredient_list_id4, 0.5, 'Cup', 'chocolate cookie crumbles', '', 'Bakery/Deli');

INSERT INTO cooking_instructions DEFAULT VALUES
DECLARE @cooking_instructions_id4 INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block (cooking_instructions_id, block_heading)
VALUES (@cooking_instructions_id4, 'How to make it')
DECLARE @block_id_14 INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block (cooking_instructions_id, block_heading)
VALUES (@cooking_instructions_id4, 'How to eat it')
DECLARE @block_id_24 INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Make pudding according to directions on the box')
DECLARE @instruction_1_id4 INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Spoon pudding into cups and top with crushed cookies and gummy worms')
DECLARE @instruction_2_id4 INT = (SELECT @@IDENTITY);

INSERT INTO instruction (text)
VALUES ('Eat and enjoy!')
DECLARE @instruction_3_id4 INT = (SELECT @@IDENTITY);

INSERT INTO instruction_block_instruction (instruction_block_id, instruction_id)
VALUES
	(@block_id_14, @instruction_1_id4),
	(@block_id_14, @instruction_2_id4),
	(@block_id_24, @instruction_3_id4);

INSERT INTO recipe (recipe_number, recipe_book_id, metadata_id, ingredient_list_id, cooking_instructions_id)
VALUES (1, @recipe_book_id4, @metadata_id4, @ingredient_list_id4, @cooking_instructions_id4)
DECLARE @recipe_id4 INT = (SELECT @@IDENTITY);

COMMIT;