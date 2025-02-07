using Abp.Localization;
using Abp.Web.Models.AbpUserConfiguration;
using AppFramework.Sessions.Dto;
using JetBrains.Annotations;

namespace AppFramework.ApiClient
{
    public interface IApplicationContext
    {
        [CanBeNull]
        TenantInformation CurrentTenant { get; }

        AbpUserConfigurationDto Configuration { get; set; }

        GetCurrentLoginInformationsOutput LoginInfo { get; }

        void ClearLoginInfo();

        void SetLoginInfo(GetCurrentLoginInformationsOutput loginInfo);

        void SetAsHost();

        void SetAsTenant(string tenancyName, int tenantId);

        LanguageInfo CurrentLanguage { get; set; }

        void Load(TenantInformation currentTenant, GetCurrentLoginInformationsOutput loginInfo);
    }
}