using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Sessions.Dto;
using AutoMapper;
using System.Threading.Tasks; 
using AppFramework.Shared.Models;

namespace AppFramework.Shared.Services.Storage
{
    public class AccountStorageService : IAccountStorageService
    {
        private readonly IDataStorageService _dataStorageManager;
        private readonly IAppMapper mapper;

        public AccountStorageService(
            IDataStorageService dataStorageManager,
            IAppMapper mapper)
        {
            _dataStorageManager = dataStorageManager;
            this.mapper = mapper;
        }

        public async Task StoreAccessTokenAsync(string newAccessToken)
        {
            var authenticateResult = _dataStorageManager.GetValue<AuthenticateResultPersistanceModel>(DataStorageKey.CurrentSession_TokenInfo);

            authenticateResult.AccessToken = newAccessToken;

            _dataStorageManager.SetValue(DataStorageKey.CurrentSession_TokenInfo, authenticateResult);

            await Task.CompletedTask;
        }

        public AbpAuthenticateResultModel RetrieveAuthenticateResult()
        {
            return mapper.Map<AbpAuthenticateResultModel>(
                _dataStorageManager.GetValue<AuthenticateResultPersistanceModel>(
                    DataStorageKey.CurrentSession_TokenInfo));
        }

        public async Task StoreAuthenticateResultAsync(AbpAuthenticateResultModel authenticateResultModel)
        {
            _dataStorageManager.SetValue(
               DataStorageKey.CurrentSession_TokenInfo,
               mapper.Map<AuthenticateResultPersistanceModel>(authenticateResultModel)
           );

            await Task.CompletedTask;
        }

        public TenantInformation RetrieveTenantInfo()
        {
            return mapper.Map<TenantInformation>(
                _dataStorageManager.GetValue<TenantInformationPersistanceModel>(
                    DataStorageKey.CurrentSession_TenantInfo
                )
            );
        }

        public async Task StoreTenantInfoAsync(TenantInformation tenantInfo)
        {
            _dataStorageManager.SetValue(
                DataStorageKey.CurrentSession_TenantInfo,
                mapper.Map<TenantInformationPersistanceModel>(tenantInfo)
            );

            await Task.CompletedTask;
        }

        public GetCurrentLoginInformationsOutput RetrieveLoginInfo()
        {
            return mapper.Map<GetCurrentLoginInformationsOutput>(
                _dataStorageManager.GetValue<CurrentLoginInformationPersistanceModel>(
                    DataStorageKey.CurrentSession_LoginInfo
                )
            );
        }

        public async Task StoreLoginInformationAsync(GetCurrentLoginInformationsOutput loginInfo)
        {
            var value = mapper
                .Map<CurrentLoginInformationPersistanceModel>(loginInfo);

            _dataStorageManager.SetValue(
               DataStorageKey.CurrentSession_LoginInfo, value);

            await Task.CompletedTask;
        }

        public void ClearSessionPersistance()
        {
            _dataStorageManager.Remove(DataStorageKey.CurrentSession_TokenInfo);
            _dataStorageManager.Remove(DataStorageKey.CurrentSession_TenantInfo);
            _dataStorageManager.Remove(DataStorageKey.CurrentSession_LoginInfo);
        }
    }
}