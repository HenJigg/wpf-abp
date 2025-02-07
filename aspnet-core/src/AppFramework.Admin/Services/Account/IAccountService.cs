using AppFramework.ApiClient.Models;
using AppFramework.Admin.Models;
using System.Threading.Tasks;

namespace AppFramework.Admin.Services
{
    public interface IAccountService
    {
        AbpAuthenticateModel AuthenticateModel { get; set; }

        AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

        Task LoginUserAsync();

        Task LoginCurrentUserAsync(UserListModel user);

        Task LogoutAsync();
    }
}