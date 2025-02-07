using Abp;
using Abp.Runtime.Security;
using AppFramework.ApiClient;
using AppFramework.Chat;
using AppFramework.Chat.Dto;
using AppFramework.Friendships;
using AppFramework.Friendships.Dto;
using AppFramework.Shared.Models.Chat;
using AppFramework.Shared.ViewModels;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services
{
    public class FriendChatService :
        ViewModelBase, IFriendChatService
    {
        public FriendChatService(IAccessTokenManager context,
            IFriendshipAppService friendshipAppService,
            IChatAppService chatAppService)
        {
            this.context = context;
            this.friendshipAppService = friendshipAppService;
            this.chatAppService = chatAppService;
            friends = new ObservableCollection<FriendModel>();
        }

        public event DelegateChatMessageHandler OnChatMessageHandler;
        private HubConnection chatAuthService;
        private HubConnection friendService;
        private readonly IAccessTokenManager context;
        private readonly IFriendshipAppService friendshipAppService;
        private readonly IChatAppService chatAppService;

        public bool IsConnected { get; private set; }

        private ObservableCollection<FriendModel> friends;

        public ObservableCollection<FriendModel> Friends
        {
            get { return friends; }
            set { friends = value; RaisePropertyChanged(); }
        }

        public async Task GetUserChatFriendsAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(async () =>
                {
                    var frientsSetting = await chatAppService.GetUserChatFriendsWithSettings();
                    if (frientsSetting != null && frientsSetting.Friends != null)
                    {
                        Friends.Clear();
                        var friendsList = Map<List<FriendModel>>(frientsSetting.Friends);

                        foreach (var item in friendsList)
                        {
                            if (string.IsNullOrWhiteSpace(item.FriendTenancyName))
                                item.FriendTenancyName = "Host";

                            Friends.Add(item);
                        }
                    }
                }, StartAsync);
            });
        }

        #region Chat

        public async Task SendMessage(SendChatMessageInput input)
        {
            if (!IsConnected)
                throw new Exception("Please try again after connecting to the server!");

            if (input == null)
                throw new ArgumentNullException(nameof(input));

            await friendService.InvokeAsync("sendMessage", input);
        }

        public async Task StartAsync()
        {
            if (IsConnected) return;

            await ChatServerConnect();

            IsConnected = true;
        }

        public async Task StopAsync()
        {
            await chatAuthService.StopAsync();
            await friendService.StopAsync();
            IsConnected = false;
        }

        private async Task ChatServerConnect()
        {
            try
            {
                var url = ApiUrlConfig.BaseUrl + "signalr";

                chatAuthService = new HubConnectionBuilder()
                       .WithUrl(url, ConfigureHttpConnection).Build();

                chatAuthService.Closed += async (error) =>
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await chatAuthService.StartAsync();
                };

                await chatAuthService.StartAsync();
                await SignalrChatConnect();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task SignalrChatConnect()
        {
            var url = ApiUrlConfig.BaseUrl + "signalr-chat";
            friendService = new HubConnectionBuilder()
                      .WithUrl(url, ConfigureHttpConnection).Build();

            friendService.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await friendService.StartAsync();
            };

            friendService.On<ChatMessageDto>("getChatMessage", GetChatMessageHandler);
            friendService.On<FriendDto, bool>("getFriendshipRequest", GetFriendshipRequestHandler);
            friendService.On<UserIdentifier, bool>("getUserConnectNotification", GetUserConnectNotificationHandler);
            friendService.On<UserIdentifier, FriendshipState>("getUserStateChange", GetUserStateChangeHandler);
            friendService.On<UserIdentifier>("getallUnreadMessagesOfUserRead", GetallUnreadMessagesOfUserReadHandler);
            friendService.On<UserIdentifier>("getReadStateChange", GetReadStateChangeHandler);

            await friendService.StartAsync();
        }

        /// <summary>
        /// 配置HTTP连接配置
        /// </summary>
        /// <param name="options"></param>
        private void ConfigureHttpConnection(HttpConnectionOptions options)
        {
            var accessToken = context.GetAccessToken();
            options.Headers.Add("enc_auth_token", SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase));
             
            options.HttpMessageHandlerFactory = factory => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };
        }

        #endregion

        #region ChatHandler

        /// <summary>
        /// 回调聊天消息
        /// </summary>
        /// <param name="message"></param>
        private void GetChatMessageHandler(ChatMessageDto message)
        {
            if (OnChatMessageHandler != null)
            {
                OnChatMessageHandler.Invoke(message);
            }
            else
            {
                var friend = Friends.FirstOrDefault(t => t.FriendUserId.Equals(message.TargetUserId));
                if (friend != null)
                    friend.UnreadMessageCount += 1;
            }
        }

        /// <summary>
        /// 好友请求
        /// </summary>
        /// <param name="friend"></param>
        /// <param name="result"></param>
        private void GetFriendshipRequestHandler(FriendDto friend, bool result)
        {

        }

        /// <summary>
        /// 用户上线通知
        /// </summary>
        /// <param name="friend"></param>
        /// <param name="isConnected"></param>
        private void GetUserConnectNotificationHandler(UserIdentifier friend, bool isConnected)
        {
            var friendUser = Friends.FirstOrDefault(t => t.FriendUserId.Equals(friend.UserId));
            if (friendUser != null)
                friendUser.IsOnline = isConnected;
        }

        /// <summary>
        /// 用户状态改变
        /// </summary>
        /// <param name="friend"></param>
        /// <param name="state"></param>
        private void GetUserStateChangeHandler(UserIdentifier friend, FriendshipState state)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        private void GetallUnreadMessagesOfUserReadHandler(UserIdentifier user)
        {

        }

        private void GetReadStateChangeHandler(UserIdentifier user)
        {
            //如果要处理消息得已读状态,可以订阅这里
        }

        #endregion
    }
}
