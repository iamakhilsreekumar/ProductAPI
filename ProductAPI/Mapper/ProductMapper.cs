using AutoMapper;
using ProductAPI.Entities;
using ProductAPI.Resources;

namespace ProductAPI.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResource>()
                .ForMember(t => t.Id, o => o.MapFrom(t => t.Id))
                .ForMember(t => t.Price, o => o.MapFrom(t => t.Price))
                .ForMember(t => t.Name, o => o.MapFrom(t => t.Name))
                .ForMember(t => t.Description, o => o.MapFrom(t => t.Description))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId.ToString("D6")))
                .ReverseMap()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => int.Parse(src.ProductId)));
        }
    }
}

