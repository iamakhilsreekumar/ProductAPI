using AutoMapper;
using ErrorOr;
using MediatR;
using ProductAPI.Entities;
using ProductAPI.Repositories;

namespace ProductAPI.Handlers.Command
{
    public class DeleteProductCommand : IRequest<ErrorOr<bool>>
    {
        public int ID { get; set; }
        public DeleteProductCommand(int Id)
        {
            ID = Id;
        }

        public class UpsertProductCommandHandler : IRequestHandler<DeleteProductCommand, ErrorOr<bool>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public UpsertProductCommandHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<ErrorOr<bool>> Handle(DeleteProductCommand request,
                CancellationToken cancellationToken)
            {
                Product product = _uow.Repository().FindQueryable<Product>(x => x.Id == request.ID).FirstOrDefault();

                if (product is null)
                {
                    return Error.NotFound(code: "product not found", description: "Please enter the existing product Id");
                }
                else
                {
                    _uow.Repository().Delete<Product>(product);
                    await _uow.CommitAsync(cancellationToken);
                    return true;
                }
                
            }
        }
    }
}
