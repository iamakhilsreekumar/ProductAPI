using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductAPI.Controllers;
using Xunit;


namespace ProductAPI.UnitTest
{

    [TestClass]
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

            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(Result.Success(productResource));

            // Act
            var result = await _controller.CreateProduct(productResource);

            // Assert
            var okResult = result as StatusCodeResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var products = new List<ProductResource> { new ProductResource { Id = 1, Name = "Product1" } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductAllQuery>(), default)).ReturnsAsync(Result.Success(products));

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = result as StatusCodeResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnOkResult_WhenProductExists()
        {
            // Arrange
            int productId = 1;
            var product = new ProductResource { Id = productId, Name = "Test Product" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default)).ReturnsAsync(Result.Success(product));

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var okResult = result as StatusCodeResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpsertProduct_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var productResource = new ProductResource { Id = 1, Name = "Updated Product" };
            var command = new UpsertProductCommand(productResource.Id, productResource);

            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(Result.Success(productResource));

            // Act
            var result = await _controller.UpsertProduct(productResource.Id, productResource);

            // Assert
            var okResult = result as StatusCodeResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            int productId = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), default)).ReturnsAsync(Result.Success());

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            var noContentResult = result as StatusCodeResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DecrementStock_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            int productId = 1;
            int quantity = 5;
            var command = new UpdateProductStockCommand(productId, quantity, StockUpdate.Dec);

            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(Result.Success());

            // Act
            var result = await _controller.DecrementStock(productId, quantity);

            // Assert
            var okResult = result as StatusCodeResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddToStock_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            int productId = 1;
            int quantity = 10;
            var command = new UpdateProductStockCommand(productId, quantity, StockUpdate.Add);

            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(Result.Success());

            // Act
            var result = await _controller.AddToStock(productId, quantity);

            // Assert
            var okResult = result as StatusCodeResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }

}


