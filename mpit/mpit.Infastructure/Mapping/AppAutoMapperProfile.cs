using AutoMapper;
using mpit.mpit.Core.DTOs.User;
using mpit.mpit.DataAccess.Entities;

namespace mpit.mpit.Infastructure.Mapping;

public sealed class AppAutoMapperProfile : Profile
{
    public AppAutoMapperProfile()
    {
        CreateMap<UserEntity, User>();
        // CreateMap<User, UserDto>();
        // CreateMap<UserEntity, UserDto>();
    }
}
