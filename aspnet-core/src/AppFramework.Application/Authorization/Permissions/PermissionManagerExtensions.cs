using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Authorization;
using Abp.Runtime.Validation;

namespace AppFramework.Authorization.Permissions
{
    public static class PermissionManagerExtensions
    {
        /// <summary>
        /// Gets all permissions by names.
        /// Throws <see cref="AbpValidationException"/> if can not find any of the permission names.
        /// </summary>
        public static IEnumerable<Permission> GetPermissionsFromNamesByValidating(this IPermissionManager permissionManager, IEnumerable<string> permissionNames)
        {
            var permissions = new List<Permission>();
            var undefinedPermissionNames = new List<string>();

            foreach (var permissionName in permissionNames)
            {
                var permission = permissionManager.GetPermissionOrNull(permissionName);
                if (permission == null)
                {
                    undefinedPermissionNames.Add(permissionName);
                }

                permissions.Add(permission);
            }

            if (undefinedPermissionNames.Count > 0)
            {
                throw new AbpValidationException($"There are {undefinedPermissionNames.Count} undefined permission names.")
                      {
                          ValidationErrors = undefinedPermissionNames.Select(permissionName => new ValidationResult("Undefined permission: " + permissionName)).ToList()
                      };
            }

            return permissions;
        }
    }
}