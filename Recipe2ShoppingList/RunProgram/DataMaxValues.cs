using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public static class DataMaxValues
    {
        //This class sets the maximum values (number of characters/length for strings, maximum numeric values
        //for quantities, maximum numbers of ingredients in a recipe, instruction blocks in a set of cooking
        //instructions, etc.

        //These are important to ensure that data being saved to the database meets the size of data the database
        //is expecting.        
        
        //<<< Recipe Book >>>
        public const int RECIPE_BOOK_NAME_LENGTH = 120;

        //<<< Recipe >>>
        //Metadata
        public const int RECIPE_TITLE_LENGTH = 200;
        public const int USER_NOTES_LENGTH = 1200;
        public const int FOOD_TYPE_LENGTH = 100;
        public const int FOOD_GENRE_LENGTH = 100;
        public const int MAX_PREP_TIME = 2880;
        public const int MAX_COOK_TIME = 1440;
        public const int MAX_LOW_SERVINGS = 500;
        public const int MAX_HIGH_SERVINGS = 500;

        //Ingredients
        public const int NUMBER_OF_INGREDIENTS = 30;
        public const int INGREDIENT_QUANTITY = 1000;
        public const int INGREDIENT_NAME_LENGTH = 100;
        public const int INGREDIENT_PREP_NOTE_LENGTH = 120;

        //Cooking Instructions
        public const int NUMBER_OF_INSTRUCTION_BLOCKS = 5;
        public const int BLOCK_HEADING_LENGTH = 100;
        public const int NUMBER_OF_INSTRUCTION_LINES = 20;
        public const int INSTRUCTION_LINE_LENGTH = 360;

        //<<< Measurement Units >>>
        public const int MEASUREMENT_UNIT_NAME_LENGTH = 30;
    }
}
