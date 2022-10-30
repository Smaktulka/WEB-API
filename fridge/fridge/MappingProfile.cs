using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using fridge.Models;

namespace fridge
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Fridges, FridgesDto>();
                    //.ForMember(c => c.FullName,
                      //  opt => opt.MapFrom(x => String.Join(' ', x.Name, x.ModelId.ToString())));

            CreateMap<FridgeProductsDto, FridgeProducts>();

            CreateMap<FridgeProducts, FridgeProductsDto>();

            CreateMap<FridgeProductsForCreationDto, FridgeProducts>();

            CreateMap<FridgeProductsForUpdateDto, FridgeProducts>();

            CreateMap<FridgeForUpdateDto, Fridges>();

            CreateMap<ProductsForUpdateDto, Products>();

            CreateMap<FridgesForCreationDto, Fridges>();

            CreateMap<UserForRegistrationDto, User>();

            CreateMap<Products, ProductsDto>();

            CreateMap<FridgeModels, FridgeModelsDto>();
        }
    }
}
