using Prism.Ioc;

#region ApplicationServices

using AppFramework.Shared.ViewModels;
using AppFramework.Shared.Views;
using AppFramework.Shared.Services.Account;
using AppFramework.Shared.Services.Storage;
using AppFramework.Shared.Services;
using AppFramework.Shared.Validations;
using AppFramework.Shared.Services.Navigation;

using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Users;
using AppFramework.MultiTenancy;
using AppFramework.Editions;
using AppFramework.Auditing;
using AppFramework.DynamicEntityProperties;
using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Configuration;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Accounts;
using AppFramework.Localization;
using AppFramework.Sessions;
using AppFramework.MultiTenancy.HostDashboard;
using AppFramework.Caching;
using AppFramework.Tenants.Dashboard;
using AppFramework.Organizations;
using AppFramework.Application;
using AppFramework.Application.Client;
using AppFramework.Application.MultiTenancy.HostDashboard;
using AppFramework.Application.Organizations;
using AppFramework.Application.MultiTenancy;
using AppFramework.Shared.Core;
using Abp.Configuration.Startup;
using AppFramework.Authorization.Permissions;
using AppFramework.Configuration.Host;
using AppFramework.Notifications;
using AppFramework.Authorization.Users.Delegation;
using AppFramework.Version;
using AppFramework.Common;
using AppFramework.Shared.Services.Datapager;
using AppFramework.Chat;
using AppFramework.Friendships;

#endregion ApplicationServices

