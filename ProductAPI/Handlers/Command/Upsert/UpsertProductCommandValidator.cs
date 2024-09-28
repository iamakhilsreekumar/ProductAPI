using FluentValidation;
using ProductAPI.Resources;

namespace ProductAPI.Handlers.Command.Upsert
{
    public class UpsertProductCommandValidator : AbstractValidator<UpsertProductCommand>
    {
        public UpsertProductCommandValidator()
        {
            RuleFor(x => x.Product).NotEmpty();
            RuleFor(x => x.Product).SetValidator(new ProductValidator());
        }
    }

    public class ProductValidator : AbstractValidator<ProductResource>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.StockAvailable).NotEmpty();
            RuleFor(x => x.ProductId).MinimumLength(6)
                .WithMessage("Please enter a valid product id");
        }
    }
}
