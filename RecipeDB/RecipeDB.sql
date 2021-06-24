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
	id		INT					IDENTITY(1,1),
	CONSTRAINT PK_recipe_book_library PRIMARY KEY (id)
);

CREATE TABLE recipe_book
(
	id		INT					IDENTITY(1,1),
	recipe_book_library_id	INT	NOT NULL,
	name	VARCHAR (120)		NOT NULL,
	CONSTRAINT PK_recipe_book PRIMARY KEY (id),
	CONSTRAINT FK_recipe_book_recipe_book_library FOREIGN KEY (recipe_book_library_id) REFERENCES recipe_book_library(id)
);

CREATE TABLE times
(
	id			INT				IDENTITY(1,1),
	prep_time	SMALLINT		NOT NULL,
	cook_time	SMALLINT		NOT NULL,
	CONSTRAINT PK_times PRIMARY KEY (id)
);

CREATE TABLE tags
(
	id			INT				IDENTITY(1,1),
	food_type	VARCHAR (100)	NOT NULL,
	food_genre	VARCHAR (100)	NOT NULL,
	CONSTRAINT PK_tags PRIMARY KEY (id)
);

CREATE TABLE servings
(
	id				INT				IDENTITY(1,1),
	low_servings	SMALLINT		NOT NULL,
	high_servings	SMALLINT		NOT NULL,
	CONSTRAINT PK_servings PRIMARY KEY (id)
);

CREATE TABLE metadata
(
	id				INT				IDENTITY(1,1),
	title			VARCHAR (200)	NOT NULL,
	notes			VARCHAR (1200)	NOT NULL,
	times_id		INT				NOT NULL,
	tags_id			INT				NOT NULL,
	servings_id		INT				NOT NULL,
	CONSTRAINT PK_metadata PRIMARY KEY (id),
	CONSTRAINT FK_metadata_times FOREIGN KEY (times_id) REFERENCES times(id),
	CONSTRAINT FK_metadata_tags FOREIGN KEY (tags_id) REFERENCES times(id),
	CONSTRAINT FK_metadata_servings FOREIGN KEY (servings_id) REFERENCES servings(id),
);

CREATE TABLE ingredient_list
(
	id			INT			IDENTITY(1,1),
	CONSTRAINT PK_ingredient_list PRIMARY KEY (id)
);

CREATE TABLE ingredient
(
	id						INT				IDENTITY(1,1),
	ingredient_list_id		INT				NOT NULL,
	quantity				DECIMAL (7,3)	NOT NULL,
	measurement_unit		VARCHAR (30)	NOT NULL,
	name					VARCHAR (100)	NOT NULL,
	prep_note				VARCHAR (120)	NOT NULL,
	store_location			VARCHAR (30)	NOT NULL,
	CONSTRAINT PK_ingredient PRIMARY KEY (id),
);

CREATE TABLE cooking_instructions
(
	id				INT				IDENTITY(1,1),
	CONSTRAINT PK_cooking_instructions PRIMARY KEY (id)
);

CREATE TABLE instruction_block
(
	id						INT				IDENTITY(1,1),
	cooking_instructions_id	INT				NOT NULL,
	block_heading			VARCHAR (100)	NOT NULL,
	CONSTRAINT PK_instruction_block PRIMARY KEY (id),
	CONSTRAINT FK_instruction_block FOREIGN KEY (cooking_instructions_id) REFERENCES cooking_instructions(id)
);

CREATE TABLE instruction
(
	id				INT				IDENTITY(1,1),
	text			VARCHAR (360)	NOT NULL,
	CONSTRAINT PK_instruction PRIMARY KEY (id)
);

CREATE TABLE instruction_block_instruction
(
	block_id			INT,
	instruction_id		INT,
	CONSTRAINT PK_instruction_block_instruction PRIMARY KEY (block_id, instruction_id),
	CONSTRAINT FK_instruction_block_instruction_block_id FOREIGN KEY (block_id) REFERENCES instruction_block(id),
	CONSTRAINT FK_instruction_block_instruction_instruction_id FOREIGN KEY (instruction_id) REFERENCES instruction(id),
);

CREATE TABLE recipe
(
	id						INT		IDENTITY(1,1),
	recipe_number			INT		NOT NULL,
	recipe_book_id			INT		NOT NULL,
	metadata_id				INT		NOT NULL,
	ingredient_list_id		INT		NOT NULL,
	cooking_instructions_id	INT		NOT NULL,
	CONSTRAINT PK_recipe PRIMARY KEY (id),
	CONSTRAINT FK_recipe_recipe_book FOREIGN KEY (recipe_book_id) REFERENCES recipe_book(id),
	CONSTRAINT FK_recipe_metadata FOREIGN KEY (metadata_id) REFERENCES metadata(id),
	CONSTRAINT FK_recipe_ingredient_list FOREIGN KEY (ingredient_list_id) REFERENCES ingredient_list(id),
	CONSTRAINT FK_recipe_cooking_instructions FOREIGN KEY (cooking_instructions_id) REFERENCES cooking_instructions(id)
);