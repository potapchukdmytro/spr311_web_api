using AutoMapper;
using spr311_web_api.BLL.Dtos.Category;
using spr311_web_api.DAL.Entities;

namespace spr311_web_api.BLL.MapperProfiles
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            // CreateCategoryDto -> CategoryEntity
            CreateMap<CreateCategoryDto, CategoryEntity>()
                .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.Name.ToUpper()))
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // UpdateCategoryDto -> CategoryEntity
            CreateMap<UpdateCategoryDto, CategoryEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // CategoryEntity -> CategoryDto
            CreateMap<CategoryEntity, CategoryDto>();
        }
    }
}
