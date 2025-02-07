using AppFramework.Friendships;
using Prism.Mvvm;
using System;

namespace AppFramework.Shared.Models.Chat
{
    public class FriendModel : BindableBase
    {
        private bool isOnline;
        private int unreadMessageCount;

        public long FriendUserId { get; set; }

        public int? FriendTenantId { get; set; }

        public string FriendUserName { get; set; }

        public string FriendTenancyName { get; set; }

        public Guid? FriendProfilePictureId { get; set; }

        public int UnreadMessageCount
        {
            get { return unreadMessageCount; }
            set { unreadMessageCount= value; RaisePropertyChanged(); }
        }

        public bool IsOnline
        {
            get { return isOnline; }
            set { isOnline= value; RaisePropertyChanged(); }
        }

        public FriendshipState State { get; set; }
    }
}
