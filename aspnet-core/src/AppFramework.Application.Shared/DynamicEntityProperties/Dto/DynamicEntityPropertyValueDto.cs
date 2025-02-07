using Abp.Application.Services.Dto;

namespace AppFramework.DynamicEntityProperties.Dto
{
    public class DynamicEntityPropertyValueDto : EntityDto
    {
        public string Value { get; set; }

        public string EntityId { get; set; }

        public int DynamicEntityPropertyId { get; set; }
    }
}
