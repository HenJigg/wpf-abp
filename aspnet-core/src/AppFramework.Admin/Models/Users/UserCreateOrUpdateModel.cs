using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Mvvm;
using System.Collections.Generic;

namespace AppFramework.Admin.Models
{
    [INotifyPropertyChanged]
    public partial class UserCreateOrUpdateModel 
    {
        [ObservableProperty]
        private bool sendActivationEmail;

        [ObservableProperty]
        private bool setRandomPassword;
         
        public UserEditModel User { get; set; }

        public string[] AssignedRoleNames { get; set; }
         
        public List<long> OrganizationUnits { get; set; }

        public UserCreateOrUpdateModel()
        {
            OrganizationUnits = new List<long>();
        }
    }
}