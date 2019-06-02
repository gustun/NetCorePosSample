using AutoMapper;
using Pos.Api.ViewModel;
using Pos.BusinessLogic.Dto;
using Pos.Contracts;
using Pos.DataAccess.Entities;

namespace Pos.Api.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile(ICryptoHelper cryptoHelper)
        {
            #region Entity -> Dto

            CreateMap<User, UserDto>()
                .ReverseMap();

            CreateMap<Product, ProductDto>()
                .ReverseMap();

            CreateMap<OrderProduct, OrderProductDto>()
                .ReverseMap();

            CreateMap<Order, OrderDto>()
                .ReverseMap();

            CreateMap<Campaign, CampaignDto>()
                .ReverseMap();

            #endregion

            #region Dto -> ViewModel

            CreateMap<UserDto, UserViewModel>()
                .ReverseMap();

            CreateMap<UserDto, NewUserViewModel>()
                .ForMember(dest=>dest.Password, src => src.MapFrom(x=>cryptoHelper.Hash(x.Password)));

            CreateMap<UserDto, LoginViewModel>()
                .ForMember(dest=>dest.Password, src => src.MapFrom(x=>cryptoHelper.Hash(x.Password)));

            CreateMap<ProductDto, ProductViewModel>()
                .ReverseMap();

            CreateMap<ProductDto, NewProductModel>()
                .ReverseMap();

            CreateMap<OrderProductDto, OrderProductViewModel>()
                .ReverseMap();

            CreateMap<OrderDto, OrderViewModel>()
                .ReverseMap();

            CreateMap<Campaign, CampaignViewModel>()
                .ReverseMap();

            #endregion

            #region ViewModel -> Dto

            CreateMap<NewUserViewModel, UserDto>()
                .ForMember(dest=>dest.Password, src => src.MapFrom(x=>cryptoHelper.Hash(x.Password)));

            CreateMap<LoginViewModel, UserDto>()
                .ForMember(dest=>dest.Password, src => src.MapFrom(x=>cryptoHelper.Hash(x.Password)));

            #endregion
        }
    }
}
