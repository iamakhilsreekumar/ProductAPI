using FluentValidation;

namespace ProductAPI.Handlers.Queries.Products.GET
{
    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
        {
            public GetProductByIdQueryValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }
}
