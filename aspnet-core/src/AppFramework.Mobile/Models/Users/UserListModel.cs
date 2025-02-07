using AppFramework.Authorization.Users.Dto;
using System;
using System.Collections.Generic;

namespace AppFramework.Shared.Models
{
    public class UserListModel : EntityObject
    {
        private byte[] _photo;

        public string FullName => Name + " " + Surname;

        public byte[] Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                RaisePropertyChanged();
            }
        }

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