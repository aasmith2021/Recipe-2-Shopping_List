using System;
using System.Collections.Generic;
using System.Text.Json;
using RestSharp;
using System.IO;

namespace Recipe2ShoppingList
{
    public class ApiService : IDataIO
    {
        private readonly string baseApiUrl;
        private readonly IRestClient client;

        //When the ApiService is instantiated, the <baseApiUrl> is set from the appsettings.json
        //file, and the <client> is created and it's base url is set to the value of the <baseApiUrl>
        public ApiService()
        {
            baseApiUrl = GetBaseApiUrl();
            client = new RestClient(baseApiUrl);
        }

        // Get the API URL from the appsettings.json file
        private string GetBaseApiUrl()
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
            RestRequest request = new RestRequest("", DataFormat.Json);
            IRestResponse<RecipeBookLibrary> response = client.Get<RecipeBookLibrary>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error: Unable to reach server. Please try again later.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error: Unable to retrieve data from the server; error code: " + response.StatusCode);
            }
            else
            {
                return response.Data;
            }
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
