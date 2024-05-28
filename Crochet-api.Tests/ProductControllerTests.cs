
using Crochet_api.Controllers;
using Crochet_api.Data;
using Crochet_api.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using static System.Net.Mime.MediaTypeNames;


namespace Crochet_api.Tests
{
    public class ProductControllerTests
    {

        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Products.CountAsync() <= 0)
            {
                for (int i = 1; i <= 11; i++)
                {
                    Products product = new Products()
                    {
                        ProductID = i,
                        Name = "Test Product",
                        Type = "Test",
                        Price = 99.99,
                        Description_Small = "Small description",
                        Description_Long = "Long Description"
                    };
                    databaseContext.Products.Add(product);
                    await databaseContext.SaveChangesAsync();
                }
            }
            if (await databaseContext.Images.CountAsync() <= 0)
            {
                Images image = new Images
                {
                    ImageID = 1,
                    ProductID = 1,
                    ImageURL = "test.png"
                };
                databaseContext.Images.Add(image);
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

        [Fact]
        public async void Products_GetAll_ReturnsProducts()
        {
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsController(dbContext);

            var result = controller.GetAll();

            result.Should().NotBeNull();
            Assert.IsType<List<Products>>(result);
        }

        [Fact]
        public async void Products_GetAll_ReturnsTestProduct()
        {
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsController(dbContext);

            var result = controller.GetAll();

            result.Should().NotBeNull();
            result.Should().Contain(x => x.Name == "Test Product");
        }

        [Fact]
        public async void Products_GetTypes_ReturnsTestType()
        {
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsController(dbContext);

            var result = controller.GetTypes();

            result.Should().NotBeNull();
            result.Should().Contain(x => x.Type == "Test");
            result.Should().OnlyHaveUniqueItems();
            List<ProductsTypeCount> expectedResult = new List<ProductsTypeCount>();
            expectedResult?.Add(new ProductsTypeCount { Type = "Test", Count = 11 });
            result.Should().Equal(expectedResult);
        }

        [Fact]
        public async void Products_GetProductByID_ReturnsProduct1()
        {
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsController(dbContext);

            var result = controller.GetProductByID(1);
            Products expectedResult = new Products
            {
                ProductID = 1,
                Name = "Test Product",
                Type = "Test",
                Price = 99.99,
                Description_Small = "Small description",
                Description_Long = "Long Description"
            };
            result.Should().NotBeNull();
            Assert.IsType<Products>(result);
            result.Equals(expectedResult);
        }

        [Fact]
        public async void Products_getProductDetailsByType_ReturnsDetailedProducts()
        {
            var dbContext = await GetDatabaseContext();
            var controller = new ProductsController(dbContext);

            List<DetailedProducts> expectedResult = new List<DetailedProducts>();
            List<Images> images = new List<Images>() {
                new Images()
                {
                    ImageID = 1,
                    ProductID = 1,
                    ImageURL = "test.png"
                }
            };
            expectedResult.Add(new DetailedProducts()
            {
                ProductID = 1,
                Name = "Test Product",
                Type = "Test",
                Price = 99.99,
                Description_Small = "Small description",
                Description_Long = "Long Description",
                Images = images
            });


            var result = controller.getProductDetailsByType("Test");

            result.Should().NotBeNull();
            Assert.IsType<List<DetailedProducts>>(result);
            result.Equals(expectedResult);

        }
    }
}
