using System.Threading.Tasks;
using Abp.Application.Services;
using AppFramework.Sessions.Dto;

namespace AppFramework.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
