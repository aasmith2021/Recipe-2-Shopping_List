using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class Ingredients
    {
        private List<Ingredient> allIngredients = new List<Ingredient>();

        public Ingredient[] AllIngredients
        {
            get { return this.allIngredients.ToArray(); }
        }

        public void AddIngredient(Ingredient newIngredient)
        {
            allIngredients.Add(newIngredient);
        }

        public void DeleteIngredient(Ingredient ingredientToDelete)
        {
            allIngredients.Remove(ingredientToDelete);
        }
    }
}
