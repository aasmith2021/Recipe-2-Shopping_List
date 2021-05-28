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

        public static string GetWebsiteDataFromURL(string url)
        {
            WebClient webClient = new WebClient();

            string websiteData = webClient.DownloadString(url);

            return websiteData;
        }

    }
}
