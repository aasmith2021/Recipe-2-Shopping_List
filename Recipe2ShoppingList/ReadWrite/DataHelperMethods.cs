using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Recipe2ShoppingList
{
    public abstract class DataHelperMethods
    {
        protected static string GetDatabaseFilePath(string alternateFilePath = "")
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

        protected static string GetAllDatabaseText(string alternateFilePath = "")
        {
            string databaseText = "";
            StreamReader sr = new StreamReader(GetDatabaseFilePath(alternateFilePath));
            string currentLineOfText = sr.ReadLine();

            while (currentLineOfText != null)
            {
                databaseText += currentLineOfText;
                currentLineOfText = sr.ReadLine();
            }
            sr.Close();

            return databaseText;
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

    }
}
