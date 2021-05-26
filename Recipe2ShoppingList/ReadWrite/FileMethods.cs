using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recipe2ShoppingList
{
    class FileMethods
    {
        protected static string GetDatabaseFilePath()
        {
            string filePath = Directory.GetCurrentDirectory();
            filePath += "\\Recipe_Database.txt";

            return filePath;
        }
    }
}
