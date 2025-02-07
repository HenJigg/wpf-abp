using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile.Dto;
using System;
using System.Threading.Tasks;

namespace AppFramework.Authorization.Users.Profile
{
    public class ProxyProfileAppService : ProxyAppServiceBase, IProfileAppService
    {
        public ProxyProfileAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit()
        {
            return await ApiClient.GetAsync<CurrentUserProfileEditDto>(
                GetEndpoint(nameof(GetCurrentUserProfileForEdit)));
        }

        public async Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(UpdateCurrentUserProfile)), input);
        }

        public async Task ChangePassword(ChangePasswordInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(ChangePassword)), input);
        }

        public async Task UpdateProfilePicture(UpdateProfilePictureInput input)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(UpdateProfilePicture)), input);
        }

        public async Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting()
        {
            return await ApiClient.GetAsync<GetPasswordComplexitySettingOutput>(
                GetEndpoint(nameof(GetPasswordComplexitySetting)));
        }

        public async Task<GetProfilePictureOutput> GetProfilePicture()
        {
            return await ApiClient.GetAsync<GetProfilePictureOutput>(GetEndpoint(nameof(GetProfilePicture)));
        }

        public async Task<GetProfilePictureOutput> GetProfilePictureByUser(long userId)
        {
            return await ApiClient.GetAsync<GetProfilePictureOutput>(GetEndpoint(nameof(GetProfilePictureByUser)),
                new { userId = userId });
        }

        public async Task<GetProfilePictureOutput> GetProfilePictureByUserName(string username)
        {
            return await ApiClient.GetAsync<GetProfilePictureOutput>(GetEndpoint(nameof(GetProfilePictureByUserName)),
                new { username = username });
        }

        public async Task<GetProfilePictureOutput> GetFriendProfilePicture(GetFriendProfilePictureInput input)
        {
            return await ApiClient.GetAsync<GetProfilePictureOutput>(
                GetEndpoint(nameof(GetFriendProfilePicture)),
                input
            );
        }

        public async Task<GetProfilePictureOutput> GetProfilePictureById(Guid profilePictureId)
        {
            return await ApiClient.GetAsync<GetProfilePictureOutput>(GetEndpoint(nameof(GetProfilePictureById)),
                new { profilePictureId = profilePictureId });
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(ChangeLanguage)), input);
        }

        public async Task<UpdateGoogleAuthenticatorKeyOutput> UpdateGoogleAuthenticatorKey()
        {
            return await ApiClient.PutAsync<UpdateGoogleAuthenticatorKeyOutput>(
                GetEndpoint(nameof(UpdateGoogleAuthenticatorKey)));
        }

        public async Task SendVerificationSms(SendVerificationSmsInputDto input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(SendVerificationSms)), input);
        }

        public async Task VerifySmsCode(VerifySmsCodeInputDto input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(VerifySmsCode)));
        }

        public async Task PrepareCollectedData()
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(PrepareCollectedData)));
        }
    }
}