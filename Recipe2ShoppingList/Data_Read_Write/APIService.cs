using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace Recipe2ShoppingList.Data_Read_Write
{
    public class APIService : IDataIO
    {
        private readonly string baseApiUrl;
        private readonly IRestClient client;

        public APIService(string _baseApiUrl)
        {
            baseApiUrl = _baseApiUrl;
            client = new RestClient(baseApiUrl);
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
}
