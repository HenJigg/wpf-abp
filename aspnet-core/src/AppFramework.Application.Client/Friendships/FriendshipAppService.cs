using AppFramework.ApiClient;
using AppFramework.Friendships.Dto; 
using System.Threading.Tasks;

namespace AppFramework.Friendships
{
    public class FriendshipAppService : ProxyAppServiceBase, IFriendshipAppService
    {
        public FriendshipAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task AcceptFriendshipRequest(AcceptFriendshipRequestInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(AcceptFriendshipRequest)), input);
        }

        public async Task BlockUser(BlockUserInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(BlockUser)), input);
        }

        public async Task<FriendDto> CreateFriendshipRequest(CreateFriendshipRequestInput input)
        {
            return await ApiClient.PostAsync<FriendDto>(GetEndpoint(nameof(CreateFriendshipRequest)), input);
        }

        public async Task<FriendDto> CreateFriendshipRequestByUserName(CreateFriendshipRequestByUserNameInput input)
        {
            return await ApiClient.PostAsync<FriendDto>(GetEndpoint(nameof(CreateFriendshipRequestByUserName)), input);
        }

        public async Task RemoveFriend(RemoveFriendInput input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(RemoveFriend)), input);
        }

        public async Task UnblockUser(UnblockUserInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(UnblockUser)), input);
        }
    }
}
