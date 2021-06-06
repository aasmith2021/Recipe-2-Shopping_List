using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Recipe2ShoppingList
{
    public abstract class DataHelperMethods
    {
        public static string GetReadDatabaseFilePath(string alternateFilePath = "")
        {
            string filePath = Directory.GetCurrentDirectory();

            if(alternateFilePath == "")
            {
                filePath += "\\Recipe_Database.txt";
            }
            else
            {
                filePath += $"\\{alternateFilePath}.txt";
            }

            return filePath;
        }

        public static string GetWriteDatabaseFilePath(string alternateFilePath = "")
        {
            string filePath = Directory.GetCurrentDirectory();

            if (alternateFilePath == "")
            {
                filePath += "\\Recipe_Database_write.txt";
            }
            else
            {
                filePath += $"\\{alternateFilePath}.txt";
            }

            return filePath;
        }

        protected static string GetAllDatabaseText(string alternateFilePath = "")
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

        public static void DeleteOldDatabaseFileAndRenameNewDatabase(string oldDatabasePath, string newDatabasePath)
        {
            File.Delete(oldDatabasePath);
            File.Move(newDatabasePath, oldDatabasePath);
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

        public static string GetWebsiteDataFromURL(string url)
        {
            WebClient webClient = new WebClient();

            string websiteData = webClient.DownloadString(url);

            return websiteData;
        }

        //<<<Stuff I was using to get data from a URL>>>

        //Console.WriteLine("Enter a URL:");
        //string url = Console.ReadLine();
        //string output = DataHelperMethods.GetWebsiteDataFromURL(url);
        ////string[] splitSeparators = { "\"", ",", ":" };
        //string splitOutput = output.Substring(output.IndexOf("prep"), 1200);//.Split(splitSeparators, StringSplitOptions.RemoveEmptyEntries);

        //Console.WriteLine(splitOutput);

        //foreach (string myString in splitOutput)
        //{
        //    Console.WriteLine(myString);
        //}

    }
}
