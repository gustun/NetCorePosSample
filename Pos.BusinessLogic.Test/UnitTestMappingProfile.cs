using AutoMapper;
using Pos.BusinessLogic.Dto;
using Pos.DataAccess.Entities;

namespace Pos.BusinessLogic.Test
{
    public class UnitTestMappingProfile : Profile
    {
        public UnitTestMappingProfile()
        {
            #region Entity -> Dto

            CreateMap<User, UserDto>()
                .ReverseMap();

            CreateMap<Product, ProductDto>()
                .ReverseMap();

            CreateMap<OrderProduct, OrderProductDto>()
                .ForMember(x=>x.ProductCode, src=>src.MapFrom(x=>x.Product.Code))
                .ReverseMap();

            CreateMap<Order, OrderDto>()
                .ReverseMap();

            CreateMap<Campaign, CampaignDto>()
                .ReverseMap();

            #endregion
        }
    }
}
