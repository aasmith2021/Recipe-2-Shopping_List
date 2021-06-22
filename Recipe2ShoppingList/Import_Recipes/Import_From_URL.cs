using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Recipe2ShoppingList
{
    class Import_From_URL
    {
        public static string GetWebsiteDataFromURL(string url)
        {
            WebClient webClient = new WebClient();

            string websiteData = webClient.DownloadString(url);

            return websiteData;
        }

        //<<<Stuff I was using to get data from a URL>>>

        //userIO.DisplayData("Enter a URL:");
        //string url = userIO.GetInput();
        //string output = DataHelperMethods.GetWebsiteDataFromURL(url);
        ////string[] splitSeparators = { "\"", ",", ":" };
        //string splitOutput = output.Substring(output.IndexOf("prep"), 1200);//.Split(splitSeparators, StringSplitOptions.RemoveEmptyEntries);

        //userIO.DisplayData(splitOutput);

        //foreach (string myString in splitOutput)
        //{
        //    userIO.DisplayData(myString);
        //}
    }
}
