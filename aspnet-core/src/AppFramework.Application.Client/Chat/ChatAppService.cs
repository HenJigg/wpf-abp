using Abp.Application.Services.Dto;
using Abp.PlugIns;
using AppFramework.ApiClient;
using AppFramework.Chat.Dto;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Chat
{
    public class ChatAppService : ProxyAppServiceBase, IChatAppService
    {
        public ChatAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<GetUserChatFriendsWithSettingsOutput> GetUserChatFriendsWithSettings()
        {
            return await ApiClient.GetAsync<GetUserChatFriendsWithSettingsOutput>(GetEndpoint(nameof(GetUserChatFriendsWithSettings)));
        }

        public async Task<ListResultDto<ChatMessageDto>> GetUserChatMessages(GetUserChatMessagesInput input)
        {
            return await ApiClient.GetAsync<ListResultDto<ChatMessageDto>>(GetEndpoint(nameof(GetUserChatMessages)), input);
        }

        public async Task MarkAllUnreadMessagesOfUserAsRead(MarkAllUnreadMessagesOfUserAsReadInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(MarkAllUnreadMessagesOfUserAsRead)), input);
        }
    }
}
