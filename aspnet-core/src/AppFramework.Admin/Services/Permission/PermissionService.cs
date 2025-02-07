using AppFramework.ApiClient;
using AppFramework.Shared;
using System;

namespace AppFramework.Admin.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IApplicationContext appContext;

        public PermissionService(IApplicationContext appContext)
        {
            this.appContext = appContext;
        }

        public bool HasPermission(string key)
        {
            if (appContext.Configuration.Auth.GrantedPermissions.TryGetValue(key, out var permissionValue))
                return string.Equals(permissionValue, "true", StringComparison.OrdinalIgnoreCase);

            return false;
        }
    }
}