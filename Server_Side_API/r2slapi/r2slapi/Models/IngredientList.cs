using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace r2slapi.Models
{
    public class IngredientList
    {
        public int Id { get; set; }

        [MaxLength(30, ErrorMessage = "A recipe cannot have more than 30 ingredients.")]
        public List<Ingredient> AllIngredients { get; set; } = new List<Ingredient>();
 
        public void AddIngredient(Ingredient newIngredient)
        {
            AllIngredients.Add(newIngredient);
        }

        public void DeleteIngredient(Ingredient ingredientToDelete)
        {
            AllIngredients.Remove(ingredientToDelete);
        }
    }
}
