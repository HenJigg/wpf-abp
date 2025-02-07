using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.DynamicEntityProperties;
using AppFramework.Authorization;
using AppFramework.DynamicEntityProperties.Dto;
using AppFramework.DynamicEntityPropertyValues.Dto;

namespace AppFramework.DynamicEntityProperties
{
    [AbpAuthorize(AppPermissions.Pages_Administration_DynamicEntityPropertyValue)]
    public class DynamicEntityPropertyValueAppService : AppFrameworkAppServiceBase, IDynamicEntityPropertyValueAppService
    {
        private readonly IDynamicEntityPropertyValueManager _dynamicEntityPropertyValueManager;
        private readonly IDynamicPropertyValueManager _dynamicPropertyValueManager;
        private readonly IDynamicEntityPropertyManager _dynamicEntityPropertyManager;
        private readonly IDynamicEntityPropertyDefinitionManager _dynamicEntityPropertyDefinitionManager;

        public DynamicEntityPropertyValueAppService(
            IDynamicEntityPropertyValueManager dynamicEntityPropertyValueManager,
            IDynamicPropertyValueManager dynamicPropertyValueManager,
            IDynamicEntityPropertyManager dynamicEntityPropertyManager,
            IDynamicEntityPropertyDefinitionManager dynamicEntityPropertyDefinitionManager)
        {
            _dynamicEntityPropertyValueManager = dynamicEntityPropertyValueManager;
            _dynamicPropertyValueManager = dynamicPropertyValueManager;
            _dynamicEntityPropertyManager = dynamicEntityPropertyManager;
            _dynamicEntityPropertyDefinitionManager = dynamicEntityPropertyDefinitionManager;
        }

        public async Task<DynamicEntityPropertyValueDto> Get(int id)
        {
            var entity = await _dynamicEntityPropertyValueManager.GetAsync(id);
            return ObjectMapper.Map<DynamicEntityPropertyValueDto>(entity);
        }

        public async Task<ListResultDto<DynamicEntityPropertyValueDto>> GetAll(GetAllInput input)
        {
            var entities = await _dynamicEntityPropertyValueManager.GetValuesAsync(input.PropertyId, input.EntityId);
            return new ListResultDto<DynamicEntityPropertyValueDto>(
                ObjectMapper.Map<List<DynamicEntityPropertyValueDto>>(entities)
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create)]
        public async Task Add(DynamicEntityPropertyValueDto input)
        {
            var entity = ObjectMapper.Map<DynamicEntityPropertyValue>(input);
            entity.TenantId = AbpSession.TenantId;
            await _dynamicEntityPropertyValueManager.AddAsync(entity);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit)]
        public async Task Update(DynamicEntityPropertyValueDto input)
        {
            var entity = await _dynamicEntityPropertyValueManager.GetAsync(input.Id);
            if (entity == null || entity.TenantId != AbpSession.TenantId)
            {
                throw new EntityNotFoundException(typeof(DynamicEntityPropertyValue), input.Id);
            }

            entity.Value = input.Value;
            entity.DynamicEntityPropertyId = input.DynamicEntityPropertyId;
            entity.EntityId = input.EntityId;

            await _dynamicEntityPropertyValueManager.UpdateAsync(entity);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete)]
        public async Task Delete(int id)
        {
            await _dynamicEntityPropertyValueManager.DeleteAsync(id);
        }

        public async Task<GetAllDynamicEntityPropertyValuesOutput> GetAllDynamicEntityPropertyValues(GetAllDynamicEntityPropertyValuesInput input)
        {
            var localCacheOfDynamicPropertyValues = new Dictionary<int, List<string>>();

            async Task<List<string>> LocalGetAllValuesOfDynamicProperty(int dynamicPropertyId)
            {
                if (!localCacheOfDynamicPropertyValues.ContainsKey(dynamicPropertyId))
                {
                    localCacheOfDynamicPropertyValues[dynamicPropertyId] = (await _dynamicPropertyValueManager
                            .GetAllValuesOfDynamicPropertyAsync(dynamicPropertyId))
                        .Select(x => x.Value).ToList();
                }

                return localCacheOfDynamicPropertyValues[dynamicPropertyId];
            }

            var output = new GetAllDynamicEntityPropertyValuesOutput();
            var dynamicEntityProperties = await _dynamicEntityPropertyManager.GetAllAsync(input.EntityFullName);

            var dynamicEntityPropertySelectedValues = (await _dynamicEntityPropertyValueManager.GetValuesAsync(input.EntityFullName, input.EntityId))
                .GroupBy(value => value.DynamicEntityPropertyId)
                .ToDictionary(
                    group => group.Key,
                    items => items.ToList().Select(value => value.Value)
                        .ToList()
                );

            foreach (var dynamicEntityProperty in dynamicEntityProperties)
            {
                var outputItem = new GetAllDynamicEntityPropertyValuesOutputItem
                {
                    DynamicEntityPropertyId = dynamicEntityProperty.Id,
                    InputType = _dynamicEntityPropertyDefinitionManager.GetOrNullAllowedInputType(dynamicEntityProperty.DynamicProperty.InputType),
                    PropertyName = dynamicEntityProperty.DynamicProperty.PropertyName,
                    AllValuesInputTypeHas = await LocalGetAllValuesOfDynamicProperty(dynamicEntityProperty.DynamicProperty.Id),
                    SelectedValues = dynamicEntityPropertySelectedValues.ContainsKey(dynamicEntityProperty.Id)
                        ? dynamicEntityPropertySelectedValues[dynamicEntityProperty.Id]
                        : new List<string>()
                };

                output.Items.Add(outputItem);
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create)]
        [AbpAuthorize(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit)]
        public async Task InsertOrUpdateAllValues(InsertOrUpdateAllValuesInput input)
        {
            if (input.Items.IsNullOrEmpty())
            {
                return;
            }

            foreach (var item in input.Items)
            {
                await _dynamicEntityPropertyValueManager.CleanValuesAsync(item.DynamicEntityPropertyId, item.EntityId);

                foreach (var newValue in item.Values)
                {
                    await _dynamicEntityPropertyValueManager.AddAsync(new DynamicEntityPropertyValue
                    {
                        DynamicEntityPropertyId = item.DynamicEntityPropertyId,
                        EntityId = item.EntityId,
                        Value = newValue,
                        TenantId = AbpSession.TenantId
                    });
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete)]
        public async Task CleanValues(CleanValuesInput input)
        {
            await _dynamicEntityPropertyValueManager.CleanValuesAsync(input.DynamicEntityPropertyId, input.EntityId);
        }
    }
}