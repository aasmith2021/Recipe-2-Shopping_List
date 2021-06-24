using System;
using System.Collections.Generic;
using System.Text;

namespace r2slapi.Models
{
    public class IngredientList
    {
        private List<Ingredient> allIngredients = new List<Ingredient>();

        public IngredientList()
        {

        }

        public int Id { get; set; }

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
