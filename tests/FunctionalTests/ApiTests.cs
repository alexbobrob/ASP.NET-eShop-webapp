using Ganges;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Threading.Tasks;
using System.Net;
using Ganges.ApplicationCore.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Ganges.FunctionalTests
{

    // Useful information: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1

    // These tests could also be described as end-to-end functional tests.

    // TODO: improve these functional tests.

    [TestClass]
    public class ApiTests
    {

        private CustomWebApplicationFactory<Startup> _factory;

        public ApiTests()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
        }

        [DataTestMethod]
        [DataRow("/api/products")]
        [DataRow("/api/products/1")]
        [DataRow("/api/products/2")]
        [DataRow("/api/products/3")]
        public async Task TestGetRequestsRespondWithOk(string url)
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GettingAllProductsWithAGetRequest()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/products");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Product[]>(responseString);

            // Assert
            result.Length.ShouldBe(3);
            result[0].Id.ShouldBe(1);
            result[0].Title.ShouldBe("Toy");
            result[1].Id.ShouldBe(2);
            result[1].Title.ShouldBe("Book");
            result[2].Id.ShouldBe(3);
            result[2].Title.ShouldBe("Lamp");
        }

        [TestMethod]
        public async Task GettingAProductWithAGetRequest()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/products/2");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Product>(responseString);

            // Assert
            result.Id.ShouldBe(2);
            result.Title.ShouldBe("Book");
            result.Description.ShouldBe("Hard back");
            result.Seller.ShouldBe("Peter");
            result.Price.ShouldBe(25);
            result.Quantity.ShouldBe(4);
            result.ImageUrl.ShouldBe("book.png");
        }

        [TestMethod]
        public async Task AddingAProductWithAPostRequest_ShouldReturnCreatedStatusCodeAndTheCreatedProduct()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();
            var product = new Product()
            {
                Id = 0,
                Title = "Sock",
                Description = "Red",
                Seller = "Anthony",
                Price = 2,
                Quantity = 3,
                ImageUrl = "sock.png"
            };
            var productString = JsonConvert.SerializeObject(product);
            var stringContent = new StringContent(productString, Encoding.UTF8, "application/json");

            // Act
            var postResponse = await client.PostAsync("/api/products/", stringContent);
            var postResponseString = await postResponse.Content.ReadAsStringAsync();
            var postResponseProduct = JsonConvert.DeserializeObject<Product>(postResponseString);

            // Assert
            postResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
            postResponseProduct.Id.ShouldNotBe(0);
            postResponseProduct.Title.ShouldBe(product.Title);
            postResponseProduct.Description.ShouldBe(product.Description);
            postResponseProduct.Seller.ShouldBe(product.Seller);
            postResponseProduct.Price.ShouldBe(product.Price);
            postResponseProduct.Quantity.ShouldBe(product.Quantity);
            postResponseProduct.ImageUrl.ShouldBe(product.ImageUrl);
        }

        [TestMethod]
        public async Task AddingAProductWithAPostRequest_ShouldEnableUsToGetTheSameProductWithAGetRequest()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();
            var product = new Product()
            {
                Id = 0,
                Title = "Sock",
                Description = "Red",
                Seller = "Anthony",
                Price = 2,
                Quantity = 3,
                ImageUrl = "sock.png"
            };
            var productString = JsonConvert.SerializeObject(product);
            var stringContent = new StringContent(productString, Encoding.UTF8, "application/json");

            // Act
            var postResponse = await client.PostAsync("/api/products/", stringContent);
            var postResponseString = await postResponse.Content.ReadAsStringAsync();
            var postResponseProduct = JsonConvert.DeserializeObject<Product>(postResponseString);

            var getResponse = await client.GetAsync("/api/products/" + postResponseProduct.Id);
            var getResponseString = await getResponse.Content.ReadAsStringAsync();
            var getResponseProduct = JsonConvert.DeserializeObject<Product>(getResponseString);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            getResponseProduct.Id.ShouldBe(postResponseProduct.Id);
            getResponseProduct.Title.ShouldBe(product.Title);
            getResponseProduct.Description.ShouldBe(product.Description);
            getResponseProduct.Seller.ShouldBe(product.Seller);
            getResponseProduct.Price.ShouldBe(product.Price);
            getResponseProduct.Quantity.ShouldBe(product.Quantity);
            getResponseProduct.ImageUrl.ShouldBe(product.ImageUrl);
        }

        [TestMethod]
        public async Task UpdatingAProductWithAPutRequest_ShouldReturnOkStatusCodeAndTheUpdatedProduct()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();
            var product = new Product()
            {
                Id = 1,
                Title = "New Title",
                Description = "New Description",
                Seller = "New Seller",
                Price = 100,
                Quantity = 20,
                ImageUrl = "new_image.png"
            };
            var productString = JsonConvert.SerializeObject(product);
            var stringContent = new StringContent(productString, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync("/api/products/", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Product>(responseString);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            result.Id.ShouldBe(product.Id);
            result.Title.ShouldBe(product.Title);
            result.Description.ShouldBe(product.Description);
            result.Seller.ShouldBe(product.Seller);
            result.Price.ShouldBe(product.Price);
            result.Quantity.ShouldBe(product.Quantity);
            result.ImageUrl.ShouldBe(product.ImageUrl);
        }

        [TestMethod]
        public async Task Test()
        {
            // Arrange
            CustomWebApplicationFactory<Startup> factory
                = new CustomWebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/products");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
