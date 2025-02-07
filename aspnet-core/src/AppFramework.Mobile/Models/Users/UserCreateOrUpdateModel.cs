using Prism.Mvvm;
using System.Collections.Generic;

namespace AppFramework.Shared.Models
{
    public class UserCreateOrUpdateModel : BindableBase
    {
        private bool sendActivationEmail;
        private bool setRandomPassword;
         
        public UserEditModel User { get; set; }

        public string[] AssignedRoleNames { get; set; }

        public bool SendActivationEmail
        {
            get { return sendActivationEmail; }
            set { sendActivationEmail = value; RaisePropertyChanged(); }
        }

        public bool SetRandomPassword
        {
            get { return setRandomPassword; }
            set { setRandomPassword = value; RaisePropertyChanged(); }
        }

        public List<long> OrganizationUnits { get; set; }

        public UserCreateOrUpdateModel()
        {
            OrganizationUnits = new List<long>();
        }
    }
}