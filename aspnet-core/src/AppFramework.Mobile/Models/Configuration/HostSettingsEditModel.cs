using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Shared.Models.Configuration
{
    public class HostSettingsEditModel
    { 
        public GeneralSettingsEditModel General { get; set; }
         
        public HostUserManagementSettingsEditModel UserManagement { get; set; }
         
        public EmailSettingsEditModel Email { get; set; }
         
        public TenantManagementSettingsEditModel TenantManagement { get; set; }
         
        public SecuritySettingsEditModel Security { get; set; }

        public HostBillingSettingsEditModel Billing { get; set; }

        public OtherSettingsEditModel OtherSettings { get; set; }

        public ExternalLoginProviderSettingsEditModel ExternalLoginProviderSettings { get; set; }
    }
}
