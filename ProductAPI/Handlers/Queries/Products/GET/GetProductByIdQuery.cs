using AutoMapper;
using ErrorOr;
using MediatR;
using ProductAPI.Entities;
using ProductAPI.Repositories;
using ProductAPI.Resources;

namespace ProductAPI.Handlers.Queries.Products.GET
{
    public class GetProductByIdQuery : IRequest<ErrorOr<ProductResource>>
    {
        public int Id { get; }
        public GetProductByIdQuery(int id)
        {
            Id = id;
        }

        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductResource>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public GetProductByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<ErrorOr<ProductResource>> Handle(GetProductByIdQuery request,
                CancellationToken cancellationToken)
            {
                var product = await _uow.Repository().GetById<Product>(request.Id);
                if (product is null)
                    return Error.NotFound(code: "product not found", description: "Please enter the existing product Id");

                return _mapper.Map<ProductResource>(product);
            }
        }

    }
}
