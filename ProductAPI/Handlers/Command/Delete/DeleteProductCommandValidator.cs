using FluentValidation;
using ProductAPI.Resources;

namespace ProductAPI.Handlers.Command
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ID).NotEmpty();
        }


    }

    public class ProductValidator : AbstractValidator<ProductResource>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.StockAvailable).NotEmpty();
        }
    }
}
