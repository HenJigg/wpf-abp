using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFramework.Demo.Dtos;
using AppFramework.Version.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Demo
{
    public interface IAbpDemoAppService : IApplicationService
    {
        Task<PagedResultDto<AbpDemoDto>> GetAll(GetAllAbpDemoInput input);
    }
}
