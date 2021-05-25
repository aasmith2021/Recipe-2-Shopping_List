using System;
using System.IO;

namespace Recipe2ShoppingList
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string line;

            //Read from file
            StreamReader sr = new StreamReader($"{currentDirectory}\\RecipesDatabase.txt");
            line = sr.ReadLine();

            while (line != null)
            {
                Console.WriteLine(line);
                line = sr.ReadLine();
            }

            sr.Close();


            //Write to file
            StreamWriter sw = new StreamWriter($"{currentDirectory}\\RecipesDatabase.txt", true);
            sw.WriteLine("I'm adding new text to this file!");
            sw.WriteLine();
            sw.Close();

        }
    }
}
