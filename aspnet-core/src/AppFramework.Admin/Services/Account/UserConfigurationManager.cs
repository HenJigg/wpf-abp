using AppFramework.ApiClient;
using AppFramework.Shared;
using AppFramework.Configuration;
using AppFramework.MultiTenancy;
using Prism.Ioc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Abp.Web.Models.AbpUserConfiguration;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Shared.Services;
using System.Diagnostics;

namespace AppFramework.Admin.Services.Account
{
    public class UserConfigurationManager
    {
        private static readonly Lazy<IApplicationContext> appContext =
          new Lazy<IApplicationContext>(
          ContainerLocator.Container.Resolve<IApplicationContext>);

        private static IAccessTokenManager AccessTokenManager =>
            ContainerLocator.Container.Resolve<IAccessTokenManager>();

        public static async Task GetIfNeedsAsync(Func<System.Exception, Task> failCallback = null)
        {
            if (appContext.Value.Configuration != null)
                return;

            await GetAsync(failCallback);
        }

        public static async Task GetAsync(Func<System.Exception, Task> failCallback = null)
        {
            var userConfigurationService = ContainerLocator.Container.Resolve<UserConfigurationService>();

            await WebRequest.Execute(() => userConfigurationService.GetAsync(AccessTokenManager.IsUserLoggedIn),
                GetConfigurationSuccessed, failCallback);
        }

        private static async Task GetConfigurationSuccessed(AbpUserConfigurationDto result)
        {
            appContext.Value.Configuration = result;
            SetCurrentCulture();

            if (!result.MultiTenancy.IsEnabled)
                appContext.Value.SetAsTenant(TenantConsts.DefaultTenantName, TenantConsts.DefaultTenantId);

            if (AccessTokenManager.IsUserLoggedIn)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                var chatService = ContainerLocator.Container.Resolve<IChatService>();
                await chatService.ConnectAsync();
                stopWatch.Stop();
            }

            var profileAppService = ContainerLocator.Container.Resolve<IProfileAppService>();
            var currentLanguage = appContext.Value.CurrentLanguage;
            if (currentLanguage != null && currentLanguage.Name != result.Localization.CurrentLanguage.Name)
            {
                if (AccessTokenManager.IsUserLoggedIn)
                {
                    //如果授权成功后, 当前客户端选择的语言和后端的不一致,那么修改默认语言
                    await profileAppService.ChangeLanguage(new Authorization.Users.Dto.ChangeUserLanguageDto()
                    {
                        LanguageName = currentLanguage.Name
                    });
                    await GetAsync();
                }
            }
            else
            {
                appContext.Value.CurrentLanguage = result.Localization.CurrentLanguage;
            }

            WarnIfUserHasNoPermission();
        }

        private static void WarnIfUserHasNoPermission()
        {
            if (!AccessTokenManager.IsUserLoggedIn)
            {
                return;
            }

            var hasAnyPermission = appContext.Value.Configuration.Auth.GrantedPermissions != null &&
                                   appContext.Value.Configuration.Auth.GrantedPermissions.Any();

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
        }

        /// <summary>
        /// 获取用户的区域化信息
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        private static CultureInfo GetUserCulture(ILocaleCulture locale)
        {
            if (appContext.Value.Configuration.Localization.CurrentCulture.Name == null)
                return locale.GetCurrentCultureInfo();

            try
            {
                return new CultureInfo(appContext.Value.Configuration.Localization.CurrentCulture.Name);
            }
            catch (CultureNotFoundException)
            {
                return locale.GetCurrentCultureInfo();
            }
        }
    }
}