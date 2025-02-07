using System;
using Abp.Application.Services.Dto;
using Abp.Events.Bus.Entities;

namespace AppFramework.Auditing.Dto
{
    public class EntityChangeDto:EntityDto<long>
    {
        public DateTime ChangeTime { get; set; }

        public EntityChangeType ChangeType { get; set; }

        public long EntityChangeSetId { get; set; }
        
        public string EntityId { get; set; }

        public string EntityTypeFullName { get; set; }

        public int? TenantId { get; set; }

        public object EntityEntry { get; set; }
    }
}