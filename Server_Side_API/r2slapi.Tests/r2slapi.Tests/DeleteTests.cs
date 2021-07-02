using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using r2slapi.Tests.Models;

namespace r2slapi.Tests
{
    [TestClass]
    public class DeleteTests
    {
        private static readonly string API_URL = "https://localhost:44302/recipes";
        private static readonly IRestClient client = new RestClient();

        [TestMethod]
        public void DELETE_recipe_deletes_recipe()
        {
            //Arrange
            RestRequest deleteRequest = new RestRequest(API_URL + "/4/4", DataFormat.Json);
            RestRequest getRequest = new RestRequest(API_URL + "/4/4", DataFormat.Json);

            //Act
            IRestResponse<Recipe> beforeGetResponse = client.Get<Recipe>(getRequest);
            IRestResponse deleteResponse = client.Delete(deleteRequest);
            IRestResponse<Recipe> afterGetResponse = client.Get<Recipe>(getRequest);

            //Assert
            Assert.AreEqual((int)beforeGetResponse.StatusCode, 200, "Status code of GET method before delete was not 200");
            Assert.AreEqual((int)afterGetResponse.StatusCode, 404, "Status code of GET method after delete was not 404");
        }

        [TestMethod]
        public void DELETE_recipe_book_deletes_recipe_book()
        {
            //Arrange
            RestRequest deleteRequest = new RestRequest(API_URL + "/3", DataFormat.Json);
            RestRequest getRequest = new RestRequest(API_URL + "/3", DataFormat.Json);

            //Act
            IRestResponse<RecipeBook> beforeGetResponse = client.Get<RecipeBook>(getRequest);
            IRestResponse deleteResponse = client.Delete(deleteRequest);
            IRestResponse<RecipeBook> afterGetResponse = client.Get<RecipeBook>(getRequest);

            //Assert
            Assert.AreEqual((int)beforeGetResponse.StatusCode, 200, "Status code of GET method before delete was not 200");
            Assert.AreEqual((int)afterGetResponse.StatusCode, 404, "Status code of GET method after delete was not 404");
        }

        [TestMethod]
        public void DELETE_recipe_for_nonexistent_recipe_returns_404_error()
        {
            //Arrange
            RestRequest request = new RestRequest(API_URL + "/100/100", DataFormat.Json);

            //Act
            IRestResponse response = client.Delete(request);

            //Assert
            Assert.AreEqual((int)response.StatusCode, 404, "Status code of deleting nonexistent recipe was not 404");
        }

        [TestMethod]
        public void DELETE_recipe_book_for_nonexistent_recipe_book_returns_404_error()
        {
            //Arrange
            RestRequest request = new RestRequest(API_URL + "/100", DataFormat.Json);

            //Act
            IRestResponse response = client.Delete(request);

            //Assert
            Assert.AreEqual((int)response.StatusCode, 404, "Status code of deleting nonexistent recipe book was not 404");
        }
    }
}
