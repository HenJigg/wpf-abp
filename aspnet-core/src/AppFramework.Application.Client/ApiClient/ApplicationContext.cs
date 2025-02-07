using Abp;
using Abp.Dependency;
using Abp.Localization;
using Abp.Web.Models.AbpUserConfiguration;
using AppFramework.Sessions.Dto;
using JetBrains.Annotations;

namespace AppFramework.ApiClient
{
    public class ApplicationContext : IApplicationContext, ISingletonDependency
    {
        public TenantInformation CurrentTenant { get; private set; }

        public GetCurrentLoginInformationsOutput LoginInfo { get; private set; }

        public AbpUserConfigurationDto Configuration { get; set; }

        public LanguageInfo CurrentLanguage { get; set; }

        public void SetAsTenant([NotNull] string tenancyName, int tenantId)
        {
            Check.NotNull(tenancyName, nameof(tenancyName));

            CurrentTenant = new TenantInformation(tenancyName, tenantId);
        }

        public void ClearLoginInfo()
        {
            LoginInfo = null;
            Configuration = null;
        }

        public void SetLoginInfo(GetCurrentLoginInformationsOutput loginInfo)
        {
            LoginInfo = loginInfo;
        }

        public void SetAsHost()
        {
            CurrentTenant = null;
        }

        public void Load(TenantInformation currentTenant, GetCurrentLoginInformationsOutput loginInfo)
        {
            CurrentTenant = currentTenant;
            LoginInfo = loginInfo;
        }
    }
}