using AutoMapper;
using ProductInventorySystem.Models;
using ProductInventorySystem.Dtos;
namespace ProductInventorySystem.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductQuantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.ProductCategory, opt => opt.MapFrom(src => src.Category))
                .ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.ProductQuantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ProductPrice))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.ProductCategory));
        }
    }
}
