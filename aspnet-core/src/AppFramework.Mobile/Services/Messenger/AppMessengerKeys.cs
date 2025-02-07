using AppFramework.Shared.ViewModels;

namespace AppFramework.Shared.Services.Messenger
{
    /// <summary>
    /// 应用程序消息键
    /// </summary>
    public class AppMessengerKeys
    {
        public const string User = nameof(UserViewModel);
        public const string Role = nameof(RoleViewModel);
        public const string Tenant = nameof(TenantViewModel);
        public const string Organization = nameof(OrganizationViewModel);
        public const string Language = nameof(LanguageViewModel);
        public const string Edition = nameof(EditionViewModel);
        public const string Dynamic = nameof(DynamicPropertyViewModel);
        public const string Auditlog = nameof(AuditLogViewModel);
        public const string Logout = "Logout";
        public const string RemoveAllRegion = "RemoveRegions";
        public const string LanguageRefresh = "LanguageRefresh";
    }
}
