using AppFramework.Authorization.Users.Dto;
using AppFramework.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AppFramework.Admin.Models
{
    public partial class UserListModel : EntityObject
    {
        [ObservableProperty]
        private byte[] photo;

        public string FullName => Name + " " + Surname;
          
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public List<UserListRoleDto> Roles { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }
    }
}