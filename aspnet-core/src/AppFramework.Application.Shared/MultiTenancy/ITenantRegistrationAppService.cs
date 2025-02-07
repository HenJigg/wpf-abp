using System.Threading.Tasks;
using Abp.Application.Services;
using AppFramework.Editions.Dto;
using AppFramework.MultiTenancy.Dto;

namespace AppFramework.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}