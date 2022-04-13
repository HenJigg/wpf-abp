

namespace Consumption.Api.Extensions
{
    using AutoMapper.Configuration;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class AutoMappingFile : MapperConfigurationExpression
    {
        /// <summary>
        /// 
        /// </summary>
        public AutoMappingFile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Group, GroupDto>().ReverseMap();
            CreateMap<Menu, MenuDto>().ReverseMap();
            CreateMap<Basic, BasicDto>().ReverseMap();

            CreateMap<GroupUserDto, GroupUser>().ReverseMap();
            CreateMap<GroupUser, GroupUserDto>().ReverseMap();
        }
    }
}
