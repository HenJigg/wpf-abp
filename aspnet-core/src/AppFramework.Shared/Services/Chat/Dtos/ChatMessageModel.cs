using AppFramework.Chat;
using Prism.Mvvm;
using System;

namespace AppFramework.Shared.Models.Chat
{
    public class ChatMessageModel : BindableBase
    {
        public int Id { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public int? TenantId { get; set; }

        public long TargetUserId { get; set; }

        public int? TargetTenantId { get; set; }

        public ChatSide Side { get; set; }

        public ChatMessageReadState ReadState { get; set; }

        public ChatMessageReadState ReceiverReadState { get; set; }

        public string Message { get; set; }

        public DateTime CreationTime { get; set; }

        public string SharedMessageId { get; set; }

        public string Color { get; set; } = "#0066FF";

        private string messageType;

        public string MessageType
        {
            get { return messageType; }
            set { messageType=value; RaisePropertyChanged(); }
        }
    }
}
