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
            CreateMap<User, UserDto>()
                .ReverseMap();

            CreateMap<UserDto, UserModel>()
                .ReverseMap();

            CreateMap<UserCreateModel, UserDto>()
                .ForMember(dest=>dest.Password, src => src.MapFrom(x=>cryptoHelper.Hash(x.Password)))
                .ReverseMap();

            CreateMap<LoginModel, UserDto>()
                .ForMember(dest=>dest.Password, src => src.MapFrom(x=>cryptoHelper.Hash(x.Password)))
                .ReverseMap();
        }
    }
}
