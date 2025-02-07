using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Sessions.Dto;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services.Storage
{
    public interface IAccountStorageService
    { 
        Task StoreAccessTokenAsync(string newAccessToken);

        Task StoreAuthenticateResultAsync(AbpAuthenticateResultModel authenticateResultModel);

        AbpAuthenticateResultModel RetrieveAuthenticateResult();

        TenantInformation RetrieveTenantInfo();

        GetCurrentLoginInformationsOutput RetrieveLoginInfo();

        void ClearSessionPersistance();

        Task StoreLoginInformationAsync(GetCurrentLoginInformationsOutput loginInfo);

        Task StoreTenantInfoAsync(TenantInformation tenantInfo);
    }
}