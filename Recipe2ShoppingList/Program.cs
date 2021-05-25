using System;
using System.IO;

namespace Recipe2ShoppingList
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            //Read from file
            StreamReader sr = new StreamReader("..\\RecipesDatabase.txt");
            line = sr.ReadLine();

            while (line != null)
            {
                Console.WriteLine(line);
                line = sr.ReadLine();
            }

            sr.Close();

            //Write to file
            StreamWriter sw = new StreamWriter(".\\RecipesDatabase.txt", true);
            sw.WriteLine("Whatup, World!");
            sw.WriteLine("Diggy Dog.");
            sw.Close();

        }
    }
}
