using AutoMapper;
using spr311_web_api.BLL.Dtos.Product;
using spr311_web_api.DAL.Entities;

namespace spr311_web_api.BLL.MapperProfiles
{
    class ProductMapperProfile : Profile
    {
        public ProductMapperProfile() 
        {
            // CreateProductDto -> ProductEntity
            CreateMap<CreateProductDto, ProductEntity>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Categories, opt => opt.Ignore());

            // ProductEntity -> ProductDto
            CreateMap<ProductEntity, ProductDto>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.ImagePath)));
        }
    }
}
