using AppFramework.Authorization.Users.Dto;
using AppFramework.Organizations.Dto;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppFramework.Shared.Models
{
    public class UserForEditModel : BindableBase
    {
        public Guid? ProfilePictureId { get; set; }

        private UserEditModel user;

        public UserEditModel User
        {
            get { return user; }
            set { user = value; RaisePropertyChanged(); }
        }

        public UserRoleDto[] Roles { get; set; }

        public List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        public List<string> MemberedOrganizationUnits { get; set; }

        private byte[] _photo;
        private List<OrganizationUnitModel> _organizationUnits;

        public string FullName => User == null ? string.Empty : User.Name + " " + User.Surname;

        public DateTime CreationTime { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public byte[] Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                RaisePropertyChanged();
            }
        }

        public List<OrganizationUnitModel> OrganizationUnits
        {
            get => _organizationUnits;
            set
            {
                _organizationUnits = value?.OrderBy(o => o.Code).ToList();
                SetAsAssignedForMemberedOrganizationUnits();
                RaisePropertyChanged();
            }
        }

        private void SetAsAssignedForMemberedOrganizationUnits()
        {
            if (_organizationUnits != null)
            {
                MemberedOrganizationUnits?.ForEach(memberedOrgUnitCode =>
                {
                    _organizationUnits
                        .Single(o => o.Code == memberedOrgUnitCode)
                        .IsAssigned = true;
                });
            }
        }
    }
}