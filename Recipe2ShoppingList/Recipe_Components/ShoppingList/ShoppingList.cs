using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recipe2ShoppingList
{
    public class ShoppingList
    {
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

        public string GetEntireShoppingList(bool includeHeader = false)
        {
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
            string entireShoppingList = includeHeader ? optionalHeader : "";

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

        public void WriteShoppingListToFile(IUserIO userIO, bool includeHeader = true, string alternateFilePath = "")
        {
            string entireShoppingList = this.GetEntireShoppingList(includeHeader);

            try
            {
                using (StreamWriter sw = new StreamWriter(FileDataHelperMethods.GetWriteShoppingListFilePath(alternateFilePath)))
                {
                    sw.WriteLine(entireShoppingList);
                }
            }
            catch (IOException exception)
            {
                userIO.DisplayData("Cannot open Shopping List file to save data.");
                userIO.DisplayData();
                userIO.DisplayData("Press \"Enter\" to continue...");
                userIO.GetInput();
            }
        }
    }
}
