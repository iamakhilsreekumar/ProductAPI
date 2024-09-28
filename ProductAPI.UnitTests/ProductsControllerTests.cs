using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductAPI.Controllers;
using ProductAPI.Handlers.Command.Upsert;
using ProductAPI.Handlers.Queries.Products;
using ProductAPI.Handlers.Queries.Products.GET;
using ProductAPI.Resources;
using System.Net;

namespace ProductAPI.UnitTests
{
    public class ProductsControllerTests
    {
        private readonly Mock<ISender> _mediatorMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mediatorMock = new Mock<ISender>();
            _controller = new ProductsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnCreatedResult_WhenSuccessful()
        {
            // Arrange
            var productResource = new ProductResource { Id = 1, Name = "Test Product", Price = 10.0M };
            var command = new UpsertProductCommand(productResource.Id, productResource);

            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(productResource);

            // Act
            var result = await _controller.CreateProduct(productResource);

            // Assert
            var okResult = result as ObjectResult;
            Assert.Equal(okResult?.StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var products = new List<ProductResource> { new ProductResource { Id = 1, Name = "Product1" } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductAllQuery>(), default)).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = result as ObjectResult;
            Assert.Equal(okResult?.StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnOkResult_WhenProductExists()
        {
            // Arrange
            int productId = 1;
            var product = new ProductResource { Id = productId, Name = "Test Product" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var okResult = result as ObjectResult;
            Assert.Equal(okResult?.StatusCode, (int)HttpStatusCode.OK);
        }
    }
}