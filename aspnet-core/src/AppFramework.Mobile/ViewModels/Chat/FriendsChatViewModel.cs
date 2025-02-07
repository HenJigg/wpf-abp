using Abp.Runtime.Security;
using Acr.UserDialogs;
using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Chat;
using AppFramework.Chat.Dto;
using AppFramework.Shared.Models.Chat;
using AppFramework.Shared.Services;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFImageLoading;
using Abp.IO.Extensions;
using AppFramework.Dto;
using MimeMapping;
using Xamarin.Essentials;

namespace AppFramework.Shared.ViewModels
{
    public class FriendsChatViewModel : NavigationViewModel
    {
        public FriendsChatViewModel(IApplicationContext context,
           IChatAppService chatApp,
           IFriendChatService chatService,
           IProfileAppService profileAppService,
           IAccessTokenManager tokenManager,
           IEventAggregator aggregator,
           ProxyChatControllerService proxyChatService)
        {
            this.context = context;
            this.chatApp = chatApp;
            this.chatService = chatService;
            this.profileAppService = profileAppService;
            this.tokenManager = tokenManager;
            this.aggregator = aggregator;
            this.proxyChatService = proxyChatService;

            chatService.OnChatMessageHandler += ChatService_OnChatMessageHandler;
            messages = new ObservableCollection<ChatMessageModel>();
            SendCommand = new DelegateCommand(Send);

            PickFileCommand = new DelegateCommand(PickFile);
            PickImageCommand = new DelegateCommand(PickImage);
        }

        #region 字段/属性

        private string userName;
        private string message;
        private FriendModel friend;
        private readonly IApplicationContext context;
        private readonly IChatAppService chatApp;
        private readonly IFriendChatService chatService;
        private readonly IProfileAppService profileAppService;
        private readonly IAccessTokenManager tokenManager;
        private readonly IEventAggregator aggregator;
        private readonly ProxyChatControllerService proxyChatService;
        private ObservableCollection<ChatMessageModel> messages;

        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }

        public FriendModel Friend
        {
            get { return friend; }
            set { friend = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<ChatMessageModel> Messages
        {
            get { return messages; }
            set { messages = value; RaisePropertyChanged(); }
        }

        public DelegateCommand SendCommand { get; private set; }
        public DelegateCommand PickImageCommand { get; private set; }
        public DelegateCommand PickFileCommand { get; private set; }
        public DelegateCommand<ChatMessageModel> OpenFolderCommand { get; private set; }

        #endregion

        #region 消息处理

        /// <summary>
        /// 加载用户的聊天记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task GetUserChatMessagesByUser(long userId)
        {
            await WebRequest.Execute(() =>
                chatApp.GetUserChatMessages(new GetUserChatMessagesInput()
                {
                    UserId = userId
                }), async result =>
                {
                    if (!result.Items.Any()) return;

                    var list = Map<List<ChatMessageModel>>(result.Items);
                    userName = chatService.Friends
                    .FirstOrDefault(t => t.FriendUserId.Equals(list.First().TargetUserId)).FriendUserName;

                    foreach (var item in list)
                    {
                        await UpdateMessageInfo(item);
                        Messages.Add(item);
                    }

                    await MarkAllUnreadMessages();
                });
        }

        /// <summary>
        /// 更新消息格式
        /// </summary>
        /// <param name="model"></param>
        private async Task UpdateMessageInfo(ChatMessageModel model)
        {
            if (model.Side == ChatSide.Sender)
            {
                model.Color = "#009933";
                model.UserName = context.LoginInfo.User.Name;
            }
            else
                model.UserName = userName;

            if (model.Message.StartsWith("[image]"))
            {
                model.MessageType = "image";
                await SaveCacheFile(model, model.Message.Replace("[image]", ""));
            }
            else if (model.Message.StartsWith("[file]"))
            {
                model.MessageType = "file";
                await SaveCacheFile(model, model.Message.Replace("[file]", ""));
            }
            else if (model.Message.StartsWith("[link]"))
            {
                model.MessageType = "link";
                var msg = model.Message.Replace("[link]", "");
                model.Message = JsonConvert.DeserializeObject<dynamic>(msg).message;
            }
            else
            {
                model.MessageType = "text";
            }
        }

        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="chatMessage"></param>
        private async void ChatService_OnChatMessageHandler(ChatMessageDto chatMessage)
        {
            var message = Messages.FirstOrDefault(t => t.Id.Equals(chatMessage.Id));
            if (message==null)
            {
                var msg = Map<ChatMessageModel>(chatMessage);
                await UpdateMessageInfo(msg);
                Messages.Add(msg);
                await MarkAllUnreadMessages();
            }
        }

        /// <summary>
        /// 标记消息已读
        /// </summary>
        /// <returns></returns>
        private async Task MarkAllUnreadMessages()
        {
            await WebRequest.Execute(async () =>
            {
                await chatApp.MarkAllUnreadMessagesOfUserAsRead(new MarkAllUnreadMessagesOfUserAsReadInput()
                {
                    UserId = Friend.FriendUserId
                });
            });
        }

        #endregion

        #region 缓存文件

        private async Task SaveCacheFile(ChatMessageModel model, string msg)
        {
            var accessToken = tokenManager.GetAccessToken();
            var code = SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);

            var output = JsonConvert.DeserializeObject<ChatUploadFileOutput>(msg);
            model.Message = output.Name; //显示文件名

            var downloadUrl = ApiUrlConfig.BaseUrl + $"Chat/GetUploadedObject?fileId={output.Id}" +
               $"&fileName={output.Name}" +
               $"&contentType={output.ContentType}" +
               $"&enc_auth_token={code}";

            await DownloadAsync(downloadUrl, SharedConsts.DocumentPath, output.Name);
        }

        private async Task DownloadAsync(string url, string localFolderPath, string fileName)
        {
            if (File.Exists($"{localFolderPath}/{fileName}"))
            {
                await Task.CompletedTask;
            }
            else
            {
                await proxyChatService.DownloadAsync(url, localFolderPath, fileName);
            }
        }

        #endregion

        #region 选择图片

        private static async Task PickProfilePictureAsync(Func<MediaFile, Task> picturePicked)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
                return;

            using (var photo = await CrossMedia.Current.PickPhotoAsync())
            {
                await picturePicked(photo);
            }
        }

