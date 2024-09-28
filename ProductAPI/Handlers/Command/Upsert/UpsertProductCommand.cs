using AutoMapper;
using ErrorOr;
using MediatR;
using ProductAPI.Entities;
using ProductAPI.Repositories;
using ProductAPI.Resources;

namespace ProductAPI.Handlers.Command.Upsert
{
    public class UpsertProductCommand : IRequest<ErrorOr<ProductResource>>
    {
        public int ID { get; set; }
        public ProductResource Product { get; }
        public UpsertProductCommand(int Id, ProductResource product)
        {
            ID = Id;
            Product = product;
        }

        public class UpsertProductCommandHandler : IRequestHandler<UpsertProductCommand, ErrorOr<ProductResource>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public UpsertProductCommandHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<ErrorOr<ProductResource>> Handle(UpsertProductCommand request,
                CancellationToken cancellationToken)
            {
                Product product = _uow.Repository().FindQueryable<Product>(x => x.Id == request.ID).FirstOrDefault();

                if (product is null)
                {
                    product = _uow.Repository().Add(_mapper.Map<Product>(request.Product));
                }
                else
                {
                    _uow.Repository().Update(_mapper.Map<Product>(request.Product));
                }
                var IsCreated = _uow.CommitAsync(cancellationToken);
                return product is null ? _mapper.Map<ProductResource>(product): request.Product;
            }
        }
    }
}
