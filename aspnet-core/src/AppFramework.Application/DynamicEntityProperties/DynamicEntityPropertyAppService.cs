using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.DynamicEntityProperties;
using AppFramework.Authorization;
using AppFramework.DynamicEntityProperties.Dto;

namespace AppFramework.DynamicEntityProperties
{
    [AbpAuthorize(AppPermissions.Pages_Administration_DynamicEntityProperties)]
    public class DynamicEntityPropertyAppService : AppFrameworkAppServiceBase, IDynamicEntityPropertyAppService
    {
        private readonly IDynamicEntityPropertyManager _dynamicEntityPropertyManager;

        public DynamicEntityPropertyAppService(IDynamicEntityPropertyManager dynamicEntityPropertyManager)
        {
            _dynamicEntityPropertyManager = dynamicEntityPropertyManager;
        }

        public async Task<DynamicEntityPropertyDto> Get(int id)
        {
            var entity = await _dynamicEntityPropertyManager.GetAsync(id);
            return ObjectMapper.Map<DynamicEntityPropertyDto>(entity);
        }

        public async Task<ListResultDto<DynamicEntityPropertyDto>> GetAllPropertiesOfAnEntity(DynamicEntityPropertyGetAllInput input)
        {
            var entities = await _dynamicEntityPropertyManager.GetAllAsync(input.EntityFullName);
            return new ListResultDto<DynamicEntityPropertyDto>(
                ObjectMapper.Map<List<DynamicEntityPropertyDto>>(entities)
            );
        }

        public async Task<ListResultDto<DynamicEntityPropertyDto>> GetAll()
        {
            var entities = await _dynamicEntityPropertyManager.GetAllAsync();
            return new ListResultDto<DynamicEntityPropertyDto>(
                ObjectMapper.Map<List<DynamicEntityPropertyDto>>(entities)
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DynamicEntityProperties_Create)]
        public async Task Add(DynamicEntityPropertyDto dto)
        {
            dto.TenantId = AbpSession.TenantId;
            await _dynamicEntityPropertyManager.AddAsync(ObjectMapper.Map<DynamicEntityProperty>(dto));
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit)]
        public async Task Update(DynamicEntityPropertyDto dto)
        {
            await _dynamicEntityPropertyManager.UpdateAsync(ObjectMapper.Map<DynamicEntityProperty>(dto));
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete)]
        public async Task Delete(int id)
        {
            await _dynamicEntityPropertyManager.DeleteAsync(id);
        }

        public async Task<ListResultDto<GetAllEntitiesHasDynamicPropertyOutput>> GetAllEntitiesHasDynamicProperty()
        {
            var entities = await _dynamicEntityPropertyManager.GetAllAsync();
            return new ListResultDto<GetAllEntitiesHasDynamicPropertyOutput>(
                entities?.Select(x => new GetAllEntitiesHasDynamicPropertyOutput()
                {
                    EntityFullName = x.EntityFullName
                }).DistinctBy(x => x.EntityFullName).ToList()
            );
        }
    }
}
