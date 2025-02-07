using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Chat;
using Flurl.Http.Content;
using System;
using System.Threading.Tasks;

namespace AppFramework.Authorization.Users.Profile
{
    public class ProxyChatControllerService : ProxyControllerBase
    {
        public ProxyChatControllerService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<ChatUploadFileOutput> UploadFile(Action<CapturedMultipartContent> buildContent)
        {
            return await ApiClient.PostMultipartAsync<ChatUploadFileOutput>(GetEndpoint(nameof(UploadFile)), buildContent);
        }

        public async Task<string> DownloadAsync(string endpoint, string localFolderPath, string localFileName = null)
        {
            return await ApiClient.DownloadAsync(endpoint, localFolderPath, localFileName);
        }
    }
}