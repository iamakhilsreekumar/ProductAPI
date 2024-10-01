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
            var productResource = new ProductResource { Id = 1, Name = "Test Product", Price = 10.0M };
            var command = new UpsertProductCommand(productResource.Id, productResource);

            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(productResource);

            var result = await _controller.CreateProduct(productResource);

            var okResult = result as ObjectResult;
            Assert.Equal(okResult?.StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOkResult_WhenSuccessful()
        {
            var products = new List<ProductResource> { new ProductResource { Id = 1, Name = "Product1" } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductAllQuery>(), default)).ReturnsAsync(products);

            var result = await _controller.GetProducts();

            var okResult = result as ObjectResult;
            Assert.Equal(okResult?.StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnOkResult_WhenProductExists()
        {
            int productId = 1;
            var product = new ProductResource { Id = productId, Name = "Test Product" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default)).ReturnsAsync(product);

            var result = await _controller.GetProduct(productId);
            
            var okResult = result as ObjectResult;
            Assert.Equal(okResult?.StatusCode, (int)HttpStatusCode.OK);
        }
    }
}
