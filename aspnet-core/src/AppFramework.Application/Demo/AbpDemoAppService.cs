using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using AppFramework.Version.Dtos;
using Abp.Application.Services.Dto;
using AppFramework.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore; 
using AppFramework.Demo.Dtos;

namespace AppFramework.Demo
{
    [AbpAuthorize(AppPermissions.Pages_AbpDemos)]
    public class AbpDemoAppService : AppFrameworkAppServiceBase, IAbpDemoAppService
    {
        private readonly IRepository<AbpDemo> _repository;

        public AbpDemoAppService(IRepository<AbpDemo> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResultDto<AbpDemoDto>> GetAll(GetAllAbpDemoInput input)
        {
            var filteredAbpModels = _repository.GetAll()
                       .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Address.Contains(input.Filter))
                       .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var pagedAndFilteredAbpVersions = filteredAbpModels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredAbpModels.CountAsync();
            var dbList = await pagedAndFilteredAbpVersions.ToListAsync();
            var results = ObjectMapper.Map<List<AbpDemoDto>>(dbList);

            return new PagedResultDto<AbpDemoDto>(totalCount, results);
        }
    }
}
