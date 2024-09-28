using AutoMapper;
using ErrorOr;
using MediatR;
using ProductAPI.Entities;
using ProductAPI.Repositories;
using ProductAPI.Resources;
using ProductAPI.Utility;

namespace ProductAPI.Handlers.Command.Update
{
    public class UpdateProductStockCommand : IRequest<ErrorOr<ProductResource>>
    {
        public int ID { get; }
        public int Stock { get; }

        public StockUpdate StockUpdateType { get;  }

        public UpdateProductStockCommand(int Id, int stock, StockUpdate stockUpdateType)
        {
            ID = Id;
            Stock = stock;
            StockUpdateType = stockUpdateType;

        }

        public class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, ErrorOr<ProductResource>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;


            public UpdateProductStockCommandHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<ErrorOr<ProductResource>> Handle(UpdateProductStockCommand request,
                CancellationToken cancellationToken)
            {
                Product product = _uow.Repository().FindQueryable<Product>(x => x.Id == request.ID).FirstOrDefault();

                if (product is null)
                {
                    return Error.NotFound();
                }
                else
                {
                    product.StockAvailable = request.StockUpdateType == StockUpdate.Add ? 
                                                product.StockAvailable + request.Stock :
                                                product.StockAvailable - request.Stock;

                    _uow.Repository().Update(_mapper.Map<Product>(product));
                    await _uow.CommitAsync(cancellationToken);
                    return _mapper.Map<ProductResource>(product);
                }
            }
        }
    }
}
