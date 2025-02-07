using Prism.Ioc; 
using AppFramework.Admin.Services;
using AppFramework.Admin.Services.Account;
using AppFramework.Admin.Services.Notification;
using AppFramework.ApiClient; 
using AppFramework.Shared;
using AppFramework.Shared.Services;
using AppFramework.Shared.Services.Mapper;
using AppFramework.Shared.Services.App;
using AppFramework.Admin.Mapper;
using AppFramework.Admin.Update;

namespace AppFramework.Admin
{ 
    public static class ServiceExtensions
    { 
        public static void AddServices(this IContainerRegistry services)
        {
            services.RegisterSingleton<IAppMapper, AppMapper>();
            services.RegisterSingleton<IUpdateService, UpdateService>();

            services.RegisterSingleton<IAccountService, AccountService>();
            services.RegisterSingleton<IAccountStorageService, AccountStorageService>();
            services.RegisterSingleton<IDataStorageService, DataStorageService>();
            services.RegisterSingleton<IPermissionService, PermissionService>();
            services.RegisterSingleton<IAccessTokenManager, AccessTokenManager>();
            services.RegisterScoped<IPermissionTreesService, PermissionTreesService>();
            services.Register<IPermissionPorxyService, PermissionPorxyService>();
            services.RegisterScoped<IFeaturesService, FeaturesService>();

            services.RegisterSingleton<IApplicationService, ApplicationService>();
            services.RegisterSingleton<INavigationMenuService, NavigationMenuService>();
            services.RegisterSingleton<NavigationService>();
            services.RegisterSingleton<NotificationService>();
        }
    }
}
