using System;
using System.Collections.Generic;
using System.Text.Json;
using RestSharp;
using System.IO;

namespace Recipe2ShoppingList
{
    //This is a client-side API used to send REST requests to the server-side API in order to peform CRUD operations on the recipe data
    public class ApiIO : IDataIO
    {
        private readonly string baseApiUrl;
        private readonly IRestClient client;

        //When the ApiService is instantiated, the <baseApiUrl> is set from the appsettings.json
        //file, and the <client> is created and it's base url is set to the value of the <baseApiUrl>
        public ApiIO()
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
                throw new Exception($"Unable to connect to server. Please try again later. Error code: {response.StatusCode}", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception($"Error: Unable to retrieve data from the server. Error code: {response.StatusCode}");
            }
            else
            {
                return response.Data;
            }
        }

        public bool WriteRecipeBookLibraryToDataSource(IUserIO userIO, RecipeBookLibrary recipeBookLibrary, string alternatePath = "")
        {
            bool writeSuccessful = false;
            RestRequest request = new RestRequest("", DataFormat.Json);
            request.AddJsonBody(recipeBookLibrary);

            IRestResponse response = client.Put(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception($"Unable to connect to server. Error code: {response.StatusCode}", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception($"Error: Unable to retrieve data from the server. Error code: {response.StatusCode}");
            }
            else
            {
                writeSuccessful = true;
                return writeSuccessful;
            }
        }
    }
    
    //Used to deserialize the json data from the appsettings.json file to get the base API URL
    public class ApiBaseUrl
    {
        public string ApiUrl { get; set; }
    }
}
