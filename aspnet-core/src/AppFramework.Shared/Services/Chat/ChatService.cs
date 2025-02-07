using Abp;
using Abp.Runtime.Security;
using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Chat;
using AppFramework.Chat.Dto;
using AppFramework.Friendships;
using AppFramework.Friendships.Dto;
using AppFramework.Shared.Models.Chat;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services
{
    public class ChatService : ViewModelBase, IChatService
    {
        public ChatService(IAccessTokenManager context,
            IProfileAppService profileAppService,
            IChatAppService chatAppService)
        {
            this.context = context;
            this.profileAppService = profileAppService;
            this.chatAppService = chatAppService;
            friends = new ObservableCollection<FriendModel>();
        }

        public event DelegateChatMessageHandler OnChatMessageHandler;
        private HubConnection chatAuthService;
        private HubConnection friendService;
        private readonly IAccessTokenManager context;
        private readonly IProfileAppService profileAppService;
        private readonly IChatAppService chatAppService;

        public bool IsConnected { get; private set; }

        private ObservableCollection<FriendModel> friends;

        public ObservableCollection<FriendModel> Friends
        {
            get { return friends; }
            set { friends = value; OnPropertyChanged(); }
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

                            if (item.FriendUserId > 0)
                            {
                                var pictureOutput = await profileAppService.GetProfilePictureByUser(item.FriendUserId);
                                if (pictureOutput != null)
                                    item.Photo = Convert.FromBase64String(pictureOutput.ProfilePicture);
                            }

                            Friends.Add(item);
                        }
                    }
                });
            });
        }

        #region Chat

        public async Task SendMessage(SendChatMessageInput input)
        {
            if (!IsConnected)
                throw new Exception("ServerTimeOut");

            if (input == null)
                throw new ArgumentNullException(nameof(input));

            await friendService.InvokeAsync("sendMessage", input);
        }

        public async Task ConnectAsync()
        {
            if (IsConnected) return;

            await ChatServerConnect();

            IsConnected = true;
        }

        public async Task CloseAsync()
        {
            if (chatAuthService != null)
            {
                chatAuthService.Closed -= ChatServiceClosed;
                await chatAuthService.StopAsync();
            }

            if (friendService != null)
            {
                friendService.Closed -= FriendServiceClosed;
                await friendService.StopAsync();
            }

            IsConnected = false;
        }

        private async Task ChatServerConnect()
        {
            chatAuthService = new HubConnectionBuilder()
                   .WithUrl(ApiUrlConfig.DefaultHostUrl + "signalr",
                   ConfigureHttpConnection).Build();
            chatAuthService.Closed += ChatServiceClosed;

            await chatAuthService.StartAsync();

            friendService = new HubConnectionBuilder()
                   .WithUrl(ApiUrlConfig.DefaultHostUrl + "signalr-chat",
                   ConfigureHttpConnection).Build();
            friendService.Closed += FriendServiceClosed;

            friendService.On<ChatMessageDto>("getChatMessage", GetChatMessageHandler);
            friendService.On<FriendDto, bool>("getFriendshipRequest", GetFriendshipRequestHandler);
            friendService.On<UserIdentifier, bool>("getUserConnectNotification", GetUserConnectNotificationHandler);
            friendService.On<UserIdentifier, FriendshipState>("getUserStateChange", GetUserStateChangeHandler);
            friendService.On<UserIdentifier>("getallUnreadMessagesOfUserRead", GetallUnreadMessagesOfUserReadHandler);
            friendService.On<UserIdentifier>("getReadStateChange", GetReadStateChangeHandler);
            await friendService.StartAsync();
        }

        private async Task ChatServiceClosed(Exception? error)
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await chatAuthService.StartAsync();
        }

        private async Task FriendServiceClosed(Exception? error)
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
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
