using AutoMapper;
using Pos.Api.ViewModel;
using Pos.BusinessLogic.Dto;
using Pos.DataAccess.Entities;

namespace Pos.Api.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();

            CreateMap<UserDto, UserModel>()
                .ReverseMap();
        }
    }
}
