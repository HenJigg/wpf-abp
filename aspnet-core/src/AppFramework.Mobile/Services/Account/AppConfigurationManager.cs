using AppFramework.ApiClient;
using AppFramework.Configuration;
using AppFramework.MultiTenancy;
using AppFramework.Shared.Localization.Resources;
using AppFramework.Shared.Localization;
using Prism.Ioc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms; 
using AppFramework.Shared.Extensions;

namespace AppFramework.Shared.Services.Account
{
    public static class AppConfigurationManager
    {
        private static readonly Lazy<IApplicationContext> AppContext =
            new Lazy<IApplicationContext>(
            ContainerLocator.Container.Resolve<IApplicationContext>
        );

        private static IAccessTokenManager AccessTokenManager =>
            ContainerLocator.Container.Resolve<IAccessTokenManager>();

        public static async Task GetIfNeedsAsync()
        {
            if (AppContext.Value.Configuration != null)
                return;

            await GetAsync();
        }

        /// <summary>
        /// 加载应用程序配置文件
        /// </summary>
        /// <param name="successCallback"></param>
        /// <returns></returns>
        public static async Task GetAsync(Func<Task> successCallback = null)
        {
            var userConfigurationService = ContainerLocator.Container.Resolve<UserConfigurationService>();
            userConfigurationService.OnAccessTokenRefresh = App.OnAccessTokenRefresh;
            userConfigurationService.OnSessionTimeOut = App.OnSessionTimeout;

            await WebRequest.Execute(
                async () => await userConfigurationService.GetAsync(AccessTokenManager.IsUserLoggedIn),
                async result =>
                {
                    AppContext.Value.Configuration = result;
                    SetCurrentCulture();

                    if (!result.MultiTenancy.IsEnabled)
                        AppContext.Value.SetAsTenant(TenantConsts.DefaultTenantName, TenantConsts.DefaultTenantId);

                    AppContext.Value.CurrentLanguage = result.Localization.CurrentLanguage;
                    WarnIfUserHasNoPermission();

                    //更新UI界面中的所有绑定多语言字符串文本
                    LocalizationResourceManager.Instance.OnPropertyChanged();

                    if (successCallback != null)
                        await successCallback();
                },
                _ =>
                {
                    return Task.CompletedTask;
                });
        }

        /// <summary>
        /// 警告如果用户没有权限
        /// </summary>
        private static void WarnIfUserHasNoPermission()
        {
            if (!AccessTokenManager.IsUserLoggedIn)
                return;

            var hasAnyPermission = AppContext.Value.Configuration.Auth.GrantedPermissions != null &&
                                   AppContext.Value.Configuration.Auth.GrantedPermissions.Any();

            if (!hasAnyPermission)
                DialogHelper.Warn(LocalizationKeys.NoPermission);
        }

        /// <summary>
        /// 设置应用的区域化配置
        /// </summary>
        private static void SetCurrentCulture()
        {
            var locale = DependencyService.Get<ILocaleCulture>();
            var userCulture = GetUserCulture(locale);

            locale.SetLocale(userCulture);
            LocalTranslation.Culture = userCulture;
        }

        /// <summary>
        /// 获取用户的区域化信息
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        private static CultureInfo GetUserCulture(ILocaleCulture locale)
        {
            if (AppContext.Value.Configuration.Localization.CurrentCulture.Name == null)
                return locale.GetCurrentCultureInfo();

            try
            {
                return new CultureInfo(AppContext.Value.Configuration.Localization.CurrentCulture.Name);
            }
            catch (CultureNotFoundException)
            {
                return locale.GetCurrentCultureInfo();
            }
        }
    }
}