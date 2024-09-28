using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Handlers.Command;
using ProductAPI.Handlers.Command.Update;
using ProductAPI.Handlers.Command.Upsert;
using ProductAPI.Handlers.Queries.Products;
using ProductAPI.Handlers.Queries.Products.GET;
using ProductAPI.Resources;
using ProductAPI.Utility;
using System.Net;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiController
    {
        private readonly ISender _mediator;

        public ProductsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductResource product)
        {
            var query = new UpsertProductCommand(product.Id, product);
            var result = await _mediator.Send(query);

            return result.Match(resp => StatusCode((int)HttpStatusCode.OK, resp),
               errors => Problem(errors));
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var query = new GetProductAllQuery();
            var result = await _mediator.Send(query);

            return result.Match(resp => StatusCode((int)HttpStatusCode.OK, resp),
               errors => Problem(errors));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(ErrorOr.Error))]
        public async Task<IActionResult> GetProduct(int id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);

            return result.Match(resp => StatusCode((int)HttpStatusCode.OK, resp),
               errors => Problem(errors));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpsertProduct(int id,[FromBody] ProductResource product)
        {
            var query = new UpsertProductCommand(id,product);
            var result = await _mediator.Send(query);

            return result.Match(resp => StatusCode((int)HttpStatusCode.OK, resp),
               errors => Problem(errors));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            
            var query = new DeleteProductCommand(id);
            var result = await _mediator.Send(query);

            return result.Match(resp => StatusCode((int)HttpStatusCode.OK, resp),
               errors => Problem(errors));
        }

        [HttpPut("decrement-stock/{id}/{quantity}")]
        public async Task<IActionResult> DecrementStock(int id, int quantity)
        {
            var query = new UpdateProductStockCommand(id,quantity,StockUpdate.Decrement);
            var result = await _mediator.Send(query);

            return result.Match(resp => StatusCode((int)HttpStatusCode.OK, resp),
               errors => Problem(errors));
        }

        [HttpPut("add-to-stock/{id}/{quantity}")]
        public async Task<IActionResult> AddToStock(int id, int quantity)
        {
            var query = new UpdateProductStockCommand(id, quantity, StockUpdate.Add);
            var result = await _mediator.Send(query);

            return result.Match(resp => StatusCode((int)HttpStatusCode.OK, resp),
               errors => Problem(errors));
        }
    }
}
