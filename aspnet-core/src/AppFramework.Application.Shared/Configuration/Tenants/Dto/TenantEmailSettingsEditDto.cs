using Abp.Auditing;
using AppFramework.Configuration.Dto;

namespace AppFramework.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}