using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recipe2ShoppingList
{
    //This is the shopping list that ingredients are added to in the program
    public class ShoppingList
    {
        //These represent the collection of ingredients in each location of a grocery store
        private List<Ingredient> produce = new List<Ingredient>();
        private List<Ingredient> bakeryDeli = new List<Ingredient>();
        private List<Ingredient> dryGoods = new List<Ingredient>();
        private List<Ingredient> meat = new List<Ingredient>();
        private List<Ingredient> refrigerated = new List<Ingredient>();
        private List<Ingredient> frozen = new List<Ingredient>();
        private List<Ingredient> nonGrocery = new List<Ingredient>();

        public Ingredient[] Produce
        {
            get
            {
                return this.produce.ToArray();
            }
        }

        public Ingredient[] BakeryDeli
        {
            get
            {
                return this.bakeryDeli.ToArray();
            }
        }

        public Ingredient[] DryGoods
        {
            get
            {
                return this.dryGoods.ToArray();
            }
        }

        public Ingredient[] Meat
        {
            get
            {
                return this.meat.ToArray();
            }
        }

        public Ingredient[] Refrigerated
        {
            get
            {
                return this.refrigerated.ToArray();
            }
        }

        public Ingredient[] Frozen
        {
            get
            {
                return this.frozen.ToArray();
            }
        }

        public Ingredient[] NonGrocery
        {
            get
            {
                return this.nonGrocery.ToArray();
            }
        }

        public string[] StoreLocations
        {
            get
            {
                string[] allStoreLocations = new string[]
                {
                    "Produce",
                    "Bakery/Deli",
                    "Dry Goods",
                    "Meat",
                    "Refrigerated",
                    "Frozen",
                    "Non-Grocery"
                };

                return allStoreLocations;
            }
        }

        //Produces the text of the shopping list to be displayed to the user
        public string GetEntireShoppingList(bool includeHeader = false)
        {   
            //Creates a dictionary that maps the names of each store location to the array of ingredients
            //set to that store location in the shopping list
            Dictionary<string, Ingredient[]> allLocations = new Dictionary<string, Ingredient[]>();
            allLocations["produce"] = this.Produce;
            allLocations["bakeryDeli"] = this.BakeryDeli;
            allLocations["dryGoods"] = this.DryGoods;
            allLocations["meat"] = this.Meat;
            allLocations["refrigerated"] = this.Refrigerated;
            allLocations["frozen"] = this.Frozen;
            allLocations["nonGrocery"] = this.NonGrocery;

            string key = "";
            string optionalHeader = $"---------- SHOPPING LIST ----------{Environment.NewLine}{Environment.NewLine}";

            //This creates the entireShoppingList string and adds the optional header if includeHeader is "true"
            string entireShoppingList = includeHeader ? optionalHeader : "";

            //This loop goes through all the store locations and sets the value of "key" to the name of that store location
            //based upon the array index that location represents in this.StoreLocations
            for (int i = 0; i < this.StoreLocations.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        key = "produce";
                        break;
                    case 1:
                        key = "bakeryDeli";
                        break;
                    case 2:
                        key = "dryGoods";
                        break;
                    case 3:
                        key = "meat";
                        break;
                    case 4:
                        key = "refrigerated";
                        break;
                    case 5:
                        key = "frozen";
                        break;
                    case 6:
                        key = "nonGrocery";
                        break;
                    default:
                        break;
                }

                //If a grocery store location doesn't have any ingredients in it, continue. Otherwise, print out the store location
                //and all the ingredients found in that location.
                if (allLocations[key].Length == 0)
                {
                    continue;
                }
                else
                {

                    entireShoppingList += $"----- {this.StoreLocations[i]} -----{Environment.NewLine}";

                    foreach (Ingredient ingredient in allLocations[key])
                    {
                        string measurementUnitString = ingredient.MeasurementUnit == "" ? "" : $"{ingredient.MeasurementUnit} ";
                        string ingredientQuantity = $"{Math.Round(ingredient.Quantity,3)} ";

                        entireShoppingList += $"{ingredientQuantity}{measurementUnitString}{ingredient.Name}{Environment.NewLine}";
                    }

                    entireShoppingList += Environment.NewLine;
                }
            }

            return entireShoppingList;
        }
        
        public void AddProduce(Ingredient newItem)
        {
            this.produce.Add(newItem);
        }

        public void AddBakeryDeli(Ingredient newItem)
        {
            this.bakeryDeli.Add(newItem);
        }

        public void AddDryGoods(Ingredient newItem)
        {
            this.dryGoods.Add(newItem);
        }

        public void AddMeat(Ingredient newItem)
        {
            this.meat.Add(newItem);
        }

        public void AddRefrigerated(Ingredient newItem)
        {
            this.refrigerated.Add(newItem);
        }

        public void AddFrozen(Ingredient newItem)
        {
            this.frozen.Add(newItem);
        }

        public void AddNonGrocery(Ingredient newItem)
        {
            this.nonGrocery.Add(newItem);
        }

        public void UpdateProduce(Ingredient updatedItem, int indexOfItemToUpdate)
        {
            this.produce.RemoveAt(indexOfItemToUpdate);
            this.produce.Insert(indexOfItemToUpdate, updatedItem);
        }

        public void UpdateBakeryDeli(Ingredient updatedItem, int indexOfItemToUpdate)
        {
            this.bakeryDeli.RemoveAt(indexOfItemToUpdate);
            this.bakeryDeli.Insert(indexOfItemToUpdate, updatedItem);
        }

        public void UpdateDryGoods(Ingredient updatedItem, int indexOfItemToUpdate)
        {
            this.dryGoods.RemoveAt(indexOfItemToUpdate);
            this.dryGoods.Insert(indexOfItemToUpdate, updatedItem);
        }
        public void UpdateMeat(Ingredient updatedItem, int indexOfItemToUpdate)
        {
            this.meat.RemoveAt(indexOfItemToUpdate);
            this.meat.Insert(indexOfItemToUpdate, updatedItem);
        }

        public void UpdateRefrigerated(Ingredient updatedItem, int indexOfItemToUpdate)
        {
            this.refrigerated.RemoveAt(indexOfItemToUpdate);
            this.refrigerated.Insert(indexOfItemToUpdate, updatedItem);
        }

        public void UpdateFrozen(Ingredient updatedItem, int indexOfItemToUpdate)
        {
            this.frozen.RemoveAt(indexOfItemToUpdate);
            this.frozen.Insert(indexOfItemToUpdate, updatedItem);
        }

        public void UpdateNonGrocery(Ingredient updatedItem, int indexOfItemToUpdate)
        {
            this.nonGrocery.RemoveAt(indexOfItemToUpdate);
            this.nonGrocery.Insert(indexOfItemToUpdate, updatedItem);
        }
    }
}