namespace AppFramework.Shared
{
    internal static class Startup
    {
        /// <summary>
        /// 配置应用程序模块服务
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSerivces(this IContainerRegistry services)
        {
            //注册应用程序依赖服务
            services.RegisterSingleton<IFriendChatService, FriendChatService>();
            services.RegisterScoped<IFriendshipAppService, FriendshipAppService>();
            services.RegisterScoped<IChatAppService, ChatAppService>();
             
            services.RegisterScoped<IRoleAppService, RoleAppService>();
            services.RegisterScoped<IUserAppService, ProxyUserAppService>();
            services.RegisterScoped<IUserLoginAppService, UserLoginAppService>();
            services.RegisterScoped<IUserDelegationAppService, UserDelegationAppService>();
            services.RegisterScoped<ITenantAppService, ProxyTenantAppService>();
            services.RegisterScoped<IEditionAppService, EditionAppService>();
            services.RegisterScoped<IAuditLogAppService, AuditLogAppService>();
            services.RegisterScoped<ILanguageAppService, LanguageAppService>();
            services.RegisterScoped<IPermissionTreesService, PermissionTreesService>();
            services.RegisterScoped<INotificationAppService, NotificationAppService>();
            services.RegisterScoped<IOrganizationUnitAppService, OrganizationUnitAppService>();
            services.RegisterScoped<IDynamicPropertyAppService, DynamicPropertyAppService>();
            services.RegisterScoped<ICachingAppService, CachingAppService>();
            services.RegisterScoped<ITenantDashboardAppService, TenantDashboardAppService>();
            services.RegisterScoped<IDynamicEntityPropertyAppService, DynamicEntityPropertyAppService>();
            services.RegisterScoped<IDynamicEntityPropertyDefinitionAppService, DynamicEntityPropertyDefinitionAppService>();
            services.RegisterScoped<IDynamicPropertyValueAppService, DynamicPropertyValueAppService>();
            services.RegisterScoped<ICommonLookupAppService, ProxyCommonLookupAppService>();
            services.RegisterScoped<IAccountAppService, ProxyAccountAppService>();
            services.RegisterScoped<IProfileAppService, ProxyProfileAppService>();
            services.RegisterScoped<ISessionAppService, ProxySessionAppService>();
            services.RegisterScoped<IHostDashboardAppService, HostDashboardAppService>();
            services.RegisterScoped<IHostSettingsAppService, HostSettingsAppService>();
            services.RegisterScoped<IPermissionAppService, PermissionAppService>();
            services.RegisterScoped<IPorxyCommandService, PorxyCommandService>();
            services.RegisterScoped<IFeaturesService, FeaturesService>();
            services.RegisterScoped<IUserLinkAppService, UserLinkAppService>();
            services.RegisterScoped<IAbpVersionsAppService, AbpVersionsAppService>();
             
            services.Register<IDataPagerService, DataPagerService>();
            services.RegisterSingleton<IPermissionService, PermissionService>();
            services.RegisterSingleton<IAccessTokenManager, AccessTokenManager>();
            services.RegisterSingleton<IMultiTenancyConfig, MultiTenancyConfig>();
            services.RegisterSingleton<IApplicationContext, ApplicationContext>();
            services.RegisterSingleton<AbpApiClient>();
            services.RegisterSingleton<AbpAuthenticateModel>();
            services.RegisterScoped<UserConfigurationService>();
            services.RegisterScoped<ProxyProfileControllerService>();
            services.RegisterScoped<ProxyTokenAuthControllerService>();

            services.RegisterSingleton<IAppMapper, AppMapper>();
            services.RegisterSingleton<IMessenger, Messenger>();
            services.RegisterSingleton<INavigationMenuService, NavigationMenuService>();
            services.RegisterSingleton<IRegionNavigateService, RegionNavigateService>();
            services.RegisterSingleton<IUserDialogService, UserDialogService>();
            services.RegisterSingleton<IApplicationService, ApplicationService>();
            services.RegisterSingleton<IAccountService, AccountService>();
            services.RegisterSingleton<IAccountStorageService, AccountStorageService>();
            services.RegisterSingleton<IDataStorageService, DataStorageService>();

            //注册应用程序验证器
            services.RegisterValidator();

            /*
             *  注册应用程序模块 (Prism区域导航 ContenView) 
             */
            services.RegisterForRegionNavigation<LoginView, LoginViewModel>(AppViews.Login);
            services.RegisterForRegionNavigation<MainView, MainViewModel>(AppViews.Main);
            services.RegisterForRegionNavigation<DashboardView, DashboardViewModel>(AppViews.Dashboard);
            services.RegisterForRegionNavigation<UserView, UserViewModel>(AppViews.User);
            services.RegisterForRegionNavigation<RoleView, RoleViewModel>(AppViews.Role);
            services.RegisterForRegionNavigation<LanguageView, LanguageViewModel>(AppViews.Language);
            services.RegisterForRegionNavigation<TenantView, TenantViewModel>(AppViews.Tenant);
            services.RegisterForRegionNavigation<EditionView, EditionViewModel>(AppViews.Edition);
            services.RegisterForRegionNavigation<AuditLogView, AuditLogViewModel>(AppViews.AuditLog);
            services.RegisterForRegionNavigation<DynamicPropertyView, DynamicPropertyViewModel>(AppViews.DynamicProperty);
            services.RegisterForRegionNavigation<OrganizationView, OrganizationViewModel>(AppViews.Organization);
            services.RegisterForRegionNavigation<SettingsView, SettingsViewModel>(AppViews.Setting);
            services.RegisterForRegionNavigation<FriendsView, FriendsViewModel>(AppViews.Friends);

            /*
             *  注册应用程序页面 (导航页 ContentPage) 
             */

            services.RegisterDialog<MyProfileView, MyProfileViewModel>();
            services.RegisterDialog<MessageBoxView, MessageBoxViewModel>();
            services.RegisterDialog<ForgotPasswordView, ForgotPasswordViewModel>();
            services.RegisterDialog<ChangePasswordView, ChangePasswordViewModel>();
            services.RegisterDialog<EmailActivationView, EmailActivationViewModel>();
            services.RegisterDialog<SendTwoFactorCodeView, SendTwoFactorCodeViewModel>();
            services.RegisterForNavigation<InitialScreenView, InitialScreenViewModel>();
            services.RegisterForNavigation<UserDetailsView, UserDetailsViewModel>();
            services.RegisterForNavigation<RoleDetailsView, RoleDetailsViewModel>();
            services.RegisterForNavigation<TenantDetailsView, TenantDetailsViewModel>();
            services.RegisterForNavigation<EditionDetailsView, EditionDetailsViewModel>();
            services.RegisterForNavigation<LanguageDetailsView, LanguageDetailsViewModel>();
            services.RegisterForNavigation<AddRolesView, AddRolesViewModel>();
            services.RegisterForNavigation<AddUsersView, AddUsersViewModel>();
            services.RegisterForNavigation<OrganizationDetailsView, OrganizationDetailsViewModel>();
            services.RegisterForNavigation<DynamicPropertyDetailsView, DynamicPropertyDetailsViewModel>(); 
            services.RegisterForNavigation<ProfilePictureView, ProfilePictureViewModel>();
            services.RegisterForNavigation<FriendsChatView, FriendsChatViewModel>(AppViews.FriendsChat);
        } 
    }
}