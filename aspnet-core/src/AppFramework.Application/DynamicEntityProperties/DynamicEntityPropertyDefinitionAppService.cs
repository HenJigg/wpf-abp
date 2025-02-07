using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.DynamicEntityProperties;
using AppFramework.Authorization;

namespace AppFramework.DynamicEntityProperties
{
    [AbpAuthorize(AppPermissions.Pages_Administration_DynamicProperties)]
    public class DynamicEntityPropertyDefinitionAppService : AppFrameworkAppServiceBase, IDynamicEntityPropertyDefinitionAppService
    {
        private readonly IDynamicEntityPropertyDefinitionManager _dynamicEntityPropertyDefinitionManager;

        public DynamicEntityPropertyDefinitionAppService(IDynamicEntityPropertyDefinitionManager dynamicEntityPropertyDefinitionManager)
        {
            _dynamicEntityPropertyDefinitionManager = dynamicEntityPropertyDefinitionManager;
        }

        public async Task<List<string>> GetAllAllowedInputTypeNames()
        {
            return await Task.Run(() => _dynamicEntityPropertyDefinitionManager.GetAllAllowedInputTypeNames());
        }

        public async Task<List<string>> GetAllEntities()
        {
            return await Task.Run(() => _dynamicEntityPropertyDefinitionManager.GetAllEntities());
        }
    }
}