        private async Task OnCompleted(MediaFile photo)
        {
            if (photo == null)
                return;

            var croppedImageBytes = File.ReadAllBytes(photo.Path);
            var fileName = Path.GetFileName(photo.Path);

            var jpgStream = await ResizeImageAsync(croppedImageBytes);
            string contentType = MimeUtility.GetMimeMapping(fileName);

            await WebRequest.Execute(() => UploadFile(jpgStream.GetAllBytes(), fileName, contentType),
                async output =>
                {
                    string message = $"[image]{{\"id\":\"{output.Id}\", " +
                                             $"\"name\":\"{output.Name}\", " +
                                             $"\"contentType\":\"{output.ContentType}\"}}";

                    await chatService.SendMessage(new SendChatMessageInput()
                    {
                        UserId = Friend.FriendUserId,
                        Message = message,
                        UserName = context.LoginInfo.User.Name
                    });
                });
        }

        private static async Task<Stream> ResizeImageAsync(byte[] imageBytes, int width = 256, int height = 256)
        {
            var result = ImageService.Instance.LoadStream(token =>
            {
                var tcs = new TaskCompletionSource<Stream>();
                tcs.TrySetResult(new MemoryStream(imageBytes));
                return tcs.Task;
            }).DownSample(width, height);

            return await result.AsJPGStreamAsync();
        }

        private async Task<ChatUploadFileOutput> UploadFile(byte[] photoAsBytes, string fileName, string contentType)
        {
            using (Stream photoStream = new MemoryStream(photoAsBytes))
            {
                return await proxyChatService.UploadFile(content =>
                {
                    content.AddFile("file", photoStream, fileName, contentType);
                    content.AddString(nameof(FileDto.FileName), fileName);
                });
            }
        }

        #endregion

        #region 选择文件

        private static async Task PickfileAsync(Func<FileResult, Task> filePicked)
        {
            var file = await FilePicker.PickAsync();

            if (file!=null) await filePicked(file);
        }

        private async Task PickFileOnCompleted(FileResult fileResult)
        {
            if (fileResult == null) return;

            var fileAsBytes = File.ReadAllBytes(fileResult.FullPath);
            var fileName = fileResult.FileName.Replace(" ", "");
            string contentType = MimeUtility.GetMimeMapping(fileName);

            await WebRequest.Execute(() => UploadFile(fileAsBytes, fileName, contentType),
                async output =>
                {
                    string message = $"[file]{{\"id\":\"{output.Id}\", " +
                                              $"\"name\":\"{output.Name}\", " +
                                              $"\"contentType\":\"{output.ContentType}\"}}";

                    await chatService.SendMessage(new SendChatMessageInput()
                    {
                        UserId = Friend.FriendUserId,
                        Message = message,
                        UserName = context.LoginInfo.User.Name
                    });
                });
        }

        #endregion

        #region 发送消息

        /// <summary>
        /// 发送消息
        /// </summary>
        public async void Send()
        {
            if (string.IsNullOrWhiteSpace(Message)) return;

            await WebRequest.Execute(() =>
              chatService.SendMessage(new SendChatMessageInput()
              {
                  UserId = Friend.FriendUserId,
                  Message = Message,
                  UserName = context.LoginInfo.User.Name
              }));

            Message = string.Empty; //发完消息就清除输入内容
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        private async void PickFile()
        {
            await PickfileAsync(PickFileOnCompleted);
        }

        /// <summary>
        /// 选择图片
        /// </summary>
        private void PickImage()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig
            {
                Options = new List<ActionSheetOption>  {
                    new ActionSheetOption(Local.Localize("PickFromGallery"),  async () => await PickProfilePictureAsync(OnCompleted)),
                }
            });
        }

        #endregion

        public override Task GoBackAsync()
        {
            chatService.OnChatMessageHandler -= ChatService_OnChatMessageHandler;
            return base.GoBackAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Friend = parameters.GetValue<FriendModel>("Value");

                await GetUserChatMessagesByUser(Friend.FriendUserId);
            }
        }
    }
}
