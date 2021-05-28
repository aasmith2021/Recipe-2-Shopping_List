using System;
using System.IO;

namespace Recipe2ShoppingList
{
    class Program
    {
        static void Main(string[] args)
        {
            string output = RawDataFromURL.GetWebsiteDataFromURL();

            Console.WriteLine(output);
        }
    }
}
