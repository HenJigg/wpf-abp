using System;

namespace AppFramework.Chat.Dto
{
    public class ChatMessageExportDto
    {
        public long TargetUserId { get; set; }

        public string TargetUserName { get; set; }

        public string TargetTenantName { get; set; }

        public int? TargetTenantId { get; set; }

        public ChatSide Side { get; set; }

        public ChatMessageReadState ReadState { get; set; }

        public ChatMessageReadState ReceiverReadState { get; set; }

        public string Message { get; set; }

        public DateTime CreationTime { get; set; }
    }
}