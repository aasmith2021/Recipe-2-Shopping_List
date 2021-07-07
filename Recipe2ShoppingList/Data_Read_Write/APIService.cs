using System;
using System.Collections.Generic;
using System.Text.Json;
using RestSharp;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Recipe2ShoppingList
{
    public class ApiService : IDataIO
    {
        private readonly string baseApiUrl;
        private readonly IRestClient client;

        public ApiService()
        {
            baseApiUrl = GetBaseApiUrl();
            client = new RestClient(baseApiUrl);
        }

        // Get the API URL from the appsettings.json file
        public static string GetBaseApiUrl()
        {
            string apiUrl = "";

            try
            {
                string appsettingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

                using (StreamReader sr = new StreamReader(appsettingsFilePath))
                {
                    string json = sr.ReadToEnd();
                    ApiBaseUrl apiBaseUrl = JsonSerializer.Deserialize<ApiBaseUrl>(json);
                    apiUrl = apiBaseUrl.ApiUrl;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to access API URL to run program.");
            }

            return apiUrl;
        }

        public RecipeBookLibrary GetRecipeBookLibraryFromDataSource(string alternatePath = "")
        {
            throw new NotImplementedException();
        }

        public void WriteRecipeBookLibraryToDataSource(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, string alternatePath = "")
        {
            throw new NotImplementedException();
        }

        public void WriteShoppingListToDataSource(IUserIO userIO, ShoppingList shoppingList, string alternatePath = "")
        {
            throw new NotImplementedException();
        }
    }
    
    //Used to deserialize the json data from the appsettings.json file to get the base API URL
    public class ApiBaseUrl
    {
        public string ApiUrl { get; set; }
    }
}
