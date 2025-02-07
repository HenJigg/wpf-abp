using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFramework.Version.Dtos;
using AppFramework.Dto;

namespace AppFramework.Version
{
    public interface IAbpVersionsAppService : IApplicationService
    {
        Task<PagedResultDto<AbpVersionDto>> GetAll(GetAllAbpVersionsInput input);

        Task<AbpVersionDto> GetAbpVersionForView(int id);

        Task<AbpVersionDto> GetAbpVersionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAbpVersionDto input);

        Task Delete(EntityDto input);

        Task<UpdateFileOutput> CheckVersion(CheckVersionInput input);
    }
}