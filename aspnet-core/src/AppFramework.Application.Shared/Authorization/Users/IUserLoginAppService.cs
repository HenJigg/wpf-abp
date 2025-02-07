using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFramework.Authorization.Users.Dto;

namespace AppFramework.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<PagedResultDto<UserLoginAttemptDto>> GetUserLoginAttempts(GetLoginAttemptsInput input);
    }
}
