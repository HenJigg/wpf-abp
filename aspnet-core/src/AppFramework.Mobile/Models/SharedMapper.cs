using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Auditing.Dto;
using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Authorization.Roles.Dto;
using AppFramework.Authorization.Users.Dto;
using AppFramework.DynamicEntityProperties.Dto;
using AppFramework.Editions.Dto;
using AppFramework.Localization.Dto;
using AppFramework.MultiTenancy.Dto;
using AppFramework.Organizations.Dto;
using AppFramework.Sessions.Dto;
using AutoMapper;
using AppFramework.Shared.Models.Configuration;
using AppFramework.Configuration.Host.Dto;
using AppFramework.Configuration.Dto;
using AppFramework.Version.Dtos;
using AppFramework.Shared.Models.Chat;
using AppFramework.Friendships.Dto;
using AppFramework.Chat.Dto;

namespace AppFramework.Shared.Models
{
    public class SharedMapper : Profile
    {
        public SharedMapper()
        {
            //系统模块中实体映射关系 
            CreateMap<UserListDto, UserListModel>().ReverseMap();
            CreateMap<UserEditDto, UserEditModel>().ReverseMap();
            CreateMap<RoleListDto, RoleListModel>().ReverseMap();
            CreateMap<RoleEditDto, RoleEditModel>().ReverseMap();
            CreateMap<TenantListDto, TenantListModel>().ReverseMap();
            CreateMap<TenantEditDto, TenantListModel>().ReverseMap();
            CreateMap<AuditLogListDto, AuditLogListModel>().ReverseMap();
            CreateMap<UserCreateOrUpdateModel, CreateOrUpdateUserInput>().ReverseMap();
            CreateMap<DynamicPropertyDto, DynamicPropertyModel>().ReverseMap();
            CreateMap<OrganizationUnitDto, OrganizationListModel>().ReverseMap();
            CreateMap<OrganizationUnitDto, OrganizationUnitModel>().ReverseMap();
            CreateMap<LanguageListModel, ApplicationLanguageListDto>().ReverseMap();
            CreateMap<LanguageTextListModel, LanguageTextListDto>().ReverseMap();
            CreateMap<UserLoginInfoDto, UserLoginInfoModel>().ReverseMap();
            CreateMap<UserLoginInfoDto, UserLoginInfoPersistanceModel>().ReverseMap();
            CreateMap<AbpAuthenticateResultModel, AuthenticateResultPersistanceModel>().ReverseMap();
            CreateMap<TenantInformation, TenantInformationPersistanceModel>().ReverseMap();
            CreateMap<TenantLoginInfoDto, TenantLoginInfoPersistanceModel>().ReverseMap();
            CreateMap<ApplicationInfoDto, ApplicationInfoPersistanceModel>().ReverseMap();
            CreateMap<EditionListDto, EditionListModel>().ReverseMap();
            CreateMap<EditionCreateDto, EditionCreateModel>().ReverseMap();
            CreateMap<EditionEditDto, EditionCreateModel>().ReverseMap();
            CreateMap<FlatFeatureDto, FlatFeatureModel>().ReverseMap();
            CreateMap<FlatPermissionDto, PermissionModel>().ReverseMap();
            CreateMap<GetUserForEditOutput, UserForEditModel>().ReverseMap();
            CreateMap<GetCurrentLoginInformationsOutput, CurrentLoginInformationPersistanceModel>().ReverseMap();
            CreateMap<TenantListModel, CreateTenantInput>().ReverseMap();
            CreateMap<AbpVersionDto, VersionListModel>().ReverseMap();
            CreateMap<FriendDto, FriendModel>().ReverseMap();
            CreateMap<ChatMessageModel, ChatMessageDto>().ReverseMap();

            #region 系统设置

            CreateMap<HostSettingsEditModel, HostSettingsEditDto>().ReverseMap();
            CreateMap<GeneralSettingsEditModel, GeneralSettingsEditDto>().ReverseMap();
            CreateMap<HostUserManagementSettingsEditModel, HostUserManagementSettingsEditDto>().ReverseMap();
            CreateMap<EmailSettingsEditModel, EmailSettingsEditDto>().ReverseMap();
            CreateMap<TenantManagementSettingsEditModel, TenantManagementSettingsEditDto>().ReverseMap();
            CreateMap<SecuritySettingsEditModel, SecuritySettingsEditDto>().ReverseMap();
            CreateMap<HostBillingSettingsEditModel, HostBillingSettingsEditDto>().ReverseMap();
            CreateMap<OtherSettingsEditModel, OtherSettingsEditDto>().ReverseMap();
            CreateMap<ExternalLoginProviderSettingsEditModel, ExternalLoginProviderSettingsEditDto>().ReverseMap();
            CreateMap<UserLockOutSettingsEditModel, UserLockOutSettingsEditDto>().ReverseMap();
            CreateMap<TwoFactorLoginSettingsEditModel, TwoFactorLoginSettingsEditDto>().ReverseMap();
            CreateMap<SessionTimeOutSettingsEditModel, SessionTimeOutSettingsEditDto>().ReverseMap();
            CreateMap<UserPasswordSettingsEditModel, UserPasswordSettingsEditDto>().ReverseMap();

            #endregion

            /*
             * 以下可添加更多的实体映射关系
             */
        }
    }
}