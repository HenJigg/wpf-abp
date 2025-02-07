using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using AppFramework.Authorization.Permissions.Dto;

namespace AppFramework.Authorization.Permissions
{
    public class PermissionAppService : AppFrameworkAppServiceBase, IPermissionAppService
    {
        public async Task<ListResultDto<FlatPermissionWithLevelDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();
            var rootPermissions = permissions.Where(p => p.Parent == null);

            var result = new List<FlatPermissionWithLevelDto>();

            return await Task.Run(() =>
            {
                var result = new List<FlatPermissionWithLevelDto>();
                foreach (var rootPermission in rootPermissions)
                {
                    var level = 0;
                    AddPermission(rootPermission, permissions, result, level);
                }
                return new ListResultDto<FlatPermissionWithLevelDto> { Items = result };
            });
        }

        private void AddPermission(Permission permission, IReadOnlyList<Permission> allPermissions, List<FlatPermissionWithLevelDto> result, int level)
        {
            var flatPermission = ObjectMapper.Map<FlatPermissionWithLevelDto>(permission);
            flatPermission.Level = level;
            result.Add(flatPermission);

            if (permission.Children == null)
            {
                return;
            }

            var children = allPermissions.Where(p => p.Parent != null && p.Parent.Name == permission.Name).ToList();

            foreach (var childPermission in children)
            {
                AddPermission(childPermission, allPermissions, result, level + 1);
            }
        }
    }
}