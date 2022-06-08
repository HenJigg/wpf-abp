using AppFramework.ApiClient;
using AppFramework.Common;
using AppFramework.Configuration;
using AppFramework.Localization;
using AppFramework.MultiTenancy;
using AppFramework.Localization.Resources;
using Prism.Ioc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Extensions;

namespace AppFramework.Services.Account
{
    public class UserConfigurationManager
    {
        private static readonly Lazy<IApplicationContext> AppContext =
          new Lazy<IApplicationContext>(
          ContainerLocator.Container.Resolve<IApplicationContext>);

        private static IAccessTokenManager AccessTokenManager =>
            ContainerLocator.Container.Resolve<IAccessTokenManager>();

        public static async Task GetIfNeedsAsync()
        {
            if (AppContext.Value.Configuration != null)
                return;

            await GetAsync();
        }

        public static async Task GetAsync(Func<Task> successCallback = null)
        {
            var userConfigurationService = ContainerLocator.Container.Resolve<UserConfigurationService>();
            userConfigurationService.OnAccessTokenRefresh = App.OnAccessTokenRefresh;
            userConfigurationService.OnSessionTimeOut = App.OnSessionTimeout;

            await WebRequest.Execute(
                async () => await userConfigurationService.GetAsync(AccessTokenManager.IsUserLoggedIn),
                async result =>
                {
                    if (result == null) return;

                    AppContext.Value.Configuration = result;
                    SetCurrentCulture();

                    if (!result.MultiTenancy.IsEnabled)
                        AppContext.Value.SetAsTenant(TenantConsts.DefaultTenantName, TenantConsts.DefaultTenantId);
                     
                    AppContext.Value.CurrentLanguage = result.Localization.CurrentLanguage;

                    WarnIfUserHasNoPermission();
                     
                    if (successCallback != null)
                        await successCallback();
                }, ex =>
                {
                    return Task.CompletedTask;
                });
        }

        private static void WarnIfUserHasNoPermission()
        {
            if (!AccessTokenManager.IsUserLoggedIn)
            {
                return;
            }

            var hasAnyPermission = AppContext.Value.Configuration.Auth.GrantedPermissions != null &&
                                   AppContext.Value.Configuration.Auth.GrantedPermissions.Any();

            if (!hasAnyPermission)
            {
                //UserDialogHelper.Warn("NoPermission");
            }
        }

        /// <summary>
        /// 设置应用的区域化配置
        /// </summary>
        private static void SetCurrentCulture()
        {
            var locale = ContainerLocator.Container.Resolve<ILocaleCulture>();
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