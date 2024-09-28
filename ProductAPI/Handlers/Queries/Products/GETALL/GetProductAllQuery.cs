using AutoMapper;
using ErrorOr;
using MediatR;
using ProductAPI.Entities;
using ProductAPI.Repositories;
using ProductAPI.Resources;
using System;

namespace ProductAPI.Handlers.Queries.Products
{
    public class GetProductAllQuery : IRequest<ErrorOr<List<ProductResource>>>
    {
        public class GetProductAllQueryHandler : IRequestHandler<GetProductAllQuery, ErrorOr<List<ProductResource>>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public GetProductAllQueryHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<ErrorOr<List<ProductResource>>> Handle(GetProductAllQuery request,
                CancellationToken cancellationToken)
            {
                var product = await _uow.Repository().FindAllAsync<Product>(cancellationToken);
                if (product is null)
                    return Error.NotFound(code: "product not found", description: "Please enter the existing product Id");

                return _mapper.Map<List<ProductResource>>(product);
            }
        }

    }
}
