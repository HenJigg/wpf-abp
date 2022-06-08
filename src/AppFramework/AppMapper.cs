using AppFramework.Auditing.Dto;
using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Common.Models;
using AppFramework.Models.Auditlogs;
using AppFramework.Models.Tenants;
using AppFramework.MultiTenancy.Dto;
using AutoMapper; 

namespace AppFramework
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            CreateMap<GetAuditLogsFilter, GetAuditLogsInput>().ReverseMap();
            CreateMap<GetEntityChangeFilter, GetEntityChangeInput>().ReverseMap();
            CreateMap<GetTenantsFilter, GetTenantsInput>().ReverseMap();
            CreateMap<FlatPermissionWithLevelDto, PermissionModel>().ReverseMap();
        }
    }
}
