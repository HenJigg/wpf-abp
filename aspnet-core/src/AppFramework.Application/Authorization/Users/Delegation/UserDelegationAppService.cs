using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using AppFramework.Authorization.Delegation;
using AppFramework.Authorization.Users.Delegation.Dto;

namespace AppFramework.Authorization.Users.Delegation
{
    [AbpAuthorize]
    public class UserDelegationAppService : AppFrameworkAppServiceBase, IUserDelegationAppService
    {
        private readonly IRepository<UserDelegation, long> _userDelegationRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IUserDelegationManager _userDelegationManager;
        private readonly IUserDelegationConfiguration _userDelegationConfiguration;

        public UserDelegationAppService(
            IRepository<UserDelegation, long> userDelegationRepository,
            IRepository<User, long> userRepository,
            IUserDelegationManager userDelegationManager,
            IUserDelegationConfiguration userDelegationConfiguration)
        {
            _userDelegationRepository = userDelegationRepository;
            _userRepository = userRepository;
            _userDelegationManager = userDelegationManager;
            _userDelegationConfiguration = userDelegationConfiguration;
        }

        public async Task<PagedResultDto<UserDelegationDto>> GetDelegatedUsers(GetUserDelegationsInput input)
        {
            CheckUserDelegationOperation();

            var query = CreateDelegatedUsersQuery(sourceUserId: AbpSession.GetUserId(), targetUserId: null, input.Sorting);
            var totalCount = await query.CountAsync();

            var userDelegations = await query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            return new PagedResultDto<UserDelegationDto>(
                totalCount,
                userDelegations
            );
        }

        public async Task DelegateNewUser(CreateUserDelegationDto input)
        {
            if (input.TargetUserId == AbpSession.GetUserId())
            {
                throw new UserFriendlyException(L("SelfUserDelegationErrorMessage"));
            }

            CheckUserDelegationOperation();

            var delegation = ObjectMapper.Map<UserDelegation>(input);

            delegation.TenantId = AbpSession.TenantId;
            delegation.SourceUserId = AbpSession.GetUserId();

            await _userDelegationRepository.InsertAsync(delegation);
        }

        public async Task RemoveDelegation(EntityDto<long> input)
        {
            CheckUserDelegationOperation();

            await _userDelegationManager.RemoveDelegationAsync(input.Id, AbpSession.ToUserIdentifier());
        }

        /// <summary>
        /// Returns active user delegations for current user
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserDelegationDto>> GetActiveUserDelegations()
        {
            var query = CreateActiveUserDelegationsQuery(AbpSession.GetUserId(), "username");
            query = query.Where(e => e.EndTime >= Clock.Now);
            return await query.ToListAsync();
        }

        private void CheckUserDelegationOperation()
        {
            if (AbpSession.ImpersonatorUserId.HasValue)
            {
                throw new Exception("Cannot execute user delegation operations with an impersonated user !");
            }

            if (!_userDelegationConfiguration.IsEnabled)
            {
                throw new Exception("User delegation configuration is not enabled !");
            }
        }

        private IQueryable<UserDelegationDto> CreateDelegatedUsersQuery(long? sourceUserId, long? targetUserId, string sorting)
        {
            var query = _userDelegationRepository.GetAll()
                .WhereIf(sourceUserId.HasValue, e => e.SourceUserId == sourceUserId)
                .WhereIf(targetUserId.HasValue, e => e.TargetUserId == targetUserId);

            return (from userDelegation in query
                    join targetUser in _userRepository.GetAll() on userDelegation.TargetUserId equals targetUser.Id into targetUserJoined
                    from targetUser in targetUserJoined.DefaultIfEmpty()
                    select new UserDelegationDto
                    {
                        Id = userDelegation.Id,
                        Username = targetUser.UserName,
                        StartTime = userDelegation.StartTime,
                        EndTime = userDelegation.EndTime
                    }).OrderBy(sorting);
        }

        private IQueryable<UserDelegationDto> CreateActiveUserDelegationsQuery(long targetUserId, string sorting)
        {
            var query = _userDelegationRepository.GetAll()
                .Where(e => e.TargetUserId == targetUserId);

            return (from userDelegation in query
                    join sourceUser in _userRepository.GetAll() on userDelegation.SourceUserId equals sourceUser.Id into sourceUserJoined
                    from sourceUser in sourceUserJoined.DefaultIfEmpty()
                    select new UserDelegationDto
                    {
                        Id = userDelegation.Id,
                        Username = sourceUser.UserName,
                        StartTime = userDelegation.StartTime,
                        EndTime = userDelegation.EndTime
                    }).OrderBy(sorting);
        }
    }
}
