using Abp.Runtime.Security;
using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Chat;
using AppFramework.Chat.Dto;
using AppFramework.Dto;
using AppFramework.Shared;
using AppFramework.Shared.Models.Chat;
using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MimeMapping;
using Abp.Application.Services.Dto;
using AppFramework.Shared.Services;

namespace AppFramework.Admin.ViewModels
{
    public class FriendsChatViewModel : HostDialogViewModel
    {
        public FriendsChatViewModel(IApplicationContext context,
           IChatAppService chatApp,
           IChatService chatService,
           IProfileAppService profileAppService,
           IAccessTokenManager tokenManager,
           ProxyChatControllerService proxyChatService)
        {
            this.context = context;
            this.chatApp = chatApp;
            this.chatService = chatService;
            this.profileAppService = profileAppService;
            this.tokenManager = tokenManager;
            this.proxyChatService = proxyChatService;

            chatService.OnChatMessageHandler += ChatService_OnChatMessageHandler;
            messages = new ObservableCollection<ChatMessageModel>();
            SendCommand = new DelegateCommand(Send);

            PickFileCommand = new DelegateCommand(PickFile);
            PickImageCommand = new DelegateCommand(PickImage);
            OpenFolderCommand = new DelegateCommand<ChatMessageModel>(OpenFolder);
        }

        #region 字段/属性

        private string userName;
        private string message;
        private FriendModel friend;
        private readonly IApplicationContext context;
        private readonly IChatAppService chatApp;
        private readonly IChatService chatService;
        private readonly IProfileAppService profileAppService;
        private readonly IAccessTokenManager tokenManager;
        private readonly ProxyChatControllerService proxyChatService;
        private ObservableCollection<ChatMessageModel> messages;

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }

        public FriendModel Friend
        {
            get { return friend; }
            set { friend = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ChatMessageModel> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged(); }
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
            await chatApp.GetUserChatMessages(new GetUserChatMessagesInput() { UserId = userId })
                         .WebAsync(GetUserChatMessagesSuccessed); 
        }

        /// <summary>
        /// 处理消息数据格式
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task GetUserChatMessagesSuccessed(ListResultDto<ChatMessageDto> result)
        {
            if (!result.Items.Any()) return;

            var list = Map<List<ChatMessageModel>>(result.Items);
            var userId = list.First().TargetUserId;
            userName = chatService.Friends.First(t => t.FriendUserId.Equals(userId)).FriendUserName;

            foreach (var item in list)
            {
                await UpdateMessageInfo(item);
                UpdateMessageGroup(item);
                Messages.Add(item);
            }
            await MarkAllUnreadMessages();
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
                model.MessageType = "text";
                var msg = model.Message.Replace("[link]", "");
                model.Message = JsonConvert.DeserializeObject<dynamic>(msg).message;
            }
            else
            {
                model.MessageType = "text";
            }
        }

        /// <summary>
        /// 更新消息分组
        /// 小于10分钟以内的消息归一组
        /// </summary>
        /// <param name="model"></param>
        private void UpdateMessageGroup(ChatMessageModel chatMessage)
        {
            var lastMessage = Messages.LastOrDefault();
            if (lastMessage != null)
            {
                var timeSpan = chatMessage.CreationTime - lastMessage.CreationTime;
                if (timeSpan.TotalMinutes < 10)
                    chatMessage.CreationTime = lastMessage.CreationTime;
            }
        }

        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="chatMessage"></param>
        private async void ChatService_OnChatMessageHandler(ChatMessageDto chatMessage)
        {
            var message = Messages.FirstOrDefault(t => t.Id.Equals(chatMessage.Id));
            if (message == null)
            {
                var msg = Map<ChatMessageModel>(chatMessage);
                await UpdateMessageInfo(msg);
                UpdateMessageGroup(msg);
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
            await chatApp.MarkAllUnreadMessagesOfUserAsRead(new MarkAllUnreadMessagesOfUserAsReadInput()
            {
                UserId = Friend.FriendUserId
            }).WebAsync(); 
        }

        #endregion

        #region 发送图片/文件

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

            await DownloadAsync(downloadUrl, AppConsts.DocumentPath, output.Name);
        }

        private async Task DownloadAsync(string url, string localFolderPath, string fileName)
        {
            if (File.Exists($"{localFolderPath}{fileName}"))
            {
                await Task.CompletedTask;
            }
            else
            {
                await proxyChatService.DownloadAsync(url, localFolderPath, fileName);
            }
        }

        private void OpenFolder(ChatMessageModel msg)
        {
            if (Directory.Exists(AppConsts.DocumentPath))
                System.Diagnostics.Process.Start("explorer.exe", AppConsts.DocumentPath);
        }

        private async void PickFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "所有文件(*.*)|*.*";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult != null && (bool)dialogResult)
            {
                string fileName = fileDialog.SafeFileName;
                string contentType = MimeUtility.GetMimeMapping(fileName);
                var fileAsBytes = File.ReadAllBytes(fileDialog.FileName);

                await SetBusyAsync(async () =>
                {
                    await UploadFile(fileAsBytes, fileName, contentType).WebAsync(async output =>
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
                });
            }
        }

        private async void PickImage()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "图片文件(*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            var dialogResult = fileDialog.ShowDialog();
            if ((bool)dialogResult)
            {
                string fileName = fileDialog.SafeFileName;
                string contentType = MimeUtility.GetMimeMapping(fileName);
                var photoAsBytes = File.ReadAllBytes(fileDialog.FileName);

                await SetBusyAsync(async () =>
                {
                    await UploadFile(photoAsBytes, fileName, contentType).WebAsync(async output =>
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
                });
            }
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

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public override void Cancel()
        {
            chatService.OnChatMessageHandler -= ChatService_OnChatMessageHandler;
            base.Cancel();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public async void Send()
        {
            if (string.IsNullOrWhiteSpace(Message)) return;

            await chatService.SendMessage(new SendChatMessageInput()
            {
                UserId = Friend.FriendUserId,
                Message = Message,
                UserName = context.LoginInfo.User.Name
            }).WebAsync();

            Message = string.Empty; //发完消息就清除输入内容
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Friend = parameters.GetValue<FriendModel>("Value");

                await GetUserChatMessagesByUser(Friend.FriendUserId);
            }
        }
    }
}
