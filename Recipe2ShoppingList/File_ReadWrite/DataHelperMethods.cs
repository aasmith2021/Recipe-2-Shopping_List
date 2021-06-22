using System.IO;

namespace Recipe2ShoppingList
{
    public static class DataHelperMethods
    {
        public static string GetReadDatabaseFilePath(string alternateFilePath = "")
        {
            string filePath = Directory.GetCurrentDirectory();

            if(alternateFilePath == "")
            {
                filePath = Path.Combine(filePath, "Recipe_Database.txt");
            }
            else
            {
                filePath = Path.Combine($"{alternateFilePath}.txt");
            }

            return filePath;
        }

        public static string GetWriteDatabaseFilePath(string alternateFilePath = "")
        {
            string filePath = Directory.GetCurrentDirectory();

            if (alternateFilePath == "")
            {
                filePath = Path.Combine(filePath, "Recipe_Database_write.txt");
            }
            else
            {
                filePath = Path.Combine(filePath, $"{alternateFilePath}.txt");
            }

            return filePath;
        }

        public static string GetReadShoppingListFilePath(string alternateFilePath = "")
        {
            string filePath = Directory.GetCurrentDirectory();

            if (alternateFilePath == "")
            {
                filePath = Path.Combine(filePath, "Shopping_List.txt");
            }
            else
            {
                filePath = Path.Combine(filePath, $"{alternateFilePath}.txt");
            }

            return filePath;
        }

        public static string GetWriteShoppingListFilePath(string alternateFilePath = "")
        {
            string filePath = Directory.GetCurrentDirectory();

            if (alternateFilePath == "")
            {
                filePath = Path.Combine(filePath, "Shopping_List_write.txt");
            }
            else
            {
                filePath = Path.Combine(filePath, $"{alternateFilePath}.txt");
            }

            return filePath;
        }

        public static string GetAllDatabaseText(string alternateFilePath = "")
        {            
            string databaseText = "";
            string currentLineOfText = "";

            //Try-Catch the exception for starting the program when the database file doesn't
            //exist yet.
            try
            {
                using (StreamReader sr = new StreamReader(GetReadDatabaseFilePath(alternateFilePath)))
                {

                    while (!sr.EndOfStream)
                    {
                        currentLineOfText = sr.ReadLine();
                        databaseText += currentLineOfText;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
            }

            return databaseText;
        }

        public static void DeleteOldFileAndRenameNewFile(string oldFilePath, string newFilePath)
        {
            File.Delete(oldFilePath);
            File.Move(newFilePath, oldFilePath);
        }

        public static string GetDataFromStartAndEndMarkers(string data, string startMarker, string endMarker)
        {
            int startIndexOfReturnData = data.IndexOf(startMarker) + startMarker.Length;
            int startIndexOfEndMarker = data.IndexOf(endMarker);
            int lengthOfReturnData = startIndexOfEndMarker - startIndexOfReturnData;

            string returnData = "";

            if (lengthOfReturnData >= 0)
            {
                returnData = data.Substring(startIndexOfReturnData, lengthOfReturnData);
            }

            return returnData;
        }
    }
}
