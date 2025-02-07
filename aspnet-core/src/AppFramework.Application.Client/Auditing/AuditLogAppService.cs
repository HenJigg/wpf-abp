using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.Auditing;
using AppFramework.Auditing.Dto;
using AppFramework.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.Application.Client
{
    public class AuditLogAppService : ProxyAppServiceBase, IAuditLogAppService
    {
        public AuditLogAppService(AbpApiClient apiClient) :
            base(apiClient)
        {
        }

        public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<AuditLogListDto>>(GetEndpoint(nameof(GetAuditLogs)), input);
        }

        public async Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetAuditLogsToExcel)), input);
        }

        public async Task<PagedResultDto<EntityChangeListDto>> GetEntityChanges(GetEntityChangeInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<EntityChangeListDto>>(GetEndpoint(nameof(GetEntityChanges)), input);
        }

        public async Task<FileDto> GetEntityChangesToExcel(GetEntityChangeInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetEntityChangesToExcel)), input);
        }

        public List<NameValueDto> GetEntityHistoryObjectTypes()
        {
            return ApiClient.GetAsync<List<NameValueDto>>(GetEndpoint(nameof(GetEntityHistoryObjectTypes))).Result;
        }

        public async Task<List<EntityPropertyChangeDto>> GetEntityPropertyChanges(long entityChangeId)
        {
            return await ApiClient.GetAsync<List<EntityPropertyChangeDto>>(GetEndpoint(nameof(GetEntityPropertyChanges)), entityChangeId);
        }

        public async Task<PagedResultDto<EntityChangeListDto>> GetEntityTypeChanges(GetEntityTypeChangeInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<EntityChangeListDto>>(GetEndpoint(nameof(GetEntityTypeChanges)), input);
        }
    }
}