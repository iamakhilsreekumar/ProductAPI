using FluentValidation;
using ProductAPI.Resources;

namespace ProductAPI.Handlers.Command.Update
{
    public class UpdateProductStockCommandValidator : AbstractValidator<UpdateProductStockCommand>
    {
        public UpdateProductStockCommandValidator()
        {
            RuleFor(x => x.Stock).NotEmpty();
            RuleFor(x => x.StockUpdateType).NotEmpty();
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
